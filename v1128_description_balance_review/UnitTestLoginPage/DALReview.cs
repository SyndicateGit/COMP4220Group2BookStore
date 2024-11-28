using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BookStoreLIB
{
    public class DALReview
    {
        private readonly SqlConnection conn;

        public DALReview()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }

        public List<Review> GetReviews(string isbn)
        {
            List<Review> reviews = new List<Review>();

            const string query = @"
                SELECT r.ReviewText, r.ReviewDate, u.Username 
                FROM BookReview r
                INNER JOIN UserData u ON r.UserID = u.UserID
                WHERE r.ISBN = @ISBN
                ORDER BY r.ReviewDate DESC";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review
                            {
                                ReviewText = reader["ReviewText"].ToString(),
                                ReviewDate = Convert.ToDateTime(reader["ReviewDate"]),
                                Username = reader["Username"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching reviews: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            return reviews;
        }

        public bool AddReview(string isbn, int userId, string reviewText)
        {
            const string query = @"
                INSERT INTO BookReview (ISBN, UserID, ReviewText, ReviewDate)
                VALUES (@ISBN, @UserID, @ReviewText, @ReviewDate)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ReviewText", reviewText);
                    cmd.Parameters.AddWithValue("@ReviewDate", DateTime.Now);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding review: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }

    public class Review
    {
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Username { get; set; }
    }
}
