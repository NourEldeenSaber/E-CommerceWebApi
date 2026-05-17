using AutoMapper;
using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sevices.Abstraction.Contracts;
using Shared.Common;

namespace Services.Implementations
{
    public class ServiceManager(
        IMapper _mapper, IUnitOfWork _unitOfWork ,ICacheRepository _cacheRepository,
        IBasketRepository _basketRepository, UserManager<User> _user ,IOptions<JwtOptions> _options , IConfiguration _configuration) 
        : IServiceManager
    {

        
        private readonly Lazy<IProductService> _productService =
           new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));

        private readonly Lazy<IBasketService> _basketService =
            new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));

        private readonly Lazy<IAuthenticationService> _authenticationService =
            new Lazy<IAuthenticationService>(() => new AuthenticationService(_user, _options, _mapper));

        private readonly Lazy<IOrderService> _orderService =
            new Lazy<IOrderService>(() => new OrderService(_unitOfWork,_mapper,_basketRepository));

        private readonly Lazy<IPaymentService> _paymentService =
            new Lazy<IPaymentService>(() => new PaymentService(_configuration,_unitOfWork,_basketRepository,_mapper));

        private readonly Lazy<ICacheService> _cacheService=
            new Lazy<ICacheService>(() => new CacheService(_cacheRepository));


        public IProductService ProductService => _productService.Value; 
        public IBasketService BasketService => _basketService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IOrderService OrderService => _orderService.Value;
        public IPaymentService PaymentService => _paymentService.Value;

        public ICacheService CacheService => _cacheService.Value;
    }
}
