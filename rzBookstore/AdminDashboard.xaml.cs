using System;
using System.Collections.Generic;
using System.Data;
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
            UserData userDataProfiles = new UserData();
            int userID;
            DataTable profiles;
            if (int.TryParse(query, out userID))
            {
                profiles = userDataProfiles.GetUsersInfo(userID);

                if(profiles!=null)
                {
                    this.userListView.ItemsSource = profiles.DefaultView;
                }
                else
                {
                    MessageBox.Show("No user found with the given ID.");
                }
            }
            //TODO if a username
            // To be implemented
            else
            {
                profiles = userDataProfiles.GetUsersInfo(query);
                if (profiles!=null)
                {
                    this.userListView.ItemsSource = profiles.DefaultView;
                }
                else
                {
                    MessageBox.Show("No user found with the given id");
                }
            }
        }

        private void userListView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedUser = userListView.SelectedItem as DataRowView;
            if (selectedUser!=null)
            {
                userIDTextBox.Text = selectedUser["UserID"].ToString();
                usernameTextBox.Text = selectedUser["UserName"].ToString();
                userPassTextBox.Text = selectedUser["Password"].ToString();
                fullNameTextBox.Text = selectedUser["FullName"].ToString();
            }
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
