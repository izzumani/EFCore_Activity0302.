using System;
using System.IO;
using Inventory.Common.ConfigBuilder;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore_DBLibrary
{
    public class InventoryDbContext: DbContext
    {
        private static IConfigurationRoot? _configuration;

        public DbSet<Item> Items { get; set; }

        // Add a default constructor if scaffolding is needed
        public InventoryDbContext() { }
        //Add the complex constructor for allowing Dependency Injection
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog= InventoryManagerDb;User ID=sa;Password=friend");
                /*
                var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", optional: true,reloadOnChange: true);
                 _configuration = builder.Build();
                */
                _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
                string cnstr = _configuration.GetConnectionString("InventoryManager");
                
                optionsBuilder.UseSqlServer(cnstr);
            }
        }

        

        

    }
}