using SupplierPortalAPI.Repositories;
using SupplierPortalAPI.Models;
using SupplierPortalAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using SupplierPortalAPI.Data;

namespace SupplierPortalAPI.Services
{
    public class PurchaseRequestService
    {
        private readonly IPurchaseRequestRepository _requestRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly SupplierPortalContext _context;

        public PurchaseRequestService(SupplierPortalContext context, IPurchaseRequestRepository requestRepository, ISupplierRepository supplierRepository)
        {
            _requestRepository = requestRepository;
            _supplierRepository = supplierRepository;
            _context = context;
        }

        public async Task<IEnumerable<PurchaseRequest>> GetPendingRequestsAsync()
        {
            return await _requestRepository.GetPendingRequestsAsync();
        }

        public async Task<IEnumerable<PurchaseRequest>> GetRequestsByUserAsync(string username)
        {
            return await _requestRepository.GetRequestsByCreatorAsync(username);
        }

        public async Task<bool> ValidatePartnershipAsync(int supplierId)
        {
            var supplier = await _supplierRepository.GetByIdAsync(supplierId) ?? throw new KeyNotFoundException($"No se encontró el proveedor con ID {supplierId}.");
            return supplier.HasPartnership;
        }

        public async Task AddRequestAsync(PurchaseRequest request)
        {
            await _requestRepository.AddRequestAsync(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task ApproveRequestAsync(int id)
        {
            var request = await _requestRepository.GetRequestByIdAsync(id) ?? throw new KeyNotFoundException($"Solicitud de compra con el ID {id} no existe.");
            request.Status = "Aprobada";
            await _requestRepository.SaveChangesAsync();
        }

        public async Task RejectRequestAsync(int id)
        {
            var request = await _requestRepository.GetRequestByIdAsync(id) ?? throw new KeyNotFoundException($"Solicitud de compra con el ID {id} no existe.");
            request.Status = "Rechazada";
            await _requestRepository.SaveChangesAsync();
        }

        public async Task<PurchaseRequest> UpdatePurchaseRequestAsync(int requestId, UpdatePurchaseRequestDto updateDto, string username)
        {
            var request = await _context.PurchaseRequests
                .Include(pr => pr.Products)
                .FirstOrDefaultAsync(pr => pr.RequestId == requestId) 
                ?? throw new KeyNotFoundException("No se encontró la solicitud especificada.");

            if (request.CreatedBy != username)
            {
                throw new UnauthorizedAccessException("No tiene permiso para actualizar esta solicitud.");
            }

            // Update supplier if it has changed
            if (request.SupplierId != updateDto.SupplierId)
            {
                var supplier = await _context.Suppliers.FindAsync(updateDto.SupplierId);
                if (supplier == null)
                {
                    throw new ArgumentException("El proveedor especificado no existe.");
                }
                if (!supplier.HasPartnership)
                {
                    throw new ArgumentException("No tiene una asociación con este proveedor.");
                }
                request.SupplierId = updateDto.SupplierId;
            }

            var existingProducts = request.Products.ToDictionary(p => p.ProductId);
            var newProducts = new List<Product>();
            
            foreach (var productDto in updateDto.Products)
            {
                if (productDto.ProductId.HasValue)
                {
                    if (existingProducts.TryGetValue(productDto.ProductId.Value, out var existingProduct))
                    {
                        existingProduct.Name = productDto.Name;
                        existingProduct.Quantity = productDto.Quantity;
                        existingProduct.UnitPrice = productDto.UnitPrice;
                        existingProduct.ProductUrl = productDto.ProductUrl;
                    }
                }
                else
                {
                    var newProduct = new Product
                    {
                        RequestId = request.RequestId,
                        Name = productDto.Name,
                        Quantity = productDto.Quantity,
                        UnitPrice = productDto.UnitPrice,
                        ProductUrl = productDto.ProductUrl
                    };
                    newProducts.Add(newProduct);
                }
            }

            if (newProducts.Count != 0)
            {
                _context.Products.AddRange(newProducts);
            }

            var updatedProductIds = updateDto.Products
                .Where(p => p.ProductId.HasValue)
                .Select(p => p.ProductId!.Value)
                .ToList();

            var productsToRemove = await _context.Products
                .Where(p => p.RequestId == request.RequestId && !updatedProductIds.Contains(p.ProductId))
                .ToListAsync();

            foreach (var product in productsToRemove)
            {
                _context.Products.Remove(product);
            }

            request.TotalPrice = updateDto.Products.Sum(p => p.Quantity * p.UnitPrice);
            _context.PurchaseRequests.Update(request);
            await _context.SaveChangesAsync();

            return request;
        }

    }
}
