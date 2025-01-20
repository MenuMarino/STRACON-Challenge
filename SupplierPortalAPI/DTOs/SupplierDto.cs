namespace SupplierPortalAPI.DTOs
{
    public class SupplierDto
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Contact { get; set; }
        public bool HasPartnership { get; set; }
    }
}
