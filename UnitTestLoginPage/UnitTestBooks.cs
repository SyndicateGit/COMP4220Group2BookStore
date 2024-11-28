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

        [TestMethod]
        public void TestFindBookByISBN()
        {
            string testISBN = "1111111111";
            string expectedTitle = "Test1";

            var dalBook = new DALBook();
            var result = dalBook.GetBookByISBNOrTitle(testISBN);

            Assert.IsNotNull(result, "Book should be found.");
            Assert.AreEqual(testISBN, result.ISBN, "Found book should have same ISBN");
            Assert.AreEqual(expectedTitle, result.Title, "Found book should have expected title");
        }

        [TestMethod]
        public void TestFindBookByTitle()
        {
            string partialTitle = "Test1"; 
            string expectedISBN = "1111111111"; 

            var dalBook = new DALBook();
            var result = dalBook.GetBookByISBNOrTitle(partialTitle);

            // Assert
            Assert.IsNotNull(result, "The book should be found.");
            Assert.AreEqual(expectedISBN, result.ISBN, "The returned book's ISBN should match the expected value.");
            Assert.IsTrue(result.Title.Contains(partialTitle), "The returned book's Title should match the search value.");
        }
    }
}
