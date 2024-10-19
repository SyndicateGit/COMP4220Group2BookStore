using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class PurchaseHistory
    {
        public DataSet GetPurchaseHistory(int userID)
        {
            DALPurchaseHistory purchaseHistory = new DALPurchaseHistory();
            return purchaseHistory.GetPhInfo(userID);
        }
    }
}
