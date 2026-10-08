using System;

/// <summary>
/// The one place to change each week. The pick form, the Thursday pick page,
/// the confirmation email and the duplicate-cleanup page all read from here.
/// The per-week results page under /2026 keeps its own copy of these values.
/// </summary>
public static class CurrentWeek
{
    public const int Year = 2026;
    public const int Number = 5;
    public const string SeasonSegment = "2026-regular";

    // Picks table for this week in bfpooldb (one2026, two2026, ...).
    public const string PicksTable = "five2026";

    // Full week, first game through last game, yyyyMMdd. Games dated before Sunday
    // are picked on thursday-picks.aspx; the rest on Default.aspx.
    public const string ScheduleFrom = "20261008";
    public const string ScheduleTo = "20261012";

    // Where the pick form sends people after they submit, and the link in the email.
    public const string ResultsPage = "/football/thepool/2026/05-the5thweek26-live.aspx";

    // Cutoffs, EASTERN time, "yyyy-MM-dd HH:mm" (24-hour). Both are checked on the server,
    // so a bookmarked link cannot get around them. Kickoff plus 15 minutes for stragglers.
    // (The football page index.asp has its own copies of these in CENTRAL time.)
    public const string EarlyPicksClose = "2026-10-08 20:30";   // Thursday game(s)
    public const string PicksClose = "2026-10-11 13:15";        // Sunday/Monday games

    public static bool EarlyPicksClosed => ScheduleHelper.IsPastEastern(EarlyPicksClose);
    public static bool PicksClosed => ScheduleHelper.IsPastEastern(PicksClose);
    public static string EarlyPicksCloseText => ScheduleHelper.DescribeEastern(EarlyPicksClose);
    public static string PicksCloseText => ScheduleHelper.DescribeEastern(PicksClose);

    // Standings shown in the sidebar of the pick form (table in bfscoresdb).
    public const string StandingsTable = "2026";
    public const string StandingsLabel = "2026 Current Standings";

    public static string Label => $"Week {Number} {Year}";

    public static string ScheduleUrl =>
        $"{CredentialStore.ApiBaseUrl}/{SeasonSegment}/full_game_schedule.json?date=from-{ScheduleFrom}-to-{ScheduleTo}";
}
