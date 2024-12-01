using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class ProfileDialog : Window
    {
        public event EventHandler ProfileUpdated;

        private int _userID; // Store the userID for updates
        private byte[] _newProfilePictureData;

        public ProfileDialog(int userID)
        {
            InitializeComponent();
            _userID = userID; // Save the user ID for later use
            LoadUserProfile(userID);
            LoadProfilePicture(_userID);
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
                decimal balance = (decimal)userDataRow["Balance"];
                balanceTextBox.Text = balance.ToString("0.00");
            }
            else
            {
                MessageBox.Show("Unable to load user profile.");
            }
        }

        private void LoadProfilePicture(int userId)
        {
            DALUpload dalUpload = new DALUpload();
            byte[] profilePictureData = dalUpload.GetProfilePicture(userId);

            if (profilePictureData != null)
            {
                using (MemoryStream ms = new MemoryStream(profilePictureData))
            {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            profilePictureImage.Source = image;
            }
        }
    }

     private void UploadButton_Click(object sender, RoutedEventArgs e)
     {
         OpenFileDialog openFileDialog = new OpenFileDialog
     {
         Title = "Select Profile Picture",
         Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
     };

     if (openFileDialog.ShowDialog() == true)
     {
         _newProfilePictureData = File.ReadAllBytes(openFileDialog.FileName);

         if (_newProfilePictureData == null || _newProfilePictureData.Length == 0)
         {
             MessageBox.Show("Failed to load profile picture data.");
             return;
         }
         else
         {
             MessageBox.Show("Profile picture loaded successfully.");
         }

         using (MemoryStream ms = new MemoryStream(_newProfilePictureData))
         {
             BitmapImage image = new BitmapImage();
             image.BeginInit();
             image.StreamSource = ms;
             image.CacheOption = BitmapCacheOption.OnLoad;
             image.EndInit();
             profilePictureImage.Source = image;
         }
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
            decimal balance = 0;
            if (!decimal.TryParse(balanceTextBox.Text, out balance))
            {
                MessageBox.Show("Please enter a valid balance amount.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Create an instance of the userProfile class
            userProfile profile = new userProfile();

            // Update the user profile in the database
            bool isUpdated = profile.UpdateUserProfile(_userID, updatedName, updatedPhone, updatedEmail, updatedAddress, updatedPassword, balance);

            // Check if the update was successful
              if (isUpdated)
                {
                    if (_newProfilePictureData != null)
                    {
                        DALUpload dalUpload = new DALUpload();
                        bool pictureUpdated = dalUpload.UpdateProfilePicture(_userID, _newProfilePictureData);

                    if (pictureUpdated)
                    {
                        MessageBox.Show("Profile and picture updated successfully.");
                    }
                    else
                    {
                MessageBox.Show("Profile updated, but failed to update picture.");
                    }
                }
            else
            {
                MessageBox.Show("Profile updated successfully.");
            }

        ProfileUpdated?.Invoke(this, EventArgs.Empty);
        this.Close();
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



