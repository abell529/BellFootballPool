<%@ Page Language="C#" AutoEventWireup="true" CodeFile="2024.aspx.cs" Debug="true" Inherits="scoreboard_2024" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
		2024 BellFusion Football Pool Standings
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
   
    <h1>2024 BellFusion Football Pool Standings</h1>


    <%
        
        // Response.Write("Testing:" + livescores2.scoreboard.gameScore[11].game.awayTeam.City);
        
        %>

</p>



<div class="divTable" style="width: 100%;border: 1px solid #000;" >
<div class="divTableBody">
<div class="divTableRow">


    <div class='divTableCell'  class='tk-ardoise-compact-std light-font smaller-font' style = 'background:#ffffff; text-align:center;'>NAME</div>

    <%
        for (int i = 0; i < 18; i++)
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

          %>

    <div class='divTableCell'  class='tk-ardoise-compact-std light-font smaller-font' style = 'background:#ffffff; text-align:center;'>TOTAL</div>
    </div>

    

        
        
        <%

            string[] theFirstNameScores = scoresFirstName.ToArray();
            string[] theLastNameScores = scoresLastName.ToArray();
            string[] theCorrectPicks = totalCorrect.ToArray();
            string[] week1List = week1Correct.ToArray();
            string[] week2List = week2Correct.ToArray();
            string[] week3List = week3Correct.ToArray();
            string[] week4List = week4Correct.ToArray();
            string[] week5List = week5Correct.ToArray();
            string[] week6List = week6Correct.ToArray();
            string[] week7List = week7Correct.ToArray();
            string[] week8List = week8Correct.ToArray();
            string[] week9List = week9Correct.ToArray();
            string[] week10List = week10Correct.ToArray();
            string[] week11List = week11Correct.ToArray();
            string[] week12List = week12Correct.ToArray();
            string[] week13List = week13Correct.ToArray();
            string[] week14List = week14Correct.ToArray();
            string[] week15List = week15Correct.ToArray();
            string[] week16List = week16Correct.ToArray();
            string[] week17List = week17Correct.ToArray();
            string[] week18List = week18Correct.ToArray();


            for (int i = 0; i < theFirstNameScores.Length; i++ )
            {

                Response.Write("<div class='divTableRow tk-ardoise-narrow-std regular-font'>");
                Response.Write("<div class='divTableCell bold-font' style='white-space:nowrap;text-align:left;'>" + theFirstNameScores[i] + " " + theLastNameScores[i] + "</div>");

                string[] gamePicksList = { week1List[i], week2List[i], week3List[i], week4List[i], week5List[i], week6List[i], week7List[i], week8List[i], week9List[i], week10List[i], week11List[i], week12List[i], week13List[i], week14List[i], week15List[i], week16List[i], week17List[i], week18List[i] };


                for (int j = 0; j < 18; j++)
                {
                    if (j % 2 == 0)
                    {
                        Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font' style = 'background:#ffffff; text-align:center;'>" + gamePicksList[j] + "</div>");
                    }
                    else
                    {
                        Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font' style = 'background:#eae9e5; text-align:center;'>" + gamePicksList[j] + "</div>");
                    }
                }

                Response.Write("<div class='divTableCell tk-ardoise-compact-std heavy-font' style = 'background:#ffffff; text-align:center;'>" + theCorrectPicks[i] + "</div></div>");


            }

            

          %>

        </div>
    </div>
    </div>


    </body>
    </html>
