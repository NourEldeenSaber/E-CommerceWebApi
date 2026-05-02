namespace Shared.Dtos.BasketModule
{
    public record BasketDto(
        string Id ,
        ICollection<BasketItemDto> BasketItems );
    
    
}
