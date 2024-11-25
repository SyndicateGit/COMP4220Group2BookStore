using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BookStoreLIB
{
    public class DALBook
    {
        private readonly SqlConnection conn;

        public DALBook()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }

        public bool AddBook(Book book)
        {
            try
            {
                const string query = @"
                        INSERT INTO BookData (ISBN, CategoryID, Title, Author, Price, SupplierId, Year, Edition, Publisher, InStock, RestockDate)
                        VALUES (@ISBN, @CategoryID, @Title, @Author, @Price, @SupplierId, @Year, @Edition, @Publisher, @InStock, @RestockDate)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@CategoryID", book.CategoryID);
                    cmd.Parameters.AddWithValue("@Title", book.Title ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Author", book.Author ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", book.Price != 0 ? (object)book.Price : DBNull.Value);
                    cmd.Parameters.AddWithValue("@SupplierId", book.SupplierId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Year", book.Year ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Edition", book.Edition);
                    cmd.Parameters.AddWithValue("@Publisher", book.Publisher ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InStock", book.Stock);
                    cmd.Parameters.AddWithValue("@RestockDate", book.RestockDate ?? (object)DBNull.Value);

                    // Open connection, execute command, and return success status
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool DeleteBook(Book book)
        {
            try
            {
                const string query = @"DELETE FROM BookData WHERE ISBN = @ISBN AND @Title = @Title AND @Author = @Author";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@Title", book.Title ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Author", book.Author ?? (object)DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }



    }
}
