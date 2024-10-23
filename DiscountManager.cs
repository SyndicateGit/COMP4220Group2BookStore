using System;

public class DiscountManager
{
    private decimal originalPrice;
    private decimal currentPrice;

    public DiscountManager(decimal price)
    {
        originalPrice = price;
        currentPrice = price;
    }

    public void ApplyDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");

        currentPrice = originalPrice - (originalPrice * (percentage / 100));
    }

    public decimal GetCurrentPrice()
    {
        return currentPrice;
    }

    public decimal RemoveDiscount()
    {
        currentPrice = originalPrice;
        return currentPrice;
    }
}
