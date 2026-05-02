using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.BasketModule
{
    public record BasketItemDto(
        string Id,
        string ProductName,
        [Range(1,double.MaxValue)]
        decimal Price,
        string PictureUrl ,
        [Range(1,99)]
        int Quantity
    );
    
}