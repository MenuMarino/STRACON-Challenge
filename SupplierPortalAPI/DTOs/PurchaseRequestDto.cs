namespace SupplierPortalAPI.DTOs
{
    public class PurchaseRequestDto
    {
        public int SupplierId { get; set; }
        public List<ProductDto> Products { get; set; } = [];
    }
}