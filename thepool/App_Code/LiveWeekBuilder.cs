using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using nflgames;

/// <summary>Which week a live page shows. One of these sits at the top of each live page.</summary>
public class LiveWeekConfig
{
    public int Year;
    public int Week;
    public int SeasonType = 2;              // ESPN: 2 = regular season
    public string SeasonSegment = "";       // "2026-regular"
    public string PicksTable = "";          // "four2026" (bfpooldb)
    public string ScheduleFrom = "";        // yyyyMMdd, first game of the week
    public string ScheduleTo = "";          // yyyyMMdd, last game of the week
    public string ScoresTable = "";         // "2026" (bfscoresdb)
    public string ClassicPage = "";         // file name of the regular results page
}

public class LivePickRow
{
    public string First = string.Empty;
    public string Last = string.Empty;
    public string[] Picks = new string[LiveWeekBuilder.MaxGames];
}

public class LiveSeasonRow
{
    public int Total;
    public int WeekSaved;
}

/// <summary>
/// Builds the data a live results page polls for.
/// The game order and the pick columns come from the regular schedule feed, so game N
/// is still column gameN in the picks table. Scores, clock and possession come from ESPN,
/// matched to each scheduled game by team nickname.
/// </summary>
public static class LiveWeekBuilder
{
    public const int MaxGames = 16;

    private const int ScheduleCacheSeconds = 6 * 60 * 60;
    private const int PicksCacheSeconds = 30;
    private const int SeasonCacheSeconds = 120;

    private static readonly object Gate = new object();
    private static readonly Dictionary<string, CacheEntry> Cache = new Dictionary<string, CacheEntry>();

    private class CacheEntry
    {
        public object Value;
        public DateTime LoadedUtc;
    }

    private class Slot
    {
        public int Column;
        public Gameentry Entry;
        public EspnGame Espn;
        public EspnTeam EspnAway;
        public EspnTeam EspnHome;
        public DateTime? KickoffUtc;
    }

    /// <summary>Loads everything (with short-lived caches) and returns an object ready for JSON.</summary>
    public static object Build(LiveWeekConfig config)
    {
        RequireSafeName(config.PicksTable);
        RequireSafeName(config.ScoresTable);

        var schedule = Cached("schedule:" + config.SeasonSegment + ":" + config.ScheduleFrom + ":" + config.ScheduleTo, ScheduleCacheSeconds, () => LoadSchedule(config));
        var picks = Cached("picks:" + config.PicksTable, PicksCacheSeconds, () => LoadPicks(config.PicksTable));

        Dictionary<string, LiveSeasonRow> season;
        try
        {
            season = Cached("season:" + config.ScoresTable + ":" + config.Week, SeasonCacheSeconds, () => LoadSeason(config.ScoresTable, config.Week));
        }
        catch (Exception)
        {
            // The season table is optional for this page; show picks without season totals.
            season = new Dictionary<string, LiveSeasonRow>();
        }

        var espn = EspnScoreboard.GetWeek(config.Year, config.SeasonType, config.Week);

        return Compose(config, schedule, espn, picks, season, DateTime.UtcNow);
    }

    /// <summary>Pure merge of the four inputs. No network or database access, so it can be tested on its own.</summary>
    public static object Compose(
        LiveWeekConfig config,
        Gameentry[] schedule,
        EspnWeek espn,
        IList<LivePickRow> picks,
        IDictionary<string, LiveSeasonRow> season,
        DateTime nowUtc)
    {
        schedule = schedule ?? new Gameentry[0];
        var espnGames = espn != null ? espn.Games : new List<EspnGame>();

        var slots = new List<Slot>();
        for (int i = 0; i < schedule.Length && i < MaxGames; i++)
        {
            var slot = new Slot { Column = i + 1, Entry = schedule[i] };
            MatchEspn(slot, espnGames);
            slot.KickoffUtc = slot.Espn != null && slot.Espn.KickoffUtc.HasValue ? slot.Espn.KickoffUtc : FeedKickoffUtc(schedule[i]);
            slots.Add(slot);
        }

        // Display order: by kickoff, then by feed position. Picks are re-ordered to match below.
        slots = slots.OrderBy(s => s.KickoffUtc ?? DateTime.MaxValue).ThenBy(s => s.Column).ToList();

        var games = slots.Select(BuildGame).ToList();

        var players = (picks ?? new List<LivePickRow>())
            .OrderBy(p => (p.Last ?? string.Empty).ToUpperInvariant())
            .ThenBy(p => (p.First ?? string.Empty).ToUpperInvariant())
            .Select(row =>
            {
                LiveSeasonRow totals = null;
                if (season != null)
                {
                    season.TryGetValue(NameKey(row.First, row.Last), out totals);
                }

                return new
                {
                    first = (row.First ?? string.Empty).Trim(),
                    last = (row.Last ?? string.Empty).Trim(),
                    picks = slots.Select(slot => PickCode(row.Picks[slot.Column - 1], slot.Entry)).ToArray(),
                    seasonTotal = totals != null ? (int?)totals.Total : null,
                    seasonWeek = totals != null ? (int?)totals.WeekSaved : null
                };
            })
            .ToList();

        bool anyLive = slots.Any(s => StateOf(s) == "in");
        bool allFinal = slots.Count > 0 && slots.All(s => StateOf(s) == "post");

        return new
        {
            year = config.Year,
            week = config.Week,
            generatedUtc = Iso(nowUtc),
            anyLive,
            allFinal,
            pollSeconds = PollSeconds(slots, anyLive, allFinal, nowUtc),
            classicPage = config.ClassicPage ?? string.Empty,
            espn = new
            {
                ok = espn != null && espn.Ok,
                ageSeconds = espn != null && espn.FetchedUtc > DateTime.MinValue ? (int?)Math.Max(0, (int)(nowUtc - espn.FetchedUtc).TotalSeconds) : null,
                error = espn != null && !espn.Ok ? espn.Error : string.Empty
            },
            games,
            players
        };
    }

    // ------------------------------------------------------------------ games

    private static object BuildGame(Slot slot)
    {
        var entry = slot.Entry;
        var espn = slot.Espn;
        string state = StateOf(slot);

        string ballSide = string.Empty;
        object situation = null;

        if (espn != null && state == "in" && espn.HasSituation)
        {
            if (!string.IsNullOrEmpty(espn.PossessionTeamId))
            {
                if (espn.PossessionTeamId == slot.EspnAway.Id) ballSide = "away";
                else if (espn.PossessionTeamId == slot.EspnHome.Id) ballSide = "home";
            }

            int? ballX = BallPosition(espn.PossessionText, slot.EspnAway.Abbr, slot.EspnHome.Abbr);
            bool hasText = !string.IsNullOrWhiteSpace(espn.DownDistanceText) || !string.IsNullOrWhiteSpace(espn.LastPlay) || ballX.HasValue;

            if (hasText)
            {
                situation = new
                {
                    text = espn.DownDistanceText,
                    down = espn.ShortDownDistanceText,
                    spot = espn.PossessionText,
                    x = ballX,                                   // yards from the away team's goal line, 0-100
                    dir = ballSide == "away" ? "r" : ballSide == "home" ? "l" : string.Empty,
                    redZone = espn.IsRedZone,
                    lastPlay = espn.LastPlay
                };
            }
        }

        DateTime? kickoffEastern = slot.KickoffUtc.HasValue ? (DateTime?)ScheduleHelper.UtcToEastern(slot.KickoffUtc.Value) : null;

        return new
        {
            col = slot.Column,
            day = kickoffEastern.HasValue ? kickoffEastern.Value.ToString("ddd", CultureInfo.InvariantCulture) : ScheduleHelper.DayName(entry),
            kickoff = slot.KickoffUtc.HasValue ? Iso(slot.KickoffUtc.Value) : null,
            state,
            status = StatusText(espn, state),
            network = espn != null ? espn.Network : string.Empty,
            away = BuildTeam(entry.awayTeam != null ? entry.awayTeam.Abbreviation : string.Empty, entry.awayTeam != null ? entry.awayTeam.Name : string.Empty, slot.EspnAway, state, ballSide == "away"),
            home = BuildTeam(entry.homeTeam != null ? entry.homeTeam.Abbreviation : string.Empty, entry.homeTeam != null ? entry.homeTeam.Name : string.Empty, slot.EspnHome, state, ballSide == "home"),
            sit = situation
        };
    }

    private static object BuildTeam(string feedAbbr, string feedNickname, EspnTeam espn, string state, bool hasBall)
    {
        string abbr = espn != null && !string.IsNullOrEmpty(espn.Abbr) ? espn.Abbr : (feedAbbr ?? string.Empty);

        return new
        {
            abbr,
            nick = espn != null && !string.IsNullOrEmpty(espn.Nickname) ? espn.Nickname : (feedNickname ?? string.Empty),
            score = espn != null && state != "pre" ? (int?)espn.Score : null,
            record = espn != null ? espn.Record : string.Empty,
            logo = LogoFile(abbr),
            logoAlt = espn != null ? espn.LogoUrl : string.Empty,   // used only when there is no local logo file
            ball = hasBall,
            winner = espn != null && state == "post" && espn.Winner
        };
    }

    private static string StateOf(Slot slot)
    {
        if (slot.Espn == null)
        {
            return "pre";
        }

        return slot.Espn.State == "in" || slot.Espn.State == "post" ? slot.Espn.State : "pre";
    }

    private static readonly Regex ClockAndQuarter = new Regex(@"^(\d{1,2}:\d{2})\s*-\s*(\d)(?:st|nd|rd|th)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex ClockAndOvertime = new Regex(@"^(\d{1,2}:\d{2})\s*-\s*(\d?OT)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>"Q2 3:42", "Halftime", "End of 3rd", "Final", "Final/OT". Empty before kickoff (the page shows the kickoff time).</summary>
    private static string StatusText(EspnGame espn, string state)
    {
        if (espn == null || state == "pre")
        {
            return string.Empty;
        }

        var detail = (espn.ShortDetail ?? string.Empty).Trim();

        if (state == "post")
        {
            return detail.Length > 0 ? detail : "Final";
        }

        var quarter = ClockAndQuarter.Match(detail);
        if (quarter.Success)
        {
            return "Q" + quarter.Groups[2].Value + " " + quarter.Groups[1].Value;
        }

        var overtime = ClockAndOvertime.Match(detail);
        if (overtime.Success)
        {
            return overtime.Groups[2].Value.ToUpperInvariant() + " " + overtime.Groups[1].Value;
        }

        if (detail.Length > 0)
        {
            return detail;
        }

        return espn.Period > 0 ? ("Q" + espn.Period + " " + espn.Clock).Trim() : "In progress";
    }

    private static readonly Regex SpotPattern = new Regex(@"^([A-Za-z]{2,4})\s+(\d{1,2})$", RegexOptions.Compiled);

    /// <summary>
    /// Turns ESPN's "KC 25" into yards from the away team's goal line (0-100),
    /// so the page can draw the ball on a field with the away end zone on the left.
    /// </summary>
    private static int? BallPosition(string possessionText, string awayAbbr, string homeAbbr)
    {
        var text = (possessionText ?? string.Empty).Trim();
        if (text.Length == 0)
        {
            return null;
        }

        if (text == "50")
        {
            return 50;
        }

        var match = SpotPattern.Match(text);
        if (!match.Success)
        {
            return null;
        }

        int yard = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        if (yard < 0 || yard > 50)
        {
            return null;
        }

        var side = match.Groups[1].Value;
        if (string.Equals(side, awayAbbr, StringComparison.OrdinalIgnoreCase))
        {
            return yard;
        }

        if (string.Equals(side, homeAbbr, StringComparison.OrdinalIgnoreCase))
        {
            return 100 - yard;
        }

        return null;
    }

    private static readonly Dictionary<string, string> LogoAliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "lv", "oak" },
        { "la", "lar" }
    };

    /// <summary>
    /// File name (without extension) of the team logo in /thepool/img. Washington has no alias on purpose:
    /// the local was.png is the retired logo, so the page falls back to the logo address ESPN supplies.
    /// </summary>
    private static string LogoFile(string abbr)
    {
        var key = (abbr ?? string.Empty).Trim().ToLowerInvariant();
        string alias;
        return LogoAliases.TryGetValue(key, out alias) ? alias : key;
    }

    private static int PollSeconds(List<Slot> slots, bool anyLive, bool allFinal, DateTime nowUtc)
    {
        if (anyLive)
        {
            return 20;
        }

        if (allFinal || slots.Count == 0)
        {
            return 0; // nothing left to watch
        }

        var next = slots.Where(s => StateOf(s) == "pre" && s.KickoffUtc.HasValue).Select(s => s.KickoffUtc.Value).DefaultIfEmpty(DateTime.MaxValue).Min();

        // Within 20 minutes of a kickoff (or just past it), check often so the start is caught.
        return next != DateTime.MaxValue && (next - nowUtc).TotalMinutes <= 20 ? 30 : 300;
    }

    // --------------------------------------------------------------- matching

    private static void MatchEspn(Slot slot, List<EspnGame> espnGames)
    {
        var entry = slot.Entry;
        if (entry == null || entry.awayTeam == null || entry.homeTeam == null)
        {
            return;
        }

        string away = Key(entry.awayTeam.Name);
        string home = Key(entry.homeTeam.Name);
        string awayFull = Key(entry.awayTeam.City + entry.awayTeam.Name);
        string homeFull = Key(entry.homeTeam.City + entry.homeTeam.Name);

        foreach (var game in espnGames)
        {
            bool awayIsAway = Same(game.Away, away, awayFull);
            bool homeIsHome = Same(game.Home, home, homeFull);
            if (awayIsAway && homeIsHome)
            {
                slot.Espn = game;
                slot.EspnAway = game.Away;
                slot.EspnHome = game.Home;
                return;
            }

            // The two feeds can disagree on who is "home" at a neutral site.
            if (Same(game.Home, away, awayFull) && Same(game.Away, home, homeFull))
            {
                slot.Espn = game;
                slot.EspnAway = game.Home;
                slot.EspnHome = game.Away;
                return;
            }
        }
    }

    private static bool Same(EspnTeam team, string nicknameKey, string fullKey)
    {
        if (team == null)
        {
            return false;
        }

        return (nicknameKey.Length > 0 && Key(team.Nickname) == nicknameKey) || (fullKey.Length > 0 && Key(team.DisplayName) == fullKey);
    }

    private static string Key(string value)
    {
        return new string((value ?? string.Empty).ToLowerInvariant().Where(char.IsLetterOrDigit).ToArray());
    }

    private static string NameKey(string first, string last)
    {
        return (first ?? string.Empty).Trim().ToUpperInvariant() + "|" + (last ?? string.Empty).Trim().ToUpperInvariant();
    }

    /// <summary>"a" = picked the away team, "h" = home, "" = no pick, "?" = text that matches neither team.</summary>
    private static string PickCode(string pick, Gameentry entry)
    {
        var text = (pick ?? string.Empty).Trim();
        if (text.Length == 0 || entry == null)
        {
            return string.Empty;
        }

        if (entry.awayTeam != null && string.Equals(text, (entry.awayTeam.City + " " + entry.awayTeam.Name).Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return "a";
        }

        if (entry.homeTeam != null && string.Equals(text, (entry.homeTeam.City + " " + entry.homeTeam.Name).Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return "h";
        }

        return "?";
    }

    private static DateTime? FeedKickoffUtc(Gameentry entry)
    {
        var date = ScheduleHelper.ParseDate(entry != null ? entry.date : null);
        if (!date.HasValue)
        {
            return null;
        }

        var eastern = date.Value.Date;
        DateTime time;
        var timeText = entry.time ?? string.Empty;
        if (DateTime.TryParse(timeText.Trim(), CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out time))
        {
            eastern = eastern.Add(time.TimeOfDay);
        }
        else
        {
            eastern = eastern.AddHours(13);
        }

        return ScheduleHelper.EasternToUtc(eastern);
    }

    private static string Iso(DateTime utc)
    {
        return utc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
    }

    // ---------------------------------------------------------------- loading

    private static Gameentry[] LoadSchedule(LiveWeekConfig config)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

        var url = CredentialStore.ApiBaseUrl + "/" + config.SeasonSegment + "/full_game_schedule.json?date=from-" + config.ScheduleFrom + "-to-" + config.ScheduleTo;

        using (var client = new WebClient { Credentials = CredentialStore.ApiCredential })
        {
            var schedule = JsonConvert.DeserializeObject<NFLschedule>(client.DownloadString(url));
            var entries = schedule != null && schedule.fullgameschedule != null ? schedule.fullgameschedule.gameentry : null;
            if (entries == null || entries.Length == 0)
            {
                throw new InvalidOperationException("The schedule feed returned no games for " + config.ScheduleFrom + " to " + config.ScheduleTo + ".");
            }

            return entries;
        }
    }

    private static List<LivePickRow> LoadPicks(string table)
    {
        var rows = new List<LivePickRow>();

        using (var conn = new MySqlConnection(CredentialStore.PoolConnectionString))
        {
            conn.Open();
            using (var command = new MySqlCommand("SELECT * FROM `" + table + "` ORDER BY UPPER(lastname), UPPER(firstname)", conn))
            using (var reader = command.ExecuteReader())
            {
                var ordinals = new int[MaxGames];
                for (int i = 0; i < MaxGames; i++)
                {
                    try
                    {
                        ordinals[i] = reader.GetOrdinal("game" + (i + 1));
                    }
                    catch (IndexOutOfRangeException)
                    {
                        ordinals[i] = -1;
                    }
                }

                while (reader.Read())
                {
                    var row = new LivePickRow
                    {
                        First = reader["firstname"].ToString(),
                        Last = reader["lastname"].ToString()
                    };

                    for (int i = 0; i < MaxGames; i++)
                    {
                        row.Picks[i] = ordinals[i] >= 0 && !reader.IsDBNull(ordinals[i]) ? reader.GetValue(ordinals[i]).ToString() : string.Empty;
                    }

                    rows.Add(row);
                }
            }
        }

        return rows;
    }

    private static Dictionary<string, LiveSeasonRow> LoadSeason(string table, int week)
    {
        var rows = new Dictionary<string, LiveSeasonRow>();

        using (var conn = new MySqlConnection(CredentialStore.ScoresConnectionString))
        {
            conn.Open();
            var sql = "SELECT firstname, lastname, IFNULL(total, 0) AS total, IFNULL(`week" + week + "`, 0) AS weekSaved FROM `" + table + "`";
            using (var command = new MySqlCommand(sql, conn))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    rows[NameKey(reader["firstname"].ToString(), reader["lastname"].ToString())] = new LiveSeasonRow
                    {
                        Total = Convert.ToInt32(reader["total"], CultureInfo.InvariantCulture),
                        WeekSaved = Convert.ToInt32(reader["weekSaved"], CultureInfo.InvariantCulture)
                    };
                }
            }
        }

        return rows;
    }

    private static void RequireSafeName(string table)
    {
        if (string.IsNullOrEmpty(table) || !Regex.IsMatch(table, "^[A-Za-z0-9_]+$"))
        {
            throw new ArgumentException("Unsafe table name: " + table);
        }
    }

    /// <summary>Small time-based cache. If a reload fails and an older value exists, the older value is kept.</summary>
    private static T Cached<T>(string key, int seconds, Func<T> load) where T : class
    {
        lock (Gate)
        {
            CacheEntry entry;
            Cache.TryGetValue(key, out entry);

            if (entry != null && (DateTime.UtcNow - entry.LoadedUtc).TotalSeconds < seconds)
            {
                return (T)entry.Value;
            }

            try
            {
                var value = load();
                Cache[key] = new CacheEntry { Value = value, LoadedUtc = DateTime.UtcNow };
                return value;
            }
            catch (Exception)
            {
                if (entry != null)
                {
                    return (T)entry.Value;
                }

                throw;
            }
        }
    }
}
