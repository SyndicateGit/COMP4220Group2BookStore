using System;
using System.Windows;
using System.ComponentModel;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class userStngs : Window, INotifyPropertyChanged
    {   
        private int _userId;
        private string loggedInUserName;
        UserData userdata = new UserData();
        public event PropertyChangedEventHandler PropertyChanged;

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
        public userStngs(int userId)
        {
            InitializeComponent();
            DataContext = this;
            _userId = userId;
            getUserName(userId);
           
        }

        private void getUserName(int userId)
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void balanceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updatePassButton_Click(object sender, RoutedEventArgs e)
        {

        }

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

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
