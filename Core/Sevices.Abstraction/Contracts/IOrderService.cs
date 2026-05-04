namespace Sevices.Abstraction.Contracts
{
    public interface IOrderService
    {
        // GetById =>> Take Id [Guid] ==> return OrderResultDto
        // GetAllByEmial =>> Take Email [string] ==> return IEnumerable<OrderResultDto>
        // CreateOrder =>> Take OrderRequest, Email [string] ==> return OrderResultDto
        // GetDelivaryMethods =>  return IEnumerable<DeliveryMethodsResultDto>
    }
}
