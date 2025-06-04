using DH.Helpdesk.CaseSolutionScheduleYearly.Helpers;
using DH.Helpdesk.CaseSolutionScheduleYearly.Resolver;
using DH.Helpdesk.CaseSolutionScheduleYearly.Services;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            //För test i debug - titta i tabellen tblCaseSolutionSchedule efter NextRun
            //var defaultDateAndTime = Convert.ToDateTime("2025-06-04 14:12:00.000"); // Sätt ett standarddatum för testning
            var defaultDateAndTime = DateTime.Now;
            var defaultWorkMode = Enums.WorkMode.Production; // 0 = normalt läge, 1 = testläge (skapa inte ärenden)

            // Parsa kommandoradsargument
            var dateAndTime = defaultDateAndTime;
            var workMode = defaultWorkMode;

            // Kontrollera om det finns argument
            if (args.Length > 0)
            {
                // Hantera datum om det finns i första argumentet
                if (DateTime.TryParse(args[0], out DateTime parsedDate))
                {
                    dateAndTime = parsedDate;
                }
                // Om första argumentet är "Test" eller "Production"
                else if (Enum.TryParse(args[0], true, out Enums.WorkMode parsedMode))
                {
                    workMode = parsedMode;
                }
            }

            // Kontrollera om det finns ett andra argument
            if (args.Length > 1)
            {
                // Om andra argumentet är "Test" eller "Production"
                if (Enum.TryParse(args[1], true, out Enums.WorkMode parsedMode))
                {
                    workMode = parsedMode;
                }
            }

            // Load configuration from app.config
            string connectionString = ConfigurationManager.ConnectionStrings["Helpdesk"].ConnectionString;
            string logFilePath = ConfigurationManager.AppSettings["LogFilePath"] ?? "logs/app.log";

            // Om det inte är en absolut sökväg, kombinera med programmets exekverings-katalog
            if (!Path.IsPathRooted(logFilePath))
            {
                logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFilePath);
            }

            // Skapa katalogen för loggfilen om den inte existerar
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            string logFileWithDate = Path.Combine(
            Path.GetDirectoryName(logFilePath) ?? "",
            Path.GetFileNameWithoutExtension(logFilePath) + "_.log");

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(
                        logFileWithDate,
                        rollingInterval: RollingInterval.Day)
                    .CreateLogger();

            ServiceResolver.Initialize();

            try
            {
                Log.Information("Programmet startat med datum: {Date}, läge: {Mode}",
                   dateAndTime, workMode); 
                // Skapa tjänster
                var scheduleService = new ScheduleService(connectionString);
                var caseSolutionService = ServiceResolver.GetCaseSolutionService();
                var caseService = ServiceResolver.GetCaseService();
                var mailTemplateService = new MailTemplateService(connectionString);
                var customerService = ServiceResolver.GetCustomerService();

                var caseProcessingService = new CaseProcessingService(connectionString, mailTemplateService, caseService, customerService);

                var caseSolutionSchedules = await scheduleService.GetSchedulesAsync(dateAndTime);
                Log.Information("Hittade {Count} schemalagda ärenden för körning", caseSolutionSchedules.Count);

                foreach (var schedule in caseSolutionSchedules)
                {
                    var caseSolution = await caseSolutionService.GetCaseSolutionAsync(schedule.CaseSolutionId);

                    if (workMode == Enums.WorkMode.Test) // Testläge - visa bara information
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
                            Log.Information(
                               "✅ Case created successfully with ID {CaseId} for CaseSolutionId {CaseSolutionId}, Caption: {Caption}",
                               caseId,
                               caseSolution.Id,
                               caseSolution.Caption
                             );
                            // I testläge uppdaterar vi inte schemaläggningen
                            if (workMode == Enums.WorkMode.Production) 
                            {
                                await scheduleService.UpdateScheduleExecutionAsync(schedule, dateAndTime);
                            }
                        }
                        else
                        {
                            SendErrorEmail("Fel vid skapande av ärende", $"Misslyckades att skapa ärende för CaseSolutionId {caseSolution.Id}");
                            Log.Warning("⚠️ No Case was created for CaseSolutionId {CaseSolutionId}. Check if the insert failed silently.",
                                caseSolution.Id);
                        }                       
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "❌ Failed to create case for CaseSolutionId {CaseSolutionId}", caseSolution.Id);
                        
                        SendErrorEmail("Fel vid skapande av ärende", $"Misslyckades att skapa ärende för CaseSolutionId {caseSolution.Id}. Fel: {ex.Message}");
                    }
                }

                Log.Information("✅ All schedules processed.");
            }

            catch (Exception ex)
            {
                Log.Error(ex, "🔴 Unhandled error during schedule run.");
                SendErrorEmail("Fel under DH.Helpdesk.CaseSolutionScheduleYearly", $"Ett fel inträffade under schemaläggningen: {ex.Message}\n{ex.StackTrace}");
               
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SendErrorEmail(string subject, string body)
        {
            try
            {
                //Get customersettings
                int customerId = int.TryParse(ConfigurationManager.AppSettings["CustomerId"], out var cid) ? cid : 1;
                var setting = ServiceResolver.GetSettingService().GetCustomerSettingsAsync(1).Result;
                if (setting.UseGraphSendingEmail)
                {
                    var token = GetOAuthToken(setting.GraphTenantId, setting.GraphClientId, setting.GraphClientSecret);

                    if (token != null)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                            var toAddress = setting.ErrorMailTo ?? ConfigurationManager.AppSettings["ErrorMailTo"]; 
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
                else
                {
                    //TODO - Anvönda samma logik som i EmailProcessor .cs för att skicka e-post utan Graph API
                    try
                    {
                        var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                        var smtpPort = int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out var port) ? port : 25;
                        var from = ConfigurationManager.AppSettings["ErrorMailSender"];
                        var to = ConfigurationManager.AppSettings["ErrorMailTo"];

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
                       Log.Error("Misslyckades att skicka felmejl.", ex);
                    }
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
                    Log.Error($"Failed to get access token: {responseString}");
                    throw new Exception($"Failed to get access token: {responseString}");
                }

                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                return responseObject.access_token;
            }
        }
    }
}


