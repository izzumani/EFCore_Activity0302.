using EFCore_Activity0302_;
using Microsoft.Extensions.Configuration;
using System;

public class Program
{
    private static IConfigurationRoot _configuration;
    //private static DBContextOptionsBuilder<InventoryDbContext> _optionBuilder;

    static void Main(string[] args)
    {
        BuildOptions();



    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        // _optionBuilder =  new DBContextOptionsBuilder<InventoryDbContext>();
        // optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));

    }
}