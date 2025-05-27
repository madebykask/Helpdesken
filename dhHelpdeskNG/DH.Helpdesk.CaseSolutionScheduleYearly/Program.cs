using DH.Helpdesk.CaseSolutionScheduleYearly.Resolver;
using DH.Helpdesk.CaseSolutionScheduleYearly.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

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
            //test
            //var defaultDateAndTime = Convert.ToDateTime("2025-07-01 14:00:00"); // Sätt ett standarddatum för testning
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
                Log.Information("Programmet startat med datum: {Date}, läge: {Mode}",
                   dateAndTime, workMode == 0 ? "Skarpt" : "Test");

                // Skapa tjänster
                var scheduleService = new ScheduleService(connectionString);
                var caseSolutionService = ServiceResolver.GetCaseSolutionService();
                var caseService = ServiceResolver.GetCaseService();
                var mailTemplateService = new MailTemplateService(connectionString);

                var caseProcessingService = new CaseProcessingService(connectionString, mailTemplateService, caseService);

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
                        SendGraphErrorEmail("Fel vid skapande av ärende", $"Misslyckades att skapa ärende för CaseSolutionId {caseSolution.Id}. Fel: {ex.Message}");

                        Log.Error(ex, "❌ Failed to create case for CaseSolutionId {CaseSolutionId}", caseSolution.Id);
                    }
                }

                Log.Information("✅ All schedules processed.");
            }
            catch (Exception ex)
            {
                // Send error mail
                SendGraphErrorEmail("Fel under DH.Helpdesk.CaseSolutionScheduleYearly", $"Ett fel inträffade under schemaläggningen: {ex.Message}\n{ex.StackTrace}");
                Log.Error(ex, "🔴 Unhandled error during schedule run.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SendGraphErrorEmail(string subject, string body)
        {
            try
            {
                //Get customersettings
                int customerId = int.TryParse(ConfigurationManager.AppSettings["CustomerId"], out var cid) ? cid : 1;
                var setting = ServiceResolver.GetSettingService().GetCustomerSettingsAsync(1).Result;
                var token = GetOAuthToken(setting.GraphTenantId, setting.GraphClientId, setting.GraphClientSecret);

                if (token != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        var toAddress = setting.ErrorMailTo ?? ConfigurationManager.AppSettings["ErrorMailTo"]; // Anpassa efter din modell
                        var message = new
                        {
                            message = new
                            {
                                subject,
                                body = new { contentType = "HTML", content = body },
                                toRecipients = new[] {
                                    new { emailAddress = new { address = toAddress } }
                                }
                            }
                        };
                        
                        var jsonMessage = JsonConvert.SerializeObject(message);
                        var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        var usr = setting.GraphUserName;
                        var emailEndpoint = $"https://graph.microsoft.com/v1.0/users/{usr}/sendMail";

                        var response = client.PostAsync(emailEndpoint, content).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            string error = response.Content.ReadAsStringAsync().Result;
                            throw new Exception($"Failed to send email via Graph: {error}");
                        }


                    }
                }
                else
                {
                    Log.Error($"Error sending email.", null);

                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error sending Graph email. ", ex);

            }
        }
        static string GetOAuthToken(string tenantId, string clientId, string clientSecret)
        {
            using (HttpClient client = new HttpClient())
            {
                var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
                var content = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("client_id", clientId),
                     new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                     new KeyValuePair<string, string>("client_secret", clientSecret),
                     new KeyValuePair<string, string>("grant_type", "client_credentials")
                 });

                HttpResponseMessage response = client.PostAsync(tokenEndpoint, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    //throw new Exception($"Failed to get access token: {responseString}");
                }

                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                return responseObject.access_token;
            }
        }
    }
}


