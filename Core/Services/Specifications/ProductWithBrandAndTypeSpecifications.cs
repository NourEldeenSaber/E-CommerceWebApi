using Domain.Entities.ProductModule;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications: BaseSpecifications<Product,int>
    {
        //Get All Products And Brand And Type
        public ProductWithBrandAndTypeSpecifications() :base(null)
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
        }
        public ProductWithBrandAndTypeSpecifications(int id) : base(p=>p.Id == id)
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
        }

    }
}
