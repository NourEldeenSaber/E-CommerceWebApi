namespace Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } = string.Empty;
        public ICollection<BasketItem> Items { get; set; } = [];
        public string? PaymentIntentId { get; set; } = string.Empty;
        public string? ClientSecret { get; set; } = string.Empty;
        public decimal? ShippingPrice { get; set; }
        public int? DeliveryMethodId { get; set; }

    }
}
