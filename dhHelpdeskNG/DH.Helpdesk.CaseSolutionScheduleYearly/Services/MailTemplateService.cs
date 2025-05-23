using DH.Helpdesk.CaseSolutionScheduleYearly.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Services
{
    internal class MailTemplateService
    {

        public async Task<MailTemplate> GetMailTemplateByIdAsync(int mailId, int customerId, int languageId, string connectionString, string dbVersion = "")
        {
            try
            {
                string sql = @"
            SELECT tblMailTemplate.MailId, tblMailTemplate_tblLanguage.Subject, 
                   tblMailTemplate_tblLanguage.Body, tblMailTemplate.SendMethod, 
                   tblMailTemplate.IncludeLogText_External
            FROM tblMailTemplate
            INNER JOIN tblMailTemplate_tblLanguage ON tblMailTemplate.Id = tblMailTemplate_tblLanguage.MailTemplate_Id
            WHERE MailId = @MailId
                AND tblMailTemplate.Customer_Id = @CustomerId
                AND tblMailTemplate_tblLanguage.Language_Id = @LanguageId
                AND tblMailTemplate_tblLanguage.Subject <> ''";

                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MailId", mailId);
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@LanguageId", languageId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Use the SqlDataReader constructor instead
                                var mailTemplate = new MailTemplate(reader);
                                return mailTemplate;
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now}, ERROR getMailTemplateById {ex.Message}");
                throw;
            }
        }
        private string GetMailTemplateIdentifier(string identifier)
        {
            return $"[#{identifier}]";
        }

        private async Task<string> CreateMessageIdAsync(string senderEmail)
        {
            // Create a message ID similar to the original method in the VB code
            // Format: GUID@domain (extracted from the sender email)
            string domain = "helpdesk.local"; // Default domain

            if (!string.IsNullOrEmpty(senderEmail) && senderEmail.Contains("@"))
            {
                domain = senderEmail.Split('@')[1];
            }

            return $"{Guid.NewGuid()}@{domain}";
        }
    }
}
