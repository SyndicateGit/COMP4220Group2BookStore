using System;
using System.Windows;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class ProfileDialog : Window
    {
        public event EventHandler ProfileUpdated;

        private int _userID; // Store the userID for updates

        public ProfileDialog(int userID)
        {
            InitializeComponent();
            _userID = userID; // Save the user ID for later use
            LoadUserProfile(userID);
        }

        private void LoadUserProfile(int userID)
        {
            userProfile profile = new userProfile();
            var userDataRow = profile.GetUserProfile(userID);

            if (userDataRow != null)
            {
                nameTextBox.Text = userDataRow["FullName"].ToString();
                phoneTextBox.Text = userDataRow["Phone"].ToString();
                emailTextBox.Text = userDataRow["Email"].ToString();
                addressTextBox.Text = userDataRow["Address"].ToString();
                passwordTextBox.Text = userDataRow["Password"].ToString();
            }
            else
            {
                MessageBox.Show("Unable to load user profile.");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Get updated values from text boxes
            string updatedName = nameTextBox.Text;
            string updatedPhone = phoneTextBox.Text;
            string updatedEmail = emailTextBox.Text;
            string updatedAddress = addressTextBox.Text;
            string updatedPassword = passwordTextBox.Text;

            // Create an instance of the userProfile class
            userProfile profile = new userProfile();

            // Update the user profile in the database
            bool isUpdated = profile.UpdateUserProfile(_userID, updatedName, updatedPhone, updatedEmail, updatedAddress, updatedPassword);

            // Check if the update was successful
            if (isUpdated)
            {
                MessageBox.Show("Profile updated successfully.");
                this.Close(); // Close the dialog after update
            }
            else
            {
                MessageBox.Show("Failed to update profile. Please try again.");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}



