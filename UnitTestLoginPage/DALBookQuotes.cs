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
    }

    

}
