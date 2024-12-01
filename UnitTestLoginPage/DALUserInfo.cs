using System.Data.SqlClient;
using System.Diagnostics;
using System;
using System.Data;
namespace BookStoreLIB
{
    internal class DALUserInfo
    {
        public int LogIn(string userName, string password)
        {
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select UserID from UserData where "
                    + " UserName = @UserName and Password = @Password ";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                int? userId = (int?)cmd.ExecuteScalar();
                if (userId.HasValue && userId.Value > 0) return userId.Value;
                else return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public int Signup(string userName, string password, string fullName)
        {
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // Check if the username already exists
                cmd.CommandText = "SELECT COUNT(*) FROM UserData WHERE UserName = @UserName";
                cmd.Parameters.AddWithValue("@UserName", userName);
                conn.Open();
                int userCount = (int)cmd.ExecuteScalar();
                if (userCount > 0)
                {
                    return -1; // Username already exists
                }

                // Get the next available UserID
                cmd.CommandText = "SELECT ISNULL(MAX(UserID), 0) + 1 FROM UserData";
                int newUserId = (int)cmd.ExecuteScalar();

                cmd.CommandText = @"INSERT INTO UserData (UserID, UserName, Password, Type, Manager, FullName, IsBanned) 
                            VALUES (@UserID, @UserName, @Password, @Type, @Manager, @FullName, @IsBanned)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@UserID", newUserId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Type", "RG"); // Assuming "RG" is a default user type
                cmd.Parameters.AddWithValue("@Manager", false);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@IsBanned", false); // Initialize as not banned

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return newUserId; // Return the new user's ID on success
                }
                else
                {
                    return -2; // Indicate an error if insertion failed
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -2; // Indicate an error if an exception occurred
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public DataTable GetUsersInfo(int userID)
        {
            DataTable dt = new DataTable();
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                string query = "SELECT UserID, FullName, Phone, Email, Address, Password, Balance, UserName FROM UserData WHERE UserID = @userID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch(Exception ex)
            {
                return null;
            }
            return dt;
            
        }



        internal bool BanUserIfValid(int userId, string username, string fullName)
        {
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = "SELECT COUNT(*) FROM UserData WHERE UserID = @UserID AND UserName = @UserName AND FullName = @FullName";
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                conn.Open();

                int userCount = (int)cmd.ExecuteScalar();
                if (userCount == 0)
                {

                    return false;
                }


                cmd.CommandText = "UPDATE UserData SET IsBanned = 1 WHERE UserID = @UserID AND UserName = @UserName AND FullName = @FullName";
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public bool IsUserBanned(int userId)
        {
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT IsBanned FROM UserData WHERE UserID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                bool isBanned = (bool)cmd.ExecuteScalar();
                return isBanned;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false; // Assume not banned if an error occurs
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }





        public DataTable GetUsersInfo(string userName)
        {
            DataTable dt = new DataTable();
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                string query = "SELECT UserID, FullName, Phone, Email, Address, Password,Balance, UserName FROM UserData WHERE username LIKE @userName + '%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userName", userName);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch(Exception ex)
            {
                return null;
            }
            return dt;
            
        }

        // Method to get balance for a user
        public decimal GetBalance(int userID)
        {
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT Balance FROM UserData WHERE UserID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", userID);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                else
                {
                    return 0.00M;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return 0.00M;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // Method to update the balance for a user
        public void UpdateBalance(int userID, decimal newBalance)
        {
            var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE UserData SET Balance = @Balance WHERE UserID = @UserID";
                cmd.Parameters.AddWithValue("@Balance", newBalance);
                cmd.Parameters.AddWithValue("@UserID", userID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DALUserInfo()
        {

        }
    }
}