using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for livescoring
/// </summary>
namespace livescoring
{

    public class LiveScoring
    {
        public Scoreboard scoreboard { get; set; }
    }

    public class Scoreboard
    {
        public string lastUpdatedOn { get; set; }
        public Gamescore[] gameScore { get; set; }
    }

    public class Gamescore
    {
        public Game game { get; set; }
        public string isUnplayed { get; set; }
        public string isInProgress { get; set; }
        public string isCompleted { get; set; }
        public string awayScore { get; set; }
        public string homeScore { get; set; }
        public Quartersummary quarterSummary { get; set; }
    }

    public class Game
    {
        public string ID { get; set; }
        public string week { get; set; }
        public string scheduleStatus { get; set; }
        public object originalDate { get; set; }
        public object originalTime { get; set; }
        public object delayedOrPostponedReason { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public Awayteam awayTeam { get; set; }
        public Hometeam homeTeam { get; set; }
        public string location { get; set; }
    }

    public class Awayteam
    {
        public string ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Hometeam
    {
        public string ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Quartersummary
    {
        public Quarter[] quarter { get; set; }
    }

    public class Quarter
    {
        public string number { get; set; }
        public string awayScore { get; set; }
        public string homeScore { get; set; }
    }

}