using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BookStoreLIB
{
    public class DALUserProfile
    {
        private SqlConnection conn;

        public DALUserProfile()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }

        // Method to get user profile information based on user ID
        public DataRow GetUserProfile(int userID)
        {
            try
            {
                string query = "SELECT FullName, Phone, Email, Address, Password FROM UserData WHERE UserID = @userID";
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
        public int UpdateUserPassword(string username, string password)
        {
            try
            {
                string query = "UPDATE UserData SET Password = @password WHERE UserName = @username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected;
                }
            }
            catch (Exception e)
            {
                return -1; 
            }
            finally
            {
                if(conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        // Method to update user profile information
        public bool UpdateUserProfile(int userID, string name, string phone, string email, string address, string password)
        {
            try
            {
                string query = "UPDATE UserData SET FullName = @Name, Phone = @Phone, Email = @Email, Address = @Address, Password = @Password WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Password", password);

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

        // Method to add a book to the user's watchlist
        public void AddToWatchlist(int userID, string isbn)
        {
            try
            {
                string query = "INSERT INTO Watchlist (UserID, ISBN) VALUES (@userID, @isbn)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@isbn", isbn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional logging)
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Method to get the user's watchlist
        public List<string> GetUserWatchlist(int userID)
        {
            List<string> isbnList = new List<string>();
            try
            {
                string query = "SELECT ISBN FROM Watchlist WHERE UserID = @userID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    isbnList.Add(reader.GetString(0));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional logging)
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return isbnList;
        }

        public int UpdateUserBalance(string username, decimal balance)
        {
            try
            {
                {
                    string query = "UPDATE UserData SET Balance = @Balance WHERE userName = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Balance", balance);
                        cmd.Parameters.AddWithValue("@Username", username);

                        conn.Open();
                        return cmd.ExecuteNonQuery(); // Returns the number of rows affected
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                return -1; // Indicate an error occurred
            }
        }

    }
}
