using CleanArchitecture.WebApi1.Domain.Common;
using CleanArchitecture.WebApi1.Domain.Entities;

namespace CleanArchitecture.WebApi1.Domain.Events
{
    public class ProductDeletedEvent:BaseEvent
    {
        public Product Product { get; }
        public ProductDeletedEvent(Product product)
        {
            Product = product;
        }
    }
}
