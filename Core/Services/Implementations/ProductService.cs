using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Specifications;
using Sevices.Abstraction.Contracts;
using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;
using Shared.Results;

namespace Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters parameters)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            var specifications = new ProductWithBrandAndTypeSpecifications(parameters);
            var products = await productRepo.GetAllAsync(specifications);

            // mapping IEnumerable<Product> => IEnumerable<ProductResultDto>
            var productsDto = _mapper.Map<IEnumerable<ProductResultDto>>(products);

            var pageSize = productsDto.Count();
            var countSpecifications = new ProductCountSpecification(parameters);
            var totalCount = await productRepo.CountAsync(countSpecifications);
            return new PaginatedResult<ProductResultDto>(parameters.PageIndex, pageSize, totalCount, productsDto);
        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();

            //mapping IEnumerable<ProductBrand> => IEnumerable<BrandResultDto>
            var brandsDto = _mapper.Map<IEnumerable<BrandResultDto>>(Brands);
            return brandsDto;
        }
        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType,int>().GetAllAsync();

            //mapping IEnumerable<ProductType> => IEnumerable<TypeResultDto>
            var typesDto = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return typesDto;
        }
        public async Task<Result<ProductResultDto>> GetProductByIdAsync(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.GetRepository<Product,int>().GetByIdAsync(specifications);

            return product is null ?  Error.NotFound("Product.NotFound",$"Product With {id} Is Not Found") 
                : _mapper.Map<ProductResultDto>(product);
        }
    }
}
