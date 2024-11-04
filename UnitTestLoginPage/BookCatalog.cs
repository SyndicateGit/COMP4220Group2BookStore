using System;
using System.Data;

namespace BookStoreLIB
{
    public class BookCatalog
    {
        private DALBookCatalog dalBookCatalog;

        
        public BookCatalog()
        {
            dalBookCatalog = new DALBookCatalog(); 
        }

       
        public Book GetBookDetails(string ISBN)
        {
           
            DataSet dsBooks = dalBookCatalog.GetBookInfo();



            
            DataRow[] foundBooks = dsBooks.Tables["Books"].Select($"ISBN = '{ISBN}'");
            if (foundBooks.Length > 0)
            {
                DataRow bookRow = foundBooks[0];


                DateTime? restockDate = null;
                if (bookRow["RestockDate"] != DBNull.Value)
                {
                    DateTime tempDate;
                    if (DateTime.TryParse(bookRow["RestockDate"].ToString(), out tempDate))
                    {
                        restockDate = tempDate;
                    }
                }

                Book book = new Book
                {
                    Title = bookRow["Title"].ToString(),
                    Author = bookRow["Author"].ToString(),
                    Price = Convert.ToDecimal(bookRow["Price"]),
                    Stock = Convert.ToInt32(bookRow["InStock"]),
                    RestockDate = restockDate
                };

                return book;
            }
            else
            {
               
                return null;
            }
        }
        public DataSet GetBookInfo()
        {
            return dalBookCatalog.GetBookInfo();  
        }
    }
}
