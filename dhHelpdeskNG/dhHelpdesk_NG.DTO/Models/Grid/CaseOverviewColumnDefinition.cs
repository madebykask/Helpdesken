namespace DH.Helpdesk.BusinessData.Models.Grid
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Here we will store relations between field id and field name to know what settings is applied for what field case GridSettingsEntity.FieldId.
    /// I used this sql to fetch all availavble columns:
    /// >> select distinct '{,"' + CaseField + '"},' from tblcasefieldsettingsorder by 1;
    /// ACHTUNG!! if you delete something from this class then put checks to all plases where you use this maping. 
    /// Now this is at least CaseOverviewToGridColumnDefMapper and GridSettingsService classes. Otherwise you could get an "key not found" exception when will process data from DB
    /// </summary>
    public partial class GridColumnsDefinition
    {
        #region "Case overview" grid

        private static readonly string[] caseOverviewColumns = {
"AgreedDate",
"Available",
"Caption",
"CaseNumber",
"CaseResponsibleUser_Id",
"CaseType_Id",
"Category_Id",
"CausingPart",
"ChangeTime",
"ClosingReason",
"ComputerType_Id",
"ContactBeforeAction",
"Cost",
"Customer_Id",
"Department_Id",
"Description",
"Filename",
"FinishingCause_Id",
"FinishingDate",
"FinishingDescription",
"Impact_Id",
"InventoryLocation",
"InventoryNumber",
"InvoiceNumber",
"Miscellaneous",
"OU_Id",
"Performer_User_Id",
"Persons_CellPhone",
"Persons_EMail",
"Persons_Name",
"Persons_Phone",
"Place",
"PlanDate",
"Priority_Id",
"ProductArea_Id",
"ReferenceNumber",
"Region_Id",
"RegTime",
"ReportedBy",
"SMS",
"SolutionRate",
"StateSecondary_Id",
"Status_Id",
"Supplier_Id",
"System_Id",
"tblLog.Charge",
"tblLog.Filename",
"tblLog.Text_External",
"tblLog.Text_Internal",
"TMDDate",
"Urgency_Id",
"User_Id",
"UserCode",
"WatchDate",
"Verified",
"VerifiedDescription",
"WorkingGroup_Id"
        };

        #endregion

         /// <summary>
        /// List of fields that we use in case overview tabel but they does not exists in case
        /// </summary>
        public static readonly HashSet<string> VirtualColumns = new HashSet<string> { "_temporary_.LeadTime", "tblProblem.ResponsibleUser_Id" };

        /// <summary>
        /// List of case names that we can not use in case overview grid
        /// </summary>
        public static readonly HashSet<string> NotAvailableField = new HashSet<string> { "Filename", "text_internal", "tblLog.Text_External", "tblLog.Text_Internal", "tblLog.Charge", "tblLog.Filename", "FinishingCause_Id" };
    }
}