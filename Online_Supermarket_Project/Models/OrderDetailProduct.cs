namespace Online_Supermarket_Project.Models
{
    public class OrderDetailProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int OrderDetailId { get; internal set; }
        public int? Price { get; internal set; }
        public string? Image { get; internal set; }
        public int? Quantity { get; internal set; }
        public int? Total { get; internal set; }
    }
}
