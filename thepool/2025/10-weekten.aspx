<%@ Page Language="C#" AutoEventWireup="true" CodeFile="10-weekten.aspx.cs" Debug="true" Inherits="_2025_10_weekten" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
                Week 10 2025 -- BellFusion Football Pool
        </title>

    <link rel="stylesheet" href="https://use.typekit.net/oct3isk.css">

    <!--- Start style sheet for text sizes --->
    <style type="text/css">
        .divTable {
            display: table;
            width: 100%;
        }

        .divTableRow {
            display: table-row;
        }

        .divTableHeading {
            background-color: #EEE;
            display: table-header-group;
        }

        .divTableCell, .divTableHead {
            border: 1px solid #017AA4;
            display: table-cell;
            padding: 3px 10px;
            text-align: center;
        }

        .divTableHeading {
            background-color: #EEE;
            display: table-header-group;
            font-weight: bold;
        }

        .divTableFoot {
            background-color: #EEE;
            display: table-footer-group;
            font-weight: bold;
        }

        .divTableBody {
            display: table-row-group;
        }

        .win {
            color: #009900;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size: .95rem;
        }

        .loss {
            color: #990000;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size: .95rem;
        }

        .home {
            color: #000000;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size: .95rem;
        }

        .away {
            color: #909090;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size: .95rem;
        }

        h1 {
            font-family: ardoise-narrow-std, sans-serif;
            font-weight: 200;
        }

        p {
            font-family: ardoise-narrow-std, sans-serif;
            font-weight: 200;
        }

        .tk-ardoise-compact-std {
            font-size: 1.25rem;
        }

        .tk-ardoise-narrow-std {
            font-size: 1.025rem;
        }

        .smaller-font {
            font-size: 1.025rem;
        }

        .light-font {
            font-weight: 200;
        }

        .regular-font {
            font-weight: 400;
        }

        bold-font {
            font-weight: 600;
        }

        .heavy-font {
            font-weight: 800;
        }
    </style>
    <!--- End style sheet for text sizes --->

    <!-- Global site tag (gtag.js) - Google Analytics -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=UA-109929635-1"></script>
    <script>
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'UA-109929635-1');
    </script>
</head>
<body>
    <h1>2025 Week 10: BellFusion Football Pool Picks</h1>
    <p>
        Names are listed in alphabetical order (by last name).<br />
        Key: <strong><span class="win">Win</span>, <span class="loss">Loss</span>, <span class="home">Home</span>, <span class="away">Away</span></strong><br />
    </p>

    <div class="divTable" style="width: 100%; border: 1px solid #000;">
        <div class="divTableBody">
            <div class="divTableRow">
                <div class="divTableCell">&nbsp;</div>
                <div class="divTableCell">&nbsp;</div>
                <div class="divTableCell">&nbsp;</div>
                <%
                    int scoreboardIndex = 0;
                    foreach (var group in DayGroups)
                    {
                        foreach (var game in group.Games)
                        {
                            string background = (scoreboardIndex % 2 == 0) ? "#ffffff" : "#eae9e5";
                %>
                <div class="divTableCell" style='background:<%= background %>; text-align:center;'>
                    <div class='tk-ardoise-compact-std light-font smaller-font'><%= Server.HtmlEncode(game.Schedule.awayTeam.City) %> <span class='heavy-font'><%= Server.HtmlEncode(game.AwayScore) %></span></div>
                    <div class='tk-ardoise-compact-std regular-font'>vs.</div>
                    <div class='tk-ardoise-compact-std light-font smaller-font'><%= Server.HtmlEncode(game.Schedule.homeTeam.City) %> <span class='heavy-font'><%= Server.HtmlEncode(game.HomeScore) %></span></div>
                    <div class='tk-ardoise-compact-std light-font smaller-font'><%= Server.HtmlEncode(group.DayName) %></div>
                </div>
                <%
                            scoreboardIndex++;
                        }
                    }
                %>
            </div>

            <div class="divTableRow">
                <div class="divTableCell tk-ardoise-compact-std heavy-font">NAME</div>
                <div class="divTableCell tk-ardoise-compact-std heavy-font" style="background:#f1f1c1;">WEEK</div>
                <div class="divTableCell tk-ardoise-compact-std heavy-font" style="background:#c1f1f1;">YTD</div>
                <%
                    for (int i = 0; i < NumberOfGames; i++)
                    {
                        string background = (i % 2 == 0) ? "#ffffff" : "#eae9e5";
                %>
                <div class="divTableCell tk-ardoise-compact-std heavy-font" style='background:<%= background %>; text-align:center;'><%= (i + 1).ToString() %></div>
                <%
                    }
                %>
            </div>

            <%
                foreach (var participant in Participants)
                {
            %>
            <div class="divTableRow tk-ardoise-narrow-std regular-font">
                <div class="divTableCell bold-font" style="white-space:nowrap;text-align:left;"><%= Server.HtmlEncode(participant.FullName) %></div>
                <div class="divTableCell" style="background:#f1f1c1; text-align:center;"><%= participant.WeeklyScore %></div>
                <div class="divTableCell" style="background:#c1f1f1; text-align:center;"><%= Server.HtmlEncode(participant.YtdTotal) %></div>
                <%
                    for (int i = 0; i < participant.Picks.Count; i++)
                    {
                        string background = (i % 2 == 0) ? "#ffffff" : "#eae9e5";
                        var pick = participant.Picks[i];
                %>
                <div class="divTableCell" style='background:<%= background %>;'>
                    <% if (!string.IsNullOrEmpty(pick.CssClass)) { %>
                        <span class='<%= pick.CssClass %>'><%= Server.HtmlEncode(pick.DisplayText) %></span>
                    <% } else { %>
                        <%= Server.HtmlEncode(pick.DisplayText) %>
                    <% } %>
                </div>
                <%
                    }
                %>
            </div>
            <%
                }
            %>
        </div>
    </div>
</body>
</html>
