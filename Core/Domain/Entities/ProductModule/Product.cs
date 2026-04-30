namespace Domain.Entities.ProductModule
{
    public class Product :BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        #region Product (1) - ProductType (M)

        public ProductType ProductType { get; set; } = null!;
        public int TypeId { get; set; } //FK

        #endregion

        #region Product (1) - ProductBrand (M)

        public ProductBrand ProductBrand { get; set; } = null!;
        public int BrandId { get; set; } //FK

        #endregion
    }
}
