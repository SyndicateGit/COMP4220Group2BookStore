using System;
using System.Windows;
using System.ComponentModel;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class userStngs : Window, INotifyPropertyChanged
    {
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
            getUserName(userId);
           
        }

        private void getUserName(int userId)
        {
            userProfile profile = new userProfile();
            var dataRow = profile.GetUserProfile(userId);

            LoggedInUserName = dataRow["FullName"].ToString();
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

        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
