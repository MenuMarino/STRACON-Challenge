using Microsoft.EntityFrameworkCore;
using SupplierPortalAPI.Data;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public class PurchaseRequestRepository : IPurchaseRequestRepository
    {
        private readonly SupplierPortalContext _context;

        public PurchaseRequestRepository(SupplierPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseRequest>> GetPendingRequestsAsync()
        {
            return await _context.PurchaseRequests
                .Include(pr => pr.Supplier)
                .Include(pr => pr.Products)
                .Where(pr => pr.Status == "Pendiente")
                .ToListAsync();
        }

        public async Task<PurchaseRequest> GetRequestByIdAsync(int id)
        {
            var request = await _context.PurchaseRequests
                .Include(pr => pr.Supplier)
                .Include(pr => pr.Products)
                .FirstOrDefaultAsync(pr => pr.RequestId == id);

            if (request == null)
            {
                throw new KeyNotFoundException($"Solicitud de compra con el {id} no fue encontrada.");
            }

            return request;
        }

        public async Task AddRequestAsync(PurchaseRequest request)
        {
            await _context.PurchaseRequests.AddAsync(request);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PurchaseRequest>> GetRequestsByCreatorAsync(string username)
        {
            return await _context.PurchaseRequests
                .Include(pr => pr.Supplier)
                .Include(pr => pr.Products)
                .Where(pr => pr.CreatedBy == username)
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseRequest>> GetAllPendingRequestsAsync()
        {
            return await _context.PurchaseRequests
                .Include(pr => pr.Supplier)
                .Where(pr => pr.Status == "Pendiente")
                .ToListAsync();
        }
    }
}
