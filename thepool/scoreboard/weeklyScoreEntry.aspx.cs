using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using nflgames;
using livescoring;

public partial class scoreboard_weeklyScoreEntry : Page
{
    private const string SeasonSegment = "2025-regular";
    private const string ScoresTableName = "`2025`";
    private const string ParticipantScoresViewStateKey = "ParticipantScores";
    private const string WeekColumnViewStateKey = "WeekColumnName";
    private const string WeekNumberViewStateKey = "WeekNumber";

    private static readonly DayOfWeek[] AutoWinDays =
    {
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WeekNumberTextBox.Text = "5";
            PicksTableTextBox.Text = "five2025";
            StartDateTextBox.Text = "2025-10-02";
            EndDateTextBox.Text = "2025-10-06";
        }
    }

    protected void LoadButton_Click(object sender, EventArgs e)
    {
        ClearStatus();
        ResultsGrid.DataSource = null;
        ResultsGrid.DataBind();
        GridWrapper.Visible = false;
        SaveButton.Enabled = false;

        if (!TryReadInputs(out var inputs, out var error))
        {
            ShowError(error);
            return;
        }

        try
        {
            var schedule = FetchSchedule(inputs.ScheduleUrl);
            var entries = schedule?.fullgameschedule?.gameentry;

            if (entries == null || entries.Length == 0)
            {
                ShowError("No games were found for the supplied date range.");
                return;
            }

            var scoresByGameId = FetchScores(entries, inputs.StartDate, inputs.EndDate);
            var participants = LoadPicks(inputs.PicksTableName, entries.Length);
            var scoreboardTotals = LoadScoreboardTotals(inputs.WeekNumber);

            var participantScores = BuildParticipantScores(entries, scoresByGameId, participants, scoreboardTotals, inputs.WeekNumber);

            ViewState[ParticipantScoresViewStateKey] = participantScores;
            ViewState[WeekColumnViewStateKey] = $"week{inputs.WeekNumber}";
            ViewState[WeekNumberViewStateKey] = inputs.WeekNumber;

            ResultsGrid.DataSource = participantScores;
            ResultsGrid.DataBind();
            GridWrapper.Visible = true;

            bool hasMissing = participantScores.Any(p => p.IsMissingFromScores);
            SaveButton.Enabled = participantScores.Count > 0 && !hasMissing;

            if (hasMissing)
            {
                ShowError("Week data loaded, but some participants are missing from the 2025 scores table.");
            }
            else
            {
                ShowSuccess($"Loaded {participantScores.Count} participant scores for week {inputs.WeekNumber}.");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to load data: {ex.Message}");
        }
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        ClearStatus();

        var scores = ViewState[ParticipantScoresViewStateKey] as List<ParticipantScore>;
        var weekColumn = ViewState[WeekColumnViewStateKey] as string;
        var weekNumberObj = ViewState[WeekNumberViewStateKey];
        int? weekNumber = weekNumberObj is int value ? value : (int?)null;

        if (scores == null || string.IsNullOrEmpty(weekColumn) || !weekNumber.HasValue)
        {
            ShowError("Load week data before attempting to save.");
            return;
        }

        if (scores.Any(p => p.IsMissingFromScores))
        {
            ShowError("Cannot save because some participants are missing from the 2025 scores table.");
            return;
        }

        try
        {
            UpdateScoreboard(scores, weekColumn);
            ShowSuccess($"Updated {scores.Count} rows for week {weekNumber.Value}.");
            SaveButton.Enabled = false;
        }
        catch (Exception ex)
        {
            ShowError($"Failed to save scores: {ex.Message}");
        }
    }

    private bool TryReadInputs(out WeekInputs inputs, out string error)
    {
        inputs = null;
        error = null;

        if (!int.TryParse(WeekNumberTextBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int weekNumber))
        {
            error = "Enter a valid numeric week.";
            return false;
        }

        if (weekNumber < 1 || weekNumber > 18)
        {
            error = "Week number must be between 1 and 18.";
            return false;
        }

        if (!DateTime.TryParseExact(StartDateTextBox.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate))
        {
            error = "Enter a valid start date in YYYY-MM-DD format.";
            return false;
        }

        if (!DateTime.TryParseExact(EndDateTextBox.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate))
        {
            error = "Enter a valid end date in YYYY-MM-DD format.";
            return false;
        }

        if (endDate < startDate)
        {
            error = "End date must be on or after the start date.";
            return false;
        }

        var picksTable = PicksTableTextBox.Text.Trim();
        if (!IsSafeTableName(picksTable))
        {
            error = "Enter a valid picks table name (letters and numbers only).";
            return false;
        }

        inputs = new WeekInputs
        {
            WeekNumber = weekNumber,
            PicksTableName = picksTable,
            StartDate = startDate,
            EndDate = endDate,
            ScheduleUrl = string.Format(CultureInfo.InvariantCulture,
                "{0}/{1}/full_game_schedule.json?date=from-{2:yyyyMMdd}-to-{3:yyyyMMdd}",
                CredentialStore.ApiBaseUrl,
                SeasonSegment,
                startDate,
                endDate)
        };

        return true;
    }

    private static bool IsSafeTableName(string value)
    {
        return Regex.IsMatch(value ?? string.Empty, "^[a-zA-Z0-9_]+$");
    }

    private static NFLschedule FetchSchedule(string scheduleUrl)
    {
        using (var client = new WebClient { Credentials = CredentialStore.ApiCredential })
        {
            var response = client.DownloadString(scheduleUrl);
            return JsonConvert.DeserializeObject<NFLschedule>(response);
        }
    }

    private static Dictionary<string, Gamescore> FetchScores(IEnumerable<Gameentry> entries, DateTime startDate, DateTime endDate)
    {
        var result = new Dictionary<string, Gamescore>(StringComparer.OrdinalIgnoreCase);
        var dates = new HashSet<DateTime>();

        foreach (var entry in entries ?? Enumerable.Empty<Gameentry>())
        {
            var date = ParseDate(entry?.date)?.Date;
            if (date.HasValue)
            {
                dates.Add(date.Value);
            }
        }

        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            dates.Add(date);
        }

        using (var client = new WebClient { Credentials = CredentialStore.ApiCredential })
        {
            foreach (var date in dates.OrderBy(d => d))
            {
                var url = string.Format(CultureInfo.InvariantCulture,
                    "{0}/{1}/scoreboard.json?fordate={2:yyyyMMdd}",
                    CredentialStore.ApiBaseUrl,
                    SeasonSegment,
                    date);

                try
                {
                    var response = client.DownloadString(url);
                    var scoreboard = JsonConvert.DeserializeObject<LiveScoring>(response);

                    if (scoreboard?.scoreboard?.gameScore == null)
                    {
                        continue;
                    }

                    foreach (var game in scoreboard.scoreboard.gameScore)
                    {
                        var id = game?.game?.ID;
                        if (!string.IsNullOrEmpty(id) && !result.ContainsKey(id))
                        {
                            result[id] = game;
                        }
                    }
                }
                catch (WebException)
                {
                    // Ignore missing days
                }
            }
        }

        return result;
    }

    private static List<ParticipantPicks> LoadPicks(string tableName, int expectedGames)
    {
        var results = new List<ParticipantPicks>();
        var connectionString = CredentialStore.PoolConnectionString;
        var commandText = string.Format(CultureInfo.InvariantCulture,
            "SELECT * FROM `{0}` ORDER BY UPPER(lastname), UPPER(firstname)",
            tableName);

        using (var connection = new MySqlConnection(connectionString))
        using (var command = new MySqlCommand(commandText, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var picks = new List<string>(expectedGames);
                    for (int i = 0; i < expectedGames; i++)
                    {
                        var column = $"game{i + 1}";
                        if (TryGetColumnValue(reader, column, out var rawValue))
                        {
                            picks.Add(rawValue?.ToString() ?? string.Empty);
                        }
                        else
                        {
                            picks.Add(string.Empty);
                        }
                    }

                    results.Add(new ParticipantPicks
                    {
                        FirstName = reader["firstname"].ToString(),
                        LastName = reader["lastname"].ToString(),
                        Picks = picks
                    });
                }
            }
        }

        return results;
    }

    private static bool TryGetColumnValue(MySqlDataReader reader, string columnName, out object value)
    {
        try
        {
            value = reader[columnName];
            return true;
        }
        catch (IndexOutOfRangeException)
        {
            value = null;
            return false;
        }
    }

    private static Dictionary<string, ScoreboardEntry> LoadScoreboardTotals(int weekNumber)
    {
        var results = new Dictionary<string, ScoreboardEntry>(StringComparer.OrdinalIgnoreCase);
        var connectionString = CredentialStore.ScoresConnectionString;
        var weekColumn = $"week{weekNumber}";
        var commandText = string.Format(CultureInfo.InvariantCulture,
            "SELECT firstname, lastname, IFNULL(total, 0) AS total, IFNULL(`{0}`, 0) AS weekScore FROM {1}",
            weekColumn,
            ScoresTableName);

        using (var connection = new MySqlConnection(connectionString))
        using (var command = new MySqlCommand(commandText, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var key = BuildParticipantKey(reader["firstname"].ToString(), reader["lastname"].ToString());
                    var entry = new ScoreboardEntry
                    {
                        FirstName = reader["firstname"].ToString(),
                        LastName = reader["lastname"].ToString(),
                        Total = Convert.ToInt32(reader["total"], CultureInfo.InvariantCulture),
                        WeekScore = Convert.ToInt32(reader["weekScore"], CultureInfo.InvariantCulture)
                    };

                    results[key] = entry;
                }
            }
        }

        return results;
    }

    private static List<ParticipantScore> BuildParticipantScores(
        Gameentry[] scheduleEntries,
        IDictionary<string, Gamescore> scoresByGameId,
        IEnumerable<ParticipantPicks> participants,
        IReadOnlyDictionary<string, ScoreboardEntry> scoreboardTotals,
        int weekNumber)
    {
        var results = new List<ParticipantScore>();

        foreach (var participant in participants)
        {
            var weeklyScore = CalculateScore(scheduleEntries, scoresByGameId, participant.Picks);
            var key = BuildParticipantKey(participant.FirstName, participant.LastName);
            scoreboardTotals.TryGetValue(key, out var scoreboardEntry);

            int previousTotal = scoreboardEntry?.Total ?? 0;
            int existingWeek = scoreboardEntry?.WeekScore ?? 0;
            bool missingFromScores = scoreboardEntry == null;
            int adjustedTotal = previousTotal - existingWeek + weeklyScore;

            results.Add(new ParticipantScore
            {
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                WeeklyScore = weeklyScore,
                ExistingWeekScore = existingWeek,
                PreviousTotal = previousTotal,
                NewTotal = adjustedTotal,
                WeekNumber = weekNumber,
                IsMissingFromScores = missingFromScores
            });
        }

        return results;
    }

    private static int CalculateScore(Gameentry[] scheduleEntries, IDictionary<string, Gamescore> scoresByGameId, IList<string> picks)
    {
        int score = 0;

        for (int i = 0; i < scheduleEntries.Length && i < picks.Count; i++)
        {
            var scheduleEntry = scheduleEntries[i];
            if (scheduleEntry == null || string.IsNullOrEmpty(scheduleEntry.id))
            {
                continue;
            }

            if (!scoresByGameId.TryGetValue(scheduleEntry.id, out var gameScore))
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

    private static void UpdateScoreboard(IEnumerable<ParticipantScore> scores, string weekColumn)
    {
        var connectionString = CredentialStore.ScoresConnectionString;
        var commandText = string.Format(CultureInfo.InvariantCulture,
            "UPDATE {0} SET `{1}` = @weekScore, total = @total WHERE firstname = @first AND lastname = @last",
            ScoresTableName,
            weekColumn);

        using (var connection = new MySqlConnection(connectionString))
        using (var command = new MySqlCommand(commandText, connection))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                command.Transaction = transaction;

                foreach (var score in scores)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@weekScore", score.WeeklyScore);
                    command.Parameters.AddWithValue("@total", score.NewTotal);
                    command.Parameters.AddWithValue("@first", score.FirstName.Trim());
                    command.Parameters.AddWithValue("@last", score.LastName.Trim());

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }
    }

    private void ShowError(string message)
    {
        StatusMessage.Text = $"<span class='error'>{Server.HtmlEncode(message)}</span>";
    }

    private void ShowSuccess(string message)
    {
        StatusMessage.Text = $"<span class='success'>{Server.HtmlEncode(message)}</span>";
    }

    private void ClearStatus()
    {
        StatusMessage.Text = string.Empty;
    }

    private static bool IsAutoWinGame(Gameentry scheduleEntry)
    {
        var date = ParseDate(scheduleEntry?.date);
        return date.HasValue && AutoWinDays.Contains(date.Value.DayOfWeek);
    }

    private static DateTime? ParseDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsed))
        {
            return parsed;
        }

        if (DateTime.TryParse(value, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out parsed))
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

    private static string BuildParticipantKey(string firstName, string lastName)
    {
        return string.Format(CultureInfo.InvariantCulture, "{0}|{1}", firstName?.Trim() ?? string.Empty, lastName?.Trim() ?? string.Empty);
    }

    [Serializable]
    private class WeekInputs
    {
        public int WeekNumber { get; set; }
        public string PicksTableName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ScheduleUrl { get; set; }
    }

    [Serializable]
    private class ParticipantPicks
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Picks { get; set; }
    }

    [Serializable]
    private class ScoreboardEntry
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Total { get; set; }
        public int WeekScore { get; set; }
    }

    [Serializable]
    private class ParticipantScore
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int WeeklyScore { get; set; }
        public int ExistingWeekScore { get; set; }
        public int PreviousTotal { get; set; }
        public int NewTotal { get; set; }
        public int WeekNumber { get; set; }
        public bool IsMissingFromScores { get; set; }
        public string FullName => string.Format(CultureInfo.InvariantCulture, "{0} {1}", FirstName, LastName).Trim();
    }
}
