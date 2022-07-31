using System;
using System.IO;
using Inventory.Common.ConfigBuilder;
using Inventory.Common.LoggerBuilder;
using InventoryModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

namespace EFCore_DBLibrary
{
    public class InventoryDbContext: DbContext
    {
        private static IConfigurationRoot? _configuration;
        private static Logger _logger;

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Genre> Genres { get; set; }

        // Add a default constructor if scaffolding is needed
        public InventoryDbContext() 
        {
            _logger = LoggerBuilderSingleton.InventoryLog;
        }
        //Add the complex constructor for allowing Dependency Injection
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
            
            _logger = LoggerBuilderSingleton.InventoryLog;
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

        public override int SaveChanges()
        {
            var tracker = ChangeTracker;

            tracker.Entries().ToList().ForEach((entry) =>
            {
                _logger.Information($"{entry.Entity} has state{entry.State}");

                if (entry.Entity is FullAuditModel)
                {
                    var referenceEntity = entry.Entity as FullAuditModel;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            referenceEntity.CreatedDate = DateTime.Now;
                            break;
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            referenceEntity.LastModifiedUserId = "SystemDeleted";
                            referenceEntity.LastModifiedDate = DateTime.Now;
                            break;
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        
                        
                        default:
                            break;
                    }
                }
            });

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Item>()
                .HasMany(x => x.Players)
                .WithMany(x => x.Items)
                .UsingEntity<Dictionary<string, object>>(
                "ItemPlayers",
                ip => ip.HasOne<Player>()
                .WithMany()
                .HasForeignKey("PlayerId")
                .HasConstraintName("FK_ItemPlayer_Players_PlayerId")
                .OnDelete(DeleteBehavior.Cascade),
                ip => ip.HasOne<Item>()
                .WithMany()
                .HasForeignKey("ItemId")
                .HasConstraintName("FK_PlayerItem_Items_ItemId")
                 .OnDelete(DeleteBehavior.ClientCascade)
                );
        }


    }
}