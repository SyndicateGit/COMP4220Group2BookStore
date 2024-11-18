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

        private int _userId;
        private byte[] _newProfilePictureData;

        public ProfileDialog(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadUserProfile(_userId);
            LoadProfilePicture(_userId);
        }

        private void LoadUserProfile(int userId)
        {
            DALUserProfile profile = new DALUserProfile();
            var dataRow = profile.GetUserProfile(userId);

            if (dataRow != null)
            {
                nameTextBox.Text = dataRow["FullName"].ToString();
                phoneTextBox.Text = dataRow["Phone"].ToString();
                emailTextBox.Text = dataRow["Email"].ToString();
                addressTextBox.Text = dataRow["Address"].ToString();
                passwordTextBox.Text = dataRow["Password"].ToString();
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
            string updatedName = nameTextBox.Text;
            string updatedPhone = phoneTextBox.Text;
            string updatedEmail = emailTextBox.Text;
            string updatedAddress = addressTextBox.Text;
            string updatedPassword = passwordTextBox.Text;

            DALUserProfile profile = new DALUserProfile();
            bool isUpdated = profile.UpdateUserProfile(_userId, updatedName, updatedPhone, updatedEmail, updatedAddress, updatedPassword);

            if (isUpdated)
            {
                if (_newProfilePictureData != null)
                {
                    DALUpload dalUpload = new DALUpload();
                    bool pictureUpdated = dalUpload.UpdateProfilePicture(_userId, _newProfilePictureData);

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
