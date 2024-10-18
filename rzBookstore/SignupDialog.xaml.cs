using System.Windows;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class SignupDialog : Window
    {
        public SignupDialog()
        {
            InitializeComponent();
        }

        private void signupOkButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Password;
            string fullName = fullnameTextBox.Text;

            var userData = new UserData();
            string result = userData.SignUp(username, password, fullName);

            if (result == "Success")
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void signupCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}