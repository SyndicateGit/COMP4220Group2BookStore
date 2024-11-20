using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookStoreLIB;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for BQDialog.xaml
    /// </summary>
    public partial class BQDialog : Window
    {
        public BQDialog()
        {
            InitializeComponent();

            BookQuotes quotes = new BookQuotes();
            DataSet ds = quotes.getBookQuotes();

            if (ds != null && ds.Tables["BookQuotes"].Rows.Count > 0)
            {
                // Bind the DataSet to the DataGrid
                bookquotedatagrid.ItemsSource = ds.Tables["BookQuotes"].DefaultView;
            }
            else
            {
                MessageBox.Show("No purchase history found for this user.");
            }
        }
    }
}
