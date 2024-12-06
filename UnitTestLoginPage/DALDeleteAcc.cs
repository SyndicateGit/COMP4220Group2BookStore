using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace BookStoreLIB
{
    public class DALDeleteAcc
    {
        private SqlConnection conn;

        public DALDeleteAcc()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }
         public bool IsConnectionClosed => conn.State == ConnectionState.Closed;

        // Method to delete a user account based on user ID
        public bool DeleteAccount(int userID)
        {
            bool isDeleted = false;
            try
            {
                string query = "DELETE FROM UserData WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0; // Returns true if at least one row was deleted

                    // Log rows affected for debugging
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error: " + sqlEx.Message);
                MessageBox.Show("SQL Error: " + sqlEx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return isDeleted;
        }
    }
}
