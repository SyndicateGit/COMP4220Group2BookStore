using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreLIB;
using System;

namespace UnitTests
{
    [TestClass]
    public class DiscountUnitTest
    {
        private OrderItem orderItem;

        [TestInitialize]
        public void Setup()
        {
            orderItem = new OrderItem("1234567890", "The Great Gatsby", 15.99, 2);
        }

        [TestMethod]
        public void TC001_ApplyDiscount_ValidPercentage()
        {
            decimal discountPercentage = 20m; // 20% discount
            double expectedPrice = 12.792; // Expected price after discount

            orderItem.ApplyDiscount(discountPercentage);

            Assert.AreEqual(expectedPrice, orderItem.UnitPrice, 0.001);
        }

        [TestMethod]
        public void TC002_RemoveDiscount_ReturnsToOriginalPrice()
        {
            double originalPrice = orderItem.UnitPrice;

            orderItem.ApplyDiscount(20m);
            orderItem.RemoveDiscount();

            Assert.AreEqual(originalPrice, orderItem.UnitPrice, 0.001);
        }

        [TestMethod]
        public void TC003_ApplyMultipleDiscounts_OnlyLastDiscountCounts()
        {
            double originalPrice = orderItem.UnitPrice;
            orderItem.ApplyDiscount(20m); // Apply 20% discount
            orderItem.RemoveDiscount(); // Reset to original price
            orderItem.ApplyDiscount(10m); // Only the last discount should count

            Assert.AreEqual(originalPrice * 0.9, orderItem.UnitPrice, 0.001);
        }

        [TestMethod]
        public void TC004_ApplyNegativeDiscount_NoChange()
        {
            double originalPrice = orderItem.UnitPrice;

            try
            {
                orderItem.ApplyDiscount(-10m); // Attempt to apply negative discount
            }
            catch (ArgumentOutOfRangeException)
            {
                // Expected exception
            }

            Assert.AreEqual(originalPrice, orderItem.UnitPrice, 0.001);
        }

        [TestMethod]
        public void TC005_ApplyDiscount_GreaterThan100_NoChange()
        {
            double originalPrice = orderItem.UnitPrice;

            try
            {
                orderItem.ApplyDiscount(150m); // Attempt to apply discount greater than 100%
            }
            catch (ArgumentOutOfRangeException)
            {
                // Expected exception
            }

            Assert.AreEqual(originalPrice, orderItem.UnitPrice, 0.001);
        }

        [TestMethod]
        public void TC006_OriginalPriceRetained_AfterDiscountRemoved()
        {
            double originalPrice = orderItem.UnitPrice;

            orderItem.ApplyDiscount(20m);
            orderItem.RemoveDiscount();

            Assert.AreEqual(originalPrice, orderItem.UnitPrice, 0.001);
        }

        [TestMethod]
        public void TC007_PriceAfterDiscountThenRemoval()
        {
            double originalPrice = orderItem.UnitPrice;
            orderItem.ApplyDiscount(20m);

            orderItem.RemoveDiscount();

            Assert.AreEqual(originalPrice, orderItem.UnitPrice, 0.001);
        }

        [TestMethod]
        public void TC008_DiscountToMissingPriceBook_ErrorOrNoChange()
        {
            OrderItem missingPriceItem = new OrderItem("1234567891", "Missing Price Book", 0, 1);

            try
            {
                missingPriceItem.ApplyDiscount(20m); // Should handle gracefully
            }
            catch (ArgumentOutOfRangeException)
            {
                // Expected exception for trying to apply discount on a zero price book
            }

            Assert.AreEqual(0, missingPriceItem.UnitPrice, 0.001); // Price should remain unchanged
        }

        [TestMethod]
        public void TC009_CheckDiscountOnlyChangesPrice_OtherPropertiesRemainSame()
        {
            double originalPrice = orderItem.UnitPrice;

            orderItem.ApplyDiscount(20m);

            Assert.AreEqual(originalPrice * 0.8, orderItem.UnitPrice, 0.001);
            Assert.AreEqual("The Great Gatsby", orderItem.BookTitle);
            Assert.AreEqual(2, orderItem.Quantity);
        }
    }
}
