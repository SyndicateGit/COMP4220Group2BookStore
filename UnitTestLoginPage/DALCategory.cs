using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BookStoreLIB
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }

    public class DALCategory
    {
        private readonly SqlConnection conn;

        public DALCategory()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            try
            {
                string query = "SELECT CategoryID, Name FROM Category";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                CategoryID = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading categories: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return categories;
        }
    }
}
