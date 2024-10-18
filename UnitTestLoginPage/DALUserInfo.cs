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
            var conn = new SqlConnection(Properties.Settings.Default.RZConnection);
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
            return -1;
        }
        public DALUserInfo()
        {

        }
    }
}