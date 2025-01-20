using SupplierPortalAPI.Models;
using SupplierPortalAPI.DTOs;

namespace SupplierPortalAPI.Repositories
{
    public interface IPurchaseRequestRepository
    {
        Task<IEnumerable<PurchaseRequest>> GetPendingRequestsAsync();
        Task<IEnumerable<PurchaseRequest>> GetRequestsByCreatorAsync(string username);
        Task<IEnumerable<PurchaseRequest>> GetAllPendingRequestsAsync();
        Task<PurchaseRequest> GetRequestByIdAsync(int id);
        Task AddRequestAsync(PurchaseRequest request);
        Task SaveChangesAsync();
    }
}
