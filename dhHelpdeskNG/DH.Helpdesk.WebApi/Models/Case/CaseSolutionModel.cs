using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.WebApi.Models.Case
{
    public class CaseSolutionModel
    {
        public int Id { get; set; }
        public int? CaseType_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? FinishingCause_Id { get; set; }
        public bool NoMailToNotifier { get; set; }
        public int? PerformerUser_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int? ProductArea_Id { get; set; }
        public int? Project_Id { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Miscellaneous { get; set; }
        public string Name { get; set; }
        public string ReportedBy { get; set; }
        public string Text_External { get; set; }
        public string Text_Internal { get; set; }
        public int? OrderNum { get; set; }
        public string PersonsName { get; set; }
        public string PersonsPhone { get; set; }
        public string PersonsCellPhone { get; set; }
        public string PersonsEmail { get; set; }
        public int? Region_Id { get; set; }
        public int? OU_Id { get; set; }
        public string Place { get; set; }
        public string UserCode { get; set; }
        public int? System_Id { get; set; }
        public int? Urgency_Id { get; set; }
        public int? Impact_Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public int? Status_Id { get; set; }
        public int? StateSecondary_Id { get; set; }
        public bool Verified { get; set; }
        public string VerifiedDescription { get; set; }
        public string SolutionRate { get; set; }
        public string InventoryNumber { get; set; }
        public string InventoryType { get; set; }
        public string InventoryLocation { get; set; }
        public int? Supplier_Id { get; set; }
        public DateTime? AgreedDate { get; set; }
        public string Available { get; set; }
        public int Cost { get; set; }
        public int OtherCost { get; set; }
        public string Currency { get; set; }
        public bool ContactBeforeAction { get; set; }
        public int? Problem_Id { get; set; }
        public int? Change_Id { get; set; }
        public DateTime? WatchDate { get; set; }
        public DateTime? FinishingDate { get; set; }
        public string FinishingDescription { get; set; }
        public bool SMS { get; set; }
        public bool? UpdateNotifierInformation { get; set; }
        public bool? AddFollowersBtn { get; set; }
        public DateTime? PlanDate { get; set; }
        public int? CausingPartId { get; set; }
        public int? RegistrationSource { get; set; }
        public bool IsActive { get; set; }
        public string CostCentre { get; set; }
        public string IsAbout_ReportedBy { get; set; }
        public string IsAbout_PersonsName { get; set; }
        public string IsAbout_PersonsEmail { get; set; }
        public string IsAbout_PersonsPhone { get; set; }
        public string IsAbout_PersonsCellPhone { get; set; }
        public int? IsAbout_UserSearchCategory_Id { get; set; }
        public int? IsAbout_Region_Id { get; set; }
        public int? IsAbout_Department_Id { get; set; }
        public int? IsAbout_OU_Id { get; set; }
        public string IsAbout_CostCentre { get; set; }
        public string IsAbout_Place { get; set; }
        public string IsAbout_UserCode { get; set; }
        public int OverWritePopUp { get; set; }
        public int? FinalAction { get; set; }
    }
}