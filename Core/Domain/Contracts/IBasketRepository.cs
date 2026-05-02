using Domain.Entities.BasketModule;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        // Get Basket By Id
        Task<CustomerBasket?> GetBasketAsync(string id);

        // Create Or Update Basket 
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket , TimeSpan? timeToLeave = null);

        // Delete Basket
        Task<bool> DeleteBasketAsync(string id);
    }
}
