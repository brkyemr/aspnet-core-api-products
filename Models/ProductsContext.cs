using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Models
{
    public class ProductsContext:DbContext
    {
        
        public ProductsContext(DbContextOptions<ProductsContext> options): base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

    }
}