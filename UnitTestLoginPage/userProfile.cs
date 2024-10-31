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
    }
}
