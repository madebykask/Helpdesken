using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.VarA;
namespace DH.Helpdesk.CaseSolutionScheduleYearly.Services
{
    internal class CaseProcessingService
    {
        private readonly string _connectionString;
        private readonly MailTemplateService _mailTemplateService;
        private readonly ICaseService _caseService;
        private readonly ICustomerService _customerService;

        public CaseProcessingService(string connection, MailTemplateService mailTemplateService, ICaseService caseService, ICustomerService customerService)
        {
            _connectionString = connection;
            _mailTemplateService = mailTemplateService;
            _caseService = caseService;
            _customerService = customerService;
        }

        public async Task<int?> CreateCaseAsync(CaseSolution c)
        {
            // First, check if there's an extended case form connected to this case solution
            var extendedCaseFormId = await GetExtendedCaseFormIdForCaseSolutionAsync(c.Id);
            var customer = _customerService.GetCustomer(c.Customer_Id);
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
                    WatchDate, PlanDate, AgreedDate, FinishingDate, RegistrationSource, RegLanguage_Id, 
                    RegUserName, CaseSolution_Id, RegTime, ChangeTime
                )
                VALUES (
                    @CaseGUID, @ExternalCaseNumber, @CaseType_Id, @Customer_Id, @ProductArea_Id, @Category_Id, @Region_Id, @ReportedBy,
                    @Department_Id, @OU_Id, @Project_Id, @System_Id, @Urgency_Id, @Impact_Id, @Supplier_Id, @SMS, @Cost, @OtherCost,
                    @Problem_Id, @Change_Id, @CausingPartId, @Verified, @VerifiedDescription, @SolutionRate, @InventoryType, @InventoryLocation,
                    @Currency, @ContactBeforeAction, @FinishingDescription, @Persons_Name, @Persons_EMail, @Persons_Phone, @Persons_CellPhone,
                    @Place, @UserCode, @CostCentre, @InventoryNumber, @InvoiceNumber, @Caption, @Description, @Miscellaneous, @Available,
                    @ReferenceNumber, @Priority_Id, @WorkingGroup_Id, @Performer_User_Id, @Status_Id, @StateSecondary_Id,
                    @WatchDate, @PlanDate, @AgreedDate, @FinishingDate, @RegistrationSource, @RegLanguage_Id, 
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
                    cmd.Parameters.AddWithValue("@RegistrationSource", 3);
                    cmd.Parameters.AddWithValue("@RegLanguage_Id", customer.Language_Id);
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
                                await _mailTemplateService.SendMailToPerformerAsync(caseCreated, c.PerformerUser_Id.Value, historyId);
                            }
                            return newCaseId;
                        }
                        throw new Exception($"Failed to create case for CaseSolution_Id: {c.Id}. Database returned no ID.");
                    }

                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to create case for CaseSolution_Id: {c.Id}", ex);
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
                    cmd.Parameters.AddWithValue("@PersonsName", ""); 
                    cmd.Parameters.AddWithValue("@PersonsEmail", ""); 
                    cmd.Parameters.AddWithValue("@Place", "System");
                    cmd.Parameters.AddWithValue("@InventoryType", "");
                    cmd.Parameters.AddWithValue("@InventoryLocation", "");
                    cmd.Parameters.AddWithValue("@Casenumber", c.CaseNumber); 
                    cmd.Parameters.AddWithValue("@IPAddress", ""); //Sätter till tomt, kan ändras om det behövs
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
                    cmd.Parameters.AddWithValue("@RegistrationSource", 3); 
                    cmd.Parameters.AddWithValue("@RelatedCaseNumber", 0);
                    cmd.Parameters.AddWithValue("@Deleted", 0);
                    cmd.Parameters.AddWithValue("@Status", 0);
                    cmd.Parameters.AddWithValue("@RegLanguage_Id", c.RegLanguage_Id); 
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
    }
}
