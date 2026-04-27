

using Domain.Entities.ProductModule;
using Shared;

namespace Services.Specifications
{
    internal class ProductCountSpecification : BaseSpecifications<Product,int>
    {
        public ProductCountSpecification(ProductSpecificationParameters parameters) :
            base(p => (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId) &&
                  (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId) &&
                   (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower())))
        {
            
        }
    }
}
