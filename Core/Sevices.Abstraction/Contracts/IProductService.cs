using Shared.Dtos;

namespace Sevices.Abstraction.Contracts
{
    public interface IProductService
    {
        //GetAllProducts
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync();

        //GetAllBrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        //GetAllTypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        //GetProductById
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
