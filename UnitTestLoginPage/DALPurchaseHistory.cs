using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class DALPurchaseHistory
    {

        DataSet dsPurHis;
        SqlConnection conn;

        public DALPurchaseHistory()
        {
            conn = new SqlConnection(Properties.Settings.Default.MSSQLConnectionString);
        }

        public DataSet GetPhInfo(int userID)
        {
            try
            {
                string strSQL = @"
            SELECT o.OrderID, b.Author, b.Title, o.OrderDate, oi.Quantity, (b.Price * oi.Quantity) AS TotalPrice
            FROM Orders o
            INNER JOIN OrderItem oi ON o.OrderID = oi.OrderID
            INNER JOIN bookData b ON oi.ISBN = b.ISBN
            WHERE o.UserID = @userID";

                SqlCommand cmdSelect = new SqlCommand(strSQL, conn);
                cmdSelect.Parameters.AddWithValue("@userID", userID);

                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                dsPurHis = new DataSet();
                da.Fill(dsPurHis, "PurchaseHistory");

                return dsPurHis;
            }
            catch
            {
                return null;
            }
        }
    }
}
