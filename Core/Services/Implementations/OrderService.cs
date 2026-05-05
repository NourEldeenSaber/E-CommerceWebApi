using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.OrderModule;

namespace Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _basketRepository = basketRepository;
        }
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            // Map AddressDto To Address
            var address = _mapper.Map<Address>(orderRequest.ShippingAddress);
            
            // GetOrderItems
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach(var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GetRepository<Product,int>()
                                  .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(product, item));
            }

            // DeliveryMethod 
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderRequest.DelivaryMethodId) 
                                        ?? throw new DeliveryMethodsNotFoundException(orderRequest.DelivaryMethodId);

            // Calculate SubTotal 
            var subTotal = orderItems.Sum(o => o.Price * o.Quantity);

            // Create Order
            var orderToCreate = new Order(userEmail, address, orderItems, deliveryMethod, subTotal);
            await _unitOfWork.GetRepository<Order,Guid>().AddAsync(orderToCreate);
            await _unitOfWork.SaveChangesAsync();

            var orderResult =  _mapper.Map<OrderResult>(orderToCreate);
            return orderResult;
        }

        #region Helper Methods
        private OrderItem CreateOrderItem(Product product, BasketItem item)
        {
            return new OrderItem(
                new ProductInOrderItem(product.Id, product.Name, product.PictureUrl),
                product.Price,
                item.Quantity
            );
        } 
        #endregion

        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
