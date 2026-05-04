using AutoMapper;
using Domain.Entities.OrderModule;
using Shared.Dtos.OrderModule;

namespace Services.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMethodResult>();
            CreateMap<OrderItem , OrderItemDto>()
                .ForMember(des => des.ProductId , opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(des => des.ProductName , opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(des => des.PictureUrl , opt => opt.MapFrom(src => src.Product.PictureUrl));
            CreateMap<Order, OrderResult>()
                .ForMember(des => des.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(des => des.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(des => des.Total, opt => opt.MapFrom(src => src.SubTotal + src.DeliveryMethod.Price));
        }
    }
}
