<%@ Page Language="C#" AutoEventWireup="true" CodeFile="00-removetheextra.aspx.cs" Debug="true" Inherits="_00_removetheextra" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remove Entries - BellFootball Pool</title>
    <style type="text/css">
        .divTable {
            display: table;
            width: 100%;
        }
        .divTableRow {
            display: table-row;
        }
        .divTableCell {
            border: 1px solid #017AA4;
            display: table-cell;
            padding: 3px 10px;
            text-align: center;
        }
        h1 {
            font-family: ardoise-narrow-std, sans-serif;
            font-weight: 200;
        }
        #successLabel {
            color: red;
        }
    </style>

    <!-- Include jQuery for handling AJAX requests -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            // Handle form submission using AJAX
            $("#deleteForm").submit(function (event) {
                event.preventDefault(); // Prevent the form from submitting the traditional way

                // Send the selected radio button value to the server
                $.ajax({
                    type: "POST",
                    url: "00-removetheextra.aspx/DeleteRow", // Call the server-side method
                    data: JSON.stringify({ id: $("input[name='deleteSelection']:checked").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        // Display success message on the page and in the console
                        $("#successLabel").text(response.d);
                        console.log(response.d);
                    },
                    error: function (xhr, status, error) {
                        // Display error message on the page and in the console
                        $("#successLabel").text("Error occurred while deleting the row.");
                        console.log("Error: " + error);
                    }
                });
            });
        });
    </script>
</head>
<body>
    <h1>Remove Entries: BellFootball Pool</h1>

    <!-- Form for selecting and deleting a row -->
    <form id="deleteForm" runat="server">
        <asp:Table ID="TableRowsContainer" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>Select</asp:TableHeaderCell>
                <asp:TableHeaderCell>First Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Last Name</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>

        <!-- Submit button for deletion -->
        <div>
            <input type="submit" value="Delete Selected Row" />
        </div>

        <!-- Label to display success or failure messages -->
        <span id="successLabel"></span>
    </form>
</body>
</html>
