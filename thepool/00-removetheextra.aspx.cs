using System;
using System.Collections.Generic;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

public partial class _00_removetheextra : System.Web.UI.Page
{
    [Serializable]
    public class Entry
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    // Store the rows in ViewState to persist across postbacks
    private List<Entry> Entries
    {
        get { return (List<Entry>)ViewState["Entries"]; }
        set { ViewState["Entries"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData(); // Load data from the database on first load
        }
    }

    private void LoadData()
    {
        string connectionString = "Server=mysql24.ezhostingserver.com;Database=bfpoolDB;User ID=MyName69;Password=ThePassword69;";
        List<Entry> entries = new List<Entry>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, firstname, lastname FROM nine2024 ORDER BY UPPER(lastname), UPPER(firstname)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                entries.Add(new Entry
                {
                    ID = Convert.ToInt32(reader["id"]),
                    FirstName = reader["firstname"].ToString().Trim(),
                    LastName = reader["lastname"].ToString().Trim()
                });
            }

            reader.Close();
        }

        Entries = entries; // Store entries in ViewState
        BindData(); // Render the table
    }

    private void BindData()
    {
        TableRowsContainer.Rows.Clear(); // Clear any existing rows

        foreach (var entry in Entries)
        {
            TableRow row = new TableRow();
            TableCell cellSelect = new TableCell();
            TableCell cellFirstName = new TableCell();
            TableCell cellLastName = new TableCell();

            cellSelect.Text = String.Format("<input type='radio' name='deleteSelection' value='{0}' />", entry.ID);
            cellFirstName.Text = entry.FirstName;
            cellLastName.Text = entry.LastName;

            row.Cells.Add(cellSelect);
            row.Cells.Add(cellFirstName);
            row.Cells.Add(cellLastName);

            TableRowsContainer.Rows.Add(row); // Add the row to the table
        }
    }

    // WebMethod to delete the selected row based on ID
    [WebMethod]
    public static string DeleteRow(int id)
    {
        string connectionString = "Server=mysql24.ezhostingserver.com;Database=bfpoolDB;User ID=MyName69;Password=ThePassword69;";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string deleteQuery = "DELETE FROM nine2024 WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                return "Data deleted successfully!";
            }
            else
            {
                return "Failed to delete data. No matching record found.";
            }
        }
    }
}
