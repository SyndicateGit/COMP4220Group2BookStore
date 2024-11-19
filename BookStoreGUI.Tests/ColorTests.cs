using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreGUI;
using System.Windows.Media; // For SolidColorBrush and Colors
using System.Windows;

namespace BookStoreGUI.Tests
{
    [TestClass]
    public class ColorTests
    {
        private MainWindow mainWindow;

        [TestInitialize]
        public void Setup()
        {
            // Initialize MainWindow instance
            mainWindow = new MainWindow();
            // Ensure it's run on the UI thread
            mainWindow.Show();
        }

        [TestMethod]
        public void TestChangeBackgroundColorToRed()
        {
            // Act: Change background color
            mainWindow.ChangeBackgroundColor("Red");

            // Assert
            SolidColorBrush backgroundBrush = mainWindow.Background as SolidColorBrush;
            Assert.IsNotNull(backgroundBrush, "Background should be a SolidColorBrush.");
            Assert.AreEqual(Colors.Red, backgroundBrush.Color, "Background color should be Red.");
        }

        [TestMethod]
        public void TestChangeBackgroundColorToBlue()
        {
            // Act: Change background color
            mainWindow.ChangeBackgroundColor("Blue");

            // Assert
            SolidColorBrush backgroundBrush = mainWindow.Background as SolidColorBrush;
            Assert.IsNotNull(backgroundBrush, "Background should be a SolidColorBrush.");
            Assert.AreEqual(Colors.Blue, backgroundBrush.Color, "Background color should be Blue.");
        }

        [TestMethod]
        public void TestChangeBackgroundColorToGreen()
        {
            // Act: Change background color
            mainWindow.ChangeBackgroundColor("Green");

            // Assert
            SolidColorBrush backgroundBrush = mainWindow.Background as SolidColorBrush;
            Assert.IsNotNull(backgroundBrush, "Background should be a SolidColorBrush.");
            Assert.AreEqual(Colors.Green, backgroundBrush.Color, "Background color should be Green.");
        }

        [TestMethod]
        public void TestChangeBackgroundColorToYellow()
        {
            // Act: Change background color
            mainWindow.ChangeBackgroundColor("Yellow");

            // Assert
            SolidColorBrush backgroundBrush = mainWindow.Background as SolidColorBrush;
            Assert.IsNotNull(backgroundBrush, "Background should be a SolidColorBrush.");
            Assert.AreEqual(Colors.Yellow, backgroundBrush.Color, "Background color should be Yellow.");
        }

        [TestMethod]
        public void TestChangeBackgroundColorToWhiteForInvalidColor()
        {
            // Act: Change background color with an invalid color
            mainWindow.ChangeBackgroundColor("Purple");

            // Assert: Should default to White
            SolidColorBrush backgroundBrush = mainWindow.Background as SolidColorBrush;
            Assert.IsNotNull(backgroundBrush, "Background should be a SolidColorBrush.");
            Assert.AreEqual(Colors.White, backgroundBrush.Color, "Background color should default to White for an invalid color.");
        }

        [TestMethod]
        public void TestDefaultBackgroundColorOnStartup()
        {
            // Act: Check the initial background color on startup
            SolidColorBrush backgroundBrush = mainWindow.Background as SolidColorBrush;

            // Assert: The background should be white by default
            Assert.IsNotNull(backgroundBrush, "Background should be a SolidColorBrush.");
            Assert.AreEqual(Colors.White, backgroundBrush.Color, "Background color should be White by default.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Close the MainWindow (run this on the UI thread if needed)
            mainWindow.Close();
        }
    }
}
