namespace SupplierPortalAPI.DTOs
{
    public class UpdatePurchaseRequestDto
    {
        public int SupplierId { get; set; }
        public List<UpdateProductDto> Products { get; set; } = new List<UpdateProductDto>();
    }

    public class UpdateProductDto
    {
        public int? ProductId { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public required string ProductUrl { get; set; }
    }
}
