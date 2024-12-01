using BookStoreLIB;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BookStoreGUI
{
    public partial class ReviewsDialog : Window
    {
        private string selectedBookISBN;
        private int currentUserId;

        public ReviewsDialog(string isbn, int userId)
        {
            InitializeComponent();

            selectedBookISBN = isbn;
            currentUserId = userId;

            DALReview dalReview = new DALReview();
            List<Review> reviews = dalReview.GetReviews(isbn);

            if (reviews.Count > 0)
            {
                reviewsListView.ItemsSource = reviews;
            }
            else
            {
                MessageBox.Show("No reviews available for this book.", "No Reviews");
            }
        }

        private void AddReviewButton_Click(object sender, RoutedEventArgs e)
        {
            AddReviewDialog addReviewDialog = new AddReviewDialog(selectedBookISBN, currentUserId);

            if (addReviewDialog.ShowDialog() == true)
            {
                DALReview dalReview = new DALReview();
                List<Review> reviews = dalReview.GetReviews(selectedBookISBN);
                reviewsListView.ItemsSource = reviews;
            }
        }
    }

}

