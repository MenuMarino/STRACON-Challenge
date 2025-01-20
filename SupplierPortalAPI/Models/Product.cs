namespace SupplierPortalAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int RequestId { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public required string ProductUrl { get; set; }
    }
}
