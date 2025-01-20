using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SupplierPortalAPI.Services;
using SupplierPortalAPI.Models;
using SupplierPortalAPI.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace SupplierPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;
        private readonly IMapper _mapper;

        public SupplierController(SupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpPost]
        [Authorize(Roles = "Colocador")]
        public async Task<IActionResult> CreateSupplier(SupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            await _supplierService.AddSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSuppliers), new { id = supplier.SupplierId }, supplier);
        }
    }
}
