using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BookStoreLIB
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string Name { get; set; }
    }

    public class DALSupplier
    {
        private readonly SqlConnection conn;

        public DALSupplier()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection);
        }

        public List<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            try
            {
                string query = "SELECT SupplierID, Name FROM Supplier";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliers.Add(new Supplier
                            {
                                SupplierID = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading suppliers: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return suppliers;
        }
    }
}
