using BookStoreLIB;
using System.Windows;

namespace BookStoreGUI
{
    public partial class AddReviewDialog : Window
    {
        private readonly string isbn;
        private readonly int userId;

        public AddReviewDialog(string isbn, int userId)
        {
            InitializeComponent();
            this.isbn = isbn;
            this.userId = userId;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string reviewText = reviewTextBox.Text.Trim();

            if (string.IsNullOrEmpty(reviewText))
            {
                MessageBox.Show("Review cannot be empty.", "Validation Error");
                return;
            }

            DALReview dalReview = new DALReview();
            if (dalReview.AddReview(isbn, userId, reviewText))
            {
                MessageBox.Show("Review added successfully.", "Success");
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Failed to add review. Please try again.", "Error");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
