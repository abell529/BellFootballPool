using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
        DayGroups.Clear();
        _orderedGames.Clear();
        _scoresByGameId.Clear();
        _scoresByDate.Clear();

        if (Schedule?.fullgameschedule?.gameentry == null)
        {
            NumberOfGames = 0;
            return;
        }

        var games = Schedule.fullgameschedule.gameentry
            .Select((entry, index) =>
            {
                var displayDate = ParseDate(entry.date, entry.time) ?? DateTime.MinValue;
                var startTime = ParseGameTime(entry.time);

                return new GameDisplay(entry, index)
                {
                    DisplayDate = displayDate,
                    StartTime = startTime
                };
            })
            .OrderBy(g => g.DisplayDate)
            .ThenBy(g => g.StartTime ?? TimeSpan.MaxValue)
            .ThenBy(g => g.Index)
            .ToList();

        if (games.Count == 0)
        {
            NumberOfGames = 0;
            return;
        }

        foreach (var date in games.Select(g => g.DisplayDate).Where(IsValidDate).Select(d => d.Date).Distinct())
        {
            CacheScores(client, date);
        }

        foreach (var game in games)
        {
            if (!_scoresByGameId.TryGetValue(game.Schedule.id, out var score))
            {
                foreach (var offset in ScoreboardSearchOffsets)
                {
                    if (!IsValidDate(game.DisplayDate))
                    {
                        continue;
                    }

                    var targetDate = game.DisplayDate.Date.AddDays(offset);
                    if (!IsValidDate(targetDate))
                    {
                        continue;
                    }

                    CacheScores(client, targetDate);

                    if (_scoresByGameId.TryGetValue(game.Schedule.id, out score))
                    {
                        break;
                    }
                }
            }

            if (score != null)
            {
                game.AttachScore(score);
            }
        }

        games = games
            .OrderBy(g => g.DisplayDate)
            .ThenBy(g => g.StartTime ?? TimeSpan.MaxValue)
            .ThenBy(g => g.Index)
            .ToList();

        foreach (var group in games
            .GroupBy(g => IsValidDate(g.DisplayDate) ? g.DisplayDate.Date : DateTime.MinValue)
            .OrderBy(g => g.Key))
        {
            var dayGroup = new DayGroup
            {
                Date = group.Key,
                DayName = IsValidDate(group.Key)
                    ? group.Key.ToString("dddd", CultureInfo.InvariantCulture)
                    : "TBD"
            };

            dayGroup.Games.AddRange(group
                .OrderBy(g => g.DisplayDate)
                .ThenBy(g => g.StartTime ?? TimeSpan.MaxValue)
                .ThenBy(g => g.Index));

            DayGroups.Add(dayGroup);
        }

        _orderedGames.AddRange(games);
        NumberOfGames = _orderedGames.Count;
    }

    private IReadOnlyList<Gamescore> CacheScores(WebClient client, DateTime date)
    {
        if (!IsValidDate(date))
        {
            return Array.Empty<Gamescore>();
        }

        var normalized = date.Date;

        if (_scoresByDate.TryGetValue(normalized, out var cached))
        {
            return cached;
        }

        var formattedDate = normalized.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var url = $"{CredentialStore.ApiBaseUrl}/{SeasonSegment}/scoreboard.json?fordate={formattedDate}";

        try
        {
            var response = client.DownloadString(url);
            var scoreboard = JsonConvert.DeserializeObject<LiveScoring>(response);
            var games = scoreboard?.scoreboard?.gameScore?.Where(g => g?.game?.ID != null).ToList() ?? new List<Gamescore>();

            _scoresByDate[normalized] = games;

            foreach (var score in games)
            {
                if (!string.IsNullOrEmpty(score?.game?.ID))
                {
                    _scoresByGameId[score.game.ID] = score;
                }
            }

            return games;
        }
        catch (WebException)
        {
            _scoresByDate[normalized] = new List<Gamescore>();
            return _scoresByDate[normalized];
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

                        if (i < _orderedGames.Count)
                        {
                            var scheduleEntry = _orderedGames[i].Schedule;
                            _scoresByGameId.TryGetValue(scheduleEntry.id, out var gameScore);
                            participant.Picks.Add(BuildPickResult(pickValue, scheduleEntry, gameScore));
                        }
                        else
                        {
                            participant.Picks.Add(new PickResult { RawPick = pickValue });
                        }
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
            if (i >= _orderedGames.Count)
            {
                break;
            }

            var scheduleEntry = _orderedGames[i].Schedule;
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

    private static DateTime? ParseDate(string value, string time = null)
    {
        if (TryParseWithTimeZone(value, time, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static bool TryParseWithTimeZone(string dateValue, string timeValue, out DateTime parsed)
    {
        parsed = default;

        if (string.IsNullOrWhiteSpace(dateValue))
        {
            return false;
        }

        string candidate = dateValue.Trim();
        if (candidate.Length <= 10 && !string.IsNullOrWhiteSpace(timeValue))
        {
            candidate = $"{candidate} {timeValue.Trim()}";
        }

        if (DateTimeOffset.TryParse(candidate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dto))
        {
            parsed = ConvertToEasternDate(dto).Date;
            return true;
        }

        if (DateTime.TryParse(candidate, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dt))
        {
            parsed = dt.Date;
            return true;
        }

        var datePortion = candidate.Length >= 10 ? candidate.Substring(0, 10) : candidate;
        if (DateTime.TryParseExact(datePortion, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var exact))
        {
            parsed = exact.Date;
            return true;
        }

        return false;
    }

    private static int ParseScore(string value)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : 0;
    }

    private static string FormatScoreDisplay(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim();
        if (trimmed == "-" || trimmed == "–" || trimmed == "—")
        {
            return string.Empty;
        }

        if (int.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed.ToString(CultureInfo.InvariantCulture);
        }

        return trimmed;
    }

    private static TimeSpan? ParseGameTime(string timeValue)
    {
        if (string.IsNullOrWhiteSpace(timeValue))
        {
            return null;
        }

        var formats = new[] { "h:mm tt", "hh:mm tt", "h:mm:ss tt", "hh:mm:ss tt" };
        if (DateTime.TryParseExact(timeValue.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsed))
        {
            return parsed.TimeOfDay;
        }

        if (DateTime.TryParse(timeValue, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out parsed))
        {
            return parsed.TimeOfDay;
        }

        return null;
    }

    private static bool IsValidDate(DateTime date)
    {
        return date.Year > 1900;
    }

    private static DateTime ConvertToEasternDate(DateTimeOffset dto)
    {
        if (EasternTimeZone != null)
        {
            return TimeZoneInfo.ConvertTime(dto, EasternTimeZone);
        }

        return dto.LocalDateTime;
    }

    private static TimeZoneInfo InitializeEasternTimeZone()
    {
        string[] candidates = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new[] { "Eastern Standard Time" }
            : new[] { "America/New_York", "US/Eastern" };

        foreach (var id in candidates)
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

        return null;
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
        public GameDisplay(Gameentry schedule, int index)
        {
            Schedule = schedule;
            Index = index;
        }

        public Gameentry Schedule { get; }
        public int Index { get; }
        public Gamescore Score { get; private set; }
        public DateTime DisplayDate { get; set; }
        public TimeSpan? StartTime { get; set; }

        public string AwayScore => FormatScoreDisplay(Score?.awayScore);
        public string HomeScore => FormatScoreDisplay(Score?.homeScore);

        public void AttachScore(Gamescore score)
        {
            Score = score;

            if (score?.game != null)
            {
                var scoreboardDate = ParseDate(score.game.date, score.game.time);
                if (scoreboardDate.HasValue)
                {
                    DisplayDate = scoreboardDate.Value;
                }

                if (!StartTime.HasValue)
                {
                    StartTime = ParseGameTime(score.game.time);
                }
            }
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
}
