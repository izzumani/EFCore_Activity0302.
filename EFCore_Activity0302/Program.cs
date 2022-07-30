using EFCore_DBLibrary;
using Inventory.Common.ConfigBuilder;
using Inventory.Common.LoggerBuilder;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using System;

public class Program
{
    private static IConfigurationRoot _configuration;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionBuilder;
    private static Logger _logger;
    static void Main(string[] args)
    {
        _logger = LoggerBuilderSingleton.InventoryLog;
        _logger.Debug("Start Logging with Serilog");
        BuildOptions();
        EnsureItems();
        ListInvetory();

    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        
        _optionBuilder =  new DbContextOptionsBuilder<InventoryDbContext>();
        _logger.Debug($"Connection String: {_configuration.GetConnectionString("InventoryManager")}");
        _optionBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));

    }

    private static void EnsureItems()
    {
        List<string> dbContextInventory = new List<string>() 
            {
            "Batman Begins",
            "Inception",
            "Remember the Titans",
            "Star Wars: The Empire Strike Back",
            "Top Gun"
        };
        dbContextInventory.ForEach((item) => EnsureItem(item));
    }

    private static void EnsureItem(string _name)
    {
        try
        {
            using (var db = new InventoryDbContext(_optionBuilder.Options))
            {
                // determine if item Exists:
                _logger.Debug($"determine if {_name} Exists");
                //var existingItem = db.Items.FirstOrDefault(x => string.Equals(x.Name, _name, StringComparison.CurrentCultureIgnoreCase));
                var existingItem = db.Items.FirstOrDefault(x => x.Name.ToLower() == _name.ToLower());
                if (existingItem == null)
                {
                    // doesn't exists, add it
                    _logger.Debug($"Item: {_name} doesn't exists, add it");
                    var item = new Item() { Name = _name };
                    db.Items.Add(item);
                    db.SaveChanges();

                }
                else
                {
                    _logger.Debug($"Item: {_name} does exists");
                }

            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Error Message: {ex?.InnerException?.Message ?? ex?.Message} Error Source {ex?.Source} Eror Stack {ex?.StackTrace}");

        }
        
    }

    private static void ListInvetory()
    {
        try
        {
            _logger.Debug($"List item from the Database");
            using (var db = new InventoryDbContext(_optionBuilder.Options))
            {
                var items = db.Items.OrderBy(x => x.Name).ToList();
                _logger.Debug($"There are {items.Count} Items. Which includes: ");
                items.ForEach(x => _logger.Information($" Item: {x.Name}"));
            }
        }
        catch (Exception ex)
        {

            _logger.Error($"Error Message: {ex?.InnerException?.Message ?? ex?.Message} Error Source {ex?.Source} Eror Stack {ex?.StackTrace}");
        }
        
    }
}