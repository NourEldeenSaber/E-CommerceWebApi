using Shared.Dtos.BasketModule;

namespace Sevices.Abstraction.Contracts
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
    }
}
