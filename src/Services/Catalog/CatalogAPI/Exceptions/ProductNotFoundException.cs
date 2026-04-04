namespace CatalogAPI.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base("Product not found!")
        {
        }

        public ProductNotFoundException(Guid id) : base($"Product id:{id} not found !")
        {
        }

        public ProductNotFoundException(string? message) : base(message)
        {
        }
    }
}
