using DH.Helpdesk.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Services
{
    internal class CaseProcessingService
    {
        private readonly string _connectionString;
        private readonly MailTemplateService _mailTemplateService;

        public CaseProcessingService(string connection, MailTemplateService mailTemplateService)
        {
            _connectionString = connection;
            _mailTemplateService = mailTemplateService;
        }

        public async Task<int?> CreateCaseAsync(CaseSolution c)
        {
            // First, check if there's an extended case form connected to this case solution
            var extendedCaseFormId = await GetExtendedCaseFormIdForCaseSolutionAsync(c.Id);

            var caseGuid = Guid.NewGuid();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
                INSERT INTO tblCase (
                    CaseGUID, ExternalCaseNumber, CaseType_Id, Customer_Id, ProductArea_Id, Category_Id, Region_Id, ReportedBy,
                    Department_Id, OU_Id, Project_Id, System_Id, Urgency_Id, Impact_Id, Supplier_Id, SMS, Cost, OtherCost,
                    Problem_Id, Change_Id, CausingPartId, Verified, VerifiedDescription, SolutionRate, InventoryType, InventoryLocation,
                    Currency, ContactBeforeAction, FinishingDescription, Persons_Name, Persons_EMail, Persons_Phone, Persons_CellPhone,
                    Place, UserCode, CostCentre, InventoryNumber, InvoiceNumber, Caption, Description, Miscellaneous, Available,
                    ReferenceNumber, Priority_Id, WorkingGroup_Id, Performer_User_Id, Status_Id, StateSecondary_Id,
                    WatchDate, PlanDate, AgreedDate, FinishingDate, RegistrationSource, RegLanguage_Id, RegistrationSourceCustomer_Id,
                    RegUserName, CaseSolution_Id, RegTime, ChangeTime
                )
                VALUES (
                    @CaseGUID, @ExternalCaseNumber, @CaseType_Id, @Customer_Id, @ProductArea_Id, @Category_Id, @Region_Id, @ReportedBy,
                    @Department_Id, @OU_Id, @Project_Id, @System_Id, @Urgency_Id, @Impact_Id, @Supplier_Id, @SMS, @Cost, @OtherCost,
                    @Problem_Id, @Change_Id, @CausingPartId, @Verified, @VerifiedDescription, @SolutionRate, @InventoryType, @InventoryLocation,
                    @Currency, @ContactBeforeAction, @FinishingDescription, @Persons_Name, @Persons_EMail, @Persons_Phone, @Persons_CellPhone,
                    @Place, @UserCode, @CostCentre, @InventoryNumber, @InvoiceNumber, @Caption, @Description, @Miscellaneous, @Available,
                    @ReferenceNumber, @Priority_Id, @WorkingGroup_Id, @Performer_User_Id, @Status_Id, @StateSecondary_Id,
                    @WatchDate, @PlanDate, @AgreedDate, @FinishingDate, @RegistrationSource, @RegLanguage_Id, @RegistrationSourceCustomer_Id,
                    @RegUserName, @CaseSolution_Id, GETUTCDATE(), GETUTCDATE()
                );
                SELECT Id FROM tblCase WHERE CaseGUID = @CaseGUID;
                ";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    object SqlVal<T>(T val) => EqualityComparer<T>.Default.Equals(val, default(T)) ? DBNull.Value : (object)val;
                    object SqlStr(string s) => s ?? "";

                    cmd.Parameters.AddWithValue("@CaseGUID", caseGuid);
                    cmd.Parameters.AddWithValue("@ExternalCaseNumber", "");
                    cmd.Parameters.AddWithValue("@CaseType_Id", SqlVal(c.CaseType_Id));
                    cmd.Parameters.AddWithValue("@Customer_Id", SqlVal(c.Customer_Id));
                    cmd.Parameters.AddWithValue("@ProductArea_Id", SqlVal(c.ProductArea_Id));
                    cmd.Parameters.AddWithValue("@Category_Id", SqlVal(c.Category_Id));
                    cmd.Parameters.AddWithValue("@Region_Id", SqlVal(c.Region_Id));
                    cmd.Parameters.AddWithValue("@ReportedBy", SqlStr(c.ReportedBy));
                    cmd.Parameters.AddWithValue("@Department_Id", SqlVal(c.Department_Id));
                    cmd.Parameters.AddWithValue("@OU_Id", SqlVal(c.OU_Id));
                    cmd.Parameters.AddWithValue("@Project_Id", SqlVal(c.Project_Id));
                    cmd.Parameters.AddWithValue("@System_Id", SqlVal(c.System_Id));
                    cmd.Parameters.AddWithValue("@Urgency_Id", SqlVal(c.Urgency_Id));
                    cmd.Parameters.AddWithValue("@Impact_Id", SqlVal(c.Impact_Id));
                    cmd.Parameters.AddWithValue("@Supplier_Id", SqlVal(c.Supplier_Id));
                    cmd.Parameters.AddWithValue("@SMS", c.SMS);
                    cmd.Parameters.AddWithValue("@Cost", c.Cost);
                    cmd.Parameters.AddWithValue("@OtherCost", c.OtherCost);
                    cmd.Parameters.AddWithValue("@Problem_Id", SqlVal(c.Problem_Id));
                    cmd.Parameters.AddWithValue("@Change_Id", SqlVal(c.Change_Id));
                    cmd.Parameters.AddWithValue("@CausingPartId", SqlVal(c.CausingPartId));
                    cmd.Parameters.AddWithValue("@Verified", c.Verified);
                    cmd.Parameters.AddWithValue("@VerifiedDescription", SqlStr(c.VerifiedDescription));
                    cmd.Parameters.AddWithValue("@SolutionRate", SqlStr(c.SolutionRate));
                    cmd.Parameters.AddWithValue("@InventoryType", SqlStr(c.InventoryType));
                    cmd.Parameters.AddWithValue("@InventoryLocation", SqlStr(c.InventoryLocation));
                    cmd.Parameters.AddWithValue("@Currency", SqlStr(c.Currency));
                    cmd.Parameters.AddWithValue("@ContactBeforeAction", c.ContactBeforeAction);
                    cmd.Parameters.AddWithValue("@FinishingDescription", SqlStr(c.FinishingDescription));
                    cmd.Parameters.AddWithValue("@Persons_Name", SqlStr(c.PersonsName));
                    cmd.Parameters.AddWithValue("@Persons_EMail", SqlStr(c.PersonsEmail));
                    cmd.Parameters.AddWithValue("@Persons_Phone", SqlStr(c.PersonsPhone));
                    cmd.Parameters.AddWithValue("@Persons_CellPhone", SqlStr(c.PersonsCellPhone));
                    cmd.Parameters.AddWithValue("@Place", SqlStr(c.Place));
                    cmd.Parameters.AddWithValue("@UserCode", SqlStr(c.UserCode));
                    cmd.Parameters.AddWithValue("@CostCentre", SqlStr(c.CostCentre));
                    cmd.Parameters.AddWithValue("@InventoryNumber", SqlStr(c.InventoryNumber));
                    cmd.Parameters.AddWithValue("@InvoiceNumber", SqlStr(c.InvoiceNumber));
                    cmd.Parameters.AddWithValue("@Caption", SqlStr(c.Caption));
                    cmd.Parameters.AddWithValue("@Description", SqlStr(c.Description));
                    cmd.Parameters.AddWithValue("@Miscellaneous", SqlStr(c.Miscellaneous));
                    cmd.Parameters.AddWithValue("@Available", SqlStr(c.Available));
                    cmd.Parameters.AddWithValue("@ReferenceNumber", SqlStr(c.ReferenceNumber));
                    cmd.Parameters.AddWithValue("@Priority_Id", SqlVal(c.Priority_Id));
                    cmd.Parameters.AddWithValue("@WorkingGroup_Id", SqlVal(c.WorkingGroup_Id));
                    cmd.Parameters.AddWithValue("@Performer_User_Id", SqlVal(c.PerformerUser_Id));
                    cmd.Parameters.AddWithValue("@Status_Id", SqlVal(c.Status_Id));
                    cmd.Parameters.AddWithValue("@StateSecondary_Id", SqlVal(c.StateSecondary_Id));
                    cmd.Parameters.AddWithValue("@WatchDate", SqlVal(c.WatchDate));
                    cmd.Parameters.AddWithValue("@PlanDate", SqlVal(c.PlanDate));
                    cmd.Parameters.AddWithValue("@AgreedDate", SqlVal(c.AgreedDate));
                    cmd.Parameters.AddWithValue("@FinishingDate", SqlVal(c.FinishingDate));
                    cmd.Parameters.AddWithValue("@RegistrationSource", 0);
                    cmd.Parameters.AddWithValue("@RegLanguage_Id", 1);
                    cmd.Parameters.AddWithValue("@RegistrationSourceCustomer_Id", SqlVal(c.RegistrationSource));
                    cmd.Parameters.AddWithValue("@RegUserName", SqlStr(c.ReportedBy));
                    cmd.Parameters.AddWithValue("@CaseSolution_Id", SqlVal(c.Id));

                    try
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        var newCaseId = result != null ? Convert.ToInt32(result) : 0;

                        if (newCaseId > 0)
                        {
                            // 2. Create isAbout data if needed
                            if (!string.IsNullOrWhiteSpace(c.IsAbout_PersonsEmail))
                            {
                                await SaveIsAboutAsync(newCaseId, c);
                            }
                            var historyId = await SaveCaseHistoryAsync(newCaseId, "Scheduled Job");

                            // 4. Log internal/external texts if needed
                            if (!string.IsNullOrWhiteSpace(c.Text_Internal) || !string.IsNullOrWhiteSpace(c.Text_External))
                            {
                                await SaveLogAsync(newCaseId, c, historyId);
                            }
                            if (extendedCaseFormId.HasValue)
                            {
                                // Create extended case data
                                var extendedCaseDataId = await CreateExtendedCaseDataAsync(extendedCaseFormId.Value);

                                // Create the connection between the case and the extended case form/data
                                await CreateExtendedCaseConnectionAsync(newCaseId, extendedCaseFormId.Value, extendedCaseDataId);
                            }
                            if (c.PerformerUser_Id.HasValue && c.PerformerUser_Id.Value > 0)
                            {
                                await SendMailToPerformerAsync(newCaseId, c.PerformerUser_Id.Value, historyId);
                            }
                            return newCaseId;
                        }
                        return null;
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Error creating case: " + ex.Message);
                        return null;
                    }
                }
            }
        }
        private async Task SaveIsAboutAsync(int caseId, CaseSolution caseSolution)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
            INSERT INTO tblCaseIsAbout (
                Case_Id, UserId, SurName, EMail, Phone, CellPhone,
                Region_Id, Department_Id, OU_Id, Location, CostCentre, UserCode
            )
            VALUES (
                @Case_Id, @UserId, @SurName, @EMail, @Phone, @CellPhone,
                @Region_Id, @Department_Id, @OU_Id, @Location, @CostCentre, @UserCode
            );";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    object SqlVal<T>(T val) => EqualityComparer<T>.Default.Equals(val, default(T)) ? DBNull.Value : (object)val;
                    object SqlStr(string s) => s ?? "";

                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@UserId", SqlStr(caseSolution.IsAbout_ReportedBy));
                    cmd.Parameters.AddWithValue("@SurName", SqlStr(caseSolution.IsAbout_PersonsName));
                    cmd.Parameters.AddWithValue("@EMail", SqlStr(caseSolution.IsAbout_PersonsEmail));
                    cmd.Parameters.AddWithValue("@Phone", SqlStr(caseSolution.IsAbout_PersonsPhone));
                    cmd.Parameters.AddWithValue("@CellPhone", SqlStr(caseSolution.IsAbout_PersonsCellPhone));
                    cmd.Parameters.AddWithValue("@Region_Id", SqlVal(caseSolution.IsAbout_Region_Id));
                    cmd.Parameters.AddWithValue("@Department_Id", SqlVal(caseSolution.IsAbout_Department_Id));
                    cmd.Parameters.AddWithValue("@OU_Id", SqlVal(caseSolution.IsAbout_OU_Id));
                    cmd.Parameters.AddWithValue("@Location", SqlStr(caseSolution.IsAbout_Place));
                    cmd.Parameters.AddWithValue("@CostCentre", SqlStr(caseSolution.IsAbout_CostCentre));
                    cmd.Parameters.AddWithValue("@UserCode", SqlStr(caseSolution.IsAbout_UserCode));

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task SaveLogAsync(int caseId, CaseSolution caseSolution, int caseHistoryId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
            INSERT INTO tblLog (
                CaseHistory_Id, Case_Id, LogDate, 
                Text_Internal, Text_External, RegUser,
                RegTime, ChangeTime, LogGUID
            )
            VALUES (
                @CaseHistory_Id, @Case_Id, GETUTCDATE(),
                @Text_Internal, @Text_External, @RegUser,
                GETUTCDATE(), GETUTCDATE(), @LogGUID
            );";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    object SqlStr(string s) => s ?? "";

                    cmd.Parameters.AddWithValue("@CaseHistory_Id", caseHistoryId);
                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@Text_Internal", SqlStr(caseSolution.Text_Internal));
                    cmd.Parameters.AddWithValue("@Text_External", SqlStr(caseSolution.Text_External));
                    cmd.Parameters.AddWithValue("@RegUser", "Scheduled Job");
                    cmd.Parameters.AddWithValue("@LogGUID", Guid.NewGuid());

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<int> SaveCaseHistoryAsync(int caseId, string createdByUser)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
            INSERT INTO tblCaseHistory (
                CaseHistoryGUID, Case_Id, CreatedDate, CreatedByUser
            )
            VALUES (
                @CaseHistoryGUID, @Case_Id, GETUTCDATE(), @CreatedByUser
            );
            SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    var caseHistoryGuid = Guid.NewGuid();

                    cmd.Parameters.AddWithValue("@CaseHistoryGUID", caseHistoryGuid);
                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@CreatedByUser", createdByUser);

                    var result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
        private async Task<int> CreateExtendedCaseDataAsync(int extendedCaseFormId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // First step: Create the extended case data entry
                var sql = @"
            INSERT INTO tblExtendedCaseData (
                ExtendedCaseGuid, ExtendedCaseFormId, CreatedOn, CreatedBy
            )
            VALUES (
                @ExtendedCaseGuid, @ExtendedCaseFormId, GETUTCDATE(), @CreatedBy
            );
            SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    var extendedCaseGuid = Guid.NewGuid();

                    cmd.Parameters.AddWithValue("@ExtendedCaseGuid", extendedCaseGuid);
                    cmd.Parameters.AddWithValue("@ExtendedCaseFormId", extendedCaseFormId);
                    cmd.Parameters.AddWithValue("@CreatedBy", "Scheduled Job");

                    var result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private async Task CreateExtendedCaseConnectionAsync(int caseId, int extendedCaseFormId, int extendedCaseDataId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
            INSERT INTO tblCase_ExtendedCase (
                Case_Id, ExtendedCaseForm_Id, ExtendedCaseData_Id
            )
            VALUES (
                @Case_Id, @ExtendedCaseForm_Id, @ExtendedCaseData_Id
            );";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@ExtendedCaseForm_Id", extendedCaseFormId);
                    cmd.Parameters.AddWithValue("@ExtendedCaseData_Id", extendedCaseDataId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<int?> GetExtendedCaseFormIdForCaseSolutionAsync(int caseSolutionId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
            SELECT TOP 1 ExtendedCaseForms_Id
            FROM dbo.tblCaseSolution_ExtendedCaseForms
            WHERE CaseSolution_Id = @CaseSolutionId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CaseSolutionId", caseSolutionId);

                    var result = await cmd.ExecuteScalarAsync();
                    return result != null ? (int?)Convert.ToInt32(result) : null;
                }
            }
        }

        private async Task<bool> SendMailToPerformerAsync(int caseId, int performerUserId, int historyId)
        {
            if (performerUserId <= 0)
            {
                return false;
            }

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // 1. Get full case data - we'll need this for template replacements
                var getCaseSql = @"
            SELECT c.*, u.FirstName AS PerformerFirstName, u.SurName AS PerformerSurName, 
                   u.Email AS PerformerEMail, u.Phone AS PerformerPhone, u.CellPhone AS PerformerCellPhone,
                   p.Name AS PriorityName, p.Description AS PriorityDescription,
                   ct.Name AS CaseTypeName, ca.Name AS CategoryName, pa.Name AS ProductAreaName,
                   wg.Name AS WorkingGroupName, wg.EMail AS WorkingGroupEMail
            FROM tblCase c
            LEFT JOIN tblUser u ON c.Performer_User_Id = u.Id
            LEFT JOIN tblPriority p ON c.Priority_Id = p.Id
            LEFT JOIN tblCaseType ct ON c.CaseType_Id = ct.Id
            LEFT JOIN tblCategory ca ON c.Category_Id = ca.Id
            LEFT JOIN tblProductArea pa ON c.ProductArea_Id = pa.Id
            LEFT JOIN tblWorkingGroup wg ON c.WorkingGroup_Id = wg.Id
            WHERE c.Id = @CaseId";

                dynamic caseData = null;
                using (var cmd = new SqlCommand(getCaseSql, conn))
                {
                    cmd.Parameters.AddWithValue("@CaseId", caseId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Create a dynamic object to store case data
                            caseData = new System.Dynamic.ExpandoObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                ((IDictionary<string, object>)caseData)[reader.GetName(i)] =
                                    reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                        }
                        else
                        {
                            return false; // Case not found
                        }
                    }
                }


                // 3. Get customer data
                var getCustomerSql = @"
            SELECT c.*, s.* 
            FROM tblCustomer c
            LEFT JOIN tblSetting s ON c.Id = s.Customer_Id
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
                var mailTemplateId = 3; // Using EMailAssignCasePerformer (ID 3)
                var mailTemplate = await _mailTemplateService.GetMailTemplateByIdAsync(mailTemplateId, (int)caseData.Customer_Id, (int)caseData.RegLanguage_Id, _connectionString);

                if (mailTemplate == null)
                {
                    Console.WriteLine($"Warning: Could not find mail template {mailTemplateId} for customer {caseData.Customer_Id} and language {caseData.RegLanguage_Id}");
                    return false;
                }

                // 6. Apply all replacements as done in Mail.vb
                var subject = mailTemplate.Subject;
                var body = mailTemplate.Body;

                // Apply all the replacements from the Mail.vb sendMail method
                // Get the case number
                string caseNumber = caseData.Casenumber?.ToString() ?? "";

                // Replace [#1] - CaseNumber
                subject = subject.Replace(GetMailTemplateIdentifier("CaseNumber"), caseNumber);
                body = body.Replace(GetMailTemplateIdentifier("CaseNumber"), caseNumber);

                // Replace [#2] - CustomerName
                subject = subject.Replace(GetMailTemplateIdentifier("CustomerName"), customerData.Name?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("CustomerName"), customerData.Name?.ToString() ?? "");

                // Replace [#3] - Persons_Name
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_Name"), caseData.Persons_Name?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_Name"), caseData.Persons_Name?.ToString() ?? "");

                // Replace [#4] - Caption
                subject = subject.Replace(GetMailTemplateIdentifier("Caption"), caseData.Caption?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Caption"), caseData.Caption?.ToString() ?? "");

                // Replace [#5] - Description
                string description = caseData.Description?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("Description"), description);
                body = body.Replace(GetMailTemplateIdentifier("Description"), description.Replace("\r\n", "<br>"));

                // Replace [#6] - FirstName (Performer)
                subject = subject.Replace(GetMailTemplateIdentifier("FirstName"), caseData.PerformerFirstName?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("FirstName"), caseData.PerformerFirstName?.ToString() ?? "");

                // Replace [#7] - SurName (Performer)
                subject = subject.Replace(GetMailTemplateIdentifier("SurName"), caseData.PerformerSurName?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("SurName"), caseData.PerformerSurName?.ToString() ?? "");

                // Replace [#8] - Persons_EMail
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_EMail"), caseData.Persons_EMail?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_EMail"), caseData.Persons_EMail?.ToString() ?? "");

                // Replace [#9] - Persons_Phone
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_Phone"), caseData.Persons_Phone?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_Phone"), caseData.Persons_Phone?.ToString() ?? "");

                // Replace [#12] - PriorityName
                subject = subject.Replace(GetMailTemplateIdentifier("PriorityName"), caseData.PriorityName?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("PriorityName"), caseData.PriorityName?.ToString() ?? "");

                // Replace [#13] - WorkingGroupEMail
                string workingGroupEmail = caseData.WorkingGroupEMail?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("WorkingGroupEMail"), workingGroupEmail);
                body = body.Replace(GetMailTemplateIdentifier("WorkingGroupEMail"), workingGroupEmail);

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

                // Replace [#15] - WorkingGroup
                string workingGroupName = caseData.WorkingGroupName?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("WorkingGroup"), workingGroupName);
                body = body.Replace(GetMailTemplateIdentifier("WorkingGroup"), workingGroupName);

                // Replace [#16] - RegTime
                string regTime = caseData.RegTime?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("RegTime"), regTime);
                body = body.Replace(GetMailTemplateIdentifier("RegTime"), regTime);

                // Replace [#17] - InventoryNumber
                subject = subject.Replace(GetMailTemplateIdentifier("InventoryNumber"), caseData.InventoryNumber?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("InventoryNumber"), caseData.InventoryNumber?.ToString() ?? "");

                // Replace [#18] - Persons_CellPhone
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_CellPhone"), caseData.Persons_CellPhone?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_CellPhone"), caseData.Persons_CellPhone?.ToString() ?? "");

                // Replace [#19] - Available
                subject = subject.Replace(GetMailTemplateIdentifier("Available"), caseData.Available?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Available"), caseData.Available?.ToString() ?? "");

                // Replace [#20] - Priority_Description
                subject = subject.Replace(GetMailTemplateIdentifier("Priority_Description"), caseData.PriorityDescription?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Priority_Description"), caseData.PriorityDescription?.ToString() ?? "");

                // Replace [#21] - WatchDate
                string watchDate = caseData.WatchDate?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("WatchDate"), watchDate);
                body = body.Replace(GetMailTemplateIdentifier("WatchDate"), watchDate);

                // Replace [#25] - CaseType
                subject = subject.Replace(GetMailTemplateIdentifier("CaseType"), caseData.CaseTypeName?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("CaseType"), caseData.CaseTypeName?.ToString() ?? "");

                // Replace [#26] - Category
                subject = subject.Replace(GetMailTemplateIdentifier("Category"), caseData.CategoryName?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Category"), caseData.CategoryName?.ToString() ?? "");

                // Replace [#27] - ProductArea
                subject = subject.Replace(GetMailTemplateIdentifier("ProductArea"), caseData.ProductAreaName?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("ProductArea"), caseData.ProductAreaName?.ToString() ?? "");

                // Replace [#28] - ReportedBy
                subject = subject.Replace(GetMailTemplateIdentifier("ReportedBy"), caseData.ReportedBy?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("ReportedBy"), caseData.ReportedBy?.ToString() ?? "");

                // Replace [#70] - Performer_Phone
                subject = subject.Replace(GetMailTemplateIdentifier("Performer_Phone"), caseData.PerformerPhone?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Performer_Phone"), caseData.PerformerPhone?.ToString() ?? "");

                // Replace [#71] - Performer_CellPhone
                subject = subject.Replace(GetMailTemplateIdentifier("Performer_CellPhone"), caseData.PerformerCellPhone?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Performer_CellPhone"), caseData.PerformerCellPhone?.ToString() ?? "");

                // Replace [#72] - Performer_Email
                subject = subject.Replace(GetMailTemplateIdentifier("Performer_Email"), caseData.PerformerEMail?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Performer_Email"), caseData.PerformerEMail?.ToString() ?? "");

                // Generate protocol
                string protocol = (int)globalSettings.ServerPort == 443 ? "https" : "http";

                // Process [#98] - Self-service link
                var emailLogGuid = Guid.NewGuid();
                var caseGuid = caseData.CaseGUID?.ToString() ?? "";

                while (body.Contains("[#98]"))
                {
                    int pos1 = body.IndexOf("[#98]");
                    int pos2 = body.IndexOf("[/#98]");

                    if (pos1 >= 0 && pos2 > pos1)
                    {
                        string textToReplace = body.Substring(pos1, pos2 - pos1 + 6);
                        string linkTextSelfService = body.Substring(pos1 + 5, pos2 - pos1 - 5);

                        string linkSelfService;
                        if (Convert.ToString(globalSettings.DBVersion) > "5")
                        {
                            linkSelfService = $"<a href=\"{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}\">{linkTextSelfService}</a>";
                        }
                        else
                        {
                            linkSelfService = $"<a href=\"{protocol}://{globalSettings.ServerName}/CI.asp?Id={caseGuid}\">{linkTextSelfService}</a>";
                        }

                        body = body.Replace(textToReplace, linkSelfService);
                    }
                    else
                    {
                        string linkSelfService;
                        if (Convert.ToString(globalSettings.DBVersion) > "5")
                        {
                            linkSelfService = $"<a href=\"{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}\">{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}</a>";
                        }
                        else
                        {
                            linkSelfService = $"<a href=\"{protocol}://{globalSettings.ServerName}/CI.asp?Id={caseGuid}\">{protocol}://{globalSettings.ServerName}/CI.asp?Id={caseGuid}</a>";
                        }

                        body = body.Replace("[#98]", linkSelfService);
                    }
                }

                // Process [#99] - Helpdesk link
                while (body.Contains("[#99]"))
                {
                    int pos1 = body.IndexOf("[#99]");
                    int pos2 = body.IndexOf("[/#99]");

                    if (pos1 >= 0 && pos2 > pos1)
                    {
                        string textToReplace = body.Substring(pos1, pos2 - pos1 + 6);
                        string linkText = body.Substring(pos1 + 5, pos2 - pos1 - 5);

                        string link;
                        if (Convert.ToString(globalSettings.DBVersion) > "5")
                        {
                            string editCasePath = Convert.ToBoolean(globalSettings.UseMobileRouting)
                                ? "/mobilecases/edit/"
                                : "/Cases/Edit/";

                            link = $"<br><a href=\"{protocol}://{globalSettings.ServerName}{editCasePath}{caseId}\">{linkText}</a>";
                        }
                        else
                        {
                            link = $"<br><a href=\"{protocol}://{globalSettings.ServerName}/Default.asp?GUID={caseGuid}\">{linkText}</a>";
                        }

                        body = body.Replace(textToReplace, link);
                    }
                    else
                    {
                        string link;
                        if (Convert.ToString(globalSettings.DBVersion) > "5")
                        {
                            string editCasePath = Convert.ToBoolean(globalSettings.UseMobileRouting)
                                ? "/mobilecases/edit/"
                                : "/Cases/Edit/";

                            link = $"<br><a href=\"{protocol}://{globalSettings.ServerName}{editCasePath}{caseId}\">{protocol}://{globalSettings.ServerName}{editCasePath}{caseId}</a>";
                        }
                        else
                        {
                            link = $"<br><a href=\"{protocol}://{globalSettings.ServerName}/Default.asp?GUID={caseGuid}\">{protocol}://{globalSettings.ServerName}/Default.asp?GUID={caseGuid}</a>";
                        }

                        body = body.Replace("[#99]", link);
                    }
                }

                // Replace line breaks with <br>
                body = body.Replace("\r\n", "<br>");

                // 7. Create a message ID
                string messageId = await CreateMessageIdAsync(customerData.HelpdeskEMail?.ToString());

                // 8. Get performer email
                var getUserSql = @"
            SELECT Email FROM tblUser 
            WHERE Id = @UserId AND IsActive = 1";

                string performerEmail;
                using (var cmd = new SqlCommand(getUserSql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", performerUserId);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                    {
                        return false; // No valid email
                    }
                    performerEmail = result.ToString();
                    if (string.IsNullOrEmpty(performerEmail))
                    {
                        return false; // No valid email
                    }
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
                NULL, @Body, @Subject, NULL, NULL, 0,
                NULL, @FromAddress, 0, NULL, 0, NULL
            )";

                using (var cmd = new SqlCommand(createEmailLogSql, conn))
                {
                    cmd.Parameters.AddWithValue("@EMailLogGUID", emailLogGuid);
                    cmd.Parameters.AddWithValue("@EmailAddress", performerEmail);
                    cmd.Parameters.AddWithValue("@MailID", mailTemplateId);
                    cmd.Parameters.AddWithValue("@MessageId", messageId);
                    cmd.Parameters.AddWithValue("@CaseHistoryId", historyId);
                    cmd.Parameters.AddWithValue("@Body", body);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@FromAddress", customerData.HelpdeskEMail?.ToString() ?? "noreply@helpdesk.com");

                    await cmd.ExecuteNonQueryAsync();
                }

                return true;
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
