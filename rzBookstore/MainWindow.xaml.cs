/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/

using System.Windows;

using BookStoreLIB;

namespace BookStoreGUI
{
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var userData = new UserData();
            var dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password) == true)
                    this.statusTextBlock.Text = "You are logged in as User #" + userData.UserID;
                else
                    MessageBox.Show(userData.authenticate(dlg.nameTextBox.Text, dlg.passwordTextBox.Password));
            }
        }
        private void exitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
        public MainWindow() { InitializeComponent(); }
        private void Window_Loaded(object sender, RoutedEventArgs e) { }
        private void addButton_Click(object sender, RoutedEventArgs e) { }
        private void chechoutButton_Click(object sender, RoutedEventArgs e) { }
    }
}
