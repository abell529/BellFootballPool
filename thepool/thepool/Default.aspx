<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Football Pool</title>

            <link rel="stylesheet" href="css/boilerplate.css" />
            <link rel="stylesheet" href="css/picks.css" />
            <meta charset="utf-8" />
            <meta name="viewport" content="initial-scale = 1.0,maximum-scale = 1.0" />
            <script>
                var __adobewebfontsappname__ = "reflow"
            </script>
            <script src="http://use.edgefonts.net/shadows-into-light:n4:all.js"></script>
            <script src="//ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
            <script src='js/modernizr-2.6.2.min.js'></script>
            <script src='js/jquery.cookie.js'></script>
</head>
<body>
    
        <div id="primaryContainer" class="primaryContainer clearfix">
            <div id="Heading" class="clearfix">
                <p id="headingtext">
                2017 BellFusion Football Pool
                </p>
            </div>
            <div id="Picks" class="clearfix">
                <div id="picksarea" class="clearfix">
                    <form id="form1" runat="server">
                    <p><b>Week 11</b></p>

                        <div id="the_games_list">

                            <% for (int i = 0; i < numberofgames; i++)
                                {
                                    Response.Write("<script>");
                                    Response.Write("jQuery(function ($) {");

                                    if (showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation == "KC")
                                    {
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "').hover(function () {");
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "info').toggleClass('hidden');");
                                    }
                                    else
                                    {
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "').hover(function () {");
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "info').toggleClass('hidden');");
                                    }

                                    Response.Write("});");
                                    Response.Write("});");
                                    Response.Write(" </script>");

                                    Response.Write("<script>");
                                    Response.Write("jQuery(function ($) {");

                                    if (showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation == "OAK")
                                    {
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "').hover(function () {");
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "info').toggleClass('hidden');");
                                    }
                                    else
                                    {
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + "').hover(function () {");
                                        Response.Write("$('." + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + "info').toggleClass('hidden');");
                                    }


                                    Response.Write("});");
                                    Response.Write("});");
                                    Response.Write(" </script>");


                                    Response.Write("<div class='the_game'>");
                                    Response.Write("<div class='game_visitor'>");

                                    if (showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation == "KC")
                                    {
                                        Response.Write("<div class='visitor_name " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name);
                                        Response.Write("</div>");
                                        Response.Write("<div class='visitor_button' style='margin-top: 4px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "'>");

                                    }
                                    else
                                    {
                                        Response.Write("<div class='visitor_name " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name);
                                        Response.Write("</div>");
                                        Response.Write("<div class='visitor_button' style='margin-top: 4px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "'>");

                                    }


                                    Response.Write("</div>");
                                    Response.Write("</div>");
                                    Response.Write("<div class='game_home'>");

                                    if (showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation == "OAK")
                                    {
                                        Response.Write("<div class='home_button' style='margin-top: 4px; margin-left: 8px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "'>");
                                        Response.Write("</div>");
                                        Response.Write("<div class='home_name " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name);
                                        
                                    }
                                    else
                                    {
                                        Response.Write("<div class='home_button' style='margin-top: 4px; margin-left: 8px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].homeTeam.City + " " + showall.fullgameschedule.gameentry[i].homeTeam.Name + "'>");
                                        Response.Write("</div>");
                                        Response.Write("<div class='home_name " + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + "'>" + showall.fullgameschedule.gameentry[i].homeTeam.City + " " + showall.fullgameschedule.gameentry[i].homeTeam.Name);
                                        
                                    }




                                    // Response.Write("<input type='hidden' name='Game#numberFormat(GAME_IN_WEEK, '9')#H' value='#FULL_TEAM_NAME#'>");
                                    Response.Write("</div>");
                                    Response.Write("</div>");
                                    Response.Write("</div>");
                                }
                             %>
                            
                            <!-- <p>Here: <asp:Label ID="TextBlock" runat="server" Text=""></asp:Label></p>
                                <p>Here too: <asp:Label ID="TextBlock3" runat="server" Text=""></asp:Label></p>
                                <asp:TextBox ID="TextBlock2" runat="server"></asp:TextBox> -->

                        </div>
                    <div style="clear:both; margin-bottom:7px; width:100%;">
                        <p>&nbsp;</p>
                        <p>
                            First Name: <asp:TextBox runat="server" type="text" name="firstnameEntry" id="firstnameEntry" />

                        </p>
                        <br />
                        <p>
                            Last Name: <asp:TextBox runat="server" type="text" name="lastnameEntry" id="lastnameEntry"  />

                        </p>
                        <br />
                        <br />
                        <p>
                            E-mail: <asp:TextBox runat="server" type="text" name="emailEntry" id="emailEntry"  />

                        </p>

                        
                        <br />
                        

                    </div>
                    
                    
                    <p>
                        <asp:Button runat="server" id="SubmitButton" text="Submit Picks" onclick="SubmitForm" />

                    </p>
                    <p>&nbsp;</p>

                    <%

                        Response.Write("<p>" + successText + "</p>");
                        

                        %>
                    
                    </form>
                </div>
            </div>
    
    <div class="Details clearfix">

        <% for (int i = 0; i < numberofgames; i++)
            {

                Response.Write("<div class='detailstext " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "info hidden' style='padding-top: 20px'>");
                Response.Write("<div class='logo' style='float: left; padding-right: 15px; '>");
                Response.Write("<img src='img/" + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + ".png' alt='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "' />");
                Response.Write("</div>");
                Response.Write("<div class='teaminfodetails' style='float: left;'>");
                Response.Write("<strong>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "</strong><br /><br />");

                for (int j = 0; j < numberofteams; j++)
                {
                    if (showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation == lastseason.overallteamstandings.teamstandingsentry[j].team.Abbreviation)
                    {
                        Response.Write("<!--- Record --->");
                        Response.Write("Current Record: " + lastseason.overallteamstandings.teamstandingsentry[j].stats.Wins.text + " - " + lastseason.overallteamstandings.teamstandingsentry[j].stats.Losses.text);
                        Response.Write("<br />");
                        // Response.Write("Team: " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + " " + lastseason.overallteamstandings.teamstandingsentry[j].team.Abbreviation);
                        // Response.Write("<br />");
                        // Response.Write("Last week they");
                        // Response.Write("theWinLoseTxt");
                        // Response.Write("<br />");
                        // Response.Write("theWinLoseTxtb again");
                        // Response.Write("<br />");
                        // Response.Write("<br />");
                        // Response.Write("<br />");
                        // Response.Write("Vegas Odds: ");
                    }

                }
                

                

                Response.Write("</div>");

                Response.Write("</div>");


                Response.Write("<div class='detailstext " + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + "info hidden' style='padding-top: 20px'>");
                Response.Write("<div class='logo' style='float: left; padding-right: 15px; '>");
                Response.Write("<img src='img/" + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + ".png' alt='" + showall.fullgameschedule.gameentry[i].homeTeam.City + " " + showall.fullgameschedule.gameentry[i].homeTeam.Name + "' />");
                Response.Write("</div>");
                Response.Write("<div class='teaminfodetails' style='float: left;'>");
                Response.Write("<strong>" + showall.fullgameschedule.gameentry[i].homeTeam.City + " " + showall.fullgameschedule.gameentry[i].homeTeam.Name + "</strong><br /><br />");

                for (int k = 0; k < numberofteams; k++)
                {
                    if (showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation == lastseason.overallteamstandings.teamstandingsentry[k].team.Abbreviation)
                    {
                        Response.Write("<!--- Record --->");
                        Response.Write("Current Record: " + lastseason.overallteamstandings.teamstandingsentry[k].stats.Wins.text + " - " + lastseason.overallteamstandings.teamstandingsentry[k].stats.Losses.text);
                        Response.Write("<br />");
                        // Response.Write("Team: " + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + " " + lastseason.overallteamstandings.teamstandingsentry[k].team.Abbreviation);
                        // Response.Write("<br />");
                        // Response.Write("Last week they");
                        // Response.Write("theWinLoseTxt");
                        // Response.Write("<br />");
                        // Response.Write("theWinLoseTxtb again");
                        // Response.Write("<br />");
                        // Response.Write("<br />");
                        // Response.Write("<br />");
                        // Response.Write("Vegas Odds: ");
                    }

                }

                Response.Write("</div>");

                Response.Write("</div>");

            }

            %>

    </div>
    <div id="MusicBox" class="clearfix">
        <audio controls autoplay>
            <source src="assets/music/fox.mp3" type="audio/mpeg" /> Your browser does not support the audio element.

        </audio>

    </div>
    <div id="Standings" class="clearfix">
        <p id="standingstext" style="line-height:20px;">
            <b>2017 Current Standings</b>
            <br /><br />
            <%
                // for (int i = 0; i < 15; i++)
                // {
                // Response.Write(theFirstName);
                // }

                string[] theFirstNamelist = theFirstName.ToArray();
                string[] theLastNamelist = theLastName.ToArray();
                string[] theTotallist = theTotal.ToArray();

                for (int i = 0; i < 16; i++)
                {
                    Response.Write(theFirstNamelist[i] + " " + theLastNamelist[i] + ": " + theTotallist[i] + "<br />");
                }
                

                // foreach (var item in theFirstName)
                // {
                //    Response.Write(item + "<br />");
                // }


                %>
            <br /><br />

        </p>

    </div>
    </div>
    

</body>
</html>
