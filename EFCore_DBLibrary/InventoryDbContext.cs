using Microsoft.EntityFrameworkCore;

namespace EFCore_DBLibrary
{
    public class InventoryDbContext: DbContext
    {
        // Add a default constructor if scaffolding is needed
        public InventoryDbContext() { }
        //Add the complex constructor for allowing Dependency Injection
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
            
        }


    }
}