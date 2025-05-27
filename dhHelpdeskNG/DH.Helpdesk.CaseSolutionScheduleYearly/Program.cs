using DH.Helpdesk.CaseSolutionScheduleYearly.Services;
using DH.Helpdesk.CaseSolutionScheduleYearly.Resolver;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Net.Mail;

namespace DH.Helpdesk.CaseSolutionScheduleYearly
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(args).GetAwaiter().GetResult();
        }

        static async Task Run(string[] args)
        {
            // Standardvärden om inga argument anges
            var defaultDateAndTime = DateTime.Now;
            var defaultWorkMode = 0; // 0 = normalt läge, 1 = testläge (skapa inte ärenden)

            // Parsa kommandoradsargument
            var dateAndTime = defaultDateAndTime;
            var workMode = defaultWorkMode;

            // Kontrollera om det finns argument
            if (args.Length > 0 && DateTime.TryParse(args[0], out DateTime parsedDate))
            {
                dateAndTime = parsedDate;
                Log.Information("Använder angivet datum: {Date}", dateAndTime);
            }

            if (args.Length > 1 && int.TryParse(args[1], out int parsedWorkMode))
            {
                workMode = parsedWorkMode;
                Log.Information("Använder arbetsläge: {WorkMode}", workMode == 0 ? "Skarpt läge" : "Testläge");
            }

            // Load configuration from app.config
            string connectionString = ConfigurationManager.ConnectionStrings["Helpdesk"].ConnectionString;
            string logFilePath = ConfigurationManager.AppSettings["LogFilePath"] ?? "logs/app.log";

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                 .CreateLogger();

            ServiceResolver.Initialize();

            try
            {
                // Skapa tjänster
                var scheduleService = new ScheduleService(connectionString);
                var caseSolutionService = ServiceResolver.GetCaseSolutionService();
                var caseService = ServiceResolver.GetCaseService();
                var mailTemplateService = new MailTemplateService(connectionString);

                var caseProcessingService = new CaseProcessingService(connectionString, mailTemplateService, caseService);

                Log.Information("Programmet startat med datum: {Date}, läge: {Mode}",
                    dateAndTime, workMode == 0 ? "Skarpt" : "Test");

                var caseSolutionSchedules = await scheduleService.GetSchedulesAsync(dateAndTime);
                Log.Information("Hittade {Count} schemalagda ärenden för körning", caseSolutionSchedules.Count);

                foreach (var schedule in caseSolutionSchedules)
                {
                    var caseSolution = await caseSolutionService.GetCaseSolutionAsync(schedule.CaseSolutionId);

                    if (workMode == 1) // Testläge - visa bara information
                    {
                        Log.Information("TEST: Skulle skapat ärende för CaseSolution_Id: {CaseSolutionId}, Caption: {Caption}",
                            caseSolution.Id, caseSolution.Caption);
                        continue;
                    }

                    try
                    {
                        var caseId = await caseProcessingService.CreateCaseAsync(caseSolution);

                        if (caseId > 0)
                        {
                            Log.Information("✅ Case created successfully with ID {CaseId} for CaseSolutionId {CaseSolutionId}",
                                caseId, caseSolution.Id);
                        }
                        else
                        {
                            Log.Warning("⚠️ No Case was created for CaseSolutionId {CaseSolutionId}. Check if the insert failed silently.",
                                caseSolution.Id);
                        }

                        // I testläge uppdaterar vi inte schemaläggningen
                        if (workMode == 0)
                        {
                            await scheduleService.UpdateScheduleExecutionAsync(schedule, dateAndTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Send error mail
                        SendErrorMail("Fel vid skapande av ärende", $"Misslyckades att skapa ärende för CaseSolutionId {caseSolution.Id}. Fel: {ex.Message}");

                        Log.Error(ex, "❌ Failed to create case for CaseSolutionId {CaseSolutionId}", caseSolution.Id);
                    }
                }

                Log.Information("✅ All schedules processed.");
            }
            catch (Exception ex)
            {
                // Send error mail
                SendErrorMail("Fel under DH.Helpdesk.CaseSolutionScheduleYearly", $"Ett fel inträffade under schemaläggningen: {ex.Message}\n{ex.StackTrace}");
                Log.Error(ex, "🔴 Unhandled error during schedule run.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SendErrorMail(string subject, string body)
        {
            try
            {
                var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                var smtpPort = int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out var port) ? port : 25;
                var from = ConfigurationManager.AppSettings["ErrorMailSender"];
                var to = ConfigurationManager.AppSettings["ErrorMailRecipient"];

                using (var client = new SmtpClient(smtpServer, smtpPort))
                using (var message = new MailMessage(from, to, subject, body))
                {
                    client.EnableSsl = false; // Ingen autentisering, ingen SSL
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = true; // Anonymt
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                // Logga till fil eller ignorera, men kasta inte vidare för att undvika loop
                Serilog.Log.Error(ex, "Misslyckades att skicka felmejl.");
            }
        }
    }
}


