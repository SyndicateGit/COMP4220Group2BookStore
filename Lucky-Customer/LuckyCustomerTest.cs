using Xunit;

namespace LuckyCustomer.Tests
{
    public class LuckyCustomerDiscountTests
    {
        [Theory]
        [InlineData(100, 50, 90)]  // Customer gets 10% discount
        [InlineData(100, 51, 100)]  // No discount
        [InlineData(200, 45, 180)]  // Customer gets 10% discount
        [InlineData(200, 75, 200)]  // No discount
        [InlineData(50, 40, 45)]    // Customer gets 10% discount
        [InlineData(50, 60, 50)]    // No discount
        [InlineData(75, 50, 67.5)]  // Customer gets 10% discount
        [InlineData(75, 51, 75)]    // No discount
        [InlineData(0, 20, 0)]      // No discount, price is zero
        [InlineData(0, 60, 0)]      // No discount, price is zero
        [InlineData(1000, 45, 900)] // Customer gets 10% discount
        [InlineData(1000, 55, 1000)] // No discount
        public void ApplyLuckyCustomerDiscount_ShouldReturnCorrectPrice(decimal price, int randomNumber, decimal expectedPrice)
        {
            // Arrange
            var luckyCustomerDiscount = new LuckyCustomerDiscount();

            // Act
            var result = luckyCustomerDiscount.ApplyDiscount(price, randomNumber);

            // Assert
            Assert.Equal(expectedPrice, result);
        }
    }

    public class LuckyCustomerDiscount
    {
        public decimal ApplyDiscount(decimal price, int randomNumber)
        {
            if (randomNumber <= 50)  // If the random number is 50 or less, apply a 10% discount
            {
                return price * 0.90m; // Apply a 10% discount
            }
            return price;  // No discount if random number is above 50
        }
    }
}
