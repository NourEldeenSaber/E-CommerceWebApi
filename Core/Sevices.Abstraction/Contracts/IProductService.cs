using Shared;
using Shared.Dtos;
using Shared.Enums;

namespace Sevices.Abstraction.Contracts
{
    public interface IProductService
    {
        //GetAllProducts
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters parameters);

        //GetAllBrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        //GetAllTypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        //GetProductById
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
