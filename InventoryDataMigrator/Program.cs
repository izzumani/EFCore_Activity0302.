using EFCore_DBLibrary;
using Inventory.Common.ConfigBuilder;
using Inventory.Common.LoggerBuilder;
using InventoryModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using InventoryModels.DTOs;
using Microsoft.Data.SqlClient;
using InventoryDataMigrator;

public class Program
{

    private static DbContextOptionsBuilder<InventoryDbContext> _optionBuilder;
    private static Logger _logger;
    private static IConfigurationRoot _configuration;
    static void Main(string[] args)
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _logger = LoggerBuilderSingleton.InventoryLog;
        _optionBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        _logger.Debug($"Connection String: {_configuration.GetConnectionString("InventoryManager")}");
        _optionBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        ApplyMigrations();
        ExecuteCustomSeedData();
    }

    private static void ApplyMigrations()
    {

        using (var db = new InventoryDbContext(_optionBuilder.Options))
        {
            _logger.Debug($"Database Migration");
            db.Database.Migrate();
        }
    }

    private static void ExecuteCustomSeedData()
    {
        using (var context = new InventoryDbContext(_optionBuilder.Options))
        {
            var categories = new BuildCategories(context);
            categories.ExecuteSeed();

            var items = new BuildItems(context);
            items.ExecuteSeed();

            var genres = new BuildGenres(context);
            genres.ExecuteSeed();
        }
    }



}