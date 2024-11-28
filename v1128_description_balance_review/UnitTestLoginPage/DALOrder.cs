/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace BookStoreLIB
{
    class DALOrder
    {
        public int Proceed2Order(string xmlOrder)
        {
            SqlConnection cn = new SqlConnection(
                Properties.Settings.Default.MSSQLConnection);
            try
            {
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insertOrder";

                // Input parameter for XML
                SqlParameter inParameter = new SqlParameter();
                inParameter.ParameterName = "@xml";
                inParameter.Value = xmlOrder;
                inParameter.DbType = DbType.String;
                inParameter.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(inParameter);

                // Return value for OrderID
                SqlParameter returnParameter = new SqlParameter();
                returnParameter.ParameterName = "@ReturnVal";
                returnParameter.SqlDbType = SqlDbType.Int;
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParameter);

                // Open connection and execute the command
                cn.Open();
                cmd.ExecuteNonQuery();

                // Retrieve the returned OrderID
                int orderId = (int)returnParameter.Value; // Return value is the first parameter

                cn.Close();
                return orderId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }

    }
}
