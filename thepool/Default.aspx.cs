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
using System.Data;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;

// ...





public partial class _Default : System.Web.UI.Page
{
    public string urlwk1 = $"{CredentialStore.ApiBaseUrl}/2024-regular/full_game_schedule.json?date=from-20250104-to-20250105";
    // public string urlts = $"{CredentialStore.ApiBaseUrl}/2020-2021-regular/overall_team_standings.json";

    public int numberofgames;
    public int numberofteams;
    public int numberofpeople;
    public List<string> theFirstName = new List<string>();
    public List<string> theLastName = new List<string>();
    public List<string> theTotal = new List<string>();
    
    public NFLschedule showall;
    public PreviousYear lastseason;
    public Currentstandings livestandings;

    public List<string> gamePicks = new List<string>();

    public string emailText;
    public string firstnameText;
    public string lastnameText;

    public string game0Pick;
    public string game1Pick;
    public string game2Pick;
    public string game3Pick;
    public string game4Pick;
    public string game5Pick;
    public string game6Pick;
    public string game7Pick;
    public string game8Pick;
    public string game9Pick;
    public string game10Pick;
    public string game11Pick;
    public string game12Pick;
    public string game13Pick;
    public string game14Pick;
    public string game15Pick;

    public string successText;

    // public NameValueCollection nvc;



    // public List<string> ListboxItems = new List<string>();







    protected void Page_Load(object sender, EventArgs e)
    {

        // var listItems = new List<currentstandings>();
        // var client = new WebClient();
        // var text = client.DownloadString($"{CredentialStore.ApiBaseUrl}/2018-regular/full_game_schedule.json?date=from-20170907-to-20170910");
        // nflgames showall = JsonConvert.DeserializeObject<nflgames>(text);
        
        // string url = $"{CredentialStore.ApiBaseUrl}/2017-regular/full_game_schedule.json?date=from-20170907-to-20170911?limit=1";
        // WebRequest request = WebRequest.Create(url);
        // request.Credentials = GetCredential();
        // request.PreAuthenticate = true;

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

        var client = new WebClient { Credentials = CredentialStore.ApiCredential };
        
        var response = client.DownloadString(urlwk1);
        // var responselastyear = client.DownloadString(urlts);

        showall = JsonConvert.DeserializeObject<NFLschedule>(response);
        // lastseason = JsonConvert.DeserializeObject<PreviousYear>(responselastyear);

        // var game1 = showall.fullgameschedule.gameentry[0];

        numberofgames = showall.fullgameschedule.gameentry.Length;
        // numberofteams = lastseason.overallteamstandings.teamstandingsentry.Length;




        // TextBlock.Text = response;
        // TextBlock3.Text = game1.homeTeam.City;



        string connectionString = CredentialStore.ScoresConnectionString;

        // Establishing MySQL connection
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        MySqlCommand command = new MySqlCommand("SELECT * FROM `2024` ORDER BY total DESC, lastname ASC, firstname ASC", connection);
        MySqlDataReader reader = command.ExecuteReader();




        /* OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\home\bellfusion.com\wwwroot\BFDB\scores.mdb");
        connection.Open();
        OleDbDataReader reader = null;
        OleDbCommand command = new OleDbCommand("SELECT * from 2023 ORDER BY total DESC, lastname ASC, firstname ASC", connection);
        reader = command.ExecuteReader(); */


        // DataSet fromStandings = new DataSet("Standings");


        while (reader.Read())
        {
            theFirstName.Add(reader["firstname"].ToString());
            theLastName.Add(reader["lastname"].ToString());
            theTotal.Add(reader["total"].ToString());

        }




        /* List<string> listAdd = new List<string>();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                object[] oarray = new object[reader.FieldCount];
                

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    oarray[i] = reader[i];
                    numberofpeople = i;
                }

            }
        } */



        // var listItems = new List<currentstandings>();

        /* while (reader.Read())
        {

            listItems.Add(new currentstandings { LastName = reader.GetString(0), FirstName = reader.GetString(1), total = reader.GetDouble(2) });
        }

        allRecords = listItems.ToArray(); */



        reader.Close();
        connection.Close();


        // numberofpeople = listAdd.Count;



    }

    protected void SubmitForm(object sender, EventArgs e)
    {
        emailText = emailEntry.Text;
        firstnameText = firstnameEntry.Text;
        lastnameText = lastnameEntry.Text;

        game0Pick = Request.Form["Game0t"];
        gamePicks.Add(Request.Form["Game0t"]);

        game1Pick = Request.Form["Game1t"];
        gamePicks.Add(Request.Form["Game1t"]);

        game2Pick = Request.Form["game2t"];
        gamePicks.Add(Request.Form["Game2t"]);

        game3Pick = Request.Form["game3t"];
        gamePicks.Add(Request.Form["Game3t"]);

        game4Pick = Request.Form["game4t"];
        gamePicks.Add(Request.Form["Game4t"]);

        game5Pick = Request.Form["game5t"];
        gamePicks.Add(Request.Form["Game5t"]);

        game6Pick = Request.Form["game6t"];
        gamePicks.Add(Request.Form["Game6t"]);

        game7Pick = Request.Form["game7t"];
        gamePicks.Add(Request.Form["Game7t"]);

        game8Pick = Request.Form["game8t"];
        gamePicks.Add(Request.Form["Game8t"]);

        game9Pick = Request.Form["game9t"];
        gamePicks.Add(Request.Form["Game9t"]);

        game10Pick = Request.Form["game10t"];
        gamePicks.Add(Request.Form["Game10t"]);

        game11Pick = Request.Form["game11t"];
        gamePicks.Add(Request.Form["Game11t"]);

        game12Pick = Request.Form["game12t"];
        gamePicks.Add(Request.Form["Game12t"]);



        if (numberofgames>12)
        {
            game13Pick = Request.Form["game13t"];
            gamePicks.Add(Request.Form["Game13t"]);

            if (numberofgames > 13)
            {
                game14Pick = Request.Form["game14t"];
                gamePicks.Add(Request.Form["Game14t"]);

                if (numberofgames > 14)
                {
                    game15Pick = Request.Form["game15t"];
                    gamePicks.Add(Request.Form["Game15t"]);
                }
            }
        }




        /* OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\home\bellfusion.com\wwwroot\BFDB\picks.mdb");
        


        try
        {

            connection.Open();
            OleDbCommand cmd = new OleDbCommand("INSERT INTO eighteen2023(firstname, lastname, email, game1, game2, game3, game4, game5, game6, game7, game8, game9, game10, game11, game12, game13, game14, game15, game16) VALUES ('" + firstnameText + "', '" + lastnameText + "', '" + emailText + "', '" + game0Pick + "', '" + game1Pick + "', '" + game2Pick + "', '" + game3Pick + "', '" + game4Pick + "', '" + game5Pick + "', '" + game6Pick + "', '" + game7Pick + "', '" + game8Pick + "', '" + game9Pick + "', '" + game10Pick + "', '" + game11Pick + "', '" + game12Pick + "', '" + game13Pick + "', '" + game14Pick + "', '" + game15Pick + "')", connection);
            cmd.ExecuteNonQuery();

            successText = "Data saved successfuly...!";
            SendMail();
            Response.Redirect("/football/thepool/2023/18-the18thweek.aspx");



        }

        catch (Exception ex)
        {
            successText = "Failed due to" + ex.Message;
        }
        finally
        {
            connection.Close();
        }
         */


        

        string connectionString = CredentialStore.PoolConnectionString;

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            // Use parameterized query to prevent SQL injection
            MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO eighteen2024 (firstname, lastname, email, game1, game2, game3, game4, game5, game6, game7, game8, game9, game10, game11, game12, game13, game14, game15, game16) " +
                "VALUES (@firstname, @lastname, @email, @game1, @game2, @game3, @game4, @game5, @game6, @game7, @game8, @game9, @game10, @game11, @game12, @game13, @game14, @game15, @game16)", connection);

            // Add parameters
            cmd.Parameters.AddWithValue("@firstname", firstnameText);
            cmd.Parameters.AddWithValue("@lastname", lastnameText);
            cmd.Parameters.AddWithValue("@email", emailText);
            cmd.Parameters.AddWithValue("@game1", game0Pick);
            cmd.Parameters.AddWithValue("@game2", game1Pick);
            cmd.Parameters.AddWithValue("@game3", game2Pick);
            cmd.Parameters.AddWithValue("@game4", game3Pick);
            cmd.Parameters.AddWithValue("@game5", game4Pick);
            cmd.Parameters.AddWithValue("@game6", game5Pick);
            cmd.Parameters.AddWithValue("@game7", game6Pick);
            cmd.Parameters.AddWithValue("@game8", game7Pick);
            cmd.Parameters.AddWithValue("@game9", game8Pick);
            cmd.Parameters.AddWithValue("@game10", game9Pick);
            cmd.Parameters.AddWithValue("@game11", game10Pick);
            cmd.Parameters.AddWithValue("@game12", game11Pick);
            cmd.Parameters.AddWithValue("@game13", game12Pick);
            cmd.Parameters.AddWithValue("@game14", game13Pick);
            cmd.Parameters.AddWithValue("@game15", game14Pick);
            cmd.Parameters.AddWithValue("@game16", game15Pick);

            cmd.ExecuteNonQuery();

            successText = "Data saved successfully!";
            SendMail();
            Response.Redirect("/football/thepool/2024/18-happynewyear.aspx");
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

    protected void SendMail()
    {
        // Gmail Address from where you send the mail
        var fromAddress = CredentialStore.EmailAddress;
        // any address where the email will be sending
        var toAddress = emailText;
        var ccAddress = CredentialStore.CcAddress;
        toAddress += "," + ccAddress;



        //Password of your gmail address
        var fromPassword = CredentialStore.EmailPassword;
        // Passing the values and make a email formate to display
        string subject = "Week 18 - 2024 Football Picks";
        string body = firstnameText + " " + lastnameText +" picks for week 18: \n\n";

        string[] gamePicksArray = gamePicks.ToArray();

        for (int i = 0; i < numberofgames; i++)
        {
            body += showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + " vs. " + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + ": " + gamePicksArray[i] + "\n";
        }
        body += $"\n Picks Link: {CredentialStore.BaseUrl}/football/thepool/2024/18-happynewyear.aspx \n";
        // body += "Subject: " + YourSubject.Text + "\n";
        // body += "Question: \n" + Comments.Text + "\n";
        // smtp settings
        var smtp = new System.Net.Mail.SmtpClient();
        {
            smtp.Host = CredentialStore.SmtpHost;
            smtp.Port = 587;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
            smtp.Timeout = 20000;

        }
        // Passing values to smtp object
        smtp.Send(fromAddress, toAddress, subject, body);
    }

    private CredentialCache GetCredential()
    {
        string url = $"{CredentialStore.ApiBaseUrl}/2024-regular/full_game_schedule.json?date=from-20250104-to-20250105";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
        CredentialCache credentialCache = new CredentialCache();
        credentialCache.Add(new System.Uri(url), "Basic", CredentialStore.ApiCredential);
        return credentialCache;
    }



}

