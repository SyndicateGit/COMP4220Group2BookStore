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

                // Insert new user
                cmd.CommandText = "INSERT INTO UserData (UserID, UserName, Password, Type, Manager, FullName)" 
                                    + "VALUES (@UserID, @UserName, @Password, @Type, @Manager, @FullName)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@UserID", newUserId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Type", "RG"); 
                cmd.Parameters.AddWithValue("@Manager", false);
                cmd.Parameters.AddWithValue("@FullName", fullName);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return newUserId;
                }
                else
                {
                    return -2; 
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -2; 
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
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



        public DALUserInfo()
        {

        }
    }
}