using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CSharpSchedule.Logging;
using CSharpSchedule.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using CSharpSchedule.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory) // Correct alternative to Directory.GetCurrentDirectory()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

        // Configure Serilog before DI setup
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(configuration["Logging:LogFilePath"] ?? "logs/app.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Set up DI container
        var services = new ServiceCollection();

        // Add Serilog logger
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();  // Remove default logging providers
            loggingBuilder.AddSerilog();
        });

        // Add database service
        services.AddSingleton<IConfiguration>(configuration);
        services.AddScoped<DatabaseService>();

        var serviceProvider = services.BuildServiceProvider();

        var databaseService = serviceProvider.GetRequiredService<DatabaseService>();

        // Insert a user
        //await databaseService.InsertUserAsync("Alice");

        // Fetch users from database
        var users = await databaseService.GetUsersAsync();

        foreach (var user in users)
        {
            Console.WriteLine($"User: {user}");
        }

        Console.WriteLine("Done!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();


        // Ensure Serilog flushes logs before exit
        Log.CloseAndFlush();
    }
}
