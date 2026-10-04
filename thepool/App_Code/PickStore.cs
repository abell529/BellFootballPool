using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

/// <summary>
/// Reads and writes one row per person in the current week's picks table.
/// A person is matched by first and last name, ignoring case and surrounding spaces,
/// so the Thursday page and the Sunday page land in the same row.
/// </summary>
public static class PickStore
{
    public const int MaxGames = 16;

    private const string NameMatch = "UPPER(TRIM(firstname)) = UPPER(TRIM(@first)) AND UPPER(TRIM(lastname)) = UPPER(TRIM(@last))";

    /// <summary>
    /// Saves the given picks, keyed by column number (1 = game1 ... 16 = game16).
    /// Updates the existing row for this name if there is one, otherwise inserts a new row.
    /// Pass null for email to leave an existing email untouched.
    /// Returns true when an existing row was updated, false when a row was inserted.
    /// </summary>
    public static bool SavePicks(string firstName, string lastName, string email, IDictionary<int, string> picksByColumn)
    {
        firstName = (firstName ?? string.Empty).Trim();
        lastName = (lastName ?? string.Empty).Trim();

        using (var conn = new MySqlConnection(CredentialStore.PoolConnectionString))
        {
            conn.Open();

            var existingId = FindRowId(conn, firstName, lastName);

            if (existingId.HasValue)
            {
                var sets = new List<string>();
                if (email != null)
                {
                    sets.Add("email = @email");
                }
                foreach (var column in picksByColumn.Keys.OrderBy(k => k))
                {
                    sets.Add($"game{column} = @game{column}");
                }

                if (sets.Count == 0)
                {
                    return true;
                }

                using (var cmd = new MySqlCommand($"UPDATE `{CurrentWeek.PicksTable}` SET {string.Join(", ", sets)} WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", existingId.Value);
                    if (email != null)
                    {
                        cmd.Parameters.AddWithValue("@email", email.Trim());
                    }
                    foreach (var pick in picksByColumn)
                    {
                        cmd.Parameters.AddWithValue($"@game{pick.Key}", pick.Value ?? string.Empty);
                    }
                    cmd.ExecuteNonQuery();
                }

                return true;
            }

            var columns = new List<string> { "firstname", "lastname", "email" };
            var values = new List<string> { "@firstname", "@lastname", "@email" };
            for (int column = 1; column <= MaxGames; column++)
            {
                columns.Add($"game{column}");
                values.Add($"@game{column}");
            }

            using (var cmd = new MySqlCommand($"INSERT INTO `{CurrentWeek.PicksTable}` ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})", conn))
            {
                cmd.Parameters.AddWithValue("@firstname", firstName);
                cmd.Parameters.AddWithValue("@lastname", lastName);
                cmd.Parameters.AddWithValue("@email", (email ?? string.Empty).Trim());
                for (int column = 1; column <= MaxGames; column++)
                {
                    string pick;
                    picksByColumn.TryGetValue(column, out pick);
                    cmd.Parameters.AddWithValue($"@game{column}", pick ?? string.Empty);
                }
                cmd.ExecuteNonQuery();
            }

            return false;
        }
    }

    /// <summary>All picks stored for this name, keyed by column number. Empty if there is no row yet.</summary>
    public static Dictionary<int, string> GetPicks(string firstName, string lastName)
    {
        var picks = new Dictionary<int, string>();

        using (var conn = new MySqlConnection(CredentialStore.PoolConnectionString))
        {
            conn.Open();

            using (var cmd = new MySqlCommand($"SELECT * FROM `{CurrentWeek.PicksTable}` WHERE {NameMatch} ORDER BY id LIMIT 1", conn))
            {
                AddNameParameters(cmd, firstName, lastName);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int column = 1; column <= MaxGames; column++)
                        {
                            int ordinal;
                            try
                            {
                                ordinal = reader.GetOrdinal($"game{column}");
                            }
                            catch (IndexOutOfRangeException)
                            {
                                continue;
                            }
                            picks[column] = reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
                        }
                    }
                }
            }
        }

        return picks;
    }

    private static long? FindRowId(MySqlConnection conn, string firstName, string lastName)
    {
        using (var cmd = new MySqlCommand($"SELECT id FROM `{CurrentWeek.PicksTable}` WHERE {NameMatch} ORDER BY id LIMIT 1", conn))
        {
            AddNameParameters(cmd, firstName, lastName);
            var result = cmd.ExecuteScalar();
            return result == null || result == DBNull.Value ? (long?)null : Convert.ToInt64(result);
        }
    }

    private static void AddNameParameters(MySqlCommand cmd, string firstName, string lastName)
    {
        cmd.Parameters.AddWithValue("@first", (firstName ?? string.Empty).Trim());
        cmd.Parameters.AddWithValue("@last", (lastName ?? string.Empty).Trim());
    }
}
