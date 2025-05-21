using DH.Helpdesk.CaseSolutionYearly.Resolver;
using DH.Helpdesk.CaseSolutionYearly.Services;
using DH.Helpdesk.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Configure Serilog logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(configuration["Logging:LogFilePath"] ?? "logs/app.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        var services = new ServiceCollection();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(); // Serilog som "motor", men kod använder ILogger<T>
        });

        try
        {
            // Initiera Ninject-baserad Helpdesk-DI
            ServiceResolver.Initialize(configuration);

            // Hämta tjänster
            var scheduleService = new ScheduleService(configuration); // använder ILogger
            var caseSolutionService = ServiceResolver.GetCaseSolutionService();

            // Testdatum (schemakörningstid)
            var now = Convert.ToDateTime("2026-02-12 16:05:00.000");

            var caseSolutionSchedules = await scheduleService.GetSchedulesAsync(now);

            foreach (var schedule in caseSolutionSchedules)
            {
                var caseSolution = await caseSolutionService.GetCaseSolutionAsync(schedule.CaseSolutionId);
                var case = 

                // TODO:
                // 1. Skapa case
                // 2. Lägg till IsAbout
                // 3. Spara caseHistory
                // 4. Hämta Customer & User
                // 5. Logg
                // 6. Koppla ExtendedCase
                // 7. Skicka mejl

                await scheduleService.UpdateScheduleExecutionAsync(schedule, now);
            }

            Log.Information("✅ All schedules processed.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "🔴 Unhandled error during schedule run.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
