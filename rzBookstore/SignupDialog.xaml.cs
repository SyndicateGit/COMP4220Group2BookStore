using System.Windows;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for SignupDialog.xaml
    /// </summary>
    public partial class SignupDialog : Window
    {
        public SignupDialog()
        {
            InitializeComponent();
        }

        // Handle the Signup button click
        private void signupOkButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void signupCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
