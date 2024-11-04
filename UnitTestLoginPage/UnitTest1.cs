using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Windows.Controls;


namespace BookStoreLIB
{
    [TestClass]
    public class UnitTest1
    {

        UserData userData = new UserData();
        string inputName, inputPassword, fullName;
        int actualUserId;

        [TestMethod]
        public void TestPurchaseHistory()
        {

            int testUserId = 8;
            PurchaseHistory purchaseHistory = new PurchaseHistory();

            DataSet result = purchaseHistory.GetPurchaseHistory(testUserId);


            Assert.IsNotNull(result, "Purchase history should not be null");
            Assert.IsTrue(result.Tables["PurchaseHistory"].Rows.Count > 0, "Purchase history should return at least one row");


            DataRow firstRow = result.Tables["PurchaseHistory"].Rows[0];
            Assert.AreEqual(15, firstRow["OrderID"]);
            Assert.AreEqual("Jon Skeet", firstRow["Author"]);
            Assert.AreEqual("NULLC# in Depth", firstRow["Title"]);

            DateTime expectedDate = DateTime.Parse("10/17/2024 5:23:40 PM");
            DateTime actualDate = Convert.ToDateTime(firstRow["OrderDate"]);

            // Allow small differences if they exist (e.g., different DateTime.Kind or milliseconds)
            Assert.IsTrue(Math.Abs((expectedDate - actualDate).TotalSeconds) < 1, "OrderDate should be the same up to seconds precision.");

            Assert.AreEqual(2, firstRow["Quantity"]);
            Assert.AreEqual(82.44, Convert.ToDouble(firstRow["TotalPrice"]), 0.01);
        }


        [TestMethod]
        public void TestCorrectLogin()
        {
            inputName = "dclair";
            inputPassword = "dc1234";
            Boolean expectedReturn = true;
            int expectedUserId = 1;
            Boolean actualReturn = userData.LogIn(inputName, inputPassword);
            String messageReturn = userData.authenticate(inputName, inputPassword);
            String expectedMessageReturn = "You are logged in as User #1";

            actualUserId = userData.UserID;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedMessageReturn, messageReturn);
        }

        [TestMethod]
        public void TestLogout()
        {
            // Need to login to test logout
            inputName = "dclair";
            inputPassword = "dc1234";
            int expectedUserId = 1;
            bool loginResult = userData.LogIn(inputName, inputPassword);

            Assert.IsTrue(loginResult);
            Assert.AreEqual(expectedUserId, userData.UserID);

            userData.LogOut();

            Assert.IsFalse(userData.LoggedIn);
            Assert.AreEqual(0, userData.UserID);
        }

        [TestMethod]
        public void TestLoginWrongUserNameOrPassword()
        {
            // Wrong Username
            inputName = "1rzeng";
            inputPassword = "rz1234";

            String messageReturn = userData.authenticate(inputName, inputPassword);
            String expectedMessageReturn = "Wrong username/password.";

            Boolean expectedReturm = false;
            Boolean actualReturn = userData.LogIn(inputName, inputPassword);
            Assert.AreEqual(expectedReturm, actualReturn);
            Assert.AreEqual(expectedMessageReturn, messageReturn);

            //Wrong password
            inputName = "rzeng";
            inputPassword = "fz1234";
            messageReturn = userData.authenticate(inputName, inputPassword);
            Assert.AreEqual(expectedReturm, actualReturn);
            Assert.AreEqual(expectedMessageReturn, messageReturn);
        }

        [TestMethod]
        public void TestLoginEmptyFieldsMessage()
        {
            inputName = "";
            inputPassword = "";
            String expectedReturm = "Please fill in all slots.";
            String actualReturn = userData.authenticate(inputName, inputPassword);
            Assert.AreEqual(expectedReturm, actualReturn);
            inputName = "rzeng";
            Assert.AreEqual(expectedReturm, actualReturn);
            inputName = "";
            inputPassword = "rz1234";
            Assert.AreEqual(expectedReturm, actualReturn);
        }
        [TestMethod]
        public void TestLoginInvalidPassword()
        {
            inputName = "rzeng";
            inputPassword = "0rz1234"; // First character non letter
            String expectedReturm = "A valid password needs to have at least six characters with both letters and numbers.";
            String actualReturn = userData.authenticate(inputName, inputPassword);

            Assert.AreEqual(expectedReturm, actualReturn);
            inputPassword = "rz123"; // Less than 6 characters
            actualReturn = userData.authenticate(inputName, inputPassword);
            Assert.AreEqual(expectedReturm, actualReturn);

            inputPassword = "rz12345"; // More than 6 characters
            actualReturn = userData.authenticate(inputName, inputPassword);
            Assert.AreEqual(expectedReturm, actualReturn);

            inputPassword = "rz123%"; // None alphanum character
            actualReturn = userData.authenticate(inputName, inputPassword);
            Assert.AreEqual(expectedReturm, actualReturn);
        }

        [TestMethod]
        public void TestSignupUniqueUser()
        {
            inputName = "test1"; // Increment by 1 every test to ensure unique username
            inputPassword = "te1234";
            fullName = "test";

            string expectedReturn = "Success";
            string actualReturn = userData.SignUp(inputName, inputPassword, fullName);
            Assert.AreEqual(expectedReturn, actualReturn);
        }

        [TestMethod]
        public void TestSignupUsernameAlreadyExist()
        {
            inputName = "rzeng"; // Username already exist
            inputPassword = "rz1234";
            fullName = "Raymond Z";

            String expectedReturm = "Username already exists. Please choose a different username.";
            String actualReturn = userData.SignUp(inputName, inputPassword, fullName);

            Assert.AreEqual(expectedReturm, actualReturn);
        }

        [TestMethod]
        public void TestSignupInvalidPassword()
        {
            inputName = "rzeng"; // Username already exist
            inputPassword = "rz123"; // Less than 6 characters
            fullName = "Raymond Z";

            String expectedReturm = "A valid password needs to have exactly six characters with both letters and numbers, starting with a letter.";
            String actualReturn = userData.SignUp(inputName, inputPassword, fullName);
            Assert.AreEqual(expectedReturm, actualReturn);

            inputPassword = "rz12345"; // More than 6 characters
            actualReturn = userData.SignUp(inputName, inputPassword, fullName);
            Assert.AreEqual(expectedReturm, actualReturn);

            inputPassword = "rz123%"; // None alphanum character
            actualReturn = userData.SignUp(inputName, inputPassword, fullName);
            Assert.AreEqual(expectedReturm, actualReturn);
        }


        [TestClass]
        public class MainWindowTests
        {
            private MainWindowTests mainWindow;
            private DataGrid ProductsDataGrid;
            private UserData userData;
            private BookOrder bookOrder;

            [TestInitialize]
            public void Setup()
            {
                mainWindow = new MainWindowTests();
                userData = new UserData();
                bookOrder = new BookOrder();
            }

            [TestMethod]
            public void AddBook_WithoutLogin_ShouldNotAddBook()
            {
                // Arrange
                userData.LogOut();
                mainWindow.ProductsDataGrid = new DataGrid();
                mainWindow.userData = userData;

                // Act
                mainWindow.addButton_Click(null, null);

                // Assert
                Assert.AreEqual(0, bookOrder.OrderItemList.Count);
            }

            private void addButton_Click(object value1, object value2)
            {
                throw new NotImplementedException();
            }

            [TestMethod]
            public void AddBook_WithLogin_ShouldAddBook()
            {
                // Arrange
                userData.LogIn("testuser", "testpassword");
                mainWindow.userData = userData;
                mainWindow.ProductsDataGrid = new DataGrid();
                var row = new DataGridRow();
                // Mock the row and add data accordingly for the book to be selected
                mainWindow.ProductsDataGrid.Items.Add(row);
                mainWindow.ProductsDataGrid.SelectedItem = row;

                // Act
                mainWindow.addButton_Click(null, null);

                // Assert
                Assert.AreEqual(1, bookOrder.OrderItemList.Count);
            }

            [TestMethod]
            public void AddDuplicateBook_ShouldNotAddBookTwice()
            {
                // Arrange
                userData.LogIn("testuser", "testpassword");
                mainWindow.userData = userData;
                mainWindow.ProductsDataGrid = new DataGrid();
                var row = new DataGridRow();
                mainWindow.ProductsDataGrid.Items.Add(row);
                mainWindow.ProductsDataGrid.SelectedItem = row;
                mainWindow.addButton_Click(null, null);

                // Act
                mainWindow.addButton_Click(null, null);

                // Assert
                Assert.AreEqual(1, bookOrder.OrderItemList.Count);
            }

            [TestMethod]
            public void AddBookToWatchlist_WithoutLogin_ShouldNotAdd()
            {
                // Arrange
                userData.LogOut();
                mainWindow.userData = userData;

                // Act
                mainWindow.AddToWatchlist_Click(null, null);

                // Assert
                // Assert that no watchlist entries were added
                var watchlist = new DALUserProfile().GetUserWatchlist(userData.UserID);
                Assert.AreEqual(0, watchlist.Count);
            }

            [TestMethod]
            public void AddBookToWatchlist_WithLogin_ShouldAdd()
            {
                // Arrange
                userData.LogIn("testuser", "testpassword");
                mainWindow.userData = userData;
                mainWindow.ProductsDataGrid = new DataGrid();
                var row = new DataGridRow();
                // Mock the row and add data accordingly for the book to be selected
                mainWindow.ProductsDataGrid.Items.Add(row);
                mainWindow.ProductsDataGrid.SelectedItem = row;

                // Act
                mainWindow.AddToWatchlist_Click(null, null);

                // Assert
                // Assert that the watchlist contains 1 entry
                var watchlist = new DALUserProfile().GetUserWatchlist(userData.UserID);
                Assert.AreEqual(1, watchlist.Count);
            }

            private void AddToWatchlist_Click(object value1, object value2)
            {
                throw new NotImplementedException();
            }
        }
    }

}
