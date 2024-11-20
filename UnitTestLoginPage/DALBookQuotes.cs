using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class DALBookQuotes
    {

        DataSet dsQuotes;
        SqlConnection conn;

        public DALBookQuotes()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }

        public DataSet getQuotes()
        {
            try {
                String SQL = @"SELECT Quote_id, Book_Title, Book_Author, Quote
                                FROM BookQuotes";
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dsQuotes = new DataSet();
                da.Fill(dsQuotes, "BookQuotes");

                return dsQuotes;
             }
            catch 
            { 
                return null;
            }
            
        }

        public bool addBookQuote(string book_title, string book_author, string quote)
        {
            try
            {
                string sql = @"INSERT INTO BookQuotes (Quote_id, Book_Title, Book_Author, Quote) 
                       VALUES (@quote_id, @book_title, @book_author, @quote)";
                int newQuoteId;

                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(Quote_id), 0) + 1 FROM BookQuotes", conn))
                {
                    conn.Open();
                    newQuoteId = (int)cmd.ExecuteScalar();
                    conn.Close();
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@quote_id", newQuoteId);
                    cmd.Parameters.AddWithValue("@book_title", book_title);
                    cmd.Parameters.AddWithValue("@book_author", book_author);
                    cmd.Parameters.AddWithValue("@quote", quote);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

    }
}
