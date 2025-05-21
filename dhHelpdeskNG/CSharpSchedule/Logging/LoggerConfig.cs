using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DH.Helpdesk.CaseSolutionYearly
{
    public static class LoggerConfig
    {
        public static ILoggerFactory ConfigureLogger(IConfiguration configuration)
        {
            var logFilePath = configuration["Logging:LogFilePath"] ?? "logs/app.log";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(Log.Logger, dispose: true);
            });
        }
    }
}