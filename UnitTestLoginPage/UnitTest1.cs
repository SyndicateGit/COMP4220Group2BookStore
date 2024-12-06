using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
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

     int testUserId = 1;
     PurchaseHistory purchaseHistory = new PurchaseHistory();

     DataSet result = purchaseHistory.GetPurchaseHistory(testUserId);


     Assert.IsNotNull(result, "Purchase history should not be null");
     Assert.IsTrue(result.Tables["PurchaseHistory"].Rows.Count > 0, "Purchase history should return at least one row");

            DataRow firstRow = result.Tables["PurchaseHistory"].Rows[0];
            Assert.AreEqual(114, firstRow["OrderID"]);
            Assert.AreEqual("Johnny Rowling", firstRow["Author"]);
            Assert.AreEqual("Python for Beginners", firstRow["Title"]);

            DateTime expectedDate = DateTime.Parse("12/6/2024 3:49:23 PM");
            DateTime actualDate = Convert.ToDateTime(firstRow["OrderDate"]);
    
               // Allow small differences if they exist (e.g., different DateTime.Kind or milliseconds)
     Assert.IsTrue(Math.Abs((expectedDate - actualDate).TotalSeconds) < 1, "OrderDate should be the same up to seconds precision.");

            Assert.AreEqual(1, firstRow["Quantity"]);
            Assert.AreEqual(79.99, Convert.ToDouble(firstRow["SubTotal"]), 0.01);
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
        public void TestWrongUserNameOrPassword()
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
            inputName = "testUser_" + DateTime.Now.ToString("HHmmss"); // Uses datetime to make username unique
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

            Assert.AreEqual(expectedReturm, actualReturn);

            inputPassword = "rz12345"; // More than 6 characters
            actualReturn = userData.SignUp(inputName, inputPassword, fullName);
            Assert.AreEqual(expectedReturm, actualReturn);

            inputPassword = "rz123%"; // None alphanum character
            actualReturn = userData.SignUp(inputName, inputPassword, fullName);
            Assert.AreEqual(expectedReturm, actualReturn);
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
        public void TestBookAvailabilityByTitle()
        {
            string testTitle = "Microsoft Visual C# 2012: An Introduction to Object-Oriented Programming";
            BookCatalog bookCatalog = new BookCatalog();


            DataSet dsBooks = bookCatalog.GetBookInfo();
            if (dsBooks == null)
            {
                Assert.Fail("DataSet dsBooks is null. Ensure GetBookInfo returns a valid DataSet.");
                return;
            }


            if (!dsBooks.Tables.Contains("Books"))
            {
                Assert.Fail("Books table not found in the dataset.");
                return;
            }


            DataRow[] foundBooks = dsBooks.Tables["Books"].Select($"Title = '{testTitle}'");

            int stockCount = 0;
            DateTime? restockDate = null;

            if (foundBooks.Length > 0)
            {
                DataRow bookRow = foundBooks[0];
                stockCount = Convert.ToInt32(bookRow["InStock"]);

                if (stockCount == 0 && bookRow["RestockDate"] != DBNull.Value)
                {
                    DateTime tempDate;
                    if (DateTime.TryParse(bookRow["RestockDate"].ToString(), out tempDate))
                    {
                        restockDate = tempDate;
                    }
                }


                Console.WriteLine($"Stock Count: {stockCount}");
                Console.WriteLine($"Restock Date: {(restockDate.HasValue ? restockDate.Value.ToString("yyyy-MM-dd") : "No Restock Date Available")}");
            }
            else
            {
                Assert.Fail("No books found with the provided title.");
            }

            if (stockCount > 0)
            {
                Assert.IsTrue(stockCount > 0, "The book should be in stock.");
            }
            else
            {
                Assert.IsNotNull(restockDate, "Restock date should be provided for out-of-stock items.");
                Assert.IsTrue(restockDate.HasValue, $"New stock expected on {restockDate.Value:yyyy-MM-dd}");
            }


            Console.WriteLine("TestBookAvailabilityByTitle completed successfully.");
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            // Arrange
            int userID = 45; // Assuming an existing user ID
            string originalUsername = "TestUserName2";
            string originalPassword = "te1234";
            string originalFullName = "TestFullName";
            string updatedUsername = "TestUserName1";
            string updatedPassword = "te1235";
            string updatedFullName = "TestFullName2";

            userProfile userInfo = new userProfile();

            int updateResult = userInfo.UpdateUser(userID, updatedUsername, updatedPassword, updatedFullName);

            Assert.AreEqual(1, updateResult, "UpdateUser should return 1 if the update was successful.");

            // Fetch the user
            DALUserProfile dalUserProfile = new DALUserProfile();
            DataRow updatedUser = dalUserProfile.GetUserProfile(userID);

   
            Assert.AreEqual(updatedPassword, updatedUser["Password"].ToString(), "Password should be updated.");
            Assert.AreEqual(updatedFullName, updatedUser["FullName"].ToString(), "Full name should be updated.");

            // Restore original
            updateResult = userInfo.UpdateUser(userID, originalUsername, originalPassword, originalFullName);
            Assert.AreEqual(1, updateResult, "Restore original user details should succeed.");

            // Fetch and check if restoration worked
            DataRow restoredUser = dalUserProfile.GetUserProfile(userID);
            Assert.AreEqual(originalPassword, restoredUser["Password"].ToString(), "Password should be restored.");
            Assert.AreEqual(originalFullName, restoredUser["FullName"].ToString(), "Full name should be restored.");
    }

        [TestMethod]
            public void GetBookQuotes_ReturnsQuotes()
            {
                BookQuotes bookQuotes = new BookQuotes();
                DataSet quotes = bookQuotes.getBookQuotes();

                // Assert
                Assert.IsNotNull(quotes);
                Assert.IsTrue(quotes.Tables["BookQuotes"].Rows.Count > 0, "Expected at least one quote in the dataset.");
            }

            [TestMethod]
            public void AddBookQuote_ValidData_AddsQuote()
            {
                BookQuotes bookQuotes = new BookQuotes();

                string title = "Test Book";
                string author = "Test Author";
                string quote = "This is a test quote.";

                // Act
                bool result = bookQuotes.addBookQuote(title, author, quote);

                // Assert
                Assert.IsTrue(result, "Adding the quote should return true.");

                // Verify that the quote is now in the dataset
                DataSet quotes = bookQuotes.getBookQuotes();
                bool quoteExists = false;
                foreach (DataRow row in quotes.Tables["BookQuotes"].Rows)
                {
                    if (row["Book_Title"].ToString() == title &&
                        row["Book_Author"].ToString() == author &&
                        row["Quote"].ToString() == quote)
                    {
                        quoteExists = true;
                        break;
                    }
                }
                Assert.IsTrue(quoteExists, "The new quote should be present in the dataset.");
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void AddBookQuote_EmptyBookTitle_ThrowsArgumentException()
            {
                BookQuotes bookQuotes = new BookQuotes();
                // Arrange
                string title = ""; // Empty title
                string author = "";
                string quote = "";

                // Act
                var result = bookQuotes.addBookQuote(title, author, quote);

                // Assert: Expecting an exception, so no explicit assert is necessary
            }


        [TestMethod]
        public void TestFindUserAdminValidID()
        {
            //Tests corresponding to userID
            actualUserId = 14;
            UserData userDataProfiles = new UserData();
            DataTable profiles = userDataProfiles.GetUsersInfo(actualUserId);
            int expectedRows = 1;
            string expectedName = "dclair500";
            Assert.AreEqual(expectedRows, profiles.Rows.Count, "There should be only 1 row returned");
            Assert.AreEqual(expectedName, profiles.Rows[0]["UserName"].ToString());

        }
        [TestMethod]
        public void TestFindUserPrefixMatch()
        {
            inputName = "dc";
            UserData userDataProfiles = new UserData();
            DataTable profiles = userDataProfiles.GetUsersInfo(inputName);
            Assert.IsTrue(profiles.Rows.Count > 1, "Multiple rows should be returned");
            string expectedName = "dclair";
            Assert.AreEqual(expectedName, profiles.Rows[0]["UserName"].ToString());
        }
        [TestMethod]
        public void TestFindUserAdminValidName()
        {
            inputName = "selse";
            int expectedRows = 1;
            UserData userDataProfiles = new UserData();
            DataTable profiles = userDataProfiles.GetUsersInfo(inputName);
            int actualLength = profiles.Rows.Count;
            Assert.AreEqual(expectedRows, actualLength, "Only 1 row should be returned");
            int expectedId = 6;
            Assert.AreEqual(expectedId, profiles.Rows[0][0], "UserID should be 6");

        }
        [TestMethod]
        public void TestFindUserAdminInvalidName()
        {
            inputName = "afjdksaffdsa";
            UserData userDataProfiles = new UserData();
            DataTable profiles = userDataProfiles.GetUsersInfo(inputName);
            Assert.IsFalse(profiles.Rows.Count > 0, "No rows should be returned");
        }
        [TestMethod]
        public void TestFindUserAdminInvalidID()
        {
            int wrongInputID = 999;
            UserData userDataProfiles = new UserData();
            DataTable profiles = userDataProfiles.GetUsersInfo(wrongInputID);
            Assert.IsFalse(profiles.Rows.Count > 0);

        }


        [TestMethod]
        public void TestReviewButtonWhenLoggedIn()
        {
            inputName = "dclair";
            inputPassword = "dc1234";

            userData.LogIn(inputName, inputPassword);
            Assert.IsTrue(userData.LoggedIn);

            var reviewButtonClicked = false;
            if (userData.LoggedIn)
            {
                reviewButtonClicked = true;
            }

            Assert.IsTrue(reviewButtonClicked);
        }

        [TestMethod]
        public void TestReviewButtonWhenNotLoggedIn()
        {
            inputName = "";
            inputPassword = "";

            var reviewButtonClicked = false;
            if (string.IsNullOrEmpty(inputName) || string.IsNullOrEmpty(inputPassword))
            {
                reviewButtonClicked = false;
            }

            Assert.IsFalse(reviewButtonClicked);
        }

        [TestMethod]
        public void TestAddReviewEmptyText()
        {
            string reviewText = "";
            string isbn = "1111111111";
            int userId = 1;

            var result = AddReview(isbn, userId, reviewText);

            Assert.IsFalse(result, "Review should not be added when it is empty.");
        }

        [TestMethod]
        public void TestAddReviewValidText()
        {
            string reviewText = "Great book nice, I like it!";
            string isbn = "1111111111"; 
            int userId = 1;

            try
            {
                var result = AddReview(isbn, userId, reviewText);
                Assert.IsTrue(result, "Review should be added successfully.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Test failed due to an exception: " + ex.Message);
            }
        }

        private bool AddReview(string isbn, int userId, string reviewText)
        {
            if (string.IsNullOrEmpty(reviewText))
            {
                return false; 
            }

            try
            {
                if (isbn.Length != 10)
                {
                    Console.WriteLine("Invalid ISBN length. Must be 10 characters.");
                    return false;
                }

                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.MSSQLConnection))
                {
                    conn.Open(); 

                    string query = "INSERT INTO dbo.BookReview (ISBN, UserID, ReviewText) VALUES (@ISBN, @UserID, @ReviewText)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ISBN", isbn);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@ReviewText", reviewText);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        Console.WriteLine($"Rows affected: {rowsAffected}");

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding review: " + ex.Message);
                return false;
            }
        }

        [TestMethod]
        public void TestDisplayReviews()
        {
            string isbn = "1111111111";

            DALReview dalReview = new DALReview();
            var reviews = dalReview.GetReviews(isbn);

            Assert.IsTrue(reviews.Count > 0, "Reviews should not be empty.");
        }
 [TestClass]
public class DALDeleteAccTests
{
    private DALDeleteAcc dalDeleteAcc;

    [TestInitialize]
    public void Setup()
    {
       
        dalDeleteAcc = new DALDeleteAcc();
    }

    [TestMethod]
    public void TestDeleteAccount_ValidUserId()
    {
        
        int validUserId = 4; 
        Boolean expectedReturn = true;

       
        Boolean actualReturn = dalDeleteAcc.DeleteAccount(validUserId);

        
        Assert.AreEqual(expectedReturn, actualReturn, "The account should be deleted successfully for a valid user ID.");
    }

    [TestMethod]
    public void TestDeleteAccount_InvalidUserId()
    {
        
        int invalidUserId = -1; 
        Boolean expectedReturn = false;

       
        Boolean actualReturn = dalDeleteAcc.DeleteAccount(invalidUserId);

        
        Assert.AreEqual(expectedReturn, actualReturn, "The account should not be deleted for an invalid user ID.");
    }
    [TestMethod]
    public void Test_DeleteAccount_SQLExceptionHandling()
    {
        
        int problematicUserId = 999; 
        Boolean expectedReturn = false;

        
        try
        {
            Boolean actualReturn = dalDeleteAcc.DeleteAccount(problematicUserId);
            Assert.AreEqual(expectedReturn, actualReturn, "No account should be deleted if an exception occurs.");
        }
        catch (SqlException ex)
        {
            Assert.Fail("SQL exception was not handled properly: " + ex.Message);
        }
    }

    [TestMethod]
    public void Test_DeleteAccount_ConnectionManagement()
    {
        
        int validUserId = 4;

        
        dalDeleteAcc.DeleteAccount(validUserId);

        
        Assert.IsTrue(dalDeleteAcc.IsConnectionClosed, "The database connection should be closed after execution.");
    }


}



    }


    }

