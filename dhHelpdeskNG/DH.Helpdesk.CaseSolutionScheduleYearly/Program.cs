using DH.Helpdesk.CaseSolutionScheduleYearly.Services;
using DH.Helpdesk.CaseSolutionScheduleYearly.Resolver;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }

        static async Task Run()
        {
            // Load configuration from appsettings.json
            string connectionString = ConfigurationManager.ConnectionStrings["Helpdesk"].ConnectionString;
            string logFilePath = ConfigurationManager.AppSettings["LogFilePath"] ?? "logs/app.log";

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                 .CreateLogger();

            ServiceResolver.Initialize();

            try
            {
                // Initiera Ninject
                ServiceResolver.Initialize();

                // Skapa tjänster
                var scheduleService = new ScheduleService(connectionString); // använder ILogger
                var caseSolutionService = ServiceResolver.GetCaseSolutionService();
                var caseService = ServiceResolver.GetCaseService();
                var mailTemplateService = new MailTemplateService(connectionString);

                var caseProcessingService = new CaseProcessingService(connectionString, mailTemplateService, caseService);

                var now = Convert.ToDateTime("2025-06-01 14:00:00.000");

                var caseSolutionSchedules = await scheduleService.GetSchedulesAsync(now);

                foreach (var schedule in caseSolutionSchedules)
                {
                    var caseSolution = await caseSolutionService.GetCaseSolutionAsync(schedule.CaseSolutionId);
                    try
                    {
                        var caseId = await caseProcessingService.CreateCaseAsync(caseSolution);

                        if (caseId > 0)
                        {
                            Log.Information("✅ Case created successfully with ID {CaseId} for CaseSolutionId {CaseSolutionId}", caseId, caseSolution.Id);
                        }
                        else
                        {
                            Log.Warning("⚠️ No Case was created for CaseSolutionId {CaseSolutionId}. Check if the insert failed silently.", caseSolution.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "❌ Failed to create case for CaseSolutionId {CaseSolutionId}", caseSolution.Id);
                    }

                    // TODO: komplettera med andra steg: IsAbout, caseHistory, e-post osv.

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
}
