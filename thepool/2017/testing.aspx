<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testing.aspx.cs" Inherits="_2017_testing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    

    <%

        if (Request.QueryString["url1"] != null)
        {
            Response.Write("<img src='" + Request.QueryString["url1"] + "' />");

        }
        

        


        %>


</body>
</html>
