using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BookStoreLIB.Tests
{
    [TestClass]
    public class UnitTestBooks
    {
        [TestMethod]
        public void TestAddBook()
        {
            var dalBook = new DALBook();

            // ISBN needs to be unique, based on time
            string uniqueISBN = DateTime.Now.ToString("yyMMddHHmm"); 

            var testBook = new Book
            {
                ISBN = uniqueISBN,
                CategoryID = 1,
                Title = "Unit Test Book",
                Author = "Test Author",
                Price = 10,
                SupplierId = 1,
                Year = "1998",
                Edition = "1",
                Publisher = "Test Publisher",
                Stock = 69,
                RestockDate = null
            };

            bool result = dalBook.AddBook(testBook);

            Assert.IsTrue(result, "AddBook should return true on successful insertion.");
        }
    }
}
