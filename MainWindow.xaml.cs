using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"];
            bookOrder = new BookOrder();
            userData = new UserData();
            this.orderListView.ItemsSource = bookOrder.OrderItemList;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password))
                    this.statusTextBlock.Text = "You are logged in as User #" + userData.UserID;
                else
                    this.statusTextBlock.Text = "Your login failed. Please try again.";
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
            orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
            orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
            orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
            orderItemDialog.Owner = this;
            orderItemDialog.ShowDialog();

            if (orderItemDialog.DialogResult == true)
            {
                string isbn = orderItemDialog.isbnTextBox.Text;
                string title = orderItemDialog.titleTextBox.Text;
                double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
        }

        private void checkoutButton_Click(object sender, RoutedEventArgs e)
        {
            int orderId = bookOrder.PlaceOrder(userData.UserID);
            MessageBox.Show("Your order has been placed. Your order id is " + orderId.ToString());
        }

        private void discountButton_Click(object sender, RoutedEventArgs e)
        {
            if (orderListView.SelectedItem is OrderItem selectedOrderItem)
            {
                string discountInput = ShowInputDialog("Enter Discount Percentage", "Discount:");

                if (double.TryParse(discountInput, out double discountPercentage))
                {
                    try
                    {
                        selectedOrderItem.ApplyDiscount((decimal)discountPercentage);
                        MessageBox.Show($"Discount applied! New price: {selectedOrderItem.UnitPrice:C}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error applying discount: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid percentage.");
                }
            }
            else
            {
                MessageBox.Show("Please select an order item to apply the discount.");
            }
        }

        private string ShowInputDialog(string title, string prompt)
        {
            // Create a new window for the input dialog
            Window inputDialog = new Window()
            {
                Title = title,
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            StackPanel panel = new StackPanel();
            TextBox textBox = new TextBox() { Margin = new Thickness(10) };
            Button submitButton = new Button() { Content = "OK", Margin = new Thickness(10) };

            panel.Children.Add(new TextBlock { Text = prompt, Margin = new Thickness(10) });
            panel.Children.Add(textBox);
            panel.Children.Add(submitButton);
            inputDialog.Content = panel;

            submitButton.Click += (sender, e) => { inputDialog.DialogResult = true; inputDialog.Close(); };
            inputDialog.ShowDialog();

            return textBox.Text;
        }
    }
}
