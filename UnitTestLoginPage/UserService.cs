using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class UserService
    {
        public bool BanUser(int userId, string username, string fullName)
        {
            
            var dbUser = new DALUserInfo();
            return dbUser.BanUserIfValid(userId, username, fullName);
        }
    }
}

