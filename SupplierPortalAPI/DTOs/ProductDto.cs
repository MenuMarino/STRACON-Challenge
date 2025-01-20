namespace SupplierPortalAPI.DTOs
{
    public class ProductDto
    {
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public required string ProductUrl { get; set; }
    }
}