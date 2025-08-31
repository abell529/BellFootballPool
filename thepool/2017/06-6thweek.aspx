<%@ Page Language="C#" AutoEventWireup="true" CodeFile="06-6thweek.aspx.cs" Inherits="_2017_06_6thweek" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
		Week Six -- BellFusion Football Pool
	</title>
	
    
    <link rel="stylesheet" href="https://use.typekit.net/oct3isk.css">


    <!--- Start style sheet for text sizes --->
    <style type="text/css">
    
        .divTable{
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
            color:#009900;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size:.95rem;
        }
        .loss {
            color:#990000;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size:.95rem;
        }
        .home {
            color:#000000;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size:.95rem;
        }
        .away {
            color:#909090;
            font-family: ardoise-narrow-std, sans-serif;
            font-style: normal;
            font-weight: 800;
            font-size:.95rem;
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
            font-size:1.25rem;
        }

        .tk-ardoise-narrow-std {
            font-size:1.025rem;
        }

        .smaller-font {
            font-size:1.025rem;
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


</head>
<body>
   
    <h1>2017 Week 6: BellFusion Football Pool Picks</h1>
<p>
	Names are listed in alphabetical order (by last name).<br />
    Key: <strong><span class="win">Win</span>, <span class="loss">Loss</span>, <span class="home">Home</span>, <span class="away">Away</span></strong><br />

</p>

    <%
        
        // Response.Write("Testing:" + livescores2.scoreboard.gameScore[11].game.awayTeam.City);
        
        %>

</p>



<div class="divTable" style="width: 100%;border: 1px solid #000;" >
<div class="divTableBody">
<div class="divTableRow">
<div class="divTableCell">&nbsp;</div>
<div class="divTableCell">&nbsp;</div>
<div class="divTableCell">&nbsp;</div>


            <% for (int i = 0; i < numberofgames; i++)
                {
                    if (i % 2 == 0)
                    {
                        Response.Write("<div class='divTableCell' style = 'background:#ffffff; text-align:center;'>");
                    }
                    else
                    {
                        Response.Write("<div class='divTableCell' style = 'background:#eae9e5; text-align:center;'>");
                    }
                    if (i == 0)
                    {
                        Response.Write("<div class='tk-ardoise-compact-std light-font smaller-font'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " <span class='heavy-font'>" + livescores.scoreboard.gameScore[i].awayScore + "</span></div>");
                        Response.Write("<div class='tk-ardoise-compact-std regular-font'>vs.</div>");
                        Response.Write("<div class='tk-ardoise-compact-std light-font smaller-font'>" + showall.fullgameschedule.gameentry[i].homeTeam.City + " <span class='heavy-font'>" + livescores.scoreboard.gameScore[i].homeScore + "</span></div>");
                        Response.Write("</div>");
                    }
                    else if (i > 0 && i < 13)
                    {
                        Response.Write("<div class='tk-ardoise-compact-std light-font smaller-font'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " <span class='heavy-font'>" + livescores2.scoreboard.gameScore[i-1].awayScore + "</span></div>");
                        Response.Write("<div class='tk-ardoise-compact-std regular-font'>vs.</div>");
                        Response.Write("<div class='tk-ardoise-compact-std light-font smaller-font'>" + showall.fullgameschedule.gameentry[i].homeTeam.City + " <span class='heavy-font'>" + livescores2.scoreboard.gameScore[i-1].homeScore + "</span></div>");
                        Response.Write("</div>");
                    }
                    else
                    {
                        Response.Write("<div class='tk-ardoise-compact-std light-font smaller-font'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " <span class='heavy-font'>" + livescores3.scoreboard.gameScore[i-13].awayScore + "</span></div>");
                        Response.Write("<div class='tk-ardoise-compact-std regular-font'>vs.</div>");
                        Response.Write("<div class='tk-ardoise-compact-std light-font smaller-font'>" + showall.fullgameschedule.gameentry[i].homeTeam.City + " <span class='heavy-font'>" + livescores3.scoreboard.gameScore[i-13].homeScore + "</span></div>");
                        Response.Write("</div>");
                    }

                }

                Response.Write("</div>");
                Response.Write("<div class='divTableRow'>");

                Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font'>NAME</div>");
                Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font' style = 'background:#f1f1c1;'>WEEK</div>");
                Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font' style = 'background:#c1f1f1;'>YTD</div>");

                for (int i = 0; i < numberofgames; i++)
                {
                    if (i % 2 == 0)
                    {
                        Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font' style = 'background:#ffffff; text-align:center;'>" + (i+1) + "</div>");
                    }
                    else
                    {
                        Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font' style = 'background:#eae9e5; text-align:center;'>" + (i+1) + "</div>");
                    }
                }

                Response.Write("</div>");

                string[] theFirstNamePicks = pickFirstName.ToArray();
                string[] theLastNamePicks = pickLastName.ToArray();
                int[] theCorrectPicks = correctPicks.ToArray();
                string[] game0List = game0Pick.ToArray();
                string[] game1List = game1Pick.ToArray();
                string[] game2List = game2Pick.ToArray();
                string[] game3List = game3Pick.ToArray();
                string[] game4List = game4Pick.ToArray();
                string[] game5List = game5Pick.ToArray();
                string[] game6List = game6Pick.ToArray();
                string[] game7List = game7Pick.ToArray();
                string[] game8List = game8Pick.ToArray();
                string[] game9List = game9Pick.ToArray();
                string[] game10List = game10Pick.ToArray();
                string[] game11List = game11Pick.ToArray();
                string[] game12List = game12Pick.ToArray();
                string[] game13List = game13Pick.ToArray();
                // string[] game14List = game14Pick.ToArray();
                // string[] game15List = game15Pick.ToArray();



                for (int i = 0; i < theFirstNamePicks.Length; i++)
                {

                    Response.Write("<div class='divTableRow tk-ardoise-narrow-std regular-font'>");
                    Response.Write("<div class='divTableCell bold-font' style='white-space:nowrap;text-align:left;'>" + theFirstNamePicks[i] + " " + theLastNamePicks[i] + "</div>");
                    Response.Write("<div class='divTableCell' style = 'background:#f1f1c1; text-align:center;'>" + theCorrectPicks[i] + "</div>");
                    Response.Write("<div class='divTableCell' style = 'background:#c1f1f1; text-align:center;'>" + theCorrectPicks[i] + "</div>");
                    Response.Write("<div class='divTableCell' style = 'background:#ffffff;'>");

                    if (game0List[i] == (livescores.scoreboard.gameScore[0].game.awayTeam.City + " " + livescores.scoreboard.gameScore[0].game.awayTeam.Name))
                    {
                        int x;
                        int y;
                        Int32.TryParse(livescores.scoreboard.gameScore[0].awayScore, out x);
                        Int32.TryParse(livescores.scoreboard.gameScore[0].homeScore, out y);
                        if (x > y)
                        {
                            Response.Write("<span class='win'>" + livescores.scoreboard.gameScore[0].game.awayTeam.Abbreviation + "</span>");
                        }
                        else
                        {
                            Response.Write("<span class='loss'>" + livescores.scoreboard.gameScore[0].game.awayTeam.Abbreviation + "</span>");
                        }
                    }
                    else
                    {
                        int x;
                        int y;
                        Int32.TryParse(livescores.scoreboard.gameScore[0].awayScore, out x);
                        Int32.TryParse(livescores.scoreboard.gameScore[0].homeScore, out y);
                        if (x > y)
                        {
                            Response.Write("<span class='loss'>" + livescores.scoreboard.gameScore[0].game.homeTeam.Abbreviation + "</span>");
                        }
                        else
                        {
                            Response.Write("<span class='win'>" + livescores.scoreboard.gameScore[0].game.homeTeam.Abbreviation + "</span>");
                        }
                    }

                    string[] gamePicksList = { game2List[i], game3List[i], game4List[i], game5List[i], game6List[i], game7List[i], game1List[i], game8List[i], game9List[i], game10List[i], game11List[i], game12List[i] };



                    for (int j = 0; j < gamePicksList.Length; j++)
                    {
                        if (j % 2 == 0)
                        {
                            Response.Write("</div><div class='divTableCell' style = 'background:#eae9e5;'>");
                        }
                        else
                        {
                            Response.Write("</div><div class='divTableCell' style = 'background:#ffffff;'>");
                        }

                        if (gamePicksList[j] == (livescores2.scoreboard.gameScore[j].game.awayTeam.City + " " + livescores2.scoreboard.gameScore[j].game.awayTeam.Name))
                        {
                            int x;
                            int y;
                            Int32.TryParse(livescores2.scoreboard.gameScore[j].awayScore, out x);
                            Int32.TryParse(livescores2.scoreboard.gameScore[j].homeScore, out y);
                            if (x > y && (livescores2.scoreboard.gameScore[j].isCompleted == "true"))
                            {
                                Response.Write("<span class='win'>" + livescores2.scoreboard.gameScore[j].game.awayTeam.Abbreviation + "</span>");
                            }
                            else if (x < y && (livescores2.scoreboard.gameScore[j].isCompleted == "true"))
                            {
                                Response.Write("<span class='loss'>" + livescores2.scoreboard.gameScore[j].game.awayTeam.Abbreviation + "</span>");
                            }
                            else
                            {
                                Response.Write("<span class='away'>" + livescores2.scoreboard.gameScore[j].game.awayTeam.Abbreviation + "</span>");
                            }
                        }
                        else
                        {
                            int x;
                            int y;
                            Int32.TryParse(livescores2.scoreboard.gameScore[j].awayScore, out x);
                            Int32.TryParse(livescores2.scoreboard.gameScore[j].homeScore, out y);
                            if (x > y && (livescores2.scoreboard.gameScore[j].isCompleted == "true"))
                            {
                                Response.Write("<span class='loss'>" + livescores2.scoreboard.gameScore[j].game.homeTeam.Abbreviation + "</span>");
                            }
                            else if (x < y && (livescores2.scoreboard.gameScore[j].isCompleted == "true"))
                            {
                                Response.Write("<span class='win'>" + livescores2.scoreboard.gameScore[j].game.homeTeam.Abbreviation + "</span>");
                            }
                            else
                            {
                                Response.Write("<span class='home'>" + livescores2.scoreboard.gameScore[j].game.homeTeam.Abbreviation + "</span>");
                            }
                        }
                    }

                    Response.Write("</div><div class='divTableCell' style = 'background:#eae9e5;'>");

                    if (game13List[i] == (livescores3.scoreboard.gameScore[0].game.awayTeam.City + " " + livescores3.scoreboard.gameScore[0].game.awayTeam.Name))
                    {
                        int x;
                        int y;
                        Int32.TryParse(livescores3.scoreboard.gameScore[0].awayScore, out x);
                        Int32.TryParse(livescores3.scoreboard.gameScore[0].homeScore, out y);
                        if (x > y && (livescores3.scoreboard.gameScore[0].isCompleted == "true"))
                        {
                            Response.Write("<span class='win'>" + livescores3.scoreboard.gameScore[0].game.awayTeam.Abbreviation + "</span>");
                        }
                        else if (x < y && (livescores3.scoreboard.gameScore[0].isCompleted == "true"))
                        {
                            Response.Write("<span class='loss'>" + livescores3.scoreboard.gameScore[0].game.awayTeam.Abbreviation + "</span>");
                        }
                        else
                        {
                            Response.Write("<span class='away'>" + livescores3.scoreboard.gameScore[0].game.awayTeam.Abbreviation + "</span>");
                        }
                    }
                    else
                    {
                        int x;
                        int y;
                        Int32.TryParse(livescores3.scoreboard.gameScore[0].awayScore, out x);
                        Int32.TryParse(livescores3.scoreboard.gameScore[0].homeScore, out y);
                        if (x > y && (livescores3.scoreboard.gameScore[0].isCompleted == "true"))
                        {
                            Response.Write("<span class='loss'>" + livescores3.scoreboard.gameScore[0].game.homeTeam.Abbreviation + "</span>");
                        }
                        else if  (x < y && (livescores3.scoreboard.gameScore[0].isCompleted == "true"))
                        {
                            Response.Write("<span class='win'>" + livescores3.scoreboard.gameScore[0].game.homeTeam.Abbreviation + "</span>");
                        }
                        else
                        {
                            Response.Write("<span class='home'>" + livescores3.scoreboard.gameScore[0].game.homeTeam.Abbreviation + "</span>");
                        }
                    }

                    

                                
                            
                    Response.Write("</div></div>");

                }



                Response.Write("</div>");



                             %>


            





</body>
</html>
