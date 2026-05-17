namespace Shared.Dtos.BasketModule
{
    public record BasketDto
    {
        public string Id {  get; set; } = string.Empty;
        public ICollection<BasketItemDto> Items { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal? ShippingPrice {  get; set; }
        public int? DeliveryMethodId { get; set; }
    }
    
}
