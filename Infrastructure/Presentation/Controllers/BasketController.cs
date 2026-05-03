using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.BasketModule;

namespace Presentation.Controllers
{
    [Authorize]
    public class BasketController : ApiController
    {
        private readonly IServiceManager _serviceManager;

        public BasketController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasketAsync(string id)
            => Ok(await _serviceManager.BasketService.GetBasketAsync(id));

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket)
            => Ok(await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasketAsync(string id)
        {
            await _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
