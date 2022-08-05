using Inventory.Common.LoggerBuilder;
using Serilog.Core;
using System;
using Inventory.Application;
using Microsoft.Extensions.Configuration;
using Inventory.Common.ConfigBuilder;

public class Program
{
    
    private static Logger _logger;
    private static ConsoleProgram _consoleProgram = null;
    private static IConfigurationRoot _configuration;
    static void Main(string[] args)
    {
        _logger = LoggerBuilderSingleton.InventoryLog;
        _logger.Debug("Start Logging with Serilog");
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        string connectionString = _configuration.GetConnectionString("InventoryManager");
        _consoleProgram = new ConsoleProgram(connectionString);
        /*
        _consoleProgram.DeleteAllItems();
        _consoleProgram.EnsureItems();
        _logger.Debug("New pr Existing Items");
        _consoleProgram.ListInvetory();

        _consoleProgram.UpdateItems();
        _logger.Debug("Updated Items");
        _consoleProgram.ListInvetory();
        */
        _logger.Debug("Get Items for Listing --> Stored Procedures");
        _consoleProgram.GetItemsForListing();
        _logger.Debug("Get All Active Items As Pipe Delimited String --> Scalar Function");
        _consoleProgram.GetAllActiveItemsAsPipeDelimitedString();

        _logger.Debug("Get Items Total Values --> Tabular Function");
        _consoleProgram.GetItemsTotalValues();

        _logger.Debug("Get Full Item Details --> View");
        _consoleProgram.GetFullItemDetails();


    }
    
}