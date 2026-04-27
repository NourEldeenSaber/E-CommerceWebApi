using Shared.Dtos;

namespace Sevices.Abstraction.Contracts
{
    public interface IProductService
    {
        //GetAllProducts
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? TypeId , int? BrandId);

        //GetAllBrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        //GetAllTypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        //GetProductById
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
