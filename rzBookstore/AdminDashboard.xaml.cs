using System;
using System.Collections.Generic;
using System.Windows;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class AdminDashboard : Window
    {
        private UserData userData;
        private List<Book> books;

        public AdminDashboard()
        {
            InitializeComponent();
            userData = new UserData();
            books = new List<Book>(); 
        }

        // Search User
        private void SearchUser_Click(object sender, RoutedEventArgs e)
        {
            string query = searchUserTextBox.Text;
            
            // To be implemented
        }

        // Update User
        private void UpdateUser_Click(object sender, RoutedEventArgs e)
        {

            // To be implemented
        }

        // Ban User
        public void BanUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userIDTextBox.Text) || string.IsNullOrWhiteSpace(usernameTextBox.Text) || string.IsNullOrWhiteSpace(fullNameTextBox.Text))
            {
                MessageBox.Show("Please fill out all fields (User ID, Username, and Full Name) before banning the user.",
                                "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (!int.TryParse(userIDTextBox.Text, out int userId))
            {
                MessageBox.Show("Invalid User ID. Please enter a valid number.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string username = usernameTextBox.Text.Trim();
            string fullName = fullNameTextBox.Text.Trim();

            var userService = new UserService();
            bool result = userService.BanUser(userId, username, fullName);

            if (result)
            {
                MessageBox.Show($"User '{username}' (ID: {userId}) has been successfully banned.",
                                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to ban the user. Please ensure the provided information is correct and try again.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Search Book
        private void SearchBook_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented
        }

        // Add New Book
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented
        }

        // Update Book
        private void UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented
        }
    }
}
