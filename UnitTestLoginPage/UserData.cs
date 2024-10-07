/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public String authenticate(String userName, String password)
        {
            if (userName.Length == 0 || password.Length == 0)
            {
                return "Please fill in all slots.";
            }
            if (password.Length != 6 ||
                !password.All(char.IsLetterOrDigit) ||
                !char.IsLetter(password[0]) ||
                !password.Any(char.IsLetter) ||
                !password.Any(char.IsDigit))
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
    }
}
