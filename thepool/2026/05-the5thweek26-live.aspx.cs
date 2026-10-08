using System;
using System.Text;
using System.Web;
using Newtonsoft.Json;

/// <summary>
/// Live version of the week 5 results page.
/// A normal request returns the page shell. The page's script then asks this same
/// address for "?data=1" every few seconds and redraws itself from the JSON, so nobody
/// has to refresh. Scores come from ESPN through App_Code/EspnScoreboard.cs (shared and
/// cached for all viewers); game order and picks come from the usual feed and database.
/// </summary>
public partial class _2026_05_the5thweek26_live : System.Web.UI.Page
{
    private static readonly LiveWeekConfig Config = new LiveWeekConfig
    {
        Year = 2026,
        Week = 5,
        SeasonSegment = "2026-regular",
        PicksTable = "five2026",
        ScheduleFrom = "20261008",
        ScheduleTo = "20261012",
        ScoresTable = "2026",
        ClassicPage = "05-the5thweek26.aspx"
    };

    public int WeekNumber { get { return Config.Week; } }
    public int Year { get { return Config.Year; } }
    public string ClassicPage { get { return Config.ClassicPage; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["data"] != "1")
        {
            return;
        }

        string json;
        int status = 200;

        try
        {
            json = JsonConvert.SerializeObject(LiveWeekBuilder.Build(Config));
        }
        catch (Exception ex)
        {
            // Keep details (connection strings, user names) out of a public response.
            status = 500;
            json = JsonConvert.SerializeObject(new { error = "Live data could not be loaded (" + ex.GetType().Name + ")." });
        }

        Response.Clear();
        Response.StatusCode = status;
        Response.TrySkipIisCustomErrors = true;
        Response.ContentType = "application/json";
        Response.ContentEncoding = Encoding.UTF8;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Write(json);
        Response.Flush();
        Response.SuppressContent = true;
        Context.ApplicationInstance.CompleteRequest();
    }
}
