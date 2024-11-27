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

        public Book GetBookByISBNOrTitle(string searchValue)
        {
            using (var conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection))
            {
                try
                {
                    string query = @"
                SELECT ISBN, CategoryID, Title, Author, Price, SupplierId, Year, Edition, Publisher, InStock, RestockDate
                FROM BookData
                WHERE ISBN = @SearchValue OR Title LIKE @SearchValuePattern";

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SearchValue", searchValue);
                    cmd.Parameters.AddWithValue("@SearchValuePattern", $"%{searchValue}%");

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Book
                            {
                                ISBN = reader["ISBN"].ToString(),
                                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                                Title = reader["Title"].ToString(),
                                Author = reader["Author"].ToString(),
                                Price = reader["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Price"]),
                                SupplierId = reader["SupplierId"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["SupplierId"]),
                                Year = reader["Year"].ToString(),
                                Edition = reader["Edition"].ToString(),
                                Publisher = reader["Publisher"].ToString(),
                                Stock = reader["InStock"] == DBNull.Value ? 0 : Convert.ToInt32(reader["InStock"]),
                                RestockDate = reader["RestockDate"] == DBNull.Value ? null : (DateTime?)reader["RestockDate"]
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetBookByISBNOrTitle: " + ex.Message);
                }
            }
            return null; 
        }
    }
}
