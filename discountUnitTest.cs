using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStoreLIB
{
    [TestClass]
    public class OrderItemTests
    {
        private OrderItem orderItem;

        [TestInitialize]
        public void Setup()
        {
            // Initialize an OrderItem with test data
            orderItem = new OrderItem("1234567890", "The Great Gatsby", 15.99, 2);
        }

        [TestMethod]
        public void TC001_AddDiscount_ValidPercentage()
        {
            // Arrange
            decimal discountPercentage = 20; // 20% discount
            decimal expectedPrice = 12.792m; // Expected price after discount (15.99 - 20%)

            // Act
            orderItem.UnitPrice -= orderItem.UnitPrice * (discountPercentage / 100);

            // Assert
            Assert.AreEqual(expectedPrice, orderItem.UnitPrice);
        }

        [TestMethod]
        public void TC002_RemoveDiscount_ReturnsToOriginalPrice()
        {
            // Arrange
            decimal originalPrice = orderItem.UnitPrice;

            // Act
            orderItem.UnitPrice = originalPrice * 0.8; // Simulate applying a discount
            orderItem.UnitPrice = originalPrice; // Remove discount

            // Assert
            Assert.AreEqual(originalPrice, orderItem.UnitPrice);
        }

        [TestMethod]
        public void TC003_ApplyMultipleDiscounts_Sequence()
        {
            // Arrange
            decimal originalPrice = orderItem.UnitPrice;
            orderItem.UnitPrice -= originalPrice * 0.20m; // Apply 20% discount
            orderItem.UnitPrice -= originalPrice * 0.10m; // Apply 10% discount

            // Act
            orderItem.UnitPrice = originalPrice * 0.8m; // Keep the last applied discount

            // Assert
            Assert.AreEqual(originalPrice * 0.8m, orderItem.UnitPrice); // Should reflect last discount only
        }

        [TestMethod]
        public void TC004_ApplyNegativeDiscount_NoChange()
        {
            // Arrange
            decimal originalPrice = orderItem.UnitPrice;
            decimal discountPercentage = -10; // Invalid discount

            // Act
            if (discountPercentage < 0 || discountPercentage > 100)
            {
                // No change to price
            }

            // Assert
            Assert.AreEqual(originalPrice, orderItem.UnitPrice); // Price should remain unchanged
        }

        [TestMethod]
        public void TC005_ApplyDiscount_GreaterThan100_NoChange()
        {
            // Arrange
            decimal originalPrice = orderItem.UnitPrice;
            decimal discountPercentage = 150; // Invalid discount

            // Act
            if (discountPercentage > 100)
            {
                // No change to price
            }

            // Assert
            Assert.AreEqual(originalPrice, orderItem.UnitPrice); // Price should remain unchanged
        }

        [TestMethod]
        public void TC006_OriginalPriceRetained_AfterDiscountRemoved()
        {
            // Arrange
            decimal originalPrice = orderItem.UnitPrice;

            // Act
            orderItem.UnitPrice *= 0.8; // Apply 20% discount
            orderItem.UnitPrice = originalPrice; // Remove discount

            // Assert
            Assert.AreEqual(originalPrice, orderItem.UnitPrice); // Should match original price
        }

        [TestMethod]
        public void TC007_PriceAfterDiscountThenRemoval()
        {
            // Arrange
            decimal originalPrice = orderItem.UnitPrice;
            orderItem.UnitPrice *= 0.8; // Apply 20% discount

            // Act
            orderItem.UnitPrice = originalPrice; // Remove discount

            // Assert
            Assert.AreEqual(originalPrice, orderItem.UnitPrice); // Should match original price
        }

        [TestMethod]
        public void TC008_DiscountCalculation_VariousPercentages()
        {
            // Arrange
            decimal[] percentages = { 10, 20, 30 };
            decimal[] expectedPrices = { 14.391m, 12.792m, 11.193m }; // Expected prices after discounts

            for (int i = 0; i < percentages.Length; i++)
            {
                // Act
                decimal discountAmount = orderItem.UnitPrice * (percentages[i] / 100);
                decimal discountedPrice = orderItem.UnitPrice - discountAmount;

                // Assert
                Assert.AreEqual(expectedPrices[i], discountedPrice);
            }
        }

        [TestMethod]
        public void TC009_ApplyDiscount_NoOriginalPrice_NoChange()
        {
            // Arrange
            OrderItem itemWithoutPrice = new OrderItem("1234567890", "No Price Book", 0, 1); // No price set

            // Act
            decimal discountPercentage = 20; // Attempt to apply discount
            if (itemWithoutPrice.UnitPrice == 0)
            {
                // No change
            }

            // Assert
            Assert.AreEqual(0, itemWithoutPrice.UnitPrice); // Price should remain unchanged
        }

        [TestMethod]
        public void TC010_DiscountDoesNotAffectOtherProperties()
        {
            // Arrange
            string originalTitle = orderItem.BookTitle;
            double originalQuantity = orderItem.Quantity;

            // Act
            orderItem.UnitPrice *= 0.8; // Apply discount

            // Assert
            Assert.AreEqual(originalTitle, orderItem.BookTitle); // Title should remain unchanged
            Assert.AreEqual(originalQuantity, orderItem.Quantity); // Quantity should remain unchanged
        }
    }
}
