﻿
using EFCore_DBLibrary;
using Inventory.Common.ConfigBuilder;
using Inventory.Common.LoggerBuilder;
using InventoryModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using InventoryModels.DTOs;
using Microsoft.Data.SqlClient;

namespace Inventory.Application
{
    public class ConsoleProgram
    {
        
        private static DbContextOptionsBuilder<InventoryDbContext> _optionBuilder;
        private static Logger _logger;
        
       

        public ConsoleProgram (string connectionString)
        {

            _logger = LoggerBuilderSingleton.InventoryLog;
            _optionBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _logger.Debug($"Connection String: {connectionString}");
            _optionBuilder.UseSqlServer(connectionString);

        }

        public  void EnsureItems()
        {
            Random rnd = new Random();
            rnd.Next(1, 100);

            List<Item> dbContextInventory = new List<Item>()
            {
            new Item () {Name ="Batman Begins",Quantity =rnd.Next(1, 100),
                        Description  ="You either die the hero or live long enough to see yourself become the villain",
                        Notes ="Christian Bale, KatieHolmes",IsActive = true, CurrentOrFinalPrice = Convert.ToDecimal(Math.Round((rnd.NextDouble() * 100),2))},
            new Item () {Name ="Inception",Quantity =rnd.Next(1, 100),
                        Description  ="You mustn't be afraid to dream a little bigger,darling",
                        Notes ="Leonardo DiCaprio, Tom Hardy, Joseph Gordon-Levitt",
                        IsActive = true, CurrentOrFinalPrice = Convert.ToDecimal(Math.Round((rnd.NextDouble() * 100),2))},

            new Item () {Name ="Remember the Titans",Quantity =rnd.Next(1, 100),
                        Description  ="Left Side, Strong Side",
                        Notes ="Denzell Washington, Will Patton",IsActive = true, CurrentOrFinalPrice = Convert.ToDecimal(Math.Round((rnd.NextDouble() * 100),2))},

            new Item () {Name ="Star Wars: The Empire Strike Back",Quantity =rnd.Next(1, 100),
                Description  ="He will join us or die, master",
                Notes ="Harrison Ford, Carrie Fisher, Mark Hamill",IsActive = true, CurrentOrFinalPrice = Convert.ToDecimal(Math.Round((rnd.NextDouble() * 100), 2))},


            new Item () {Name ="Top Gun",Quantity =rnd.Next(1, 100),
                Description  ="I feel the need, the need for speed!",
                Notes ="Tom Cruise, Anthony Edwards, Val Kilmer",IsActive = true,
                CurrentOrFinalPrice = Convert.ToDecimal(Math.Round((rnd.NextDouble() * 100), 2))},

        };
            dbContextInventory.ForEach((item) => EnsureItem(item));
        }

        private  void EnsureItem(Item _item)
        {
            try
            {
                using (var db = new InventoryDbContext(_optionBuilder.Options))
                {
                    // determine if item Exists:
                    _logger.Debug($"determine if {_item.Name} Exists");
                    //var existingItem = db.Items.FirstOrDefault(x => string.Equals(x.Name, _name, StringComparison.CurrentCultureIgnoreCase));
                    var existingItem = db.Items.FirstOrDefault(x => x.Name.ToLower() == _item.Name.ToLower());
                    if (existingItem == null)
                    {
                        // doesn't exists, add it
                        _logger.Debug($"Item: {_item.Name} doesn't exists, add it");
                        //var item = new Item() { Name = _name };
                        db.Items.Add(_item);
                        db.SaveChanges();

                    }
                    else
                    {
                        _logger.Debug($"Item: {_item.Name} exists");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error Message: {ex?.InnerException?.Message ?? ex?.Message} Error Source {ex?.Source} Eror Stack {ex?.StackTrace}");

            }

        }

        public void ListInvetory()
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

        public void UpdateItems()
        {
            using (var db = new InventoryDbContext(_optionBuilder.Options))
            {
                var items = db.Items.ToList();
                foreach (var item in items)
                {
                    item.CurrentOrFinalPrice = (item.CurrentOrFinalPrice * Convert.ToDecimal(0.97));
                }
                db.Items.UpdateRange(items);
                db.SaveChanges();
            }
        }


        public void DeleteAllItems()
        {
            using (var db = new InventoryDbContext(_optionBuilder.Options))
            {
                var items = db.Items.ToList();
                db.Items.RemoveRange(items);
                db.SaveChanges();
            }
        }

        public void GetItemsForListing()
        {
            using (var db = new InventoryDbContext(_optionBuilder.Options))
            {
                var results = db.ItemsForListing.FromSqlRaw("EXECUTE dbo.GetItemsForListing").ToList();
                foreach (var item in results)
                {
                    //Console.WriteLine($"ITEM {item.Id}] {item.Name}");
                    _logger.Debug($"ITEM {item.Name} {item.Description} {item.CategoryName} {item.IsDeleted}");
                }
            }
        }

        public void GetAllActiveItemsAsPipeDelimitedString()
        {
            using (var db = new InventoryDbContext(_optionBuilder.Options))
            {
                var isActiveParm = new SqlParameter("IsActive", 1);
                var result = db.AllItemsOutput.FromSqlRaw("SELECT [dbo].[ItemNamesPipeDelimitedString] (@IsActive)AllItems", isActiveParm).FirstOrDefault();
                //Console.WriteLine($"All active Items: {result.AllItems}");
                _logger.Debug($"All active Items: {result?.AllItems}");

            }
        }

        public void GetItemsTotalValues()
        {
            using (var db = new InventoryDbContext(_optionBuilder.Options))
            {
                var isActiveParm = new SqlParameter("IsActive", 1);
                var result = db.GetItemsTotalValues.FromSqlRaw("SELECT * from [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParm).ToList();
                foreach (var item in result)
                {
                    //Console.WriteLine($"New Item] {item.Id,-10}" + $"|{item.Name,-50}" + $"|{item.Quantity,-4}" + $"|{item.TotalValue,-5}");
                    _logger.Debug($"New Item] {item.Id,-10}" + $"|{item.Name,-50}" + $"|{item.Quantity,-4}" + $"|{item.TotalValue,-5}");
                }
            }
        }




    }
}