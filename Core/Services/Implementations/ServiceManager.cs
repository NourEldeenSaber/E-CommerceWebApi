using AutoMapper;
using Domain.Contracts;
using Sevices.Abstraction.Contracts;

namespace Services.Implementations
{
    public class ServiceManager(IMapper _mapper, IUnitOfWork _unitOfWork , IBasketRepository _basketRepository) : IServiceManager
    {

        
        private readonly Lazy<IProductService> _productService =
           new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));

        private readonly Lazy<IBasketService> _basketService =
            new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));


        public IProductService ProductService => _productService.Value; 
        public IBasketService BasketService => _basketService.Value;
    }
}
