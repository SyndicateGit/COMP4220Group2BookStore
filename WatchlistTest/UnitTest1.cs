using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreGUI;
using BookStoreLIB;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WatchlistTest
{
    [TestClass]
    public class MainWindowTests
    {
        private MainWindow mainWindow;
        private UserData userData;
        private BookOrder bookOrder;

        [TestInitialize]
        public void Setup()
        {
            mainWindow = new MainWindow();
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
    }
}
