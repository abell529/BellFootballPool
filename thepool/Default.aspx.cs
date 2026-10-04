using System;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using nflgames;

/// <summary>
/// Sunday/Monday pick form. Games played before Sunday are picked on thursday-picks.aspx,
/// so this page hides them and only writes the remaining game columns. If the person already
/// has a row for this week (from the Thursday page) it is updated; otherwise a new row is inserted.
/// Which week this is comes from App_Code/CurrentWeek.cs.
/// </summary>
public partial class _Default : System.Web.UI.Page
{
    public NFLschedule showall;
    public int numberofgames;
    public int earlyGameCount;

    public List<string> theFirstName = new List<string>();
    public List<string> theLastName = new List<string>();
    public List<string> theTotal = new List<string>();

    public string successText = string.Empty;
    public string emailText = string.Empty;
    public string firstnameText = string.Empty;
    public string lastnameText = string.Empty;

    private Dictionary<int, string> savedPicks = new Dictionary<int, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        using (var client = new WebClient { Credentials = CredentialStore.ApiCredential })
        {
            var response = client.DownloadString(CurrentWeek.ScheduleUrl);
            showall = JsonConvert.DeserializeObject<NFLschedule>(response);
        }

        numberofgames = showall?.fullgameschedule?.gameentry?.Length ?? 0;

        earlyGameCount = 0;
        for (int i = 0; i < numberofgames; i++)
        {
            if (ScheduleHelper.IsEarlyGame(showall.fullgameschedule.gameentry[i]))
            {
                earlyGameCount++;
            }
        }

        LoadStandings();
    }

    private void LoadStandings()
    {
        theFirstName.Clear();
        theLastName.Clear();
        theTotal.Clear();

        try
        {
            using (var connection = new MySqlConnection(CredentialStore.ScoresConnectionString))
            {
                connection.Open();
                var sql = $"SELECT firstname, lastname, IFNULL(total, 0) AS total FROM `{CurrentWeek.StandingsTable}` ORDER BY total DESC, lastname ASC, firstname ASC";
                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        theFirstName.Add(reader["firstname"].ToString());
                        theLastName.Add(reader["lastname"].ToString());
                        theTotal.Add(reader["total"].ToString());
                    }
                }
            }
        }
        catch (MySqlException)
        {
            // Standings table not populated yet; the sidebar just shows no names.
        }
    }

    protected void SubmitForm(object sender, EventArgs e)
    {
        if (CurrentWeek.PicksClosed)
        {
            successText = "Picks for " + CurrentWeek.Label + " closed at " + CurrentWeek.PicksCloseText + ". Your picks were not saved.";
            return;
        }

        firstnameText = (firstnameEntry.Text ?? string.Empty).Trim();
        lastnameText = (lastnameEntry.Text ?? string.Empty).Trim();
        emailText = (emailEntry.Text ?? string.Empty).Trim();

        if (firstnameText.Length == 0 || lastnameText.Length == 0)
        {
            successText = "Please enter your first and last name.";
            return;
        }

        // Only the games shown on this page are written. Column numbers are 1-based and
        // follow the feed order, so they line up with the Thursday page and the results page.
        var picks = new Dictionary<int, string>();
        for (int i = 0; i < numberofgames && i < PickStore.MaxGames; i++)
        {
            if (ScheduleHelper.IsEarlyGame(showall.fullgameschedule.gameentry[i]))
            {
                continue;
            }

            var pick = Request.Form["Game" + i + "t"];
            if (string.IsNullOrWhiteSpace(pick))
            {
                successText = "Please pick a team for every game.";
                return;
            }

            picks[i + 1] = pick.Trim();
        }

        try
        {
            PickStore.SavePicks(firstnameText, lastnameText, emailText, picks);
            savedPicks = PickStore.GetPicks(firstnameText, lastnameText);
        }
        catch (Exception ex)
        {
            successText = "Failed due to: " + ex.Message;
            return;
        }

        try
        {
            SendMail();
        }
        catch (Exception ex)
        {
            successText = "Your picks were saved, but the confirmation email could not be sent: " + ex.Message;
            return;
        }

        Response.Redirect(CurrentWeek.ResultsPage);
    }

    protected void SendMail()
    {
        var fromAddress = CredentialStore.EmailAddress;
        var toAddress = emailText + "," + CredentialStore.CcAddress;
        var fromPassword = CredentialStore.EmailPassword;

        string subject = $"Week {CurrentWeek.Number} - {CurrentWeek.Year} Football Picks";
        string body = $"{firstnameText} {lastnameText} picks for week {CurrentWeek.Number}: \n\n";

        for (int i = 0; i < numberofgames && i < PickStore.MaxGames; i++)
        {
            var game = showall.fullgameschedule.gameentry[i];
            string pick;
            savedPicks.TryGetValue(i + 1, out pick);
            pick = (pick ?? string.Empty).Trim();

            string note = string.Empty;
            if (ScheduleHelper.IsEarlyGame(game))
            {
                note = pick.Length == 0
                    ? $" (no {ScheduleHelper.DayName(game)} pick was entered)"
                    : $" ({ScheduleHelper.DayName(game)} pick)";
            }

            body += $"{game.awayTeam.Abbreviation} vs. {game.homeTeam.Abbreviation}: {pick}{note}\n";
        }

        body += $"\n Picks Link: {CredentialStore.BaseUrl}{CurrentWeek.ResultsPage} \n";

        var smtp = new System.Net.Mail.SmtpClient
        {
            Host = CredentialStore.SmtpHost,
            Port = 587,
            DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(fromAddress, fromPassword),
            Timeout = 20000
        };

        smtp.Send(fromAddress, toAddress, subject, body);
    }
}
