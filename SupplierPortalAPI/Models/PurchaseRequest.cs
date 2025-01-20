namespace SupplierPortalAPI.Models
{
    public class PurchaseRequest
    {
        public int RequestId { get; set; }
        public int SupplierId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pendiente";
        public required string CreatedBy { get; set; }

        public required Supplier Supplier { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
