using AutoMapper;
using SupplierPortalAPI.Models;
using SupplierPortalAPI.DTOs;

namespace SupplierPortalAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SupplierDto, Supplier>()
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore());

            CreateMap<PurchaseRequestDto, PurchaseRequest>()
                .ForMember(dest => dest.RequestId, opt => opt.Ignore());

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        }
    }
}
