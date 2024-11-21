using System;
using System.Windows;
using BookStoreLIB; 

namespace BookStoreGUI
{
    public partial class BookDetails : Window
    {
        private string ISBN; 
        private BookCatalog bookCatalog; 

        public BookDetails(string ISBN)
        {
            InitializeComponent();
            this.ISBN = ISBN;
            bookCatalog = new BookCatalog(); 
            LoadBookDetails();
        }

        private void LoadBookDetails()
        {
            
            var book = bookCatalog.GetBookDetails(ISBN);

            if (book != null)
            {
                titleTextBlock.Text = book.Title;
                authorTextBlock.Text = book.Author;
                priceTextBlock.Text = book.Price.ToString("C"); 

                if (book.Stock <= 0)
                {
                    stockStatusTextBlock.Text = "Sold Out";
                    stockStatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;

                    if (book.RestockDate.HasValue)  
                    {
                        restockDateTextBlock.Text = $"New stock expected on {book.RestockDate.Value.ToString("yyyy-MM-dd")}";  
                    }
                    else
                    {
                        restockDateTextBlock.Text = "Restock date not yet available";
                    }
                }
                else
                {
                    stockStatusTextBlock.Text = "In Stock";
                    restockDateTextBlock.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Book details not found.");
            }
        }

        private void NotifyMeButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (notifyMeCheckBox.IsChecked == true)
            {
               
                MessageBox.Show("You will be notified when the book is back in stock.");
            }
            else
            {
                MessageBox.Show("Please check the box if you'd like to be notified.");
            }
        }
    }
}
