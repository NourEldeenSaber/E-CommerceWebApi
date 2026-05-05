namespace Domain.Exceptions
{
    public class DeliveryMethodsNotFoundException : NotFoundException
    {
        public DeliveryMethodsNotFoundException(int id) : base($"DeliveryMethod with id : {id} Not Found")
        {
            
        }
    }
}
