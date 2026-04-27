using Microsoft.AspNetCore.Mvc;
using Sevices.Abstraction.Contracts;
using Shared.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ProductsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        //GetAllProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProductsAsync(int? typeId , int? brandId)
            => Ok(await _serviceManager.ProductService.GetAllProductsAsync(typeId , brandId));

        //GetAllBrands
        [HttpGet("Brands")] //BaseUrl/Products/Brands
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrandsAsync()
            => Ok(await _serviceManager.ProductService.GetAllBrandsAsync());

        //GetAllTypes 
        [HttpGet("Types")]//BaseUrl/Products/Types
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypesAsync()
            => Ok(await _serviceManager.ProductService.GetAllTypesAsync());

        //GetProductById
        [HttpGet("{id:int}")]//BaseUrl/Products/10
        public async Task<ActionResult<ProductResultDto>> GetProductByIdAsync(int id)
            => Ok( await _serviceManager.ProductService.GetProductByIdAsync(id));
        
    }
}
