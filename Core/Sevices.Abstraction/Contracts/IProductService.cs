using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;
using Shared.Results;

namespace Sevices.Abstraction.Contracts
{
    public interface IProductService
    {
        //GetAllProducts
        Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters parameters);

        //GetAllBrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        //GetAllTypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        //GetProductById
        Task<Result<ProductResultDto>> GetProductByIdAsync(int id);
    }
}
