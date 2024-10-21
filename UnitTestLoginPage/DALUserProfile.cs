using System;
using System.Data;
using System.Data.SqlClient;

namespace BookStoreLIB
{
    public class DALUserProfile
    {
        private SqlConnection conn;

        public DALUserProfile()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnectionString);
        }

        // Method to get user profile information based on user ID
        public DataRow GetUserProfile(int userID)
        {
            try
            {
                string query = "SELECT FullName, Phone, Email, Address FROM UserData WHERE UserID = @userID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional logging)
                return null;
            }
        }

        // Method to update user profile information
        public bool UpdateUserProfile(int userID, string name, string phone, string email, string address)
        {
            try
            {
                string query = "UPDATE UserData SET FullName = @Name, Phone = @Phone, Email = @Email, Address = @Address WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    return rowsAffected > 0; // Returns true if at least one row was updated
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional logging)
                return false; // Return false if there was an exception
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}


