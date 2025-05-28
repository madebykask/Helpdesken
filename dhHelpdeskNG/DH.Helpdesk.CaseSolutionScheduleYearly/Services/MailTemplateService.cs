using DH.Helpdesk.CaseSolutionScheduleYearly.Helpers;
using DH.Helpdesk.CaseSolutionScheduleYearly.Models;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Services
{
    public class MailTemplateService
    {
        //Add a constructor to initialize the connection string
        private readonly string _connectionString;
        public MailTemplateService(string connectionString)
        {
            _connectionString = connectionString;
        }

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
        public async Task<bool> SendMailToPerformerAsync(Case caseData, int performerUserId, int historyId)
        {
            try
            {
                if (performerUserId <= 0)
                {
                    return false;
                }

                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Get performer user data
                    var getPerformerSql = @"
                    SELECT FirstName, SurName, Email, Phone, CellPhone 
                    FROM tblUsers 
                    WHERE Id = @UserId";

                    dynamic performerData = null;
                    using (var cmd = new SqlCommand(getPerformerSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", performerUserId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                performerData = new System.Dynamic.ExpandoObject();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    ((IDictionary<string, object>)performerData)[reader.GetName(i)] =
                                        reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }
                            }
                            else
                            {
                                return false; // Performer not found
                            }
                        }
                    }

                    // Get working group data if applicable
                    dynamic workingGroupData = null;
                    if (caseData.WorkingGroup_Id.HasValue)
                    {
                        var getWorkingGroupSql = @"
                        SELECT WorkingGroup, WorkingGroupEMail 
                        FROM tblWorkingGroup 
                        WHERE Id = @WorkingGroupId";

                        using (var cmd = new SqlCommand(getWorkingGroupSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@WorkingGroupId", caseData.WorkingGroup_Id.Value);
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    workingGroupData = new System.Dynamic.ExpandoObject();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        ((IDictionary<string, object>)workingGroupData)[reader.GetName(i)] =
                                            reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    }
                                }
                            }
                        }
                    }

                    // Get priority data if applicable
                    dynamic priorityData = null;
                    if (caseData.Priority_Id.HasValue)
                    {
                        var getPrioritySql = @"
                        SELECT Priority, PriorityName, PriorityDescription 
                        FROM tblPriority 
                        WHERE Id = @PriorityId";

                        using (var cmd = new SqlCommand(getPrioritySql, conn))
                        {
                            cmd.Parameters.AddWithValue("@PriorityId", caseData.Priority_Id.Value);
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    priorityData = new System.Dynamic.ExpandoObject();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        ((IDictionary<string, object>)priorityData)[reader.GetName(i)] =
                                            reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    }
                                }
                            }
                        }
                    }

                    // Get case type data if applicable
                    dynamic caseTypeData = null;
                    if (caseData.CaseType_Id > 0)
                    {
                        var getCaseTypeSql = @"
                        SELECT CaseType 
                        FROM tblCaseType 
                        WHERE Id = @CaseTypeId";

                        using (var cmd = new SqlCommand(getCaseTypeSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@CaseTypeId", caseData.CaseType_Id);
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    caseTypeData = new System.Dynamic.ExpandoObject();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        ((IDictionary<string, object>)caseTypeData)[reader.GetName(i)] =
                                            reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    }
                                }
                            }
                        }
                    }

                    // Get category data if applicable
                    dynamic categoryData = null;
                    if (caseData.Category_Id.HasValue)
                    {
                        var getCategorySql = @"
                        SELECT Category 
                        FROM tblCategory 
                        WHERE Id = @CategoryId";

                        using (var cmd = new SqlCommand(getCategorySql, conn))
                        {
                            cmd.Parameters.AddWithValue("@CategoryId", caseData.Category_Id.Value);
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    categoryData = new System.Dynamic.ExpandoObject();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        ((IDictionary<string, object>)categoryData)[reader.GetName(i)] =
                                            reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    }
                                }
                            }
                        }
                    }

                    // Get product area data if applicable
                    dynamic productAreaData = null;
                    if (caseData.ProductArea_Id.HasValue)
                    {
                        var getProductAreaSql = @"
                        SELECT ProductArea 
                        FROM tblProductArea 
                        WHERE Id = @ProductAreaId";

                        using (var cmd = new SqlCommand(getProductAreaSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProductAreaId", caseData.ProductArea_Id.Value);
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    productAreaData = new System.Dynamic.ExpandoObject();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        ((IDictionary<string, object>)productAreaData)[reader.GetName(i)] =
                                            reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    }
                                }
                            }
                        }
                    }

                    // Continue with customer data, global settings, etc. (as in your existing code)
                    // 3. Get customer data
                    var getCustomerSql = @"
                    SELECT c.*, s.* 
                    FROM tblCustomer c
                    LEFT JOIN tblSettings s ON c.Id = s.Customer_Id
                    WHERE c.Id = @CustomerId";

                    dynamic customerData = null;
                    using (var cmd = new SqlCommand(getCustomerSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", caseData.Customer_Id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Create a dynamic object to store customer data
                                customerData = new System.Dynamic.ExpandoObject();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    ((IDictionary<string, object>)customerData)[reader.GetName(i)] =
                                        reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }
                            }
                            else
                            {
                                return false; // Customer not found
                            }
                        }
                    }

                    // 4. Get global settings
                    var getGlobalSettingsSql = @"
                    SELECT TOP 1 * FROM tblGlobalSettings";

                    dynamic globalSettings = null;
                    using (var cmd = new SqlCommand(getGlobalSettingsSql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Create a dynamic object to store global settings
                                globalSettings = new System.Dynamic.ExpandoObject();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    ((IDictionary<string, object>)globalSettings)[reader.GetName(i)] =
                                        reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }
                            }
                            else
                            {
                                return false; // Global settings not found
                            }
                        }
                    }

                    // 5. Get the mail template for performer assignment
                    var mailTemplateId = 2; // Using EMailAssignCasePerformer (ID 3)
                    var mailTemplate = await GetMailTemplateByIdAsync(mailTemplateId, caseData.Customer_Id, caseData.RegLanguage_Id, _connectionString);

                    if (mailTemplate == null)
                    {
                        Console.WriteLine($"Warning: Could not find mail template {mailTemplateId} for customer {caseData.Customer_Id} and language {1}");
                        return false;
                    }

                    // 6. Apply all replacements as done in Mail.vb
                    var subject = mailTemplate.Subject;
                    var body = mailTemplate.Body;

                    // Apply all the replacements from the Mail.vb sendMail method
                    // Get the case number
                    string caseNumber = caseData.CaseNumber.ToString() ?? "";
                    // [#1] - CaseNumber
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("CaseNumber"), caseNumber);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("CaseNumber"), caseNumber);

                    // [#2] - CustomerName
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("CustomerName"), customerData.Name?.ToString() ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("CustomerName"), customerData.Name?.ToString() ?? "");

                    // [#3] - Persons_Name
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_Name"), caseData.PersonsName ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_Name"), caseData.PersonsName ?? "");

                    // [#4] - Caption
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Caption"), caseData.Caption ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Caption"), caseData.Caption ?? "");

                    // [#5] - Description
                    string description = caseData.Description ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Description"), description);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Description"), description.Replace("\r\n", "<br>"));

                    // [#6] - FirstName (Performer)
                    string performerFirstName = performerData?.FirstName?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("FirstName"), performerFirstName);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("FirstName"), performerFirstName);

                    // [#7] - SurName (Performer)
                    string performerSurName = performerData?.SurName?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("SurName"), performerSurName);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("SurName"), performerSurName);

                    // [#8] - Persons_EMail
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_EMail"), caseData.PersonsEmail ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_EMail"), caseData.PersonsEmail ?? "");

                    // [#9] - Persons_Phone
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_Phone"), caseData.PersonsPhone ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_Phone"), caseData.PersonsPhone ?? "");

                    // Get the most recent log data if needed (for Text_External and Text_Internal)
                    // You might need to retrieve this from the database or pass it in as a parameter
                    string textExternal = ""; // Get from database or parameter if available
                    string textInternal = ""; // Get from database or parameter if available

                    // [#10] - Text_External
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Text_External"), textExternal);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Text_External"), textExternal);

                    // [#11] - Text_Internal
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Text_Internal"), textInternal);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Text_Internal"), textInternal);

                    // [#12] - PriorityName
                    string priorityName = priorityData?.PriorityName?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("PriorityName"), priorityName);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("PriorityName"), priorityName);

                    // [#13] - WorkingGroupEMail
                    string workingGroupEmail = workingGroupData?.WorkingGroupEMail?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("WorkingGroupEMail"), workingGroupEmail);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("WorkingGroupEMail"), workingGroupEmail);

                    // Handle [#14] - External attachments flag
                    bool attachExternalFiles = body.Contains("[#14]");
                    if (attachExternalFiles)
                    {
                        body = body.Replace("[#14]", string.Empty);
                    }

                    // Handle [#30] - Internal attachments flag
                    bool attachInternalFiles = body.Contains("[#30]");
                    if (attachInternalFiles)
                    {
                        body = body.Replace("[#30]", string.Empty);
                    }

                    // [#15] - WorkingGroup
                    string workingGroupName = workingGroupData?.WorkingGroup?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("WorkingGroup"), workingGroupName);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("WorkingGroup"), workingGroupName);

                    // [#16] - RegTime
                    string regTime = caseData.RegTime.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("RegTime"), regTime);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("RegTime"), regTime);

                    // [#17] - InventoryNumber
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("InventoryNumber"), caseData.InventoryNumber ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("InventoryNumber"), caseData.InventoryNumber ?? "");

                    // [#18] - Persons_CellPhone
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_CellPhone"), caseData.PersonsCellphone ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Persons_CellPhone"), caseData.PersonsCellphone ?? "");

                    // [#19] - Available
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Available"), caseData.Available ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Available"), caseData.Available ?? "");

                    // [#20] - Priority_Description
                    string priorityDescription = priorityData?.PriorityDescription?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Priority_Description"), priorityDescription);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Priority_Description"), priorityDescription);

                    // [#21] - WatchDate
                    string watchDate = caseData.WatchDate?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("WatchDate"), watchDate);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("WatchDate"), watchDate);

                    //// [#22] - LastChangedByUser
                    //string lastChangedByUser = (caseData.ChangedName ?? "") + " " + (caseData.ChangedSurName ?? "");
                    //subject = subject.Replace(GetMailTemplateIdentifier("LastChangedByUser"), lastChangedByUser);
                    //body = body.Replace(GetMailTemplateIdentifier("LastChangedByUser"), lastChangedByUser);

                    // [#23] - Miscellaneous
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Miscellaneous"), caseData.Miscellaneous ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Miscellaneous"), caseData.Miscellaneous ?? "");

                    // [#24] - Place
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Place"), caseData.Place ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Place"), caseData.Place ?? "");

                    // [#25] - CaseType
                    string caseTypeName = caseTypeData?.CaseType?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("CaseType"), caseTypeName);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("CaseType"), caseTypeName);

                    // [#26] - Category
                    string categoryName = categoryData?.Category?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Category"), categoryName);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Category"), categoryName);

                    // [#27] - ProductArea
                    string productAreaName = productAreaData?.ProductArea?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("ProductArea"), productAreaName);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("ProductArea"), productAreaName);

                    // [#28] - ReportedBy
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("ReportedBy"), caseData.ReportedBy ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("ReportedBy"), caseData.ReportedBy ?? "");

                    // [#29] - RegUser
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("RegUser"), caseData.RegUserName ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("RegUser"), caseData.RegUserName ?? "");

                    // [#70] - Performer_Phone
                    string performerPhone = performerData?.Phone?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Performer_Phone"), performerPhone);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Performer_Phone"), performerPhone);

                    // [#71] - Performer_CellPhone
                    string performerCellPhone = performerData?.CellPhone?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Performer_CellPhone"), performerCellPhone);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Performer_CellPhone"), performerCellPhone);

                    // [#72] - Performer_Email
                    string performerEmail = performerData?.Email?.ToString() ?? "";
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Performer_Email"), performerEmail);
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("Performer_Email"), performerEmail);

                    // [#73] - IsAbout_PersonsName
                    subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("IsAbout_PersonsName"), caseData.IsAbout_PersonsName ?? "");
                    body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("IsAbout_PersonsName"), caseData.IsAbout_PersonsName ?? "");

                    //// [#80] - AutoCloseDays
                    //subject = subject.Replace(MailTemplateHelper.GetMailTemplateIdentifier("AutoCloseDays"), caseData.AutoCloseDays?.ToString() ?? "");
                    //body = body.Replace(MailTemplateHelper.GetMailTemplateIdentifier("AutoCloseDays"), caseData.AutoCloseDays?.ToString() ?? "");


                    // Continue with the rest of your existing code...
                    // Generate protocol
                    string protocol = (int)globalSettings.ServerPort == 443 ? "https" : "http";

                    // Process [#98] - Self-service link
                    var emailLogGuid = Guid.NewGuid();
                    var caseGuid = caseData.CaseGUID.ToString();

                    // Continue with the rest of your existing URL handling code...
                    // Process [#98] - Self-service link
                    // Process [#98] - Self service link
                    int maxIterations98 = 100;
                    int iteration98 = 0;
                    while (body.Contains("[#98]"))
                    {
                        if (++iteration98 > maxIterations98)
                        {
                            break;
                        }

                        int pos1 = body.IndexOf("[#98]");
                        int pos2 = body.IndexOf("[/#98]");

                        if (pos1 >= 0 && pos2 > pos1)
                        {
                            string textToReplace = body.Substring(pos1, pos2 - pos1 + 6);
                            string linkTextSelfService = body.Substring(pos1 + 5, pos2 - pos1 - 5);
                            string linkSelfService =
                                $"<a href=\"{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}\">{linkTextSelfService}</a>";
                            body = body.Replace(textToReplace, linkSelfService);
                        }
                        else
                        {
                            string linkSelfService =
                                $"<a href=\"{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}\">" +
                                $"{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}</a>";
                            body = body.Replace("[#98]", linkSelfService);
                        }
                    }

                    // Process [#99] - Helpdesk link
                    int maxIterations99 = 100;
                    int iteration99 = 0;
                    while (body.Contains("[#99]"))
                    {
                        if (++iteration99 > maxIterations99)
                        {
                            break;
                        }

                        int pos1 = body.IndexOf("[#99]");
                        int pos2 = body.IndexOf("[/#99]");

                        if (pos1 >= 0 && pos2 > pos1)
                        {
                            string textToReplace = body.Substring(pos1, pos2 - pos1 + 6);
                            string linkText = body.Substring(pos1 + 5, pos2 - pos1 - 5);
                            string editCasePath = Convert.ToBoolean(globalSettings.UseMobileRouting)
                                ? CasePaths.EDIT_CASE_MOBILEROUTE
                                : CasePaths.EDIT_CASE_DESKTOP;
                            string link =
                                $"<br><a href=\"{protocol}://{globalSettings.ServerName}{editCasePath}{caseData.Id}\">{linkText}</a>";
                            body = body.Replace(textToReplace, link);
                        }
                        else
                        {
                            string editCasePath = Convert.ToBoolean(globalSettings.UseMobileRouting)
                                ? CasePaths.EDIT_CASE_MOBILEROUTE
                                : CasePaths.EDIT_CASE_DESKTOP;
                            string url = $"{protocol}://{globalSettings.ServerName}{editCasePath}{caseData.Id}";
                            string link = $"<br><a href=\"{url}\">{url}</a>";
                            body = body.Replace("[#99]", link);
                        }
                    }



                    // Replace line breaks with <br>
                    body = body.Replace("\r\n", "<br>");

                    // 7. Create a message ID
                    string messageId = await CreateMessageIdAsync(customerData.HelpDeskEmail?.ToString());

                    // 8. Get performer email for sending the actual mail (we already have it but the code keeps the same structure)
                    string receiverEmail = performerData.Email?.ToString();
                    if (string.IsNullOrEmpty(receiverEmail))
                    {
                        return false; // No valid email
                    }

                    // 9. Insert into tblEmailLog
                    var createEmailLogSql = @"
                    INSERT INTO tblEmailLog (
                        EMailLogGUID, Log_Id, EMailAddress, MailID, MessageId, 
                        CaseHistory_Id, CreatedDate, ChangedDate, SendTime, 
                        ResponseMessage, Body, Subject, Cc, Bcc, HighPriority, 
                        Files, [From], SendStatus, LastAttempt, Attempts, FilesInternal
                    ) 
                    VALUES (
                        @EMailLogGUID, NULL, @EmailAddress, @MailID, @MessageId,
                        @CaseHistoryId, GETUTCDATE(), GETUTCDATE(), NULL,
                        'Enqueued', @Body, @Subject, NULL, NULL, 0,
                        NULL, @FromAddress, 1, NULL, 0, NULL
                    )";

                    using (var cmd = new SqlCommand(createEmailLogSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EMailLogGUID", emailLogGuid);
                        cmd.Parameters.AddWithValue("@EmailAddress", receiverEmail);
                        cmd.Parameters.AddWithValue("@MailID", mailTemplateId);
                        cmd.Parameters.AddWithValue("@MessageId", messageId);
                        cmd.Parameters.AddWithValue("@CaseHistoryId", historyId);
                        cmd.Parameters.AddWithValue("@Body", body);
                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@FromAddress", customerData.HelpDeskEmail?.ToString() ?? "noreply@helpdesk.com");

                        await cmd.ExecuteNonQueryAsync();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now}, ERROR SendMailToPerformerAsync {ex.Message}");
                throw;
            }
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
