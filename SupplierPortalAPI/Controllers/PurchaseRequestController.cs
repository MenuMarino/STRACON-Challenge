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
    public class PurchaseRequestController : ControllerBase
    {
        private readonly PurchaseRequestService _requestService;
        private readonly IMapper _mapper;

        public PurchaseRequestController(PurchaseRequestService requestService, IMapper mapper)
        {
            _requestService = requestService;
            _mapper = mapper;
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Aprobador")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var requests = await _requestService.GetPendingRequestsAsync();
            return Ok(requests);
        }

        [Authorize(Roles = "Aprobador")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            await _requestService.ApproveRequestAsync(id);
            return Ok(new { message = "Solicitud de compra aprobada" });
        }

        [Authorize(Roles = "Aprobador")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            await _requestService.RejectRequestAsync(id);
            return Ok(new { message = "Solicitud de compra rechazada" });
        }

        [HttpGet("myrequests")]
        [Authorize(Roles = "Colocador")]
        public async Task<IActionResult> GetMyRequests()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Usuario no autenticado." });
            }

            var requests = await _requestService.GetRequestsByUserAsync(username);

            if (!requests.Any())
            {
                return NotFound(new { message = "No se encontraron solicitudes pendientes para este usuario." });
            }

            return Ok(requests);
        }

        [HttpPost]
        [Authorize(Roles = "Colocador")]
        public async Task<IActionResult> CreateRequest([FromBody] PurchaseRequestDto requestDto)
        {
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized(new { message = "Usuario no autenticado." });

            var hasPartnership = await _requestService.ValidatePartnershipAsync(requestDto.SupplierId);
            if (!hasPartnership)
            {
                return BadRequest(new { message = "No tiene una asociación con este proveedor." });
            }

            var request = _mapper.Map<PurchaseRequest>(requestDto);
            request.CreatedBy = username;
            request.Status = "Pendiente";
            request.TotalPrice = requestDto.Products.Sum(p => p.Quantity * p.UnitPrice);

            await _requestService.AddRequestAsync(request);
            return Ok(new { message = "Solicitud creada exitosamente." });
        }

        [HttpPut("{requestId}")]
        [Authorize(Roles = "Colocador")]
        public async Task<IActionResult> UpdateRequest(int requestId, [FromBody] UpdatePurchaseRequestDto updateDto)
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { message = "Usuario no autenticado." });
                }

                var updatedRequest = await _requestService.UpdatePurchaseRequestAsync(requestId, updateDto, username);

                return Ok(new { message = "Solicitud actualizada exitosamente.", updatedRequest });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = $"Error al actualizar la solicitud." });
            }
        }
    }
}
