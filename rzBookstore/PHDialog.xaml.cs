using BookStoreLIB;
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

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for PHDialog.xaml
    /// </summary>
    public partial class PHDialog : Window
    {
        public PHDialog(int userID)
        {
            InitializeComponent();

            PurchaseHistory ph = new PurchaseHistory();
            DataSet ds = ph.GetPurchaseHistory(userID);

            // Check if data was retrieved
            if (ds != null && ds.Tables["PurchaseHistory"].Rows.Count > 0)
            {
                // Bind the DataSet to the DataGrid
                purchaseHistoryDataGrid.ItemsSource = ds.Tables["PurchaseHistory"].DefaultView;
            }
            else
            {
                MessageBox.Show("No purchase history found for this user.");
            }
        }

        private void exitButton_Click(Object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
