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
    public sealed class GridColumnsDefinition
    {
        #region "Case overview" grid

        public static readonly Dictionary<int, string> CaseOverviewColumnsDictionary = new Dictionary<int, string>
        {
            { 1, "AgreedDate" },
            { 2, "Available" },
            { 3, "Caption" },
            { 4, "CaseNumber" },
            { 5, "CaseResponsibleUser_Id" },
            { 6, "CaseType_Id" },
            { 7, "Category_Id" },
            { 8, "CausingPart" },
            { 9, "ChangeTime" },
            { 10, "ClosingReason" },
            { 11, "ComputerType_Id" },
            { 12, "ContactBeforeAction" },
            { 13, "Cost" },
            { 14, "Customer_Id" },
            { 15, "Department_Id" },
            { 16, "Description" },
            { 17, "Filename" },
            { 18, "FinishingDate" },
            { 19, "FinishingDescription" },
            { 20, "Impact_Id" },
            { 21, "InventoryLocation" },
            { 22, "InventoryNumber" },
            { 23, "InvoiceNumber" },
            { 24, "Miscellaneous" },
            { 25, "OU_Id" },
            { 26, "Performer_User_Id" },
            { 27, "Persons_CellPhone" },
            { 28, "Persons_EMail" },
            { 29, "Persons_Name" },
            { 30, "Persons_Phone" },
            { 31, "Place" },
            { 32, "PlanDate" },
            { 33, "Priority_Id" },
            { 34, "ProductArea_Id" },
            { 35, "ReferenceNumber" },
            { 36, "Region_Id" },
            { 37, "RegTime" },
            { 38, "ReportedBy" },
            { 39, "SMS" },
            { 40, "SolutionRate" },
            { 41, "StateSecondary_Id" },
            { 42, "Status_Id" },
            { 43, "Supplier_Id" },
            { 44, "System_Id" },
            { 45, "tblLog.Charge" },
            { 46, "tblLog.Filename" },
            { 47, "tblLog.Text_External" },
            { 48, "tblLog.Text_Internal" },
            { 49, "TMDDate" },
            { 50, "Urgency_Id" },
            { 51, "User_Id" },
            { 52, "UserCode" },
            { 53, "WatchDate" },
            { 54, "Verified" },
            { 55, "VerifiedDescription" },
            { 56, "WorkingGroup_Id" },
            { 57, "_temporary_.LeadTime" }
        };

        #endregion

         /// <summary>
        /// List of fields that we use in case overview tabel but they does not exists in case
        /// </summary>
        public static readonly HashSet<string> VirtualColumns = new HashSet<string> { "_temporary_.LeadTime", "tblProblem.ResponsibleUser_Id" };

        /// <summary>
        /// List of case names that we can not use in case overview grid
        /// </summary>
        public static readonly HashSet<string> NotAvailableField = new HashSet<string> { "Filename", "text_internal", "tblLog.Text_External", "tblLog.Text_Internal", "tblLog.Charge", "tblLog.Filename" };
    }
}