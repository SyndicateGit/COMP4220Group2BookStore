using System;
using System.ComponentModel;
using System.Windows;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class userStngs : Window, INotifyPropertyChanged
    {
        private int _userId; // Holds the logged-in user's ID
        private string loggedInUserName;
        UserData userdata = new UserData();
        public event PropertyChangedEventHandler PropertyChanged;

        // Property for binding to display the logged-in user's name
        public string LoggedInUserName
        {
            get => loggedInUserName;
            set
            {
                if (loggedInUserName != value)
                {
                    loggedInUserName = value;
                    OnPropertyChanged(nameof(LoggedInUserName));
                }
            }
        }

        // Constructor that takes the logged-in user's ID
        public userStngs(int userId)
        {
            InitializeComponent();
            DataContext = this;
            _userId = userId; // Initialize _userId with the logged-in user's ID
            LoadUserProfile(userId); // Load user profile data
        }

        // Method to load user profile information based on user ID
        private void LoadUserProfile(int userId)
        {
            userProfile profile = new userProfile();
            var dataRow = profile.GetUserProfile(userId);

            if (dataRow != null)
            {
                LoggedInUserName = dataRow["FullName"].ToString();
            }
            else
            {
                MessageBox.Show("Unable to load user profile.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to notify when a property changes (for data binding)
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Event handler for the balance button click (to be implemented as needed)
        private void balanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Balance functionality here
            MessageBox.Show("Balance feature is under development.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Event handler for updating the password (to be implemented as needed)
        private void updatePassButton_Click(object sender, RoutedEventArgs e)
        {
            // Update password functionality here
            MessageBox.Show("Update password feature is under development.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Event handler for deleting the account
        
        private void deleteAccButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete your account? This action cannot be undone.",
                "Confirm Account Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                DALDeleteAcc dalDeleteAcc = new DALDeleteAcc();
                MessageBox.Show("Attempting to delete account with UserID: " + _userId);

                bool isDeleted = dalDeleteAcc.DeleteAccount(_userId); // Use the initialized _userId

                if (isDeleted)
                {
                    MessageBox.Show("Account deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.Shutdown(); // Close application
                }
                else
                {
                    MessageBox.Show("Failed to delete the account. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Event handler to close the window without saving
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
