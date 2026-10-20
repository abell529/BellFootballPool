<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin-entry.aspx.cs" Inherits="_AdminEntry" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="robots" content="noindex, nofollow">
    <title>Admin pick entry</title>
    <style type="text/css">
        body { font-family: Arial, Helvetica, sans-serif; margin: 20px; background: #f7f7f7; color: #222; }
        h1 { font-size: 22px; margin: 0 0 4px; }
        .lead { color: #555; margin: 0 0 18px; }
        .box { background: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 14px 16px; margin-bottom: 16px; max-width: 760px; }
        .box h2 { font-size: 14px; text-transform: uppercase; letter-spacing: .08em; color: #666; margin: 0 0 10px; }
        .row { display: flex; flex-wrap: wrap; gap: 10px 18px; align-items: flex-end; }
        .field label { display: block; font-size: 12px; color: #555; margin-bottom: 3px; }
        .field input[type=text] { padding: 6px 8px; border: 1px solid #bbb; border-radius: 4px; font-size: 14px; }
        .btn { padding: 7px 14px; border: 1px solid #0b5fa5; background: #0b5fa5; color: #fff; border-radius: 4px; font-size: 14px; cursor: pointer; }
        .btn.secondary { background: #fff; color: #0b5fa5; }
        .game { display: grid; grid-template-columns: 28px 1fr 28px 1fr auto; gap: 8px; align-items: center; padding: 7px 4px; border-bottom: 1px solid #eee; }
        .game:last-child { border-bottom: 0; }
        .game.early { background: #fff8e6; }
        .game .n { color: #888; font-size: 12px; text-align: right; }
        .game .at { color: #888; font-size: 12px; text-align: center; }
        .game label { display: flex; align-items: center; gap: 6px; cursor: pointer; }
        .game .when { font-size: 12px; color: #777; white-space: nowrap; }
        .game .when b { color: #a66b00; }
        .msg { padding: 10px 12px; border-radius: 6px; margin: 0 0 14px; max-width: 760px; }
        .msg.ok { background: #e8f6ea; border: 1px solid #9fd3a6; color: #1d5c29; }
        .msg.bad { background: #fdecec; border: 1px solid #f0a8a8; color: #8a1f1f; }
        .msg.warn { background: #fff4d6; border: 1px solid #f0d58a; color: #6b4f00; }
        .summary { columns: 2; column-gap: 24px; font-size: 13px; margin: 8px 0 0; padding-left: 18px; }
        .note { font-size: 12px; color: #777; }
        @media (max-width: 560px) { .game { grid-template-columns: 24px 1fr; } .game .at, .game .when { display: none; } .summary { columns: 1; } }
    </style>
</head>
<body>
    <h1>Admin pick entry</h1>
    <p class="lead">For picks that came in by email. No cutoff, no confirmation email. Saved by name, so an existing row for that person is updated.</p>

<% if (!Authorized) { %>
    <div class="msg bad">This page needs the admin key. Open it as <code>admin-entry.aspx?key=YOUR_KEY</code> using the AdminKey value from Web.config.</div>
<% } else { %>
<% if (!KeyConfigured) { %>
    <div class="msg warn">No AdminKey is set in Web.config, so anyone who finds this address can use it. Add <code>&lt;add key="AdminKey" value="..." /&gt;</code> to appSettings to lock it down.</div>
<% } %>
    <form id="form1" runat="server">
        <div class="box">
            <h2>Week</h2>
            <div class="row">
                <div class="field"><label for="PicksTableBox">Picks table</label><asp:TextBox runat="server" ID="PicksTableBox" /></div>
                <div class="field"><label for="FromBox">First game (yyyyMMdd)</label><asp:TextBox runat="server" ID="FromBox" /></div>
                <div class="field"><label for="ToBox">Last game (yyyyMMdd)</label><asp:TextBox runat="server" ID="ToBox" /></div>
                <asp:Button runat="server" ID="LoadButton" CssClass="btn secondary" Text="Load games" OnClick="LoadButton_Click" />
            </div>
            <p class="note">Defaults to the current week. For an earlier week, type its table (one2026, two2026, ...) and its Thursday-to-Monday dates, then Load games.</p>
        </div>

<% if (loadError.Length > 0) { %>
        <div class="msg bad"><%= Server.HtmlEncode(loadError) %></div>
<% } %>
<% if (successText.Length > 0) { %>
        <div class="msg <%= saveFailed ? "bad" : "ok" %>">
            <%= Server.HtmlEncode(successText) %>
<% if (savedSummary.Count > 0) { %>
            <ol class="summary">
<% foreach (var line in savedSummary) { %>
                <li><%= Server.HtmlEncode(line) %></li>
<% } %>
            </ol>
<% } %>
        </div>
<% } %>

<% if (showall != null && numberofgames > 0) { %>
        <div class="box">
            <h2>Person</h2>
            <div class="row">
                <div class="field"><label for="FirstNameBox">First name</label><asp:TextBox runat="server" ID="FirstNameBox" /></div>
                <div class="field"><label for="LastNameBox">Last name</label><asp:TextBox runat="server" ID="LastNameBox" /></div>
                <div class="field"><label for="EmailBox">Email (optional)</label><asp:TextBox runat="server" ID="EmailBox" /></div>
            </div>
            <p class="note">Spell the name the way it appears on the results page so it lands on the same row and matches the scoreboard.</p>
        </div>

        <div class="box">
            <h2>Picks for <%= Server.HtmlEncode(TableName) %></h2>
<%
    for (int i = 0; i < numberofgames; i++)
    {
        var g = showall.fullgameschedule.gameentry[i];
        bool early = ScheduleHelper.IsEarlyGame(g);
        string away = TeamName(g.awayTeam.City, g.awayTeam.Name);
        string home = TeamName(g.homeTeam.City, g.homeTeam.Name);
        string prev = Previous(i);
%>
            <div class="game<%= early ? " early" : "" %>">
                <span class="n"><%= i + 1 %></span>
                <label><input type="radio" name="Game<%= i %>t" value="<%= Server.HtmlEncode(away) %>"<%= prev == away ? " checked" : "" %>> <%= Server.HtmlEncode(away) %></label>
                <span class="at">at</span>
                <label><input type="radio" name="Game<%= i %>t" value="<%= Server.HtmlEncode(home) %>"<%= prev == home ? " checked" : "" %>> <%= Server.HtmlEncode(home) %></label>
                <span class="when"><%= Server.HtmlEncode(ScheduleHelper.DayName(g)) %> <%= Server.HtmlEncode(g.time ?? "") %><%= early ? " <b>pre-Sunday</b>" : "" %></span>
            </div>
<%
    }
%>
            <p style="margin:14px 0 0;"><asp:CheckBox runat="server" ID="IncludeEarlyBox" Text=" Also save the pre-Sunday game(s) marked above (normally entered on the Thursday page)" /></p>
            <p style="margin:12px 0 0;"><asp:Button runat="server" ID="SaveButton" CssClass="btn" Text="Save picks" OnClick="SaveButton_Click" /></p>
            <p class="note">Games left blank are not changed. Picks are stored in the same columns the pick form uses, so the results pages read them the same way.</p>
        </div>
<% } %>
    </form>
<% } %>
</body>
</html>
