using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace BookStoreLIB
{
    public class DALUpload
    {
        private SqlConnection conn;

        public DALUpload()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }

        // Method to update profile picture
        public bool UpdateProfilePicture(int userId, byte[] profilePicture)
        {
            try
            {
                string query = "UPDATE UserData SET ProfilePicture = @ProfilePicture WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProfilePicture", profilePicture ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show($"Rows affected during profile picture update: {rowsAffected}");
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile picture: " + ex.Message);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Method to get profile picture
        public byte[] GetProfilePicture(int userId)
        {
            byte[] profilePicture = null;
            string query = "SELECT ProfilePicture FROM UserData WHERE UserID = @UserID";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        profilePicture = (byte[])reader["ProfilePicture"];
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving profile picture: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return profilePicture;
        }
    }
}
