using Online_Supermarket_Project.Models;

namespace Online_Supermarket_Project.ModelView
{
    public class CartItem
    {
        public Product? Product { get; set; }
        public int Amount { get; set; }
        public double TotalDiscountAmount => Product?.Discount.GetValueOrDefault() > 0 ? Amount * Product.Discount.GetValueOrDefault() : 0;
        public double TotalPriceAmount => Amount * Product.Price.Value;

        public double TotalAmount => TotalPriceAmount - TotalDiscountAmount;

    }
}
