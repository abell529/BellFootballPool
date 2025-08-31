using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for nflscores
/// </summary>


namespace nflgames
{

    public class NFLschedule
    {
        public Fullgameschedule fullgameschedule { get; set; }


    }

    public class Fullgameschedule
    {
        public string lastUpdatedOn { get; set; }
        public Gameentry[] gameentry { get; set; }
    }

    public class Gameentry
    {
        public string id { get; set; }
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

}


