using Shared.Dtos.OrderModule;

namespace Sevices.Abstraction.Contracts
{
    public interface IOrderService
    {
        // GetById =>> Take Id [Guid] ==> return OrderResultDto
        Task<OrderResult> GetOrderByIdAsync(Guid id);
        // GetAllByEmail =>> Take Email [string] ==> return IEnumerable<OrderResultDto>
        Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail);
        // CreateOrder =>> Take OrderRequest, Email [string] ==> return OrderResultDto
        Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest , string userEmail);
        // GetDelivaryMethods =>  return IEnumerable<DeliveryMethodsResultDto>
        Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();
    }
}
