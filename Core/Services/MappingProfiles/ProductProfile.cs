using AutoMapper;
using Domain.Entities.ProductModule;
using Shared.Dtos.ProductModule;
using static System.Net.WebRequestMethods;

namespace Services.MappingProfiles
{
    internal class ProductProfile :Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductType, TypeResultDto>();
            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<Product, ProductResultDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(des => des.PictureUrl,opt => opt.MapFrom<PictureUrlResolver>());
        }
    }
}
