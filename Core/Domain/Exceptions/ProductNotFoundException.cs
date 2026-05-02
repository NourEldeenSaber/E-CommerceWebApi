namespace Domain.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id) 
            : base($"Product With {id} Not Found")
        {
            
        }
    }
}
