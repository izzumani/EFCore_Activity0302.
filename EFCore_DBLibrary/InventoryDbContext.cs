using System;
using System.IO;
using Inventory.Common.ConfigBuilder;
using Inventory.Common.LoggerBuilder;
using InventoryModels.DTOs;
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

        public DbSet<GetItemsForListingDto> ItemsForListing { get; set; }
        public DbSet<AllItemsPipeDelimitedStringDto> AllItemsOutput { get; set; }
        public DbSet<GetItemsTotalValueDto> GetItemsTotalValues { get; set; }

        public DbSet<FullItemDetailDTO> FullItemDetailDtos { get; set; }

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

            modelBuilder.Entity<GetItemsForListingDto>(x =>
            {
                x.HasNoKey();
                x.ToView("ItemsForListing");
            });

            modelBuilder.Entity<AllItemsPipeDelimitedStringDto>(x => {
                x.HasNoKey();
                x.ToView("AllItemsOutput");
            });

            modelBuilder.Entity<GetItemsTotalValueDto>(x =>
            {
                x.HasNoKey();
                x.ToView("GetItemsTotalValues");
            });

            modelBuilder.Entity<FullItemDetailDTO>(x =>
            {
                x.HasNoKey();
                x.ToView("FullItemDetailDtos");
            });
            /*
            var genreCreateDate = new DateTime(2021, 01, 01);
            modelBuilder.Entity<Genre>(x =>
            {
                x.HasData(
                            new Genre() { Id = 1, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Fantasy" },
                            new Genre() { Id = 2, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Sci/Fi" },
                            new Genre() { Id = 3, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Horror" },
                            new Genre() { Id = 4, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Comedy" },
                            new Genre() { Id = 5, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Drama" }
                        );
            });
            */

        }


    }
}