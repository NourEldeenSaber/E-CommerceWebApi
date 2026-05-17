using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Specifications;
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
            var address = _mapper.Map<Address>(orderRequest.ShipToAddress);
            
            // GetOrderItems
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product,int>()
                                  .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(product, item));
            }

            var orderRepo = _unitOfWork.GetRepository<Order,Guid>();


            // DeliveryMethod 
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderRequest.DeliveryMethodId) 
                                        ?? throw new DeliveryMethodsNotFoundException(orderRequest.DeliveryMethodId);


            var orderExist = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId));
            if(orderExist != null)
            {
                orderRepo.Delete(orderExist);
            }

            // Calculate SubTotal 
            var subTotal = orderItems.Sum(o => o.Price * o.Quantity);

            // Create Order
            var orderToCreate = new Order(userEmail, address, orderItems, deliveryMethod, subTotal, basket.PaymentIntentId!);
            await orderRepo.AddAsync(orderToCreate);
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

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods =  await _unitOfWork.GetRepository<DeliveryMethod,int>()
                .GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResult>>(deliveryMethods);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithIncludesSpecifications(id))
                ?? throw new OrderNotFoundException(id);
            
            return _mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>()
                .GetAllAsync(new OrderWithIncludesSpecifications(userEmail));
            return _mapper.Map<IEnumerable<OrderResult>>(orders);
        }
    }
}
