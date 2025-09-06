<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Debug="true" Inherits="_Default" %>

<!DOCTYPE html>
<html lang="en">
	
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1">
	<meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="mobile-web-app-capable" content="yes">
	<title>BellFootball</title>
	<meta name="description" content="BellFootball for picking football and basketball games.">
	<meta name="keywords" content="BellFootball">
	<meta property="og:title" content="BellFootball | Football Pool">
	<meta property="og:url" content="https://www.bellfootball.edu/index.html">
	<meta property="og:type" content="website">
	<meta property="og:image" content="https://www.bellfootball.edu/images/bf-logo-og-image.png">
	<meta property="og:description" content="BellFootball for picking football and basketball games.">
	<meta property="og:site_name" content="BellFootball">
	<meta name="twitter:card" content="summary">
	<meta name="twitter:site" content="BellFootball">
	<meta name="twitter:creator" content="@Real_AndyB">
	<meta name="twitter:url" content="https://www.bellfootball.edu/index.html">
	<meta property="twitter:title" content="BellFootball | Football Pool">
	<meta name="twitter:description" content="BellFootball for picking football and basketball games.">
	<meta name="twitter:image" content="https://www.bellfootball.edu/images/twitter-meta.png">
	
	<!-- Favions / Touch Icons -->
	<link rel="apple-touch-icon" sizes="57x57" href="../images/favicons/apple-touch-icon-57x57.png"/>
	<link rel="apple-touch-icon" sizes="114x114" href="../images/favicons/apple-touch-icon-114x114.png"/>
	<link rel="apple-touch-icon" sizes="72x72" href="../images/favicons/apple-touch-icon-72x72.png"/>
	<link rel="apple-touch-icon" sizes="144x144" href="../images/favicons/apple-touch-icon-144x144.png"/>
	<link rel="apple-touch-icon" sizes="60x60" href="../images/favicons/apple-touch-icon-60x60.png"/>
	<link rel="apple-touch-icon" sizes="120x120" href="../images/favicons/apple-touch-icon-120x120.png"/>
	<link rel="apple-touch-icon" sizes="76x76" href="../images/favicons/apple-touch-icon-76x76.png"/>
	<link rel="apple-touch-icon" sizes="152x152" href="../images/favicons/apple-touch-icon-152x152.png"/>
	<link rel="icon" type="image/png" href="../images/favicons/favicon-196x196.png" sizes="196x196"/>
	<link rel="icon" type="image/png" href="../images/favicons/favicon-96x96.png" sizes="96x96"/>
	<link rel="icon" type="image/png" href="../images/favicons/favicon-16x16.png" sizes="16x16"/>
	<link rel="icon" type="image/png" href="../images/favicons/favicon-32x32.png" sizes="32x32"/>
	<meta name="msapplication-TileColor" content="#D0021B"/>
	<meta name="msapplication-TileImage" content="../images/favicons/mstile-144x144.png"/>

    <!-- <link rel="stylesheet" href="css/boilerplate.css" /> -->
    <link rel="stylesheet" href="css/picks.css" />

    <link rel="stylesheet" href="/css/bellf.css" />
    <link rel="stylesheet" href="/css/animate-custom.css" />
    <link rel="stylesheet" href="/css/design-layer.css" />
    <link rel="stylesheet" href="/css/newsroom_ou_styles.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/remixicon@2.5.0/fonts/remixicon.css" rel="stylesheet">
    <link  rel="icon" type="image/x-icon" href="images/tu-favicon.png" />
    <script src="https://kit.fontawesome.com/152ae4b0b7.js" crossorigin="anonymous"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4=" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://use.typekit.net/oct3isk.css">
    <script src="/js/jquery.aniview.js"></script>

            


	
	<script>
        $(document).ready(function () {
            $('.aniview-v3').AniView();
            $('.aniview-v4').AniView({
                animateClass: 'animate__animated',
                animateThreshold: 300
            });
        });
        $('.aniview').AniView();
    </script>


</head>

<body class="template-article subpage pickspage">
    <div class="outer-wrapper"><section class="navigation-top"><a href="#content" id="hidden">Skip to Main Content</a><div class="header-container">
    <!-- <div class="utility-nav heading-constrain">
       <div class="publications-group">
          <div class="menu-trigger">
             <p class="publications-label">All Sports <i class="fa-solid fa-caret-right" aria-hidden="true"></i></p>
          </div>
          <div class="slide-outer">
             <div class="pub-list-slide">
                <ul class="publication-items">
                   <li><a href="football/">Football</a></li>
                   <li><a href="march/">Basketball</a></li>
                   <li><a href="archives/">Other</a></li>
                </ul>
             </div>
          </div>
       </div>
       <div class="bf-home-link">
          <p>BellFootball</p>
       </div>
    </div> -->
    <div class="global-header stripe lite-graphite">
       <div class="heading-constrain nav-flex-wrap">
         <a href="../../index.html"> 
         <div class="masthead-container">
            <div class="gradient"></div>
            <div class="masthead-logo-container">
                
                <!-- <img src="images/logo-top.svg" alt="BellFootball Logo" /> -->

                <p>BELL</p>

             </div>
             <div class="vertical-id newsroom-vertical">
                <p class="vertical-name">Football</p>
                <div class="vertical-issue">
                   <p class="issue-date">Fall 2024</p>
                </div>
             </div>
          </div></a>
          <div class="mobile-menu-icon-container">
             <div class="mobile">
                <div class="container-nav"><a href="#" class="mobile-nav">
                      <div class="hamburger-nav"><i></i><i></i><i></i></div></a></div>
                <div class="mobile-links-nav">
                   <div class="container-nav">
                      <ul>
                        <li><a href="../../football/">Football Pool</a></li>
                         <li><a href="../../march/">March Madness</a></li>
                         <li><a href="../../archive/">Archive</a></li>
                      </ul>
                      <!-- <ul class="mobile-publication-items">
                         <li><a href="football/">Football Pool</a></li>
                         <li><a href="march/">March Madness</a></li>
                         <li><a href="archives/">Archives</a></li>
                      </ul> -->
                      <!-- <form action="/_dev/_news-mag-redesign/search/searchnews.html" class="search_form_mobile">
                         <div class="search-section form secondary"><label for="site-search-mobile" aria-hidden="true">Search</label><input type="search" id="site-search-mobile" name="q" class="search_input" aria-label="Search the Towson University website" placeholder="Enter a search term"><button type="submit" class="search_submit_word">Search  <i class="fa-solid fa-magnifying-glass" aria-hidden="true"></i></button></div>
                      </form> -->
                   </div>
                </div>
             </div>
          </div>
          <div class="nav-container">
             <div class="search-section"><!-- <i class="fa-solid fa-magnifying-glass" aria-label="Search icon" aria-hidden="true"></i><form action="/_dev/_news-mag-redesign/search/searchnews.html" class="search_form_desktop"><label for="site-search-desktop" aria-hidden="true">Search</label><input type="search" id="site-search-desktop" name="q" class="search_input" aria-label="Search the Towson University website" onfocus="if(this.value==this.defaultValue)this.value=''" onblur="if(this.value=='')this.value=this.defaultValue" value="Enter a search term"><button type="submit" class="search_submit_word">Search</button></form> -->
             </div>
             <div class="nav">
                <ul class="nav-items">
                   <li><a href="../../football/">Football Pool</a></li>
                   <li><a href="../../march/">NCAA March Madness</a></li>
                   <li><a href="../../archive/">Archive</a></li>
                </ul>
             </div>
          </div>
       </div>
    </div>
 </div>
</section>

	<main id="content" class="main-content-area no-margin">
        <section class="hero primary">
           <div class="hero-image animate__animated animate__slow animate__fadeInLeft" style="background-image:url('../../images/lions-rams.jpg');">
              <div class="gradient"></div>
           </div>
           <div class="hero-content">
              <h1 class="animate__animated animate__fadeInRight animate__slow animate__delay-500ms">
                  
                  
                  <span>Picks for <strong><br>Week 1 2025</strong></span></h1>


              <!--<p class="subhead animate__animated animate__fadeIn animate__slower animate__delay-2s">Start making your picks today on the updated site</p>
              <div class="byline-container animate__animated animate__fadeIn animate__slower animate__delay-2s">
                 <p class="byline-item"><span class="icon far fa-pencil-alt" aria-hidden="true"></span>By Andy Bell</p>
              </div> -->
           </div>
        </section>
        <section class="body-container two-col">
           <section class="main-content">
              <div class="body-section">
               
               
                      <div id="picksarea" class="clearfix">




                          <form id="form1" runat="server">
                    

                        <div id="the_games_list">
                            

                            <% for (int i = 0; i < numberofgames; i++)
                                {
                                    Response.Write("<script>");
                                    Response.Write("jQuery(function ($) {");

                                    if (showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation == "KCC")
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

                                    if (showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation == "OAKK")
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

                                    if (showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation == "KCC")
                                    {
                                        Response.Write("<div class='visitor_name " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name);
                                        Response.Write("</div>");
                                        Response.Write("<div class='visitor_button' style='margin-top: 4px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "' required>");

                                    }
                                    else
                                    {
                                        Response.Write("<div class='visitor_name " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name);
                                        Response.Write("</div>");
                                        Response.Write("<div class='visitor_button' style='margin-top: 4px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "' required>");

                                    }


                                    Response.Write("</div>");
                                    Response.Write("</div>");
                                    Response.Write("<div class='game_home'>");

                                    if (showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation == "OAKK")
                                    {
                                        Response.Write("<div class='home_button' style='margin-top: 4px; margin-left: 8px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "' required>");
                                        Response.Write("</div>");
                                        Response.Write("<div class='home_name " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "'>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name);
                                        
                                    }
                                    else
                                    {
                                        Response.Write("<div class='home_button' style='margin-top: 4px; margin-left: 8px;'>");
                                        Response.Write("<input type='radio' name='Game" + i + "t' value='" + showall.fullgameschedule.gameentry[i].homeTeam.City + " " + showall.fullgameschedule.gameentry[i].homeTeam.Name + "' required>");
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
                          <div class="info-form">
                              
                              <p>
                                  First Name: <asp:TextBox runat="server" type="text" name="firstnameEntry" id="firstnameEntry" />

                        </p>
                              <p>
                                  Last Name: <asp:TextBox runat="server" type="text" name="lastnameEntry" id="lastnameEntry"  />

                        </p>
                              <p>
                                  E-mail: <asp:TextBox runat="server" type="text" name="emailEntry" id="emailEntry"  />

                        </p>
                           </div>
                           <p>
                              <asp:Button runat="server" id="SubmitButton" text="Submit Picks" onclick="SubmitForm" />

                    </p>
                    

                    <%

                        Response.Write("<p>" + successText + "</p>");
                        

                        %>
                    
                    </form>
                </div>
                  </div>
          
               </section>
            
                <section class="secondary-content">
                <div class="sidebar card round shim-padding-40 separator">
               <h2>Team Info:</h2>
                    <div class="Details clearfix">

                        

        <% for (int i = 0; i < numberofgames; i++)
            {

                Response.Write("<div class='detailstext " + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + "info hidden' style='padding-top: 20px'>");
                Response.Write("<div class='logo' style='float: left; padding-right: 15px; '>");
                Response.Write("<img src='img/" + showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation + ".png' alt='" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "' />");
                Response.Write("</div>");
                Response.Write("<div class='teaminfodetails' style='float: left;'>");
                Response.Write("<strong>" + showall.fullgameschedule.gameentry[i].awayTeam.City + " " + showall.fullgameschedule.gameentry[i].awayTeam.Name + "</strong><br /><br />");

                /* for (int j = 0; j < numberofteams; j++)
                {
                    if (showall.fullgameschedule.gameentry[i].awayTeam.Abbreviation == lastseason.overallteamstandings.teamstandingsentry[j].team.Abbreviation)
                    { */
                        Response.Write("<p>Record not avaialble</p>");
                        // Response.Write("Current Record: " + lastseason.overallteamstandings.teamstandingsentry[j].stats.Wins.text + " - " + lastseason.overallteamstandings.teamstandingsentry[j].stats.Losses.text);
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
                    /* }

                } */
                

                

                Response.Write("</div>");

                Response.Write("</div>");


                Response.Write("<div class='detailstext " + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + "info hidden' style='padding-top: 20px'>");
                Response.Write("<div class='logo' style='float: left; padding-right: 15px; '>");
                Response.Write("<img src='img/" + showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation + ".png' alt='" + showall.fullgameschedule.gameentry[i].homeTeam.City + " " + showall.fullgameschedule.gameentry[i].homeTeam.Name + "' />");
                Response.Write("</div>");
                Response.Write("<div class='teaminfodetails' style='float: left;'>");
                Response.Write("<strong>" + showall.fullgameschedule.gameentry[i].homeTeam.City + " " + showall.fullgameschedule.gameentry[i].homeTeam.Name + "</strong><br /><br />");

                /* for (int k = 0; k < numberofteams; k++)
                {
                    if (showall.fullgameschedule.gameentry[i].homeTeam.Abbreviation == lastseason.overallteamstandings.teamstandingsentry[k].team.Abbreviation)
                    { */
                        Response.Write("<p>Record not avaialble</p>");
                        // Response.Write("Current Record: " + lastseason.overallteamstandings.teamstandingsentry[k].stats.Wins.text + " - " + lastseason.overallteamstandings.teamstandingsentry[k].stats.Losses.text);
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
                    /* }

                } */

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
            <b>2024 Final Standings</b>
            <br /><br />
            <%
                 //for (int i = 0; i < 15; i++)
                 //{
                 //Response.Write(theFirstName);
                 //}


                
                //Use this below here
                string[] theFirstNamelist = theFirstName.ToArray();
                string[] theLastNamelist = theLastName.ToArray();
                string[] theTotallist = theTotal.ToArray();

                for (int i = 0; i < 16; i++)
                {
                    Response.Write(theFirstNamelist[i] + " " + theLastNamelist[i] + ": " + theTotallist[i] + "<br />");
                }
                




                 //foreach (var item in theFirstName)
                 //{
                  //  Response.Write(item + "<br />");
                 //}


                %>
            </p>
          
              </div>
               <p>&nbsp;</p>
               </div>
         </section>

    </section>
     </main>
     <section class="footer">
<footer class="site-footer">
  <div class="footer-row">
     <div class="footer-left">
        <p class="footer-title">BellFootball</p>
        <p class="footer-address">www.bellfootball.com</p>
     </div>
     <div class="footer-right">
        <nav class="footer-nav"><!-- <a href="/privacy.html" class="footer-link">Privacy</a><a href="/about/accessibility/index.html" class="footer-link">Accessibility</a><a href="/copyright.html" class="footer-link">Copyright</a> --> </nav>
        <p class="footer-copy">Copyright © <script type="text/javascript">var d = new Date(); document.write(d.getFullYear());</script></p>
     </div>
  </div>
</footer>
</section>
<script type="text/javascript" src="../../js/bf-scripts.js"></script>

</body>
</html>

