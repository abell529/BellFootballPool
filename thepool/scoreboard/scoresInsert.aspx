<%@ Page Language="C#" AutoEventWireup="true" CodeFile="scoresInsert.aspx.cs" Inherits="scoreboard_scoresInsert"   Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <%  




                theFirstNamePicks = pickFirstName.ToArray();
                theLastNamePicks = pickLastName.ToArray();
                theCorrectPicks = correctPicks.ToArray();
                theTotalPicks = totalPicks.ToArray();

                int i;

                for (i = 0; i < theFirstNamePicks.Length; i++)
                {

                    int addedTotal = theCorrectPicks[i] + theTotalPicks[i];
                    //int addedTotal = theCorrectPicks[i];

                    Response.Write("<input type='text' name='firstCorrect" + i + "' id='firstCorrect" + i + "' value='" + theFirstNamePicks[i] + "' /><input type='text' name='lastCorrect" + i + "' id='lastCorrect" + i + "' value='" + theLastNamePicks[i] + "' /> :");
                    Response.Write("<input type='text' name='weekCorrect" + i + "' id='weekCorrect" + i + "' value='" + theCorrectPicks[i] + "' />");
                    Response.Write("<input type='text' name='totalCorrect" + i + "' id='totalCorrect" + i + "' value='" + addedTotal + "' /><br /> ");
                   

                }

                 Response.Write("<br /><input type='text' name='totalEntries' id='totalEntries' value='" + i + "' /><br />");


                %>



        <p>
                        <asp:Button runat="server" id="SubmitButton" text="Submit" onclick="SubmitForm" />

                    </p>
                    <p>&nbsp;</p>

                    <%

                        Response.Write("<p>" + successText + "</p>");
                        

                        %>
                    </div>
                    </form>
</body>
</html>
