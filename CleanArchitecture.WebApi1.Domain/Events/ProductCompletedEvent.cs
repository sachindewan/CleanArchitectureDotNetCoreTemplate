using CleanArchitecture.WebApi1.Domain.Common;
using CleanArchitecture.WebApi1.Domain.Entities;

namespace CleanArchitecture.WebApi1.Domain.Events
{
    public class ProductCompletedEvent:BaseEvent
    {
        public Product Product { get; }
        public ProductCompletedEvent(Product product)
        {
            Product = product;
        }
    }
}
