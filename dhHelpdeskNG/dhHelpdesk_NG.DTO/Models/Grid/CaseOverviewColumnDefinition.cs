namespace DH.Helpdesk.BusinessData.Models.Grid
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Case.Fields;

    /// <summary>
    /// Here we will store relations between field id and field name to know what settings is applied for what field case GridSettingsEntity.FieldId.
    /// I used this sql to fetch all availavble columns:
    /// >> select distinct '{,"' + CaseField + '"},' from tblcasefieldsettingsorder order by 1;
    /// ACHTUNG!! if you delete something from this class then put checks to all plases where you use this maping. 
    /// Now this is at least CaseOverviewToGridColumnDefMapper and GridSettingsService classes. Otherwise you could get an "key not found" exception when will process data from DB
    /// </summary>
    public partial class GridColumnsDefinition
    {
        #region "Case overview" grid

        private static readonly string[] CaseOverviewColumns = {
            CaseInfoFields.AgreedDate,
            CaseInfoFields.Available,
            CaseInfoFields.Caption,
            CaseInfoFields.Case,
            OtherFields.Responsible,
            CaseInfoFields.CaseType,
            CaseInfoFields.Category,
            OtherFields.CausingPart,
            CaseInfoFields.ChangeDate,
            LogFields.FinishingCause,
            ComputerFields.ComputerType,
            CaseInfoFields.PhoneContact,
            CaseInfoFields.Cost,
            UserFields.CostCentre,
            UserFields.Customer,
            UserFields.Department,
            CaseInfoFields.Description,
            CaseInfoFields.AttachedFile,
            "FinishingCause_Id",
            LogFields.FinishingDate,
            LogFields.FinishingDescription,
            CaseInfoFields.Impact,
            ComputerFields.Place,
            ComputerFields.PcNumber,
            CaseInfoFields.InvoiceNumber,
            CaseInfoFields.Other,
            UserFields.Unit,
            OtherFields.Administrator,
            UserFields.CellPhone,
            UserFields.Email,
            UserFields.Notifier,
            UserFields.Phone,
            UserFields.Place,
            OtherFields.PlannedActionDate,
            OtherFields.Priority,
            CaseInfoFields.ProductArea,
            CaseInfoFields.ReferenceNumber,
            UserFields.Region,
            CaseInfoFields.RegistrationDate,
            UserFields.User,
            CaseInfoFields.Sms,
            OtherFields.SolutionRate,
            OtherFields.SubState,
            OtherFields.State,
            CaseInfoFields.Supplier,
            CaseInfoFields.System,
            LogFields.Debiting,
            LogFields.AttachedFile,
            LogFields.ExternalLogNote,
            LogFields.InternalLogNote,
            "TMDDate",
            CaseInfoFields.UrgentDegree,
            CaseInfoFields.RegisteredBy,
            UserFields.OrdererCode,
            OtherFields.WatchDate,
            OtherFields.Verified,
            OtherFields.VerifiedDescription,
            OtherFields.WorkingGroup,
            CaseInfoFields.RegistrationSourceCustomer,
            
            UserFields.IsAbout_User,
            UserFields.IsAbout_Persons_Name,
            UserFields.IsAbout_Persons_CellPhone,
            OtherFields.Problem,
            OtherFields.Project  
        };

        #endregion

        #region Case connect to parent grid

        /// <summary>
        /// List of fields for case connect to parent grid
        /// </summary>
        public static readonly string[] CaseConnectToParentColumns =
        {
            CaseInfoFields.Case,
            CaseInfoFields.RegistrationDate,
            CaseInfoFields.Caption,
            CaseInfoFields.CaseType,
            OtherFields.Administrator,
            OtherFields.SubState,
            LogFields.FinishingDate
        };

        #endregion

        /// <summary>
        /// List of fields that we use in case overview tabel but they does not exists in case
        /// </summary>
        public static readonly HashSet<string> VirtualColumns = new HashSet<string> { "_temporary_LeadTime", "tblProblem.ResponsibleUser_Id" };

        /// <summary>
        /// List of case names that we can not use in case overview grid
        /// </summary>
        public static readonly HashSet<string> NotAvailableField = new HashSet<string>
                                                                       {
                                                                           CaseInfoFields.AttachedFile,
                                                                           LogFields.Debiting, 
                                                                           LogFields.AttachedFile,
                                                                           LogFields.AttachedFileInternal,
                                                                           UserFields.UserSearchCategory_Id,
                                                                           UserFields.IsAbout_UserSearchCategory_Id,
                                                                           "FinishingCause_Id",
                                                                           "AddUserBtn",
                                                                           "UpdateNotifierInformation",
                                                                           "AddFollowersBtn"
                                                                       };
    }
}