using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for previousseason
/// </summary>
/// 
namespace previousseason
{

    public class PreviousYear
    {
        public Overallteamstandings overallteamstandings { get; set; }
    }

    public class Overallteamstandings
    {
        public string lastUpdatedOn { get; set; }
        public Teamstandingsentry[] teamstandingsentry { get; set; }
    }

    public class Teamstandingsentry
    {
        public Team team { get; set; }
        public string rank { get; set; }
        public Stats stats { get; set; }
    }

    public class Team
    {
        public string ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Stats
    {
        public Gamesplayed GamesPlayed { get; set; }
        public Wins Wins { get; set; }
        public Losses Losses { get; set; }
        public Ties Ties { get; set; }
        public Pointsfor PointsFor { get; set; }
        public Pointsagainst PointsAgainst { get; set; }
    }

    public class Gamesplayed
    {
        public string abbreviation { get; set; }
        public string text { get; set; }
    }

    public class Wins
    {
        public string category { get; set; }
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Losses
    {
        public string category { get; set; }
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Ties
    {
        public string category { get; set; }
        public string abbreviation { get; set; }
        public string text { get; set; }
    }

    public class Pointsfor
    {
        public string category { get; set; }
        public string abbreviation { get; set; }
        public string text { get; set; }
    }

    public class Pointsagainst
    {
        public string category { get; set; }
        public string abbreviation { get; set; }
        public string text { get; set; }
    }

}