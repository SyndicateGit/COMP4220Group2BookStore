using System.ComponentModel;

namespace BookStoreLIB
{
    public class OrderItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string BookID { get; set; }
        public string BookTitle { get; set; }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                Notify(nameof(Quantity));
                UpdateSubTotal();
            }
        }

        private double unitPrice;
        public double UnitPrice
        {
            get => unitPrice;
            set
            {
                unitPrice = value;
                Notify(nameof(UnitPrice));
                UpdateSubTotal();
            }
        }

        private double subTotal;
        public double SubTotal
        {
            get => subTotal;
            private set
            {
                subTotal = value;
                Notify(nameof(SubTotal));
            }
        }

        private DiscountManager _discountManager;

        public OrderItem(string isbn, string title, double unitPrice, int quantity)
        {
            BookID = isbn;
            BookTitle = title;
            UnitPrice = unitPrice;
            Quantity = quantity;
            _discountManager = new DiscountManager((decimal)unitPrice);
            UpdateSubTotal();
        }

        public void ApplyDiscount(decimal percentage)
        {
            _discountManager.ApplyDiscount(percentage);
            UnitPrice = (double)_discountManager.GetCurrentPrice();
            UpdateSubTotal(); // Ensure subtotal is updated
        }

        private void UpdateSubTotal()
        {
            SubTotal = UnitPrice * Quantity; // Update subtotal
        }

        public void RemoveDiscount()
        {
            UnitPrice = (double)_discountManager.RemoveDiscount();
            UpdateSubTotal(); // Ensure subtotal is updated
        }

        public override string ToString()
        {
            return $"<OrderItem ISBN='{BookID}' Quantity='{Quantity}' />";
        }
    }
}
