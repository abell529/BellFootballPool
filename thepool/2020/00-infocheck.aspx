<%@ Page Language="C#" AutoEventWireup="true" CodeFile="00-infocheck.aspx.cs" Inherits="_2020_00_infocheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
		Week 4 2020 -- BellFusion Football Pool
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
   
    <h1>2020 Week 4: BellFusion Football Pool Picks</h1>
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

        
            <% 
                            string[] sundayPickGameIDsA = { "56804", "56805", "56806", "56807", "56808", "56809", "56811", "56812", "56813", "56814", "56815", "56816", "56817", "56818", "56819" };


                            Response.Write("Number of games: " + numberofgames + "<br /><br />");
                            Response.Write("Number of ALL games: " + numberofgamescores + "<br /><br />");
                            



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
                            string[] game14List = game14Pick.ToArray();
                            // string[] game15List = game15Pick.ToArray();





                            Response.Write(livescores.scoreboard.gameScore[0].game.awayTeam.City + " " + livescores.scoreboard.gameScore[0].game.awayTeam.Name + "<br /><br />");
                            

                            



                             %>


            





</body>
</html>
