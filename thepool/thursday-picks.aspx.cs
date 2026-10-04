using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using nflgames;

/// <summary>
/// Pick form for the games played before Sunday (usually just the Thursday game).
/// Name only, no email. The pick is stored in the same weekly table and row that
/// the Sunday form uses, matched by first and last name.
/// </summary>
public partial class _ThursdayPicks : System.Web.UI.Page
{
    public NFLschedule showall;
    public int numberofgames;
    public int earlyGameCount;

    public bool Submitted;
    public string successText = string.Empty;
    public string firstnameText = string.Empty;
    public string lastnameText = string.Empty;

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
    }

    protected void SubmitForm(object sender, EventArgs e)
    {
        if (CurrentWeek.EarlyPicksClosed)
        {
            successText = "The Thursday game closed at " + CurrentWeek.EarlyPicksCloseText + ". Your pick was not saved.";
            return;
        }

        firstnameText = (firstnameEntry.Text ?? string.Empty).Trim();
        lastnameText = (lastnameEntry.Text ?? string.Empty).Trim();

        if (firstnameText.Length == 0 || lastnameText.Length == 0)
        {
            successText = "Please enter your first and last name.";
            return;
        }

        var picks = new Dictionary<int, string>();
        for (int i = 0; i < numberofgames && i < PickStore.MaxGames; i++)
        {
            if (!ScheduleHelper.IsEarlyGame(showall.fullgameschedule.gameentry[i]))
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

        if (picks.Count == 0)
        {
            successText = "There are no games before Sunday this week, so there is nothing to pick here.";
            return;
        }

        try
        {
            // Email is null so an address already saved for this name is left alone.
            PickStore.SavePicks(firstnameText, lastnameText, null, picks);
            Submitted = true;
        }
        catch (Exception ex)
        {
            successText = "Failed due to: " + ex.Message;
        }
    }
}
