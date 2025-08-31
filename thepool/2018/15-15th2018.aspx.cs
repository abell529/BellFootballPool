using System.Net;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Configuration;
using nflgames;
using previousseason;
using System.Data.OleDb;
using System.Collections.Generic;
using standings;
using livescoring;
using System.Data;
using System.Collections.Specialized;

public partial class _2018_15_15th2018 : System.Web.UI.Page
{
    public string urlwk2 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2018-regular/full_game_schedule.json?date=from-20181213-to-20181217";
    // public string urlts = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2016-2017-regular/overall_team_standings.json?teamstats=W,L,T,PF,PA";

    public string urlscoreswk1pt1 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2018-regular/scoreboard.json?fordate=20181213";
    public string urlscoreswk1SAT = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2018-regular/scoreboard.json?fordate=20181215";
    public string urlscoreswk1pt2 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2018-regular/scoreboard.json?fordate=20181216";
    public string urlscoreswk1pt3 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2018-regular/scoreboard.json?fordate=20181217";


    public int numberofgames;
    public int numberofteams;
    public int numberofpeople;
    public int numberofgamescores;
    public int numberofgamescoresSAT;
    public int numberofgamescores2;
    public int numberofgamescores3;


    public List<string> theFirstName = new List<string>();
    public List<string> theLastName = new List<string>();
    public List<string> theTotal = new List<string>();


    public NFLschedule showall;
    // public PreviousYear lastseason;
    // public Currentstandings livestandings;

    public LiveScoring livescores;
    public LiveScoring livescoresSAT;
    public LiveScoring livescores2;
    public LiveScoring livescores3;
    // public LiveScoring allscores;

    public List<string> gamePicks = new List<string>();

    public string emailText;
    public string firstnameText;
    public string lastnameText;

    public List<string> pickFirstName = new List<string>();
    public List<string> pickLastName = new List<string>();
    public List<int> correctPicks = new List<int>();

    public List<string> game0Pick = new List<string>();
    public List<string> game1Pick = new List<string>();
    public List<string> game2Pick = new List<string>();
    public List<string> game3Pick = new List<string>();
    public List<string> game4Pick = new List<string>();
    public List<string> game5Pick = new List<string>();
    public List<string> game6Pick = new List<string>();
    public List<string> game7Pick = new List<string>();
    public List<string> game8Pick = new List<string>();
    public List<string> game9Pick = new List<string>();
    public List<string> game10Pick = new List<string>();
    public List<string> game11Pick = new List<string>();
    public List<string> game12Pick = new List<string>();
    public List<string> game13Pick = new List<string>();
    public List<string> game14Pick = new List<string>();
    public List<string> game15Pick = new List<string>();


    public List<string> sundayGameIDs = new List<string>();

    public string successText;


    protected void Page_Load(object sender, EventArgs e)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var client = new WebClient { Credentials = new NetworkCredential("shocktor", "1998PlaySF$") };


        var response = client.DownloadString(urlwk2);
        // var responselastyear = client.DownloadString(urlts);

        var responsescores = client.DownloadString(urlscoreswk1pt1);
        var responsescoresSAT = client.DownloadString(urlscoreswk1SAT);
        var responsescores2 = client.DownloadString(urlscoreswk1pt2);
        var responsescores3 = client.DownloadString(urlscoreswk1pt3);


        showall = JsonConvert.DeserializeObject<NFLschedule>(response);
        // lastseason = JsonConvert.DeserializeObject<PreviousYear>(responselastyear);

        livescores = JsonConvert.DeserializeObject<LiveScoring>(responsescores);
        livescoresSAT = JsonConvert.DeserializeObject<LiveScoring>(responsescoresSAT);
        livescores2 = JsonConvert.DeserializeObject<LiveScoring>(responsescores2);
        livescores3 = JsonConvert.DeserializeObject<LiveScoring>(responsescores3);

        numberofgames = showall.fullgameschedule.gameentry.Length;
        // numberofteams = lastseason.overallteamstandings.teamstandingsentry.Length;

        numberofgamescores = livescores.scoreboard.gameScore.Length;
        numberofgamescoresSAT = livescoresSAT.scoreboard.gameScore.Length;
        numberofgamescores2 = livescores2.scoreboard.gameScore.Length;
        numberofgamescores3 = livescores3.scoreboard.gameScore.Length;

        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\home\bellfusion.com\wwwroot\BFDB\scores.mdb");
        connection.Open();
        OleDbDataReader reader = null;
        OleDbCommand command = new OleDbCommand("SELECT * from 2018", connection);
        reader = command.ExecuteReader();

        OleDbConnection connPicks = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\home\bellfusion.com\wwwroot\BFDB\picks.mdb");
        connPicks.Open();
        OleDbDataReader readerPicks = null;
        OleDbCommand comPicks = new OleDbCommand("SELECT * FROM fifteen2018 ORDER BY uCase(lastname),uCase(firstname)", connPicks);
        readerPicks = comPicks.ExecuteReader();



        while (reader.Read())
        {
            theFirstName.Add(reader["firstname"].ToString());
            theLastName.Add(reader["lastname"].ToString());
            theTotal.Add(reader["total"].ToString());

        }



        while (readerPicks.Read())
        {
            pickFirstName.Add(readerPicks["firstname"].ToString());
            pickLastName.Add(readerPicks["lastname"].ToString());

            game0Pick.Add(readerPicks["game1"].ToString());
            game1Pick.Add(readerPicks["game2"].ToString());
            game2Pick.Add(readerPicks["game3"].ToString());
            game3Pick.Add(readerPicks["game4"].ToString());
            game4Pick.Add(readerPicks["game5"].ToString());
            game5Pick.Add(readerPicks["game6"].ToString());
            game6Pick.Add(readerPicks["game7"].ToString());
            game7Pick.Add(readerPicks["game8"].ToString());
            game8Pick.Add(readerPicks["game9"].ToString());
            game9Pick.Add(readerPicks["game10"].ToString());
            game10Pick.Add(readerPicks["game11"].ToString());
            game11Pick.Add(readerPicks["game12"].ToString());
            game12Pick.Add(readerPicks["game13"].ToString());
            game13Pick.Add(readerPicks["game14"].ToString());
            game14Pick.Add(readerPicks["game15"].ToString());
            game15Pick.Add(readerPicks["game16"].ToString());


            correctPicks.Add(ScoringGames(readerPicks["firstname"].ToString(), readerPicks["lastname"].ToString(), readerPicks["game1"].ToString(), readerPicks["game2"].ToString(), readerPicks["game3"].ToString(), readerPicks["game4"].ToString(), readerPicks["game5"].ToString(), readerPicks["game6"].ToString(), readerPicks["game7"].ToString(), readerPicks["game8"].ToString(), readerPicks["game9"].ToString(), readerPicks["game10"].ToString(), readerPicks["game11"].ToString(), readerPicks["game12"].ToString(), readerPicks["game13"].ToString(), readerPicks["game14"].ToString(), readerPicks["game15"].ToString(), readerPicks["game16"].ToString()));


        }



        connection.Close();
        connPicks.Close();


    }

    public int ScoringGames(string scoringFirstName, string scoringLastName, string game1, string game2, string game3, string game4, string game5, string game6, string game7, string game8, string game9, string game10, string game11, string game12, string game13, string game14, string game15, string game16)
    {
        int score = 0;
        if (game1 == (livescores.scoreboard.gameScore[0].game.awayTeam.City + " " + livescores.scoreboard.gameScore[0].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescores.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescores.scoreboard.gameScore[0].homeScore, out y);
            if (x > y)
            {
                score++;
            }
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescores.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescores.scoreboard.gameScore[0].homeScore, out y);
            if (x < y)
            {
                score++;
            }
        }

        if (game2 == (livescoresSAT.scoreboard.gameScore[0].game.awayTeam.City + " " + livescoresSAT.scoreboard.gameScore[0].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[0].homeScore, out y);
            if (x > y)
            {
                score++;
            }
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[0].homeScore, out y);
            if (x < y)
            {
                score++;
            }
        }

        if (game3 == (livescoresSAT.scoreboard.gameScore[1].game.awayTeam.City + " " + livescoresSAT.scoreboard.gameScore[1].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[1].awayScore, out x);
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[1].homeScore, out y);
            if (x > y)
            {
                score++;
            }
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[1].awayScore, out x);
            Int32.TryParse(livescoresSAT.scoreboard.gameScore[1].homeScore, out y);
            if (x < y)
            {
                score++;
            }
        }


        string[] gamePicks = { game4, game5, game6, game7, game8, game9, game10, game11, game12, game13, game14, game15 };

        for (int a = 0; a < gamePicks.Length; a++)
        {
            int b = 0;
            while (b < livescores2.scoreboard.gameScore.Length)
            {

                if (gamePicks[a] == (livescores2.scoreboard.gameScore[b].game.awayTeam.City + " " + livescores2.scoreboard.gameScore[b].game.awayTeam.Name))
                {
                    int x;
                    int y;
                    Int32.TryParse(livescores2.scoreboard.gameScore[b].awayScore, out x);
                    Int32.TryParse(livescores2.scoreboard.gameScore[b].homeScore, out y);
                    if (x > y)
                    {
                        score++;
                    }
                    b++;
                }
                else if (gamePicks[a] == (livescores2.scoreboard.gameScore[b].game.homeTeam.City + " " + livescores2.scoreboard.gameScore[b].game.homeTeam.Name))
                {
                    int x;
                    int y;
                    Int32.TryParse(livescores2.scoreboard.gameScore[b].awayScore, out x);
                    Int32.TryParse(livescores2.scoreboard.gameScore[b].homeScore, out y);
                    if (x < y)
                    {
                        score++;
                    }
                    b++;
                }
                else
                {
                    b++;
                }

            }
        }



        string[] mondaypicks = { game16 };

        for (int i = 0; i < livescores3.scoreboard.gameScore.Length; i++)
        {
            if (mondaypicks[i] == (livescores3.scoreboard.gameScore[i].game.awayTeam.City + " " + livescores3.scoreboard.gameScore[i].game.awayTeam.Name))
            {
                int x;
                int y;
                Int32.TryParse(livescores3.scoreboard.gameScore[i].awayScore, out x);
                Int32.TryParse(livescores3.scoreboard.gameScore[i].homeScore, out y);
                if (x > y)
                {
                    score++;
                }
            }
            else
            {
                int x;
                int y;
                Int32.TryParse(livescores3.scoreboard.gameScore[i].awayScore, out x);
                Int32.TryParse(livescores3.scoreboard.gameScore[i].homeScore, out y);
                if (x < y)
                {
                    score++;
                }
            }
        }

        return (score);

    }


}