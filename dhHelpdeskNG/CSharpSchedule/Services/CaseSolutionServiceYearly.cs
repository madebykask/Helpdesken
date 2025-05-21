using DH.Helpdesk.CaseSolutionYearly.Models;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Services.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace DH.Helpdesk.CaseSolutionYearly.Services
{
    public class CaseSolutionServiceYearly
    {
        private readonly string _connectionString;
        private readonly ILogger<ScheduleService> _logger;
        private readonly ICaseSolutionService _caseSolutionService;

        public CaseSolutionServiceYearly(IConfiguration configuration, ILogger<ScheduleService> logger, ICaseSolutionService ics)
        {
            _connectionString = configuration.GetConnectionString("Helpdesk");
            _logger = logger;
            _caseSolutionService = ics;
        }

        public async Task<CaseSolutionLite?> GetCaseSolutionAsync(int id)
        {
            var caseSolution = _caseSolutionService.GetCaseSolutionAsync(id);

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
            SELECT CaseSolutionCategory_Id,
                   CaseWorkingGroup_Id,
                   CaseType_Id,
                   Category_Id,
                   Department_Id,
                   Customer_Id,
                   FinishingCause_Id,
                   NoMailToNotifier,
                   PerformerUser_Id,
                   Priority_Id,
                   ProductArea_Id,
                   Project_Id,
                   WorkingGroup_Id,
                   Caption,
                   CaseSolutionName,
                   Description,
                   Miscellaneous,
                   CaseSolutionDescription,
                   ReportedBy,
                   Text_External,
                   Text_Internal,
                   ChangedDate,
                   CreatedDate,
                   TemplatePath,
                   ShowInSelfService,
                   OrderNum,
                   PersonsName,
                   PersonsPhone,
                   PersonsCellPhone,
                   PersonsEmail,
                   UserSearchCategory_Id,
                   Region_Id,
                   OU_Id,
                   Place,
                   UserCode,
                   System_Id,
                   Urgency_Id,
                   Impact_Id,
                   InvoiceNumber,
                   ReferenceNumber,
                   Status_Id,
                   StateSecondary_Id,
                   Verified,
                   VerifiedDescription,
                   SolutionRate,
                   InventoryNumber,
                   InventoryType,
                   InventoryLocation,
                   Supplier_Id,
                   FormGUID,
                   AgreedDate,
                   Available,
                   Cost,
                   OtherCost,
                   Currency,
                   ContactBeforeAction,
                   Problem_Id,
                   Change_Id,
                   WatchDate,
                   FinishingDate,
                   FinishingDescription,
                   SMS,
                   UpdateNotifierInformation,
                   AddFollowersBtn,
                   PlanDate,
                   CausingPartId,
                   RegistrationSource,
                   Status,
                   CostCentre,
                   IsAbout_ReportedBy,
                   IsAbout_PersonsName,
                   IsAbout_PersonsEmail,
                   IsAbout_PersonsPhone,
                   IsAbout_PersonsCellPhone,
                   IsAbout_UserSearchCategory_Id,
                   IsAbout_Region_Id,
                   IsAbout_Department_Id,
                   IsAbout_OU_Id,
                   IsAbout_CostCentre,
                   IsAbout_Place,
                   IsAbout_UserCode,
                   ShowOnCaseOverview,
                   ShowInsideCase,
                   SetCurrentUserAsPerformer,
                   SetCurrentUsersWorkingGroup,
                   OverWritePopUp,
                   ConnectedButton,
                   SaveAndClose,
                   ShortDescription,
                   Information,
                   DefaultTab,
                   AvailableTabsSelfsevice,
                   ActiveTabSelfservice,
                   ValidateOnChange,
                   NextStepState,
                   CaseRelationType,
                   SplitToCaseSolution_Id,
                   ShowOnMobile
            FROM dbo.tblCaseSolution
            WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapCaseSolutionFromReader(reader);
            }

            return null;
        }
        public static CaseSolutionLite MapCaseSolutionFromReader(SqlDataReader reader)
        {
            return new CaseSolutionLite
            {
                CaseSolutionCategory_Id = reader["CaseSolutionCategory_Id"] != DBNull.Value ? (int?)reader["CaseSolutionCategory_Id"] : null,
                CaseWorkingGroup_Id = reader["CaseWorkingGroup_Id"] != DBNull.Value ? (int?)reader["CaseWorkingGroup_Id"] : null,
                CaseType_Id = reader["CaseType_Id"] != DBNull.Value ? (int?)reader["CaseType_Id"] : null,
                Category_Id = reader["Category_Id"] != DBNull.Value ? (int?)reader["Category_Id"] : null,
                Department_Id = reader["Department_Id"] != DBNull.Value ? (int?)reader["Department_Id"] : null,
                Customer_Id = (int)reader["Customer_Id"],
                FinishingCause_Id = reader["FinishingCause_Id"] != DBNull.Value ? (int?)reader["FinishingCause_Id"] : null,
                NoMailToNotifier = (int)reader["NoMailToNotifier"],
                PerformerUser_Id = reader["PerformerUser_Id"] != DBNull.Value ? (int?)reader["PerformerUser_Id"] : null,
                Priority_Id = reader["Priority_Id"] != DBNull.Value ? (int?)reader["Priority_Id"] : null,
                ProductArea_Id = reader["ProductArea_Id"] != DBNull.Value ? (int?)reader["ProductArea_Id"] : null,
                Project_Id = reader["Project_Id"] != DBNull.Value ? (int?)reader["Project_Id"] : null,
                WorkingGroup_Id = reader["WorkingGroup_Id"] != DBNull.Value ? (int?)reader["WorkingGroup_Id"] : null,
                Caption = reader["Caption"] as string,
                CaseSolutionName = reader["CaseSolutionName"] as string,
                Description = reader["Description"] as string,
                Miscellaneous = reader["Miscellaneous"] as string,
                CaseSolutionDescription = reader["CaseSolutionDescription"] as string,
                ReportedBy = reader["ReportedBy"] as string,
                Text_External = reader["Text_External"] as string,
                Text_Internal = reader["Text_Internal"] as string,
                ChangedDate = (DateTime)reader["ChangedDate"],
                CreatedDate = (DateTime)reader["CreatedDate"],
                TemplatePath = reader["TemplatePath"] as string,
                ShowInSelfService = (bool)reader["ShowInSelfService"],
                OrderNum = reader["OrderNum"] != DBNull.Value ? (int?)reader["OrderNum"] : null,
                PersonsName = reader["PersonsName"] as string,
                PersonsPhone = reader["PersonsPhone"] as string,
                PersonsCellPhone = reader["PersonsCellPhone"] as string,
                PersonsEmail = reader["PersonsEmail"] as string,
                UserSearchCategory_Id = reader["UserSearchCategory_Id"] != DBNull.Value ? (int?)reader["UserSearchCategory_Id"] : null,
                Region_Id = reader["Region_Id"] != DBNull.Value ? (int?)reader["Region_Id"] : null,
                OU_Id = reader["OU_Id"] != DBNull.Value ? (int?)reader["OU_Id"] : null,
                Place = reader["Place"] as string,
                UserCode = reader["UserCode"] as string,
                System_Id = reader["System_Id"] != DBNull.Value ? (int?)reader["System_Id"] : null,
                Urgency_Id = reader["Urgency_Id"] != DBNull.Value ? (int?)reader["Urgency_Id"] : null,
                Impact_Id = reader["Impact_Id"] != DBNull.Value ? (int?)reader["Impact_Id"] : null,
                InvoiceNumber = reader["InvoiceNumber"] as string,
                ReferenceNumber = reader["ReferenceNumber"] as string,
                Status_Id = reader["Status_Id"] != DBNull.Value ? (int?)reader["Status_Id"] : null,
                StateSecondary_Id = reader["StateSecondary_Id"] != DBNull.Value ? (int?)reader["StateSecondary_Id"] : null,
                Verified = (int)reader["Verified"],
                VerifiedDescription = reader["VerifiedDescription"] as string,
                SolutionRate = reader["SolutionRate"] as string,
                InventoryNumber = reader["InventoryNumber"] as string,
                InventoryType = reader["InventoryType"] as string,
                InventoryLocation = reader["InventoryLocation"] as string,
                Supplier_Id = reader["Supplier_Id"] != DBNull.Value ? (int?)reader["Supplier_Id"] : null,
                FormGUID = reader["FormGUID"] != DBNull.Value ? (Guid?)reader["FormGUID"] : null,
                AgreedDate = reader["AgreedDate"] != DBNull.Value ? (DateTime?)reader["AgreedDate"] : null,
                Available = reader["Available"] as string,
                Cost = (int)reader["Cost"],
                OtherCost = (int)reader["OtherCost"],
                Currency = reader["Currency"] as string,
                ContactBeforeAction = (int)reader["ContactBeforeAction"],
                Problem_Id = reader["Problem_Id"] != DBNull.Value ? (int?)reader["Problem_Id"] : null,
                Change_Id = reader["Change_Id"] != DBNull.Value ? (int?)reader["Change_Id"] : null,
                WatchDate = reader["WatchDate"] != DBNull.Value ? (DateTime?)reader["WatchDate"] : null,
                FinishingDate = reader["FinishingDate"] != DBNull.Value ? (DateTime?)reader["FinishingDate"] : null,
                FinishingDescription = reader["FinishingDescription"] as string,
                SMS = (int)reader["SMS"],
                UpdateNotifierInformation = reader["UpdateNotifierInformation"] != DBNull.Value ? (int?)reader["UpdateNotifierInformation"] : null,
                AddFollowersBtn = reader["AddFollowersBtn"] != DBNull.Value ? (bool?)reader["AddFollowersBtn"] : null,
                PlanDate = reader["PlanDate"] != DBNull.Value ? (DateTime?)reader["PlanDate"] : null,
                CausingPartId = reader["CausingPartId"] != DBNull.Value ? (int?)reader["CausingPartId"] : null,
                RegistrationSource = reader["RegistrationSource"] != DBNull.Value ? (int?)reader["RegistrationSource"] : null,
                Status = (int)reader["Status"],
                CostCentre = reader["CostCentre"] as string,
                IsAbout_ReportedBy = reader["IsAbout_ReportedBy"] as string,
                IsAbout_PersonsName = reader["IsAbout_PersonsName"] as string,
                IsAbout_PersonsEmail = reader["IsAbout_PersonsEmail"] as string,
                IsAbout_PersonsPhone = reader["IsAbout_PersonsPhone"] as string,
                IsAbout_PersonsCellPhone = reader["IsAbout_PersonsCellPhone"] as string,
                IsAbout_UserSearchCategory_Id = reader["IsAbout_UserSearchCategory_Id"] != DBNull.Value ? (int?)reader["IsAbout_UserSearchCategory_Id"] : null,
                IsAbout_Region_Id = reader["IsAbout_Region_Id"] != DBNull.Value ? (int?)reader["IsAbout_Region_Id"] : null,
                IsAbout_Department_Id = reader["IsAbout_Department_Id"] != DBNull.Value ? (int?)reader["IsAbout_Department_Id"] : null,
                IsAbout_OU_Id = reader["IsAbout_OU_Id"] != DBNull.Value ? (int?)reader["IsAbout_OU_Id"] : null,
                IsAbout_CostCentre = reader["IsAbout_CostCentre"] as string,
                IsAbout_Place = reader["IsAbout_Place"] as string,
                IsAbout_UserCode = reader["IsAbout_UserCode"] as string,
                ShowOnCaseOverview = (int)reader["ShowOnCaseOverview"],
                ShowInsideCase = (int)reader["ShowInsideCase"],
                SetCurrentUserAsPerformer = reader["SetCurrentUserAsPerformer"] != DBNull.Value ? (int?)reader["SetCurrentUserAsPerformer"] : null,
                SetCurrentUsersWorkingGroup = reader["SetCurrentUsersWorkingGroup"] != DBNull.Value ? (int?)reader["SetCurrentUsersWorkingGroup"] : null,
                OverWritePopUp = (int)reader["OverWritePopUp"],
                ConnectedButton = reader["ConnectedButton"] != DBNull.Value ? (int?)reader["ConnectedButton"] : null,
                SaveAndClose = reader["SaveAndClose"] != DBNull.Value ? (int?)reader["SaveAndClose"] : null,
                ShortDescription = reader["ShortDescription"] as string,
                Information = reader["Information"] as string,
                DefaultTab = reader["DefaultTab"] as string,
                AvailableTabsSelfsevice = reader["AvailableTabsSelfsevice"] as string,
                ActiveTabSelfservice = reader["ActiveTabSelfservice"] as string,
                ValidateOnChange = reader["ValidateOnChange"] as string,
                NextStepState = reader["NextStepState"] != DBNull.Value ? (int?)reader["NextStepState"] : null,
                CaseRelationType = (int)reader["CaseRelationType"],
                SplitToCaseSolution_Id = reader["SplitToCaseSolution_Id"] != DBNull.Value ? (int?)reader["SplitToCaseSolution_Id"] : null,
                ShowOnMobile = (int)reader["ShowOnMobile"]
            };
        }

    }

}
