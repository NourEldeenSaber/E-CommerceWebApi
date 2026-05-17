using Domain.Entities.OrderModule;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentIdSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrderWithPaymentIntentIdSpecifications(string paymentIntentId) : base( o => o.PaymentIntentId == paymentIntentId )
        {
        }
    }
}
