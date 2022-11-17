using CleanArchitecture.WebApi1.Domain.Common;
using CleanArchitecture.WebApi1.Domain.Entities;

namespace CleanArchitecture.WebApi1.Domain.Events
{
    public class ProductCreatedEvent:BaseEvent
    {
        public Product Product { get; }
        public ProductCreatedEvent(Product product)
        {
            Product = product;
        }
    }
}
