using EFCore_DBLibrary;
using Inventory.Common.LoggerBuilder;
using Inventory.mvc.Models;
using InventoryModels.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using System.Diagnostics;

namespace Inventory.mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InventoryDbContext _inventoryDbContext = null;
        //private readonly Logger _logger = null;
        public HomeController(
                                InventoryDbContext inventoryDbContext,
                                
                                ILogger<HomeController> logger
                            )
        {
            _logger = logger;
            _logger.LogDebug("Start Logging with Serilog on HomeController");
            _inventoryDbContext = inventoryDbContext;


        }

        public IActionResult Index()
        {
            List<Item> items = new List<Item>();
            items = _inventoryDbContext.Items.OrderBy(x => x.Name).ToList();
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}