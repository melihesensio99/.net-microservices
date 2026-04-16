using BuildingBlocks.Exceptions;

namespace CatalogAPI.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException() : base("Product", "All")
        {
        }

        public ProductNotFoundException(Guid id) : base("Product", id)
        {
        }

        public ProductNotFoundException(string name, object key) : base(name, key)
        {
        }
    }
}
