using System.ComponentModel.DataAnnotations;
namespace Shared.Dtos.BasketModule
{
    public record BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName {  get; set; } =string.Empty;
        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }
        public string PictureUrl { get; set; } =string.Empty;
        [Range(1, 99)]
        public int Quantity { get; set; }
}
    
}