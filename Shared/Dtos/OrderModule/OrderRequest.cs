namespace Shared.Dtos.OrderModule
{
    public record OrderRequest
    {
        public string BasketId { get; init; } = string.Empty;
        public int DelivaryMethodId { get; init; }
        public AddressDto ShippingAddress { get; init; }
    }
}
