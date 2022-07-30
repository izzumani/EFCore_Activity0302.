using EFCore_DBLibrary;
using Inventory.Common.ConfigBuilder;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

public class Program
{
    private static IConfigurationRoot _configuration;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionBuilder;

    static void Main(string[] args)
    {
        BuildOptions();
        EnsureItems();
        ListInvetory();

    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionBuilder =  new DbContextOptionsBuilder<InventoryDbContext>();
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
        using (var db = new InventoryDbContext(_optionBuilder.Options))
        {
            // determine if item Exists:
            //var existingItem = db.Items.FirstOrDefault(x => string.Equals(x.Name, _name, StringComparison.CurrentCultureIgnoreCase));
            var existingItem = db.Items.FirstOrDefault(x => x.Name.ToLower() ==_name.ToLower());
            if (existingItem ==null)
            {
                // doesn't exists, add it
                var item = new Item() { Name = _name };
                db.Items.Add(item); 
                db.SaveChanges();   

            }

        }
    }

    private static void ListInvetory()
    {
        using (var db = new InventoryDbContext(_optionBuilder.Options))
        {
            var items = db.Items.OrderBy(x => x.Name).ToList();
            items.ForEach(x => Console.WriteLine($"New Item: {x.Name}"));
        }
    }
}