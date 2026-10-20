using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using nflgames;

/// <summary>
/// Admin page for entering picks that arrived by email.
/// - Defaults to the current week (from App_Code/CurrentWeek.cs) but any week's table and dates can be typed in.
/// - Never closes: there is no cutoff check here.
/// - Saves only the Sunday/Monday games unless "include pre-Sunday games" is ticked.
/// - Sends no confirmation email.
/// - If Web.config has an AdminKey setting, the page only works with ?key=THAT_VALUE on the address.
/// </summary>
public partial class _AdminEntry : System.Web.UI.Page
{
    public bool Authorized;
    public bool KeyConfigured;

    public NFLschedule showall;
    public int numberofgames;
    public string loadError = string.Empty;
    public string successText = string.Empty;
    public bool saveFailed;
    public List<string> savedSummary = new List<string>();

    public string TableName { get { return PicksTableBox.Text.Trim(); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        var key = ConfigurationManager.AppSettings["AdminKey"] ?? string.Empty;
        KeyConfigured = key.Trim().Length > 0;
        Authorized = !KeyConfigured || string.Equals(Request.QueryString["key"] ?? string.Empty, key.Trim(), StringComparison.Ordinal);

        if (!Authorized)
        {
            return;
        }

        if (!IsPostBack)
        {
            PicksTableBox.Text = CurrentWeek.PicksTable;
            FromBox.Text = CurrentWeek.ScheduleFrom;
            ToBox.Text = CurrentWeek.ScheduleTo;
        }

        LoadSchedule();
    }

    private void LoadSchedule()
    {
        showall = null;
        numberofgames = 0;

        var table = TableName;
        var from = FromBox.Text.Trim();
        var to = ToBox.Text.Trim();

        if (!Regex.IsMatch(table, "^[A-Za-z0-9_]+$"))
        {
            loadError = "The picks table name can only contain letters, digits and underscores.";
            return;
        }

        if (!Regex.IsMatch(from, @"^\d{8}$") || !Regex.IsMatch(to, @"^\d{8}$"))
        {
            loadError = "Dates must be eight digits, yyyyMMdd, for example 20261015.";
            return;
        }

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var url = CredentialStore.ApiBaseUrl + "/" + CurrentWeek.SeasonSegment + "/full_game_schedule.json?date=from-" + from + "-to-" + to;
            using (var client = new WebClient { Credentials = CredentialStore.ApiCredential })
            {
                showall = JsonConvert.DeserializeObject<NFLschedule>(client.DownloadString(url));
            }

            numberofgames = showall != null && showall.fullgameschedule != null && showall.fullgameschedule.gameentry != null
                ? showall.fullgameschedule.gameentry.Length
                : 0;

            if (numberofgames == 0)
            {
                loadError = "The schedule feed returned no games for " + from + " to " + to + ".";
            }
        }
        catch (Exception ex)
        {
            loadError = "Could not load the schedule: " + ex.Message;
        }
    }

    protected void LoadButton_Click(object sender, EventArgs e)
    {
        // Page_Load has already reloaded the schedule with the values in the boxes.
        successText = string.Empty;
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (!Authorized || showall == null || numberofgames == 0)
        {
            return;
        }

        var first = (FirstNameBox.Text ?? string.Empty).Trim();
        var last = (LastNameBox.Text ?? string.Empty).Trim();
        var email = (EmailBox.Text ?? string.Empty).Trim();

        if (first.Length == 0 || last.Length == 0)
        {
            saveFailed = true;
            successText = "Please enter the person's first and last name.";
            return;
        }

        bool includeEarly = IncludeEarlyBox.Checked;
        var picks = new Dictionary<int, string>();
        int skippedEarly = 0, blank = 0;

        for (int i = 0; i < numberofgames && i < PickStore.MaxGames; i++)
        {
            var entry = showall.fullgameschedule.gameentry[i];
            if (ScheduleHelper.IsEarlyGame(entry) && !includeEarly)
            {
                skippedEarly++;
                continue;
            }

            var pick = Request.Form["Game" + i + "t"];
            if (string.IsNullOrWhiteSpace(pick))
            {
                blank++;
                continue;
            }

            picks[i + 1] = pick.Trim();
        }

        if (picks.Count == 0)
        {
            saveFailed = true;
            successText = "No teams were selected, so nothing was saved.";
            return;
        }

        try
        {
            // Blank email leaves any stored address alone on an existing row.
            bool updated = PickStore.SavePicks(TableName, first, last, email.Length == 0 ? null : email, picks);
            var stored = PickStore.GetPicks(TableName, first, last);

            var message = new StringBuilder();
            message.Append("Saved ").Append(picks.Count).Append(picks.Count == 1 ? " pick" : " picks").Append(" for ").Append(first).Append(' ').Append(last)
                   .Append(" in ").Append(TableName).Append(updated ? " (existing row updated)." : " (new row added).");
            if (blank > 0)
            {
                message.Append(' ').Append(blank).Append(blank == 1 ? " game was" : " games were").Append(" left blank and not changed.");
            }
            if (skippedEarly > 0)
            {
                message.Append(' ').Append(skippedEarly).Append(" pre-Sunday game").Append(skippedEarly == 1 ? " was" : "s were").Append(" skipped (tick the box to include them).");
            }
            successText = message.ToString();

            for (int i = 0; i < numberofgames && i < PickStore.MaxGames; i++)
            {
                var entry = showall.fullgameschedule.gameentry[i];
                string value;
                stored.TryGetValue(i + 1, out value);
                savedSummary.Add(entry.awayTeam.Abbreviation + " at " + entry.homeTeam.Abbreviation + ": " + (string.IsNullOrWhiteSpace(value) ? "(blank)" : value));
            }
        }
        catch (Exception ex)
        {
            saveFailed = true;
            successText = "Save failed: " + ex.Message;
        }
    }

    /// <summary>Full team name as stored in the picks table, "City Name".</summary>
    public static string TeamName(string city, string name)
    {
        return ((city ?? string.Empty) + " " + (name ?? string.Empty)).Trim();
    }

    /// <summary>The value the admin had selected before this postback, so the choice survives a reload.</summary>
    public string Previous(int index)
    {
        return Request.Form["Game" + index + "t"] ?? string.Empty;
    }
}
