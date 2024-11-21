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

            string updatedPassword = userPassTextBox.Text;
            string username = usernameTextBox.Text;
            userProfile profile = new userProfile();
            int isUpdated = profile.UpdateUserPassword(username ,updatedPassword);
            if(isUpdated>0)
            {
                MessageBox.Show("Password updated successfully.");
            }
            else if (isUpdated ==0)
            {
                MessageBox.Show("Error in updating password");
            }
            else
            {
                MessageBox.Show("SQL returned an error.");
            }
        }

        // Ban User
        private void BanUser_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented
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
