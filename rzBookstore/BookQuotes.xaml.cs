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
    /// Interaction logic for BookQuotes.xaml
    /// </summary>
    public partial class BookQuotes : Window
    {
        public BookQuotes()
        {
            InitializeComponent();

            BookQuotes bookQuotes = new BookQuotes();
            DataSet ds = bookQuotes.getBookQuotes();
        }
    }
}
