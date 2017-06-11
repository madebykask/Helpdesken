using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.CaseRules
{
    public class CaseCurrentDataModel
    {
        public int Customer_Id { get; set; }

        #region Initiator      

        public int Id { get; set; }

        public decimal CaseNumber { get; set; }


        public string ReportedBy { get; set; }

        public string PersonsName { get; set; }

        public string PersonsEmail { get; set; }

        public int NoMailToNotifier { get; set; }

        public string PersonsPhone { get; set; }

        public string PersonsCellPhone { get; set; }

        public string CostCentre { get; set; }

        public string Place { get; set; }

        public string UserCode { get; set; }

        public int? UpdateNotifierInformation { get; set; }

        public int? Region_Id { get; set; }

        public int? Department_Id { get; set; }

        public int? OU_Id { get; set; }

        #endregion

        #region IsAbout

        public string IsAbout_ReportedBy { get; set; }

        public string IsAbout_PersonsName { get; set; }

        public string IsAbout_PersonsEmail { get; set; }

        public string IsAbout_PersonsPhone { get; set; }

        public string IsAbout_PersonsCellPhone { get; set; }

        public string IsAbout_CostCentre { get; set; }

        public string IsAbout_Place { get; set; }

        public string IsAbout_UserCode { get; set; }

        public int? IsAbout_Region_Id { get; set; }

        public int? IsAbout_Department_Id { get; set; }

        public int? IsAbout_OU_Id { get; set; }

        #endregion

        #region Computer Info

        public string InventoryNumber { get; set; }

        public string InventoryType { get; set; }

        public string InventoryLocation { get; set; }

        #endregion

        #region Case Info

        public int? RegistrationSource { get; set; }

        public int? CaseType_Id { get; set; }

        public int? ProductArea_Id { get; set; }

        public int? System_Id { get; set; }

        public int? Urgency_Id { get; set; }

        public int? Impact_Id { get; set; }

        public int? Category_Id { get; set; }

        public int? Supplier_Id { get; set; }

        public string InvoiceNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public string Miscellaneous { get; set; }

        public int ContactBeforeAction { get; set; }

        public int SMS { get; set; }

        public DateTime? AgreedDate { get; set; }

        public string Available { get; set; }

        public int Cost { get; set; }

        public int OtherCost { get; set; }

        public string Currency { get; set; }

        #endregion

        #region Other Info

        public int? WorkingGroup_Id { get; set; }

        public int? PerformerUser_Id { get; set; }

        public int? Priority_Id { get; set; }

        public int? Status_Id { get; set; }

        public int? StateSecondary_Id { get; set; }

        public int? Project_Id { get; set; }

        public int? Problem_Id { get; set; }

        public int? CausingPart_Id { get; set; }

        public int? Change_Id { get; set; }

        public DateTime? PlanDate { get; set; }

        public DateTime? WatchDate { get; set; }

        public int Verified { get; set; }

        public string VerifiedDescription { get; set; }

        public string SolutionRate { get; set; }

        #endregion

        #region Log

        public string Text_External { get; set; }

        public string Text_Internal { get; set; }

        public string FinishingDescription { get; set; }

        public DateTime? FinishingDate { get; set; }

        public int? FinishingCause_Id { get; set; }

        #endregion    

    }

    public sealed class CaseCurrentDataModelJS : CaseCurrentDataModel
    {
        public CaseCurrentDataModelJS()
        {
        }

        public string DateFormat { get; set; }

        public string PlanDateJS { get; set; }

        public string WatchDateJS { get; set; }

        public string StatusName { get; set; }

        public string SubStateName { get; set; }

        public string PriorityName { get; set; }

        public string WorkingGroupName { get; set; }

        public string CaseTypeName { get; set; }

        public string ProductAreaName { get; set; }

        public string RegionName { get; set; }

        public string DepartmentName { get; set; }

        public string OUName { get; set; }
    }

    public sealed class LogJS
    {
        public int Id { get; set; }

        public string LogDate { get; set; }

        public string Text_External { get; set; }

        public string Text_Internal { get; set; }

        public string FinishingDescription { get; set; }

        public string FinishingDate { get; set; }

        public int? FinishingCause_Id { get; set; }

        public string UserId { get; set; }
    }

    public sealed class FileJS
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string CreateDate { get; set; }

        public string UserId { get; set; }
        
    }
}