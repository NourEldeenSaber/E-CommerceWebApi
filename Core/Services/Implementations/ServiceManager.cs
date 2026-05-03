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
        IMapper _mapper, IUnitOfWork _unitOfWork ,
        IBasketRepository _basketRepository, UserManager<User> _user ,IOptions<JwtOptions> _options ) 
        : IServiceManager
    {

        
        private readonly Lazy<IProductService> _productService =
           new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));

        private readonly Lazy<IBasketService> _basketService =
            new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));

        private readonly Lazy<IAuthenticationService> _authenticationService =
            new Lazy<IAuthenticationService>(() => new AuthenticationService(_user, _options));


        public IProductService ProductService => _productService.Value; 
        public IBasketService BasketService => _basketService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;

    }
}
