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

public partial class scoreboard_2013 : System.Web.UI.Page
{
    public int numberofpeople;

    public List<string> scoresFirstName = new List<string>();
    public List<string> scoresLastName = new List<string>();


    public List<string> week0Correct = new List<string>();
    public List<string> week1Correct = new List<string>();
    public List<string> week2Correct = new List<string>();
    public List<string> week3Correct = new List<string>();
    public List<string> week4Correct = new List<string>();
    public List<string> week5Correct = new List<string>();
    public List<string> week6Correct = new List<string>();
    public List<string> week7Correct = new List<string>();
    public List<string> week8Correct = new List<string>();
    public List<string> week9Correct = new List<string>();
    public List<string> week10Correct = new List<string>();
    public List<string> week11Correct = new List<string>();
    public List<string> week12Correct = new List<string>();
    public List<string> week13Correct = new List<string>();
    public List<string> week14Correct = new List<string>();
    public List<string> week15Correct = new List<string>();
    public List<string> week16Correct = new List<string>();
    public List<string> week17Correct = new List<string>();
    public List<string> week18Correct = new List<string>();

    public List<string> totalCorrect = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {

        string connectionString = "Server=mysql24.ezhostingserver.com;Database=bfscoresDB;User ID=MyName69;Password=ThePassword69;";


        // First connection (for scores.mdb equivalent)
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        MySqlCommand command = new MySqlCommand("SELECT * FROM `2013` ORDER BY total DESC", connection);
        MySqlDataReader reader = command.ExecuteReader();




        /* OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\home\bellfootball.com\wwwroot\BFDB\scores.mdb");
        connection.Open();
        OleDbDataReader reader = null;
        OleDbCommand command = new OleDbCommand("SELECT * from 2023 ORDER BY total DESC", connection);
        reader = command.ExecuteReader(); */

        while (reader.Read())
        {
            scoresFirstName.Add(reader["firstname"].ToString());
            scoresLastName.Add(reader["lastname"].ToString());
            week1Correct.Add(reader["week1"].ToString());
            week2Correct.Add(reader["week2"].ToString());
            week3Correct.Add(reader["week3"].ToString());
            week4Correct.Add(reader["week4"].ToString());
            week5Correct.Add(reader["week5"].ToString());
            week6Correct.Add(reader["week6"].ToString());
            week7Correct.Add(reader["week7"].ToString());
            week8Correct.Add(reader["week8"].ToString());
            week9Correct.Add(reader["week9"].ToString());
            week10Correct.Add(reader["week10"].ToString());
            week11Correct.Add(reader["week11"].ToString());
            week12Correct.Add(reader["week12"].ToString());
            week13Correct.Add(reader["week13"].ToString());
            week14Correct.Add(reader["week14"].ToString());
            week15Correct.Add(reader["week15"].ToString());
            week16Correct.Add(reader["week16"].ToString());
            week17Correct.Add(reader["week17"].ToString());
            totalCorrect.Add(reader["total"].ToString());

        }

        // Close connections
        reader.Close();

        connection.Close();
    }
}