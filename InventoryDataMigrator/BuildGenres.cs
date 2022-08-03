using EFCore_DBLibrary;
using Inventory.Common.LoggerBuilder;
using InventoryModels.Models;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDataMigrator
{
    public class BuildGenres
    {
        private readonly InventoryDbContext _context;
        private const string SEED_USER_ID = "SYSTEM";
        private static Logger _logger;
        private static DateTime genreCreateDate = new DateTime(2021, 01, 01);

        public BuildGenres(InventoryDbContext context)
        {
            _context = context;
            _logger = LoggerBuilderSingleton.InventoryLog;
            _logger.Debug($"Build Genres Migration");
        }
        public void ExecuteSeed()
        {
            _logger.Debug($"The number of Genres records {_context.Genres.Count()}");
            if (_context.Genres.Count() == 0) 
            {
                _logger.Debug($"Initiate Genre Migration");
                _context.Genres.AddRange
                    (
                     new Genre() { Id = 1, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Fantasy" },
                            new Genre() { Id = 2, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Sci/Fi" },
                            new Genre() { Id = 3, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Horror" },
                            new Genre() { Id = 4, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Comedy" },
                            new Genre() { Id = 5, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Drama" }
                    );

                _context.SaveChanges();
                _logger.Debug($"Completed Genre Migration");
            }

        }
    }
}
