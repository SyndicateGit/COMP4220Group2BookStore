/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
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
using System.Data;
using BookStoreLIB;
using System.Collections.ObjectModel;

namespace BookStoreGUI
{
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;
        PurchaseHistory purchaseHistory;
        private int currentUserId; // Added to track the logged-in user's ID

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
            if (userData.LoggedIn)
            {
                userData.LogOut();
                this.statusTextBlock.Text = "You have logged out.";
                this.loginButton.Content = "Login";
                this.adminDashboardButton.Visibility = Visibility.Hidden;
                bookOrder.OrderItemList.Clear();
                this.orderListView.ItemsSource = null;
            }
            else
            {
                LoginDialog dlg = new LoginDialog();
                dlg.Owner = this;
                dlg.ShowDialog();

                if (dlg.DialogResult == true)
                {
                    if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password) == true)
                    {
                        currentUserId = userData.UserID; // Set currentUserId upon successful login
                        this.statusTextBlock.Text = "You are logged in as User #" + currentUserId;
                        this.loginButton.Content = "Logout";
                        this.orderListView.ItemsSource = bookOrder.OrderItemList;
                        this.adminDashboardButton.Visibility = Visibility.Hidden;
                    }
                    // Check if the user is admin and show the admin button if true
                    else if (dlg.nameTextBox.Text == "admin" && dlg.passwordTextBox.Password == "admin")
                    {
                        this.adminDashboardButton.Visibility = Visibility.Visible;
                        this.statusTextBlock.Text = "You are logged in as Admin";
                        this.loginButton.Content = "Logout";
                        userData.LoggedIn = true;
                        this.orderListView.ItemsSource = bookOrder.OrderItemList;
                    }
                    else
                        this.statusTextBlock.Text = "Your login failed. Please try again.";
                }
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData != null && userData.LoggedIn)
            {
                if (this.ProductsDataGrid.SelectedItems.Count > 0)
                {
                    OrderItemDialog orderItemDialog = new OrderItemDialog();
                    DataRowView selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
                    orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
                    orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
                    orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
                    orderItemDialog.Owner = this;
                    orderItemDialog.ShowDialog();


                    int stock = Convert.ToInt32(selectedRow.Row["InStock"]);
                    if (stock <= 0)
                    {
                        string isbn = selectedRow.Row.ItemArray[0].ToString();
                        BookDetails bookDetailsWindow = new BookDetails(isbn);
                        bookDetailsWindow.ShowDialog();
                        return;
                    }

                    if (orderItemDialog.DialogResult == true)
                    {
                        string isbn = orderItemDialog.isbnTextBox.Text;
                        string title = orderItemDialog.titleTextBox.Text;
                        double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                        int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);

                        // Check if the book is already in the order list
                        if (bookOrder.OrderItemList.Any(item => item.BookID == isbn))
                        {
                            MessageBox.Show("This book is already in your order list.");
                        }
                        else
                        {
                            bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a book to add to your order list.");
                }
            }
            else
            {
                MessageBox.Show("Please log in to add a book to your order list.");
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

        private Random _random = new Random();
        private void checkoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Define the chance for the lucky customer discount
            double discountChance = 0.10;  // Set the discount chance here (e.g., 10% chance)

            if (userData != null && userData.LoggedIn)
            {
                if (bookOrder.OrderItemList.Count > 0)
                {
                    // Lucky Customer logic (e.g., 10% chance)
                    double randomValue = _random.NextDouble(); // Get a random number between 0 and 1
                    double discountPercentage = randomValue <= discountChance ? 10 : 0;

                    // Apply discount to each order item if applicable
                    if (discountPercentage > 0)
                    {
                        foreach (var orderItem in bookOrder.OrderItemList)
                        {
                            orderItem.ApplyDiscount((decimal)discountPercentage);
                        }

                        // Show popup informing the user they are a lucky customer
                        MessageBox.Show($"Congratulations! You've received a {discountPercentage}% discount on your order.", "Lucky Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Show popup informing the user they are not a lucky customer
                        MessageBox.Show("No discount this time. Better luck next time!", "Lucky Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    // Calculate the total amount after any discounts
                    decimal totalAmount = bookOrder.OrderItemList.Sum(item => (decimal)item.SubTotal);

                    // Deduct balance after purchase
                    string deductionResult = userData.DeductBalance(totalAmount);
                    MessageBox.Show(deductionResult);

                    // Place the order if deduction was successful
                    if (!deductionResult.StartsWith("Insufficient"))
                    {
                        int orderId = bookOrder.PlaceOrder(currentUserId);
                        MessageBox.Show("Your order has been placed. Your order id is " + orderId.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Your order list is empty. Please add books before checking out.");
                }
            }
            else
            {
                MessageBox.Show("Please log in to place an order.");
            }
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

        /// DescriptionButton feature
        private void descriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is DataRowView selectedBook)
            {
                string title = selectedBook["Title"].ToString();
                string author = selectedBook["Author"].ToString();
                string price = string.Format("{0:C}", selectedBook["Price"]);
                string year = selectedBook["Year"].ToString();

                string description = $"Title: {title}\nAuthor: {author}\nPrice: {price}\nYear: {year}";

                MessageBox.Show(description,"Book Description");
            }
            else
            {
                MessageBox.Show("Please select a book from the list to view the description.","No Book Selected");
            }
        }

        private string ShowInputDialog(string title, string prompt)
        {
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

        private void purchaseHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData != null && userData.LoggedIn)
            {
                PHDialog pHDialog = new PHDialog(currentUserId);
                pHDialog.Owner = this;
                pHDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please log in to view your purchase history.");
            }
        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData != null && userData.LoggedIn)
            {
                ProfileDialog profileDialog = new ProfileDialog(currentUserId);
                profileDialog.Owner = this;
                profileDialog.ProfileUpdated += ProfileDialog_ProfileUpdated;
                profileDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please log in to access your profile.");
            }
        }

        private void ProfileDialog_ProfileUpdated(object sender, EventArgs e)
        {
            MessageBox.Show("Profile updated successfully!");
        }

        // Updated AddToWatchlist_Click to use currentUserId directly
        private void AddToWatchlist_Click(object sender, RoutedEventArgs e)
        {
            if (userData != null && userData.LoggedIn)
            {
                var selectedISBN = GetSelectedBookISBN();

                if (selectedISBN != null)
                {
                    try
                    {
                        DALUserProfile userProfileDal = new DALUserProfile();
                        var watchlist = userProfileDal.GetUserWatchlist(currentUserId);

                        // Check if the book is already in the watchlist
                        if (watchlist.Contains(selectedISBN))
                        {
                            MessageBox.Show("This book is already in your watchlist.");
                        }
                        else
                        {
                            userProfileDal.AddToWatchlist(currentUserId, selectedISBN);
                            MessageBox.Show("Book added to watchlist.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding book to watchlist: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a book to add to your watchlist.");
                }
            }
            else
            {
                MessageBox.Show("Please log in to add books to your watchlist.");
            }
        }

        private void viewWatchlistButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData != null && userData.LoggedIn)
            {
                DALUserProfile userProfileDal = new DALUserProfile();
                var watchlist = userProfileDal.GetUserWatchlist(currentUserId);

                MessageBox.Show(string.Join(Environment.NewLine, watchlist), "Your Watchlist");
            }
            else
            {
                MessageBox.Show("Please log in to view your watchlist.");
            }
        }

        private string GetSelectedBookISBN()
        { 
            if (ProductsDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProductsDataGrid.SelectedItem;
                return selectedRow["ISBN"].ToString();
            }
            return null;
        }
        public void ChangeBackgroundColor(string colorName)
        {
            // Change the background color of the window based on the passed color name
            switch (colorName.ToLower())
            {
                case "red":
                    this.Background = new SolidColorBrush(Colors.Red);
                    break;
                case "blue":
                    this.Background = new SolidColorBrush(Colors.Blue);
                    break;
                case "green":
                    this.Background = new SolidColorBrush(Colors.Green);
                    break;
                case "yellow":
                    this.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                default:
                    this.Background = new SolidColorBrush(Colors.White);  // Default to White if color is invalid
                    break;
            }
        }
        private void ChangeColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Example color change logic
            // Here you can let the user choose or pick a color. I'll use a hardcoded example for now.
            // Change the background color to a random color each time the button is clicked.

            string[] colors = new string[] { "red", "blue", "green", "yellow" };
            Random random = new Random();
            string selectedColor = colors[random.Next(colors.Length)];

            // Call the ChangeBackgroundColor method to update the background
            ChangeBackgroundColor(selectedColor);
        }


        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData != null && userData.LoggedIn)
            {
                currentUserId = userData.UserID;
                userStngs stg = new userStngs(currentUserId);
                stg.Owner = this;
                stg.ShowDialog();
            }
            else
                MessageBox.Show("Must sign in first");
        }
         private void adminDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Owner = this;
            adminDashboard.ShowDialog();
        }

        private void bookQuotesButton_Click(Object sender, RoutedEventArgs e)
        {
            BQDialog bq = new BQDialog();
            bq.Owner = this;
            bq.ShowDialog();
        }

        private void reviewButton_Click(Object sender, RoutedEventArgs e)
        {
            if (userData != null && userData.LoggedIn)
            {
                string selectedISBN = GetSelectedBookISBN();

                if (selectedISBN != null)
                {
                    ReviewsDialog reviewsDialog = new ReviewsDialog(selectedISBN, currentUserId);
                    reviewsDialog.Owner = this;
                    reviewsDialog.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please select a book.", "No Book Selected");
                }
            }
            else
            {
                MessageBox.Show("Please log in first.");
            }
        }
    }
}
