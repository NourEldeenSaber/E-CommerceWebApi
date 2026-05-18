using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Specifications;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.BasketModule;
using Stripe;

using Product = Domain.Entities.ProductModule.Product;
using Order = Domain.Entities.OrderModule.Order;
using Domain.Entities.OrderModule.Enums;

namespace Services.Implementations
{
    public class PaymentService(IConfiguration _configuration,IUnitOfWork _unitOfWork,
        IBasketRepository _basketRepository, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            //  Configure Stripe Secret API Key
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSetting")["SecretKey"];

            //  Retrieve Basket from Repository
            //  Try to get basket using basketId.
            var basket = await GetBasketAsync(basketId);

            //  Refresh Product Prices from Database
            // Validate Delivery Method
            // Assign shipping price to basket
            await validateBasketAsync(basket);

            var amount =  CalculateTotalAsync(basket);

            await CreationOrUpdatePaymentIntentAsync(basket, amount);


            // Save Updated Basket
            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            // Map Basket Entity to BasketDto
            return _mapper.Map<BasketDto>(basket);
        }
        public async Task UpdatePaymentStatusAsync(string json, string signtureHeader)
        {
            string endpointSecret =_configuration.GetSection("StripeSetting")["EndPointSecret"]!;
            
            var stripeEvent = EventUtility.ParseEvent(json,throwOnApiVersionMismatch: false);

            stripeEvent = EventUtility.ConstructEvent(json, signtureHeader, endpointSecret, throwOnApiVersionMismatch: false);
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            // Handle the event
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                // Change order payment status => paymentRecieved
                await updatePaymentStatusRecievedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                // Change order payment status => paymentFailed
                await updatePaymentStatusFailedAsync(paymentIntent.Id);

            }
            else
            {
                // Unexpected event type
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
        }
        #region Stripe Helper Methods
        private async Task updatePaymentStatusFailedAsync(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            if(order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task updatePaymentStatusRecievedAsync(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentRecieved;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        #endregion
        #region Helper Methods
        private async Task CreationOrUpdatePaymentIntentAsync(CustomerBasket basket, long amount)
        {
            // Create Stripe PaymentIntent Service
            var stripeService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create New PaymentIntent
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // Update Existing PaymentIntent
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }
        }
        private async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            return await _basketRepository.GetBasketAsync(basketId)
                 ?? throw new BasketNotFoundException(basketId);
        }
        private async Task validateBasketAsync(CustomerBasket basket)
        {
            //  Refresh Product Prices from Database
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product!.Price;
            }

            // Validate Delivery Method
            if (!basket.DeliveryMethodId.HasValue)
                throw new Exception("No Delivery Method Selected");

            // Retrieve Delivery Method from Database
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodsNotFoundException(basket.DeliveryMethodId.Value);

            // Assign shipping price to basket
            basket.ShippingPrice = deliveryMethod.Price;
        }
        private long CalculateTotalAsync(CustomerBasket basket)
        {
            // Calculate Total Payment Amount
            // SubTotal = Sum(item.Quantity * item.Price)
            // Total = SubTotal + ShippingPrice
            var amount = (long)(basket.Items.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
            return amount;
        }
        #endregion
        
    }
}
