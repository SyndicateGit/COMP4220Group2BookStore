using System.Windows;
namespace BookStoreGUI
{
    public partial class LoginDialog : Window
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            SignupDialog signupDialog = new SignupDialog();
            this.Close(); 
            signupDialog.ShowDialog(); 
        }
    }
}