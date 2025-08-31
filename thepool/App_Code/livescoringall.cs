using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for livescoringall
/// </summary>
namespace livescoringall
{
    public class AwayTeam
    {
        public int id { get; set; }
        public string abbreviation { get; set; }
    }

    public class HomeTeam
    {
        public int id { get; set; }
        public string abbreviation { get; set; }
    }

    public class Venue
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Official
    {
        public int id { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class Speed
    {
        public int milesPerHour { get; set; }
        public int kilometersPerHour { get; set; }
    }

    public class Direction
    {
        public int degrees { get; set; }
        public string label { get; set; }
    }

    public class Wind
    {
        public Speed speed { get; set; }
        public Direction direction { get; set; }
    }

    public class Temperature
    {
        public int fahrenheit { get; set; }
        public int celsius { get; set; }
    }

    public class Amount
    {
        public int millimeters { get; set; }
        public object centimeters { get; set; }
        public object inches { get; set; }
        public object feet { get; set; }
    }

    public class Precipitation
    {
        public string type { get; set; }
        public object percent { get; set; }
        public Amount amount { get; set; }
    }

    public class Weather
    {
        public string type { get; set; }
        public string description { get; set; }
        public Wind wind { get; set; }
        public Temperature temperature { get; set; }
        public Precipitation precipitation { get; set; }
        public int humidityPercent { get; set; }
    }

    public class Schedule
    {
        public int id { get; set; }
        public int week { get; set; }
        public DateTime startTime { get; set; }
        public object endedTime { get; set; }
        public AwayTeam awayTeam { get; set; }
        public HomeTeam homeTeam { get; set; }
        public Venue venue { get; set; }
        public string venueAllegiance { get; set; }
        public string scheduleStatus { get; set; }
        public DateTime? originalStartTime { get; set; }
        public string delayedOrPostponedReason { get; set; }
        public string playedStatus { get; set; }
        public int attendance { get; set; }
        public List<Official> officials { get; set; }
        public List<string> broadcasters { get; set; }
        public Weather weather { get; set; }
    }

    public class Quarter
    {
        public int quarterNumber { get; set; }
        public int awayScore { get; set; }
        public int homeScore { get; set; }
    }

    public class Score
    {
        public object currentQuarter { get; set; }
        public object currentQuarterSecondsRemaining { get; set; }
        public object currentIntermission { get; set; }
        public object teamInPossession { get; set; }
        public object currentDown { get; set; }
        public object currentYardsRemaining { get; set; }
        public object lineOfScrimmage { get; set; }
        public int awayScoreTotal { get; set; }
        public int homeScoreTotal { get; set; }
        public List<Quarter> quarters { get; set; }
    }

    public class Game
    {
        public Schedule schedule { get; set; }
        public Score score { get; set; }
    }

    public class HomeVenue
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class SocialMediaAccount
    {
        public string mediaType { get; set; }
        public string value { get; set; }
    }

    public class TeamReference
    {
        public int id { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public HomeVenue homeVenue { get; set; }
        public List<string> teamColoursHex { get; set; }
        public List<SocialMediaAccount> socialMediaAccounts { get; set; }
        public string officialLogoImageSrc { get; set; }
    }

    public class GeoCoordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class CapacitiesByEventType
    {
        public string eventType { get; set; }
        public int capacity { get; set; }
    }

    public class VenueReference
    {
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public GeoCoordinates geoCoordinates { get; set; }
        public List<CapacitiesByEventType> capacitiesByEventType { get; set; }
        public string playingSurface { get; set; }
        public List<object> baseballDimensions { get; set; }
        public bool hasRoof { get; set; }
        public bool hasRetractableRoof { get; set; }
    }

    public class References
    {
        public List<TeamReference> teamReferences { get; set; }
        public List<VenueReference> venueReferences { get; set; }
    }

    public class Root
    {
        public DateTime lastUpdatedOn { get; set; }
        public List<Game> games { get; set; }
        public List<object> teamsWithByes { get; set; }
        public References references { get; set; }
    }



}