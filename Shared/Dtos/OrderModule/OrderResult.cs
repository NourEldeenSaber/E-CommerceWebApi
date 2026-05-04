namespace Shared.Dtos.OrderModule
{
    public record OrderResult
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public string ShippingAddress { get; init; } = string.Empty;
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
        public string PaymentStatus { get; init; } = string.Empty;
        public string DeliveryMethod { get; init; } = string.Empty;
        public int? DeliveryMethodId { get; init; }
        public decimal SubTotal { get; init; } // OrderItems01.Q * OrderItems01.Price + OrderItems02.Q * OrderItems02.Price
        public decimal Total { get; init; } // SubTotal + DeliveryMethod.Price
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; init; } = string.Empty;

    }
}
