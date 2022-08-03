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
    public class BuildCategories
    {
        private readonly InventoryDbContext _context;
        private const string SEED_USER_ID = "SYSTEM";
		private static Logger _logger;
		public BuildCategories(InventoryDbContext context)
        {
            _context = context;
			_logger = LoggerBuilderSingleton.InventoryLog;
			_logger.Debug($"Build Category Migration");
		}

        public void ExecuteSeed()
        {
			_logger.Debug($"The number of categories records {_context.Categories.Count()}");
			if (_context.Categories.Count() == 0)
			{
				_logger.Debug($"Initiate Category Migration");
				_context.Categories.AddRange(
				new Category()
				{
					CreatedDate = DateTime.Now,
					IsActive = true,
					IsDeleted = false,
					Name = "Movies",
					CategoryDetail = new CategoryDetail()
					{
						ColorValue = "#0000FF",
						ColorName = "Blue"
					}
				},
				new Category()
				{
					CreatedDate = DateTime.Now,
					IsActive = true,
					IsDeleted = false,
					Name = "Books",
					CategoryDetail = new CategoryDetail() { ColorValue = "#FF0000", ColorName = "Red" }
				},

				new Category()
				{
					CreatedDate = DateTime.Now,
					IsActive = true,
					IsDeleted = false,
					Name = "Games",
					CategoryDetail = new CategoryDetail() { ColorValue = "#008000", ColorName = "Green" }
				}
			);
				_context.SaveChanges();
				_logger.Debug($"Complete Item Migration");
			}

		}
    }
}
