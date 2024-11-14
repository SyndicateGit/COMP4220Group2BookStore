/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BookStoreLIB
{
    public class UserData
    {
        public int UserID { set; get; }
        public string LoginName { set; get; }
        public string Password { set; get; }
        public Boolean LoggedIn { set; get; }
 
        public Boolean LogIn(string loginName, string passWord) 
        {
            var dbUser = new DALUserInfo();            
            UserID = dbUser.LogIn(loginName, passWord);  
            if (UserID > 0)
            {

                if (dbUser.IsUserBanned(UserID))
                {
                    MessageBox.Show("Your account has been banned.",
                                    "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                LoginName = loginName;
                Password = passWord;
                LoggedIn = true;
                return true;
            }
            else
            {
                LoggedIn = false;
                return false;
            }

        }

        public void LogOut()
        {
            UserID = 0;
            LoggedIn = false;
        }

        public String SignUp(string signupName, string password, string fullName)
        {
            if (signupName.Length == 0 || password.Length == 0 || fullName.Length == 0)
            {
                return "Please fill in all fields.";
            }

            if (!validPassword(password))
            {
                return "A valid password needs to have exactly six characters with both letters and numbers, starting with a letter.";
            }

            var dbUser = new DALUserInfo();
            int result = dbUser.Signup(signupName, password, fullName);

            if (result > 0)
            {
                UserID = result;
                LoginName = signupName;
                Password = password;
                LoggedIn = true;
                return "Success";
            }
            else if (result == -1)
            {
                return "Username already exists. Please choose a different username.";
            }
            else
            {
                return "An error occurred during signup. Please try again.";
            }
        }

        public String authenticate(String userName, String password)
        {
            if (userName.Length == 0 || password.Length == 0)
            {
                return "Please fill in all slots.";
            }
            if (!validPassword(password))
            {
                return "A valid password needs to have at least six characters with both letters and numbers.";
            }

            var userData = new UserData();
            if (userData.LogIn(userName
                , password))
            {
                return "You are logged in as User #" + userData.UserID;
            }
            else
            {
                return "Wrong username/password.";
            }
        }

        public Boolean validPassword(String password)
        {
            if (password.Length != 6 ||
                !password.All(char.IsLetterOrDigit) ||
                !char.IsLetter(password[0]) ||
                !password.Any(char.IsLetter) ||
                !password.Any(char.IsDigit))
            {
                return false;
            }
            return true;
        }
    }
}
