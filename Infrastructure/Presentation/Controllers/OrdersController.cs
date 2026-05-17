using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.OrderModule;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ApiController
    {
        //CreateOrder
        [HttpPost]
        public async Task<ActionResult<OrderResult>> CreateOrderAsync([FromBody]OrderRequest orderRequest)
        {   
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManager.OrderService.CreateOrderAsync(orderRequest, userEmail!);
            return Ok(order);
        }
        
        //GetOrderById
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResult>> GetOrderByIdAsunc(Guid id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        
        //GetAllOrdersByEmail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetAllOrdersByEmailAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManager.OrderService.GetOrdersByEmailAsync(userEmail!);
            return Ok(orders);
        }

        //GetDeliveryMethods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethodsAsync()
            => Ok(await _serviceManager.OrderService.GetDeliveryMethodsAsync());
    }
}
