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
using MySql.Data.MySqlClient;

public partial class scoreboard_scoresInsert : System.Web.UI.Page
{

    // Copy from weekly cs file below

    public string urlwk2 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2024-regular/full_game_schedule.json?date=from-20241225-to-20241230";
    // public string urlts = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2016-2017-regular/overall_team_standings.json?teamstats=W,L,T,PF,PA";

    public string urlscoreswk1xmas = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2024-regular/scoreboard.json?fordate=20241225";
    public string urlscoreswk1pt1 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2024-regular/scoreboard.json?fordate=20241226";
    public string urlscoreswk1ptsat = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2024-regular/scoreboard.json?fordate=20241228";
    public string urlscoreswk1pt2 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2024-regular/scoreboard.json?fordate=20241229";
    public string urlscoreswk1pt3 = "https://api.mysportsfeeds.com/v1.1/pull/nfl/2024-regular/scoreboard.json?fordate=20241230";


    public int numberofgames;
    public int numberofteams;
    public int numberofpeople;
    public int numberofgamescoresxmas;
    public int numberofgamescores;
    public int numberofgamescoressat;
    public int numberofgamescores2;
    public int numberofgamescores3;


    public List<string> theFirstName = new List<string>();
    public List<string> theLastName = new List<string>();
    public List<string> theTotal = new List<string>();


    public NFLschedule showall;
    // public PreviousYear lastseason;
    // public Currentstandings livestandings;

    public LiveScoring livescoresxmas;
    public LiveScoring livescores;
    public LiveScoring livescoressat;
    public LiveScoring livescores2;
    public LiveScoring livescores3;
    // public LiveScoring allscores;

    public List<string> pickFirstName = new List<string>();
    public List<string> pickLastName = new List<string>();
    public List<int> correctPicks = new List<int>();
    public List<int> totalPicks = new List<int>();
    public List<string> massiveName = new List<string>();

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

    public string[] theFirstNamePicks;
    public string[] theLastNamePicks;
    public int[] theCorrectPicks;
    public int[] theTotalPicks;

    public string successText;



    protected void Page_Load(object sender, EventArgs e)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var client = new WebClient { Credentials = new NetworkCredential("bbd5a2c0-xxxx-xxxx-xxxx-305d0a", "My69Password") };


        var response = client.DownloadString(urlwk2);
        // var responselastyear = client.DownloadString(urlts);

        var responsescoresxmas = client.DownloadString(urlscoreswk1xmas);
        var responsescores = client.DownloadString(urlscoreswk1pt1);
        var responsescoressat = client.DownloadString(urlscoreswk1ptsat);
        var responsescores2 = client.DownloadString(urlscoreswk1pt2);
        var responsescores3 = client.DownloadString(urlscoreswk1pt3);


        showall = JsonConvert.DeserializeObject<NFLschedule>(response);
        // lastseason = JsonConvert.DeserializeObject<PreviousYear>(responselastyear);

        livescoresxmas = JsonConvert.DeserializeObject<LiveScoring>(responsescoresxmas);
        livescores = JsonConvert.DeserializeObject<LiveScoring>(responsescores);
        livescoressat = JsonConvert.DeserializeObject<LiveScoring>(responsescoressat);
        livescores2 = JsonConvert.DeserializeObject<LiveScoring>(responsescores2);
        livescores3 = JsonConvert.DeserializeObject<LiveScoring>(responsescores3);

        numberofgames = showall.fullgameschedule.gameentry.Length;
        // numberofteams = lastseason.overallteamstandings.teamstandingsentry.Length;

        numberofgamescoresxmas = livescoresxmas.scoreboard.gameScore.Length;
        numberofgamescores = livescores.scoreboard.gameScore.Length;
        numberofgamescoressat = livescoressat.scoreboard.gameScore.Length;
        numberofgamescores2 = livescores2.scoreboard.gameScore.Length;
        numberofgamescores3 = livescores3.scoreboard.gameScore.Length;


        //string connectionString = "Server=mysql24.ezhostingserver.com;Database=bfscoresDB;User ID=MyName69;Password=ThePassword69;";
        string connectionString2 = "Server=mysql24.ezhostingserver.com;Database=bfpoolDB;User ID=MyName69;Password=ThePassword69;";

        // First connection (for scores.mdb equivalent)
        /* MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM `2023`", connection);
        MySqlDataReader reader = command.ExecuteReader(); */



        // Second connection (for picks.mdb equivalent)
        MySqlConnection connPicks = new MySqlConnection(connectionString2);
        connPicks.Open();
        MySqlCommand comPicks = new MySqlCommand("SELECT * FROM `seventeen2024` ORDER BY UPPER(lastname), UPPER(firstname)", connPicks);
        MySqlDataReader readerPicks = comPicks.ExecuteReader();

        while (readerPicks.Read())
        {
            pickFirstName.Add(readerPicks["firstname"].ToString());
            pickLastName.Add(readerPicks["lastname"].ToString());



            // Copy from weekly cs file below
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
            // Copy from weekly cs file above




            totalPicks.Add(AllTotals(readerPicks["firstname"].ToString(), readerPicks["lastname"].ToString()));



        }




        connPicks.Close();


    }


    // Copy from weekly cs file below

    public int ScoringGames(string scoringFirstName, string scoringLastName, string game1, string game2, string game3, string game4, string game5, string game6, string game7, string game8, string game9, string game10, string game11, string game12, string game13, string game14, string game15, string game16)
    {
        int score = 0;
        if (game1 == (livescoresxmas.scoreboard.gameScore[0].game.awayTeam.City + " " + livescoresxmas.scoreboard.gameScore[0].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[0].homeScore, out y);
            score++;
            //if (x > y)
            //{
            //   score++;
            //}
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[0].homeScore, out y);
            score++;
            //if (x < y)
            //{
            //    score++;
            //}
        }

        if (game2 == (livescoresxmas.scoreboard.gameScore[1].game.awayTeam.City + " " + livescoresxmas.scoreboard.gameScore[1].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[1].awayScore, out x);
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[1].homeScore, out y);
            score++;
            //if (x > y)
            //{
            //   score++;
            //}
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[1].awayScore, out x);
            Int32.TryParse(livescoresxmas.scoreboard.gameScore[1].homeScore, out y);
            score++;
            //if (x < y)
            //{
            //    score++;
            //}
        }

        if (game3 == (livescores.scoreboard.gameScore[0].game.awayTeam.City + " " + livescores.scoreboard.gameScore[0].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescores.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescores.scoreboard.gameScore[0].homeScore, out y);
            score++;
            //if (x > y)
            //{
            //   score++;
            //}
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescores.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescores.scoreboard.gameScore[0].homeScore, out y);
            score++;
            //if (x < y)
            //{
            //    score++;
            //}
        }

        if (game4 == (livescoressat.scoreboard.gameScore[0].game.awayTeam.City + " " + livescoressat.scoreboard.gameScore[0].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescoressat.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescoressat.scoreboard.gameScore[0].homeScore, out y);
            score++;
            //if (x > y)
            //{
            //   score++;
            //}
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescoressat.scoreboard.gameScore[0].awayScore, out x);
            Int32.TryParse(livescoressat.scoreboard.gameScore[0].homeScore, out y);
            score++;
            //if (x < y)
            //{
            //    score++;
            //}
        }

        if (game5 == (livescoressat.scoreboard.gameScore[1].game.awayTeam.City + " " + livescoressat.scoreboard.gameScore[1].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescoressat.scoreboard.gameScore[1].awayScore, out x);
            Int32.TryParse(livescoressat.scoreboard.gameScore[1].homeScore, out y);
            score++;
            //if (x > y)
            //{
            //   score++;
            //}
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescoressat.scoreboard.gameScore[1].awayScore, out x);
            Int32.TryParse(livescoressat.scoreboard.gameScore[1].homeScore, out y);
            score++;
            //if (x < y)
            //{
            //    score++;
            //}
        }

        if (game6 == (livescoressat.scoreboard.gameScore[2].game.awayTeam.City + " " + livescoressat.scoreboard.gameScore[2].game.awayTeam.Name))
        {
            int x;
            int y;
            Int32.TryParse(livescoressat.scoreboard.gameScore[2].awayScore, out x);
            Int32.TryParse(livescoressat.scoreboard.gameScore[2].homeScore, out y);
            score++;
            //if (x > y)
            //{
            //   score++;
            //}
        }
        else
        {
            int x;
            int y;
            Int32.TryParse(livescoressat.scoreboard.gameScore[2].awayScore, out x);
            Int32.TryParse(livescoressat.scoreboard.gameScore[2].homeScore, out y);
            score++;
            //if (x < y)
            //{
            //    score++;
            //}
        }







        string[] gamePicks = { game7, game8, game9, game10, game11, game12, game13, game14, game15 };

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

        for (int i = 0; i < 1; i++)
        {
            if (mondaypicks[i] == (livescores3.scoreboard.gameScore[0].game.awayTeam.City + " " + livescores3.scoreboard.gameScore[0].game.awayTeam.Name))
            {
                int x;
                int y;
                Int32.TryParse(livescores3.scoreboard.gameScore[0].awayScore, out x);
                Int32.TryParse(livescores3.scoreboard.gameScore[0].homeScore, out y);
                if (x > y)
                {
                    score++;
                }
            }
            else
            {
                int x;
                int y;
                Int32.TryParse(livescores3.scoreboard.gameScore[0].awayScore, out x);
                Int32.TryParse(livescores3.scoreboard.gameScore[0].homeScore, out y);
                if (x < y)
                {
                    score++;
                }
            }
        }

        return (score);

    }

    // Copy from weekly cs file above



    public int AllTotals(string theFirst, string theLast)
    {
        string connectionString = "Server=mysql24.ezhostingserver.com;Database=bfscoresDB;User ID=MyName69;Password=ThePassword69;";

        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string query = "SELECT * FROM `2024` WHERE firstname = @firstname AND lastname = @lastname";
        MySqlCommand command = new MySqlCommand(query, connection);

        // Using parameters to prevent SQL injection
        command.Parameters.AddWithValue("@firstname", theFirst.Trim());
        command.Parameters.AddWithValue("@lastname", theLast.Trim());

        MySqlDataReader reader = command.ExecuteReader();

        int x = -1;

        if (reader.Read())
        {
            // Read the "total" value only if the firstname and lastname match
            x = Convert.ToInt32(reader["total"]);
        }

        connection.Close();
        return x;
    }

    protected void SubmitForm(object sender, EventArgs e)
    {
        int totalEntries;
        Int32.TryParse(Request.Form["totalEntries"].ToString(), out totalEntries);

        string connectionString = "Server=mysql24.ezhostingserver.com;Database=bfscoresDB;User ID=MyName69;Password=ThePassword69;";
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            for (int i = 0; i < totalEntries; i++)
            {
                int weekCorrect;
                Int32.TryParse(Request.Form["weekCorrect" + i].ToString(), out weekCorrect);
                int totalCorrect;
                Int32.TryParse(Request.Form["totalCorrect" + i].ToString(), out totalCorrect);

                string firstName = Request.Form["firstCorrect" + i].ToString();
                string lastName = Request.Form["lastCorrect" + i].ToString();

                string query = "UPDATE `2024` SET week17 = @weekCorrect, total = @totalCorrect WHERE firstname = @firstname AND lastname = @lastname";
                //string query = "INSERT INTO `2024` (firstname, lastname, week1, total) VALUES (@firstname, @lastname, @weekCorrect, @totalCorrect)";


                MySqlCommand cmd = new MySqlCommand(query, connection);

                // Using parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@weekCorrect", weekCorrect);
                cmd.Parameters.AddWithValue("@totalCorrect", totalCorrect);
                cmd.Parameters.AddWithValue("@firstname", firstName.Trim());
                cmd.Parameters.AddWithValue("@lastname", lastName.Trim());

                cmd.ExecuteNonQuery();
            }

            successText = "Data saved successfully!";
        }
        catch (Exception ex)
        {
            successText = "Failed due to: " + ex.Message;
        }
        finally
        {
            connection.Close();
        }

    }
}




 