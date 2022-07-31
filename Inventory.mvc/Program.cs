using EFCore_DBLibrary;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Host.ConfigureServices((hostContext, services) =>
{
    IConfiguration configuration = hostContext.Configuration;
    services.AddDbContext<InventoryDbContext>(_optionBuilder =>
    {
        _optionBuilder.UseSqlServer(configuration.GetConnectionString("InventoryManager"));
    });



}).UseSerilog((ctx, _logger) => {

    //var logger = new LoggerConfiguration()
    _logger.MinimumLevel.Debug()
                            .WriteTo.Console(
                                                restrictedToMinimumLevel: LogEventLevel.Debug,
                                                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                                             )
                            .WriteTo.File(
                                            "log.txt",
                                            rollingInterval: RollingInterval.Day,
                                            rollOnFileSizeLimit: true
                                           );
                            //.CreateLogger();

});



//builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
