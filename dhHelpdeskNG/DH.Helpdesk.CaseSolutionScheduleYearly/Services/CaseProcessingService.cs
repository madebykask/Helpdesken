using DH.Helpdesk.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Services
{
    internal class CaseProcessingService
    {
        private readonly string _connectionString;

        public CaseProcessingService(string connection)
        {
            _connectionString = connection;
        }

        public async Task<int?> CreateCaseAsync(CaseSolution c)
        {
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

                                // Return the new case ID
                                return newCaseId;
                            }
                            return null;
                        }
                        /*
                        // 3. Lägg till case history
                        var historyId = await SaveCaseHistoryAsync(newCaseId, "Scheduled Job");

                       // 4. Logga interna/externa texter
                       if (!string.IsNullOrWhiteSpace(caseSolution.Text_Internal) || !string.IsNullOrWhiteSpace(caseSolution.Text_External))
                       {
                           await SaveLogAsync(newCaseId, caseSolution, historyId);
                       }

                       // 5. Skicka mejl till utförare (om finns)
                       if (caseSolution.PerformerUser_Id.HasValue)
                       {
                           await SendMailToPerformerAsync(newCaseId, caseSolution.PerformerUser_Id.Value);
                       }

                       // 6. Extended case form?
                       if (caseSolution.ExtendedCaseFormId.HasValue)
                       {
                           await CreateExtendedCaseConnectionAsync(newCaseId, caseSolution.ExtendedCaseFormId.Value);
                       }

                       _logger.LogInformation("Created case for CaseSolution_Id={0}, Case_Id={1}", caseSolution.Id, newCaseId);
               */
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
            INSERT INTO tblCaseIsAbout (Case_Id, IsAbout_UserCode, PersonsEmail)
            VALUES (@Case_Id, @IsAbout_UserCode, @PersonsEmail);
        ";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Case_Id", caseId);
                    cmd.Parameters.AddWithValue("@IsAbout_UserCode", caseSolution.IsAbout_UserCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PersonsEmail", caseSolution.PersonsEmail ?? (object)DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
