using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.UI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using nflgames;
using livescoring;

public partial class _2025_05_alt_fifthwk : System.Web.UI.Page
{
    private const string SeasonSegment = "2025-regular";

    public string ScheduleUrl = $"{CredentialStore.ApiBaseUrl}/{SeasonSegment}/full_game_schedule.json?date=from-20251002-to-20251006";

    public NFLschedule Schedule { get; private set; }
    public List<DayGroup> DayGroups { get; } = new List<DayGroup>();
    public List<ParticipantRow> Participants { get; } = new List<ParticipantRow>();
    public int NumberOfGames { get; private set; }

    public List<string> theFirstName = new List<string>();
    public List<string> theLastName = new List<string>();
    public List<string> theTotal = new List<string>();

    private readonly Dictionary<string, Gamescore> _scoresByGameId = new Dictionary<string, Gamescore>(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<DateTime, IReadOnlyList<Gamescore>> _scoresByDate = new Dictionary<DateTime, IReadOnlyList<Gamescore>>();

    protected void Page_Load(object sender, EventArgs e)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        using (var client = new WebClient { Credentials = CredentialStore.ApiCredential })
        {
            LoadSchedule(client);
            BuildDayGroups(client);
        }

        LoadParticipants();
    }

    private void LoadSchedule(WebClient client)
    {
        var response = client.DownloadString(ScheduleUrl);
        Schedule = JsonConvert.DeserializeObject<NFLschedule>(response);

        NumberOfGames = Schedule?.fullgameschedule?.gameentry?.Length ?? 0;
    }

    private void BuildDayGroups(WebClient client)
    {
        if (Schedule?.fullgameschedule?.gameentry == null)
        {
            return;
        }

        var entriesWithDate = Schedule.fullgameschedule.gameentry
            .Select(entry => new
            {
                Entry = entry,
                Date = ParseDate(entry.date)
            })
            .Where(x => x.Date.HasValue)
            .Select(x => new
            {
                x.Entry,
                Date = x.Date.Value.Date
            })
            .ToList();

        foreach (var group in entriesWithDate.GroupBy(x => x.Date).OrderBy(g => g.Key))
        {
            var dayGroup = new DayGroup
            {
                Date = group.Key,
                DayName = group.Key.ToString("dddd", CultureInfo.InvariantCulture)
            };

            var scoresForDate = GetScoresForDate(client, group.Key);
            foreach (var score in scoresForDate)
            {
                if (!string.IsNullOrEmpty(score?.game?.ID))
                {
                    _scoresByGameId[score.game.ID] = score;
                }
            }

            foreach (var item in group.OrderBy(x => x.Entry.time))
            {
                _scoresByGameId.TryGetValue(item.Entry.id, out var score);
                dayGroup.Games.Add(new GameDisplay(item.Entry, score));
            }

            DayGroups.Add(dayGroup);
        }
    }

    private IReadOnlyList<Gamescore> GetScoresForDate(WebClient client, DateTime date)
    {
        if (_scoresByDate.TryGetValue(date, out var cached))
        {
            return cached;
        }

        var formattedDate = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var url = $"{CredentialStore.ApiBaseUrl}/{SeasonSegment}/scoreboard.json?fordate={formattedDate}";

        try
        {
            var response = client.DownloadString(url);
            var scoreboard = JsonConvert.DeserializeObject<LiveScoring>(response);
            var games = scoreboard?.scoreboard?.gameScore?.Where(g => g?.game?.ID != null).ToList() ?? new List<Gamescore>();
            _scoresByDate[date] = games;
            return games;
        }
        catch (WebException)
        {
            _scoresByDate[date] = new List<Gamescore>();
            return _scoresByDate[date];
        }
    }

    private void LoadParticipants()
    {
        if (Schedule?.fullgameschedule?.gameentry == null || Schedule.fullgameschedule.gameentry.Length == 0)
        {
            return;
        }

        var connectionString = CredentialStore.PoolConnectionString;

        using (var conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            var commandText = "SELECT * FROM `five2025` ORDER BY UPPER(lastname), UPPER(firstname)";
            using (var command = new MySqlCommand(commandText, conn))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var participant = new ParticipantRow
                    {
                        FirstName = reader["firstname"].ToString(),
                        LastName = reader["lastname"].ToString()
                    };

                    var picks = new List<string>();

                    for (int i = 0; i < NumberOfGames; i++)
                    {
                        var columnName = $"game{i + 1}";
                        var pickValue = HasColumn(reader, columnName) ? reader[columnName].ToString() : string.Empty;
                        picks.Add(pickValue);

                        var scheduleEntry = Schedule.fullgameschedule.gameentry[i];
                        _scoresByGameId.TryGetValue(scheduleEntry.id, out var gameScore);
                        participant.Picks.Add(BuildPickResult(pickValue, scheduleEntry, gameScore));
                    }

                    participant.WeeklyScore = CalculateScore(picks);
                    participant.YtdTotal = FindYtdTotal(participant.FirstName, participant.LastName);

                    Participants.Add(participant);
                }
            }
        }
    }

    private int CalculateScore(IReadOnlyList<string> picks)
    {
        int score = 0;

        for (int i = 0; i < picks.Count && i < NumberOfGames; i++)
        {
            var scheduleEntry = Schedule.fullgameschedule.gameentry[i];
            if (!_scoresByGameId.TryGetValue(scheduleEntry.id, out var gameScore))
            {
                continue;
            }

            if (!string.Equals(gameScore?.isCompleted, "true", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var pick = (picks[i] ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(pick))
            {
                continue;
            }

            var awayName = GetTeamFullName(scheduleEntry.awayTeam);
            var homeName = GetTeamFullName(scheduleEntry.homeTeam);

            bool pickedAway = string.Equals(pick, awayName, StringComparison.OrdinalIgnoreCase);
            bool pickedHome = string.Equals(pick, homeName, StringComparison.OrdinalIgnoreCase);

            if (!pickedAway && !pickedHome)
            {
                continue;
            }

            int awayScore = ParseScore(gameScore.awayScore);
            int homeScore = ParseScore(gameScore.homeScore);

            if (awayScore == homeScore)
            {
                continue;
            }

            bool awayWon = awayScore > homeScore;

            if ((pickedAway && awayWon) || (pickedHome && !awayWon))
            {
                score++;
            }
        }

        return score;
    }

    private PickResult BuildPickResult(string pickValue, Gameentry scheduleEntry, Gamescore gameScore)
    {
        var trimmedPick = (pickValue ?? string.Empty).Trim();
        var awayName = GetTeamFullName(scheduleEntry.awayTeam);
        var homeName = GetTeamFullName(scheduleEntry.homeTeam);

        bool pickedAway = string.Equals(trimmedPick, awayName, StringComparison.OrdinalIgnoreCase);
        bool pickedHome = string.Equals(trimmedPick, homeName, StringComparison.OrdinalIgnoreCase);

        var pickResult = new PickResult
        {
            RawPick = trimmedPick,
            Abbreviation = pickedAway ? scheduleEntry.awayTeam.Abbreviation : pickedHome ? scheduleEntry.homeTeam.Abbreviation : trimmedPick,
            CssClass = pickedAway ? "away" : pickedHome ? "home" : string.Empty
        };

        if (gameScore == null)
        {
            return pickResult;
        }

        int awayScore = ParseScore(gameScore.awayScore);
        int homeScore = ParseScore(gameScore.homeScore);
        bool isCompleted = string.Equals(gameScore.isCompleted, "true", StringComparison.OrdinalIgnoreCase);

        if (pickedAway)
        {
            if (isCompleted)
            {
                pickResult.CssClass = awayScore > homeScore ? "win" : awayScore < homeScore ? "loss" : "away";
            }
            else
            {
                pickResult.CssClass = "away";
            }
        }
        else if (pickedHome)
        {
            if (isCompleted)
            {
                pickResult.CssClass = homeScore > awayScore ? "win" : homeScore < awayScore ? "loss" : "home";
            }
            else
            {
                pickResult.CssClass = "home";
            }
        }

        return pickResult;
    }

    private string FindYtdTotal(string firstName, string lastName)
    {
        for (int i = 0; i < theFirstName.Count; i++)
        {
            if (string.Equals(firstName, theFirstName[i], StringComparison.OrdinalIgnoreCase) &&
                string.Equals(lastName, theLastName[i], StringComparison.OrdinalIgnoreCase))
            {
                return theTotal[i];
            }
        }

        return string.Empty;
    }

    private static bool HasColumn(MySqlDataReader reader, string columnName)
    {
        try
        {
            return reader.GetOrdinal(columnName) >= 0;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    private static DateTime? ParseDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static int ParseScore(string value)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : 0;
    }

    private static string GetTeamFullName(nflgames.Awayteam team)
    {
        return team == null ? string.Empty : $"{team.City} {team.Name}".Trim();
    }

    private static string GetTeamFullName(nflgames.Hometeam team)
    {
        return team == null ? string.Empty : $"{team.City} {team.Name}".Trim();
    }

    public class DayGroup
    {
        public DateTime Date { get; set; }
        public string DayName { get; set; }
        public List<GameDisplay> Games { get; } = new List<GameDisplay>();
    }

    public class GameDisplay
    {
        public GameDisplay(Gameentry schedule, Gamescore score)
        {
            Schedule = schedule;
            Score = score;
        }

        public Gameentry Schedule { get; }
        public Gamescore Score { get; }

        public string AwayScore => !string.IsNullOrWhiteSpace(Score?.awayScore) ? Score.awayScore : "–";
        public string HomeScore => !string.IsNullOrWhiteSpace(Score?.homeScore) ? Score.homeScore : "–";
    }

    public class ParticipantRow
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int WeeklyScore { get; set; }
        public string YtdTotal { get; set; }
        public List<PickResult> Picks { get; } = new List<PickResult>();
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    public class PickResult
    {
        public string RawPick { get; set; }
        public string Abbreviation { get; set; }
        public string CssClass { get; set; }
        public string DisplayText => !string.IsNullOrWhiteSpace(Abbreviation) ? Abbreviation : string.IsNullOrWhiteSpace(RawPick) ? " " : RawPick;
    }
}