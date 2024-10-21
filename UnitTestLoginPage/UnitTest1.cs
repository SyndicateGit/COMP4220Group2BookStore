using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace BookStoreLIB
{
    [TestClass]
    public class UnitTest1
    {
        
        UserData userData = new UserData();
        string inputName, inputPassword;
        int actualUserId;

        [TestMethod]
        public void TestRetrieveProfile_UserID1_ReturnsCorrectProfile()
        {

            int userId = 1;
            string expectedFullName = "Donald Clark";
            string expectedEmail = "john@example.com";
            string expectedPhone = "1234567890";
            string expectedAddress = "123 Main St";

            userProfile userProfile = new userProfile();


            DataRow profile = userProfile.GetUserProfile(userId);


            Assert.AreEqual(expectedFullName, profile["FullName"].ToString());
            Assert.AreEqual(expectedEmail, profile["Email"].ToString());
            Assert.AreEqual(expectedPhone, profile["Phone"].ToString());
            Assert.AreEqual(expectedAddress, profile["Address"].ToString());
        }

        [TestMethod]
        public void TestUpdateProfile_UserID1_UpdatesSuccessfully()
        {

            int userId = 1;
            string updatedFullName = "Donald Clark";
            string updatedEmail = "john@example.com";
            string updatedPhone = "1234567890";
            string updatedAddress = "123 Main St";

            userProfile userProfile = new userProfile();


            bool isUpdated = userProfile.UpdateUserProfile(userId, updatedFullName, updatedPhone, updatedEmail, updatedAddress);


            Assert.IsTrue(isUpdated, "Profile should be updated successfully.");
        }


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
            inputName = "dclark";
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
        public void TestEmptyFieldsMessage()
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
        public void TestInvalidPassword()
        {
            inputName = "rzeng";
            inputPassword = "0rz1234"; // First character non letter
            String expectedReturm = "A valid password needs to have at least six characters with both letters and numbers.";
            String actualReturn = userData.authenticate(inputName, inputPassword);
            
            Assert.AreEqual(expectedReturm, actualReturn);
            inputPassword = "rz123"; // Less than 6 characters
            Assert.AreEqual(expectedReturm, actualReturn);
            inputPassword = "rz12345"; // More than 6 characters
            Assert.AreEqual(expectedReturm, actualReturn);
            inputPassword = "rz123%"; // None alphanum character
            Assert.AreEqual(expectedReturm, actualReturn);
        }
    }
}
