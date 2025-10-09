<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weeklyScoreEntry.aspx.cs" Inherits="scoreboard_weeklyScoreEntry" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2025 Weekly Score Entry</title>
    <style type="text/css">
        body {
            font-family: Arial, Helvetica, sans-serif;
            margin: 20px;
            background-color: #f7f7f7;
        }

        h1 {
            font-weight: 300;
            color: #0a4a6b;
        }

        .form-container {
            background-color: #ffffff;
            border: 1px solid #d0d0d0;
            border-radius: 6px;
            padding: 20px;
            margin-bottom: 20px;
            max-width: 720px;
        }

        .form-row {
            display: flex;
            flex-wrap: wrap;
            margin-bottom: 12px;
        }

        .form-row label {
            flex: 0 0 180px;
            font-weight: 600;
            color: #333333;
            margin-right: 12px;
        }

        .form-row input[type="text"] {
            flex: 1 1 240px;
            padding: 6px 8px;
            border: 1px solid #b0b0b0;
            border-radius: 4px;
            font-size: 14px;
        }

        .form-actions {
            margin-top: 20px;
        }

        .form-actions asp\:Button,
        .form-actions input[type="submit"] {
            margin-right: 10px;
        }

        .status-message {
            margin-bottom: 16px;
            font-size: 14px;
        }

        .status-message .success {
            color: #0a7d36;
        }

        .status-message .error {
            color: #b00020;
        }

        .grid-container {
            background-color: #ffffff;
            border: 1px solid #d0d0d0;
            border-radius: 6px;
            padding: 16px;
            max-width: 960px;
        }

        .grid-container table {
            width: 100%;
            border-collapse: collapse;
        }

        .grid-container th,
        .grid-container td {
            border: 1px solid #d0d0d0;
            padding: 8px 10px;
            text-align: left;
        }

        .grid-container th {
            background-color: #0a4a6b;
            color: #ffffff;
        }

        .grid-container tr:nth-child(even) td {
            background-color: #f0f6f9;
        }

        .missing-flag {
            color: #b00020;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>2025 Weekly Score Entry</h1>
        <div class="form-container">
            <div class="form-row">
                <label for="WeekNumberTextBox">Week number</label>
                <asp:TextBox runat="server" ID="WeekNumberTextBox" />
            </div>
            <div class="form-row">
                <label for="PicksTableTextBox">Picks table name</label>
                <asp:TextBox runat="server" ID="PicksTableTextBox" />
            </div>
            <div class="form-row">
                <label for="StartDateTextBox">Schedule start date (YYYY-MM-DD)</label>
                <asp:TextBox runat="server" ID="StartDateTextBox" />
            </div>
            <div class="form-row">
                <label for="EndDateTextBox">Schedule end date (YYYY-MM-DD)</label>
                <asp:TextBox runat="server" ID="EndDateTextBox" />
            </div>
            <div class="form-actions">
                <asp:Button runat="server" ID="LoadButton" Text="Load week data" OnClick="LoadButton_Click" />
                <asp:Button runat="server" ID="SaveButton" Text="Save scores" OnClick="SaveButton_Click" Enabled="false" />
            </div>
            <div class="status-message">
                <asp:Literal runat="server" ID="StatusMessage" />
            </div>
        </div>

        <div class="grid-container" runat="server" id="GridWrapper" visible="false">
            <asp:GridView runat="server" ID="ResultsGrid" AutoGenerateColumns="false" CssClass="results-grid">
                <Columns>
                    <asp:BoundField HeaderText="Participant" DataField="FullName" />
                    <asp:BoundField HeaderText="Weekly score" DataField="WeeklyScore" />
                    <asp:BoundField HeaderText="Existing week" DataField="ExistingWeekScore" />
                    <asp:BoundField HeaderText="Previous total" DataField="PreviousTotal" />
                    <asp:BoundField HeaderText="New total" DataField="NewTotal" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="<%# (bool)Eval("IsMissingFromScores") ? "missing-flag" : string.Empty %>">
                                <%# (bool)Eval("IsMissingFromScores") ? "Missing in 2025 table" : "Ready" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
