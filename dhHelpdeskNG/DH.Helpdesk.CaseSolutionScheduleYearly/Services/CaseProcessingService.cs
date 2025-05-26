using DH.Helpdesk.BusinessData.Enums.Email;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
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
        private readonly ICaseService _caseService;

        public CaseProcessingService(string connection, MailTemplateService mailTemplateService, ICaseService caseService)
        {
            _connectionString = connection;
            _mailTemplateService = mailTemplateService;
            _caseService = caseService;
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
                            //Get the full case data to use in the next steps
                            Case caseCreated = await _caseService.GetCaseByIdAsync(newCaseId);
                            // 2. Create isAbout data if needed
                            if (!string.IsNullOrWhiteSpace(c.IsAbout_PersonsEmail))
                            {
                                await SaveIsAboutAsync(newCaseId, c);
                            }
                            // 3. Create case history
                            var historyId = await SaveCaseHistoryAsync(newCaseId, caseCreated);
                            // 4. Log internal/external texts if needed
                            if (!string.IsNullOrWhiteSpace(c.Text_Internal) || !string.IsNullOrWhiteSpace(c.Text_External))
                            {
                                await SaveLogAsync(newCaseId, c, historyId);
                            }
                            if (extendedCaseFormId.HasValue && extendedCaseFormId.Value > 0)
                            {
                                var extendedCaseDataId = await CreateExtendedCaseDataRowAsync(extendedCaseFormId.Value);
                                await CreateExtendedCaseConnectionAsync(newCaseId, extendedCaseFormId.Value, extendedCaseDataId);
                            }

                            if (c.PerformerUser_Id.HasValue && c.PerformerUser_Id.Value > 0)
                            {
                                await SendMailToPerformerAsync(caseCreated, c.PerformerUser_Id.Value, historyId);
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
                        Case_Id, ReportedBy, Person_Name, Person_Email, Person_Phone, Person_CellPhone,
                        Region_Id, Department_Id, OU_Id, CostCentre, Place, UserCode
                    )
                    VALUES (
                        @Case_Id, @ReportedBy, @Person_Name, @Person_Email, @Person_Phone, @Person_CellPhone,
                        @Region_Id, @Department_Id, @OU_Id, @CostCentre, @Place, @UserCode
                    );";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    object SqlVal<T>(T val) => EqualityComparer<T>.Default.Equals(val, default(T)) ? DBNull.Value : (object)val;
                    object SqlStr(string s) => s ?? "";

                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@ReportedBy", SqlStr(caseSolution.IsAbout_ReportedBy));
                    cmd.Parameters.AddWithValue("@Person_Name", SqlStr(caseSolution.IsAbout_PersonsName));
                    cmd.Parameters.AddWithValue("@Person_Email", SqlStr(caseSolution.IsAbout_PersonsEmail));
                    cmd.Parameters.AddWithValue("@Person_Phone", SqlStr(caseSolution.IsAbout_PersonsPhone));
                    cmd.Parameters.AddWithValue("@Person_CellPhone", SqlStr(caseSolution.IsAbout_PersonsCellPhone));
                    cmd.Parameters.AddWithValue("@Region_Id", SqlVal(caseSolution.IsAbout_Region_Id));
                    cmd.Parameters.AddWithValue("@Department_Id", SqlVal(caseSolution.IsAbout_Department_Id));
                    cmd.Parameters.AddWithValue("@OU_Id", SqlVal(caseSolution.IsAbout_OU_Id));
                    cmd.Parameters.AddWithValue("@CostCentre", SqlStr(caseSolution.IsAbout_CostCentre));
                    cmd.Parameters.AddWithValue("@Place", SqlStr(caseSolution.IsAbout_Place));
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
        //Todo - Kolla alla värden här
        public async Task<int> SaveCaseHistoryAsync(int caseId, Case c)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                int customerId = c.Customer_Id;
                int caseTypeId = c.CaseType_Id;
                string caption = "";
                string description = "";

                var sql = @"
            INSERT INTO tblCaseHistory (
                CaseHistoryGUID, Case_Id, CreatedDate, CreatedByUser,
                Persons_Name, Persons_EMail, Place, InventoryType, 
                InventoryLocation, Casenumber, IPAddress, CaseType_Id,
                InvoiceNumber, Caption, Description, Miscellaneous,
                ContactBeforeAction, SMS, Available, Cost, OtherCost,
                ExternalTime, RegistrationSource, RelatedCaseNumber,
                Deleted, Status, RegLanguage_Id, LeadTime, Customer_Id,
                CaseExtraFollowers, ActionLeadTime, ActionExternalTime
            )
            VALUES (
                @CaseHistoryGUID, @Case_Id, GETUTCDATE(), @CreatedByUser,
                @PersonsName, @PersonsEmail, @Place, @InventoryType,
                @InventoryLocation, @Casenumber, @IPAddress, @CaseType_Id,
                @InvoiceNumber, @Caption, @Description, @Miscellaneous,
                @ContactBeforeAction, @SMS, @Available, @Cost, @OtherCost,
                @ExternalTime, @RegistrationSource, @RelatedCaseNumber,
                @Deleted, @Status, @RegLanguage_Id, @LeadTime, @Customer_Id,
                @CaseExtraFollowers, @ActionLeadTime, @ActionExternalTime
            );
            SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    var caseHistoryGuid = Guid.NewGuid();

                    cmd.Parameters.AddWithValue("@CaseHistoryGUID", caseHistoryGuid);
                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@CreatedByUser", "");
                    cmd.Parameters.AddWithValue("@PersonsName", ""); // Using createdByUser as the person's name
                    cmd.Parameters.AddWithValue("@PersonsEmail", ""); // Required field
                    cmd.Parameters.AddWithValue("@Place", "System");
                    cmd.Parameters.AddWithValue("@InventoryType", "");
                    cmd.Parameters.AddWithValue("@InventoryLocation", "");
                    cmd.Parameters.AddWithValue("@Casenumber", c.CaseNumber); // Using the case ID as casenumber
                    cmd.Parameters.AddWithValue("@IPAddress", "127.0.0.1");
                    cmd.Parameters.AddWithValue("@CaseType_Id", caseTypeId);
                    cmd.Parameters.AddWithValue("@InvoiceNumber", "");
                    cmd.Parameters.AddWithValue("@Caption", caption);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Miscellaneous", "");
                    cmd.Parameters.AddWithValue("@ContactBeforeAction", 0);
                    cmd.Parameters.AddWithValue("@SMS", 0);
                    cmd.Parameters.AddWithValue("@Available", "");
                    cmd.Parameters.AddWithValue("@Cost", 0);
                    cmd.Parameters.AddWithValue("@OtherCost", 0);
                    cmd.Parameters.AddWithValue("@ExternalTime", 0);
                    cmd.Parameters.AddWithValue("@RegistrationSource", 0);
                    cmd.Parameters.AddWithValue("@RelatedCaseNumber", 0);
                    cmd.Parameters.AddWithValue("@Deleted", 0);
                    cmd.Parameters.AddWithValue("@Status", 0);
                    cmd.Parameters.AddWithValue("@RegLanguage_Id", 1); // Default to Swedish
                    cmd.Parameters.AddWithValue("@LeadTime", 0);
                    cmd.Parameters.AddWithValue("@Customer_Id", customerId);
                    cmd.Parameters.AddWithValue("@CaseExtraFollowers", "");
                    cmd.Parameters.AddWithValue("@ActionLeadTime", 0);
                    cmd.Parameters.AddWithValue("@ActionExternalTime", 0);

                    var result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
        private async Task<int> CreateExtendedCaseDataRowAsync(int extendedCaseFormId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
            INSERT INTO ExtendedCaseData (ExtendedCaseFormId, CreatedOn, CreatedBy)
            VALUES (@ExtendedCaseFormId, GETUTCDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(sql, conn))
                {
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
            INSERT INTO tblCase_ExtendedCaseData (
                case_Id, ExtendedCaseData_Id, ExtendedCaseForm_Id
            )
            VALUES (
                @Case_Id, @ExtendedCaseData_Id, @ExtendedCaseForm_Id
            );";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@ExtendedCaseData_Id", extendedCaseDataId);
                    cmd.Parameters.AddWithValue("@ExtendedCaseForm_Id", extendedCaseFormId);

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

        private async Task<bool> SendMailToPerformerAsync(Case caseData, int performerUserId, int historyId)
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
                var mailTemplate = await _mailTemplateService.GetMailTemplateByIdAsync(mailTemplateId, caseData.Customer_Id, caseData.RegLanguage_Id, _connectionString);

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
                subject = subject.Replace(GetMailTemplateIdentifier("CaseNumber"), caseNumber);
                body = body.Replace(GetMailTemplateIdentifier("CaseNumber"), caseNumber);

                // [#2] - CustomerName
                subject = subject.Replace(GetMailTemplateIdentifier("CustomerName"), customerData.Name?.ToString() ?? "");
                body = body.Replace(GetMailTemplateIdentifier("CustomerName"), customerData.Name?.ToString() ?? "");

                // [#3] - Persons_Name
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_Name"), caseData.PersonsName ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_Name"), caseData.PersonsName ?? "");

                // [#4] - Caption
                subject = subject.Replace(GetMailTemplateIdentifier("Caption"), caseData.Caption ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Caption"), caseData.Caption ?? "");

                // [#5] - Description
                string description = caseData.Description ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("Description"), description);
                body = body.Replace(GetMailTemplateIdentifier("Description"), description.Replace("\r\n", "<br>"));

                // [#6] - FirstName (Performer)
                string performerFirstName = performerData?.FirstName?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("FirstName"), performerFirstName);
                body = body.Replace(GetMailTemplateIdentifier("FirstName"), performerFirstName);

                // [#7] - SurName (Performer)
                string performerSurName = performerData?.SurName?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("SurName"), performerSurName);
                body = body.Replace(GetMailTemplateIdentifier("SurName"), performerSurName);

                // [#8] - Persons_EMail
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_EMail"), caseData.PersonsEmail ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_EMail"), caseData.PersonsEmail ?? "");

                // [#9] - Persons_Phone
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_Phone"), caseData.PersonsPhone ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_Phone"), caseData.PersonsPhone ?? "");

                // Get the most recent log data if needed (for Text_External and Text_Internal)
                // You might need to retrieve this from the database or pass it in as a parameter
                string textExternal = ""; // Get from database or parameter if available
                string textInternal = ""; // Get from database or parameter if available

                // [#10] - Text_External
                subject = subject.Replace(GetMailTemplateIdentifier("Text_External"), textExternal);
                body = body.Replace(GetMailTemplateIdentifier("Text_External"), textExternal);

                // [#11] - Text_Internal
                subject = subject.Replace(GetMailTemplateIdentifier("Text_Internal"), textInternal);
                body = body.Replace(GetMailTemplateIdentifier("Text_Internal"), textInternal);

                // [#12] - PriorityName
                string priorityName = priorityData?.PriorityName?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("PriorityName"), priorityName);
                body = body.Replace(GetMailTemplateIdentifier("PriorityName"), priorityName);

                // [#13] - WorkingGroupEMail
                string workingGroupEmail = workingGroupData?.WorkingGroupEMail?.ToString() ?? "";
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

                // [#15] - WorkingGroup
                string workingGroupName = workingGroupData?.WorkingGroup?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("WorkingGroup"), workingGroupName);
                body = body.Replace(GetMailTemplateIdentifier("WorkingGroup"), workingGroupName);

                // [#16] - RegTime
                string regTime = caseData.RegTime.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("RegTime"), regTime);
                body = body.Replace(GetMailTemplateIdentifier("RegTime"), regTime);

                // [#17] - InventoryNumber
                subject = subject.Replace(GetMailTemplateIdentifier("InventoryNumber"), caseData.InventoryNumber ?? "");
                body = body.Replace(GetMailTemplateIdentifier("InventoryNumber"), caseData.InventoryNumber ?? "");

                // [#18] - Persons_CellPhone
                subject = subject.Replace(GetMailTemplateIdentifier("Persons_CellPhone"), caseData.PersonsCellphone ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Persons_CellPhone"), caseData.PersonsCellphone ?? "");

                // [#19] - Available
                subject = subject.Replace(GetMailTemplateIdentifier("Available"), caseData.Available ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Available"), caseData.Available ?? "");

                // [#20] - Priority_Description
                string priorityDescription = priorityData?.PriorityDescription?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("Priority_Description"), priorityDescription);
                body = body.Replace(GetMailTemplateIdentifier("Priority_Description"), priorityDescription);

                // [#21] - WatchDate
                string watchDate = caseData.WatchDate?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("WatchDate"), watchDate);
                body = body.Replace(GetMailTemplateIdentifier("WatchDate"), watchDate);

                //// [#22] - LastChangedByUser
                //string lastChangedByUser = (caseData.ChangedName ?? "") + " " + (caseData.ChangedSurName ?? "");
                //subject = subject.Replace(GetMailTemplateIdentifier("LastChangedByUser"), lastChangedByUser);
                //body = body.Replace(GetMailTemplateIdentifier("LastChangedByUser"), lastChangedByUser);

                // [#23] - Miscellaneous
                subject = subject.Replace(GetMailTemplateIdentifier("Miscellaneous"), caseData.Miscellaneous ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Miscellaneous"), caseData.Miscellaneous ?? "");

                // [#24] - Place
                subject = subject.Replace(GetMailTemplateIdentifier("Place"), caseData.Place ?? "");
                body = body.Replace(GetMailTemplateIdentifier("Place"), caseData.Place ?? "");

                // [#25] - CaseType
                string caseTypeName = caseTypeData?.CaseType?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("CaseType"), caseTypeName);
                body = body.Replace(GetMailTemplateIdentifier("CaseType"), caseTypeName);

                // [#26] - Category
                string categoryName = categoryData?.Category?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("Category"), categoryName);
                body = body.Replace(GetMailTemplateIdentifier("Category"), categoryName);

                // [#27] - ProductArea
                string productAreaName = productAreaData?.ProductArea?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("ProductArea"), productAreaName);
                body = body.Replace(GetMailTemplateIdentifier("ProductArea"), productAreaName);

                // [#28] - ReportedBy
                subject = subject.Replace(GetMailTemplateIdentifier("ReportedBy"), caseData.ReportedBy ?? "");
                body = body.Replace(GetMailTemplateIdentifier("ReportedBy"), caseData.ReportedBy ?? "");

                // [#29] - RegUser
                subject = subject.Replace(GetMailTemplateIdentifier("RegUser"), caseData.RegUserName ?? "");
                body = body.Replace(GetMailTemplateIdentifier("RegUser"), caseData.RegUserName ?? "");

                // [#70] - Performer_Phone
                string performerPhone = performerData?.Phone?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("Performer_Phone"), performerPhone);
                body = body.Replace(GetMailTemplateIdentifier("Performer_Phone"), performerPhone);

                // [#71] - Performer_CellPhone
                string performerCellPhone = performerData?.CellPhone?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("Performer_CellPhone"), performerCellPhone);
                body = body.Replace(GetMailTemplateIdentifier("Performer_CellPhone"), performerCellPhone);

                // [#72] - Performer_Email
                string performerEmail = performerData?.Email?.ToString() ?? "";
                subject = subject.Replace(GetMailTemplateIdentifier("Performer_Email"), performerEmail);
                body = body.Replace(GetMailTemplateIdentifier("Performer_Email"), performerEmail);

                // [#73] - IsAbout_PersonsName
                subject = subject.Replace(GetMailTemplateIdentifier("IsAbout_PersonsName"), caseData.IsAbout_PersonsName ?? "");
                body = body.Replace(GetMailTemplateIdentifier("IsAbout_PersonsName"), caseData.IsAbout_PersonsName ?? "");

                //// [#80] - AutoCloseDays
                //subject = subject.Replace(GetMailTemplateIdentifier("AutoCloseDays"), caseData.AutoCloseDays?.ToString() ?? "");
                //body = body.Replace(GetMailTemplateIdentifier("AutoCloseDays"), caseData.AutoCloseDays?.ToString() ?? "");


                // Continue with the rest of your existing code...
                // Generate protocol
                string protocol = (int)globalSettings.ServerPort == 443 ? "https" : "http";

                // Process [#98] - Self-service link
                var emailLogGuid = Guid.NewGuid();
                var caseGuid = caseData.CaseGUID.ToString();

                // Continue with the rest of your existing URL handling code...
                // Process [#98] - Self-service link
                while (body.Contains("[#98]"))
                {
                    int pos1 = body.IndexOf("[#98]");
                    int pos2 = body.IndexOf("[/#98]");

                    if (pos1 >= 0 && pos2 > pos1)
                    {
                        string textToReplace = body.Substring(pos1, pos2 - pos1 + 6);
                        string linkTextSelfService = body.Substring(pos1 + 5, pos2 - pos1 - 5);

                        string linkSelfService;
                        linkSelfService = $"<a href=\"{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}\">{linkTextSelfService}</a>";

                        body = body.Replace(textToReplace, linkSelfService);
                    }
                    else
                    {
                        string linkSelfService;
                        linkSelfService = $"<a href=\"{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}\">{protocol}://{globalSettings.ExternalSite}/case/index/{emailLogGuid}</a>";

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
                        string editCasePath = Convert.ToBoolean(globalSettings.UseMobileRouting)
                                 ? CasePaths.EDIT_CASE_MOBILEROUTE
                                 : CasePaths.EDIT_CASE_DESKTOP;

                        link = $"<br><a href=\"{protocol}://{globalSettings.ServerName}{editCasePath}{caseData.Id}\">{linkText}</a>";

                        body = body.Replace(textToReplace, link);
                    }
                    else
                    {
                        string link;
                        string editCasePath = Convert.ToBoolean(globalSettings.UseMobileRouting)
                                ? CasePaths.EDIT_CASE_MOBILEROUTE
                                 : CasePaths.EDIT_CASE_DESKTOP;

                        link = $"<br><a href=\"{protocol}://{globalSettings.ServerName}{editCasePath}{caseData.Id}\">{protocol}://{globalSettings.ServerName}{editCasePath}{caseData.Id}</a>";

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

        private string GetMailTemplateIdentifier(string identifier)
        {
            // Make case-insensitive like the VB version by converting to uppercase
            switch (identifier.ToUpperInvariant())
            {
                case "CASENUMBER": return "[#1]";
                case "CUSTOMERNAME": return "[#2]";
                case "PERSONS_NAME": return "[#3]";
                case "CAPTION": return "[#4]";
                case "DESCRIPTION": return "[#5]";
                case "FIRSTNAME": return "[#6]";
                case "SURNAME": return "[#7]";
                case "PERSONS_EMAIL": return "[#8]";
                case "PERSONS_PHONE": return "[#9]";
                case "TEXT_EXTERNAL": return "[#10]";
                case "TEXT_INTERNAL": return "[#11]";
                case "PRIORITYNAME": return "[#12]";
                case "WORKINGGROUPEMAIL": return "[#13]";
                case "WORKINGGROUP": return "[#15]";
                case "REGTIME": return "[#16]";
                case "INVENTORYNUMBER": return "[#17]";
                case "PERSONS_CELLPHONE": return "[#18]";
                case "AVAILABLE": return "[#19]";
                case "PRIORITY_DESCRIPTION": return "[#20]";
                case "WATCHDATE": return "[#21]";
                case "LASTCHANGEDBYUSER": return "[#22]"; // This was "ChangeTime" before - fixed!
                case "MISCELLANEOUS": return "[#23]";
                case "PLACE": return "[#24]";
                case "CASETYPE": return "[#25]";
                case "CATEGORY": return "[#26]";
                case "PRODUCTAREA": return "[#27]";
                case "REPORTEDBY": return "[#28]";
                case "REGUSER": return "[#29]";
                case "PERFORMER_PHONE": return "[#70]";
                case "PERFORMER_CELLPHONE": return "[#71]";
                case "PERFORMER_EMAIL": return "[#72]";
                case "ISABOUT_PERSONSNAME": return "[#73]";
                case "AUTOCLOSEDAYS": return "[#80]";
                default: return ""; // Return empty string for unknown identifiers like in VB
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
