using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class userProfile
    {
        // Property for the user's watchlist
        public List<string> Watchlist { get; set; } = new List<string>();

        // Method to get user profile information based on user ID
        public DataRow GetUserProfile(int userID)
        {
            DALUserProfile dalUserProfile = new DALUserProfile();
            return dalUserProfile.GetUserProfile(userID);
        }

        // Method to update user profile information
        public bool UpdateUserProfile(int userID, string name, string phone, string email, string address)
        {
            DALUserProfile dalUserProfile = new DALUserProfile();
            return dalUserProfile.UpdateUserProfile(userID, name, phone, email, address);
        }

        // Method to add a book to the user's watchlist
        public void AddBookToWatchlist(string isbn)
        {
            if (!Watchlist.Contains(isbn))
            {
                Watchlist.Add(isbn);
            }
        }

        // Method to remove a book from the user's watchlist
        public void RemoveBookFromWatchlist(string isbn)
        {
            if (Watchlist.Contains(isbn))
            {
                Watchlist.Remove(isbn);
            }
        }

        // Method to get all books in the user's watchlist
        public List<string> GetAllWatchlistBooks()
        {
            return Watchlist;
        }
    }
}
