using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

/// <summary>One team's side of a game as ESPN reports it.</summary>
public class EspnTeam
{
    public string Id = string.Empty;
    public string Abbr = string.Empty;
    public string Location = string.Empty;
    public string Nickname = string.Empty;
    public string DisplayName = string.Empty;
    public string Record = string.Empty;
    public string LogoUrl = string.Empty;
    public int Score;
    public bool Winner;
}

/// <summary>One game from ESPN's public scoreboard, flattened to what the pool pages need.</summary>
public class EspnGame
{
    public string Id = string.Empty;
    public DateTime? KickoffUtc;

    // "pre", "in" or "post"
    public string State = "pre";
    public int Period;
    public string Clock = string.Empty;
    public string ShortDetail = string.Empty;
    public bool Completed;
    public string Network = string.Empty;

    public EspnTeam Away = new EspnTeam();
    public EspnTeam Home = new EspnTeam();

    // Only filled in while a game is in progress.
    public bool HasSituation;
    public string PossessionTeamId = string.Empty;
    public string DownDistanceText = string.Empty;       // "1st & 10 at KC 25"
    public string ShortDownDistanceText = string.Empty;  // "1st & 10"
    public string PossessionText = string.Empty;         // "KC 25"
    public bool IsRedZone;
    public string LastPlay = string.Empty;
}

public class EspnWeek
{
    public List<EspnGame> Games = new List<EspnGame>();
    public DateTime FetchedUtc = DateTime.MinValue;
    public bool Ok;
    public string Error = string.Empty;
}

/// <summary>
/// Reads ESPN's public NFL scoreboard (the feed espn.com itself uses; no key needed).
/// Results are shared by every visitor and refreshed at most once every CacheSeconds,
/// so ESPN sees a handful of requests a minute however many people have a page open.
/// The feed is unofficial: if it changes or goes away, GetWeek returns the last good
/// answer with Ok = false instead of throwing.
/// </summary>
public static class EspnScoreboard
{
    public const int CacheSeconds = 15;
    private const int RetryAfterFailureSeconds = 10;
    private const string BaseUrl = "https://site.api.espn.com/apis/site/v2/sports/football/nfl/scoreboard";

    private static readonly object Gate = new object();
    private static readonly Dictionary<string, EspnWeek> LastGood = new Dictionary<string, EspnWeek>();
    private static readonly Dictionary<string, DateTime> LastAttemptUtc = new Dictionary<string, DateTime>();
    private static readonly Dictionary<string, string> LastError = new Dictionary<string, string>();

    /// <param name="seasonType">1 = preseason, 2 = regular season, 3 = playoffs.</param>
    public static EspnWeek GetWeek(int year, int seasonType, int week)
    {
        var key = year + "-" + seasonType + "-" + week;

        lock (Gate)
        {
            var now = DateTime.UtcNow;
            EspnWeek good;
            LastGood.TryGetValue(key, out good);

            if (good != null && (now - good.FetchedUtc).TotalSeconds < CacheSeconds)
            {
                return good;
            }

            DateTime lastAttempt;
            if (LastAttemptUtc.TryGetValue(key, out lastAttempt) && (now - lastAttempt).TotalSeconds < RetryAfterFailureSeconds)
            {
                string error;
                LastError.TryGetValue(key, out error);
                return Stale(good, error ?? "ESPN request failed recently.");
            }

            LastAttemptUtc[key] = now;

            try
            {
                var url = BaseUrl + "?seasontype=" + seasonType + "&week=" + week + "&dates=" + year;
                var fresh = new EspnWeek
                {
                    Games = Parse(Download(url)),
                    FetchedUtc = DateTime.UtcNow,
                    Ok = true
                };

                LastGood[key] = fresh;
                LastAttemptUtc.Remove(key);
                LastError.Remove(key);
                return fresh;
            }
            catch (Exception ex)
            {
                LastError[key] = ex.Message;
                return Stale(good, ex.Message);
            }
        }
    }

    private static EspnWeek Stale(EspnWeek good, string error)
    {
        return new EspnWeek
        {
            Games = good != null ? good.Games : new List<EspnGame>(),
            FetchedUtc = good != null ? good.FetchedUtc : DateTime.MinValue,
            Ok = false,
            Error = error
        };
    }

    private static string Download(string url)
    {
        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Accept = "application/json";
        request.UserAgent = "Mozilla/5.0 (compatible; BellFootballPool/1.0)";
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        request.Timeout = 8000;
        request.ReadWriteTimeout = 8000;

        using (var response = (HttpWebResponse)request.GetResponse())
        using (var stream = response.GetResponseStream())
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    /// <summary>Turns the raw scoreboard JSON into games. Every field is optional; missing ones stay at their defaults.</summary>
    public static List<EspnGame> Parse(string json)
    {
        var games = new List<EspnGame>();

        // Keep dates as the raw ISO text ESPN sent, so parsing never depends on the server's regional settings.
        JObject root;
        using (var reader = new Newtonsoft.Json.JsonTextReader(new StringReader(json)) { DateParseHandling = Newtonsoft.Json.DateParseHandling.None })
        {
            root = JObject.Load(reader);
        }

        var events = root["events"] as JArray;
        if (events == null)
        {
            return games;
        }

        foreach (var ev in events)
        {
            var competition = ev["competitions"] is JArray comps && comps.Count > 0 ? comps[0] : null;
            if (competition == null)
            {
                continue;
            }

            var game = new EspnGame
            {
                Id = Text(ev["id"]),
                KickoffUtc = ParseUtc(Text(competition["date"]) ?? Text(ev["date"])),
                Network = Text(competition["broadcast"]) ?? string.Empty
            };

            var status = competition["status"] ?? ev["status"];
            if (status != null)
            {
                var type = status["type"];
                game.State = (Text(type?["state"]) ?? "pre").ToLowerInvariant();
                game.Completed = Bool(type?["completed"]);
                game.ShortDetail = Text(type?["shortDetail"]) ?? string.Empty;
                game.Period = Int(status["period"]);
                game.Clock = Text(status["displayClock"]) ?? string.Empty;
            }

            if (competition["competitors"] is JArray competitors)
            {
                foreach (var competitor in competitors)
                {
                    var team = ParseTeam(competitor);
                    if (string.Equals(Text(competitor["homeAway"]), "home", StringComparison.OrdinalIgnoreCase))
                    {
                        game.Home = team;
                    }
                    else
                    {
                        game.Away = team;
                    }
                }
            }

            var situation = competition["situation"];
            if (situation != null && situation.Type == JTokenType.Object)
            {
                game.HasSituation = true;
                game.PossessionTeamId = Text(situation["possession"]) ?? string.Empty;
                game.DownDistanceText = Text(situation["downDistanceText"]) ?? string.Empty;
                game.ShortDownDistanceText = Text(situation["shortDownDistanceText"]) ?? string.Empty;
                game.PossessionText = Text(situation["possessionText"]) ?? string.Empty;
                game.IsRedZone = Bool(situation["isRedZone"]);
                game.LastPlay = Text(situation["lastPlay"]?["text"]) ?? string.Empty;
            }

            games.Add(game);
        }

        return games;
    }

    private static EspnTeam ParseTeam(JToken competitor)
    {
        var info = competitor["team"];
        var team = new EspnTeam
        {
            Id = Text(competitor["id"]) ?? Text(info?["id"]) ?? string.Empty,
            Abbr = Text(info?["abbreviation"]) ?? string.Empty,
            Location = Text(info?["location"]) ?? string.Empty,
            Nickname = Text(info?["name"]) ?? Text(info?["shortDisplayName"]) ?? string.Empty,
            LogoUrl = Text(info?["logo"]) ?? string.Empty,
            DisplayName = Text(info?["displayName"]) ?? string.Empty,
            Score = Int(competitor["score"]),
            Winner = Bool(competitor["winner"])
        };

        if (competitor["records"] is JArray records)
        {
            foreach (var record in records)
            {
                if (string.Equals(Text(record["type"]), "total", StringComparison.OrdinalIgnoreCase))
                {
                    team.Record = Text(record["summary"]) ?? string.Empty;
                    break;
                }
            }
        }

        return team;
    }

    private static string Text(JToken token)
    {
        if (token == null || token.Type == JTokenType.Null || token.Type == JTokenType.Object || token.Type == JTokenType.Array)
        {
            return null;
        }

        return token.ToString();
    }

    private static int Int(JToken token)
    {
        var text = Text(token);
        double value;
        return text != null && double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? (int)value : 0;
    }

    private static bool Bool(JToken token)
    {
        var text = Text(token);
        return text != null && (text.Equals("true", StringComparison.OrdinalIgnoreCase) || text == "1");
    }

    private static DateTime? ParseUtc(string value)
    {
        DateTime parsed;
        if (!string.IsNullOrWhiteSpace(value) &&
            DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out parsed))
        {
            return parsed;
        }

        return null;
    }
}
