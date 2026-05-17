namespace Shared.Dtos.OrderModule
{
    public record OrderRequest
    {
        public string BasketId { get; init; } = string.Empty;
        public int DeliveryMethodId { get; init; }
        public AddressDto ShipToAddress { get; init; }
    }
}
