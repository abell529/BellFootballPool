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

public partial class _2025_06_sixthwk : Page
{
    private const string SeasonSegment = "2025-regular";

    public string ScheduleUrl = $"{CredentialStore.ApiBaseUrl}/{SeasonSegment}/full_game_schedule.json?date=from-20251009-to-20251013";

    public NFLschedule Schedule { get; private set; }
    public List<DayGroup> DayGroups { get; } = new List<DayGroup>();
    public List<ParticipantRow> Participants { get; } = new List<ParticipantRow>();
    public int NumberOfGames { get; private set; }

    public List<string> theFirstName = new List<string>();
    public List<string> theLastName = new List<string>();
    public List<string> theTotal = new List<string>();

    private readonly Dictionary<string, Gamescore> _scoresByGameId = new Dictionary<string, Gamescore>(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<DateTime, IReadOnlyList<Gamescore>> _scoresByDate = new Dictionary<DateTime, IReadOnlyList<Gamescore>>();
    private readonly List<GameDisplay> _orderedGames = new List<GameDisplay>();

    private static readonly int[] ScoreboardSearchOffsets = new[] { -1, 1, -2, 2 };
    private static readonly TimeZoneInfo EasternTimeZone = InitializeEasternTimeZone();

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

            foreach (var item in group.OrderBy(x => x.Entry.time))
            {
                var score = FindGameScore(client, item.Entry, group.Key);
                dayGroup.Games.Add(new GameDisplay(item.Entry, score));
            }

            DayGroups.Add(dayGroup);
        }
    }

    private IReadOnlyList<Gamescore> GetScoresForDate(WebClient client, DateTime date)
    {
        var dateKey = date.Date;

        if (_scoresByDate.TryGetValue(dateKey, out var cached))
        {
            return cached;
        }

        var formattedDate = dateKey.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var url = $"{CredentialStore.ApiBaseUrl}/{SeasonSegment}/scoreboard.json?fordate={formattedDate}";

        try
        {
            var response = client.DownloadString(url);
            var scoreboard = JsonConvert.DeserializeObject<LiveScoring>(response);
            var games = scoreboard?.scoreboard?.gameScore?.Where(g => g?.game?.ID != null).ToList() ?? new List<Gamescore>();
            _scoresByDate[dateKey] = games;
            return games;
        }
        catch (WebException)
        {
            _scoresByDate[dateKey] = new List<Gamescore>();
            return _scoresByDate[dateKey];
        }
    }

    private Gamescore FindGameScore(WebClient client, Gameentry entry, DateTime scheduledDate)
    {
        if (entry == null || string.IsNullOrWhiteSpace(entry.id))
        {
            return null;
        }

        if (_scoresByGameId.TryGetValue(entry.id, out var cached))
        {
            return cached;
        }

        var baseDate = scheduledDate.Date;
        var searchOffsets = new int[ScoreboardSearchOffsets.Length + 1];
        searchOffsets[0] = 0;
        Array.Copy(ScoreboardSearchOffsets, 0, searchOffsets, 1, ScoreboardSearchOffsets.Length);

        foreach (var offset in searchOffsets)
        {
            var targetDate = baseDate.AddDays(offset);
            var scores = GetScoresForDate(client, targetDate);
            var match = scores.FirstOrDefault(g => string.Equals(g?.game?.ID, entry.id, StringComparison.OrdinalIgnoreCase));

            if (match != null)
            {
                _scoresByGameId[entry.id] = match;
                return match;
            }
        }

        _scoresByGameId[entry.id] = null;
        return null;
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
            var commandText = "SELECT * FROM `six2025` ORDER BY UPPER(lastname), UPPER(firstname)";
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

    private static readonly DayOfWeek[] AutoWinDays =
    {
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday
    };

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

            if (IsAutoWinGame(scheduleEntry))
            {
                score++;
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

        bool autoWin = IsAutoWinGame(scheduleEntry) && !string.IsNullOrEmpty(trimmedPick);

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

        if (autoWin && isCompleted)
        {
            pickResult.CssClass = "win";
            return pickResult;
        }

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

        var trimmed = value.Trim();
        bool hasTimeComponent = trimmed.IndexOf('T') >= 0 || trimmed.IndexOf(':') >= 0;

        if (hasTimeComponent && DateTimeOffset.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out var parsedOffset))
        {
            var eastern = TimeZoneInfo.ConvertTime(parsedOffset, EasternTimeZone);
            return eastern.DateTime;
        }

        if (DateTime.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsedInvariant))
        {
            return parsedInvariant;
        }

        if (DateTime.TryParse(trimmed, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out var parsedUs))
        {
            return parsedUs;
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

    private static bool IsAutoWinGame(Gameentry scheduleEntry)
    {
        var gameDate = ParseDate(scheduleEntry?.date);
        if (!gameDate.HasValue)
        {
            return false;
        }

        return AutoWinDays.Contains(gameDate.Value.DayOfWeek);
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

        public string AwayScore => FormatScore(Score?.awayScore);
        public string HomeScore => FormatScore(Score?.homeScore);

        private static string FormatScore(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
        }
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

    private static TimeZoneInfo InitializeEasternTimeZone()
    {
        var timeZoneIds = new[] { "Eastern Standard Time", "America/New_York" };

        foreach (var id in timeZoneIds)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return TimeZoneInfo.Utc;
    }
}
