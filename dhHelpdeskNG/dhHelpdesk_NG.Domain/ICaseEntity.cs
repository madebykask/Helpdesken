using System;

namespace DH.Helpdesk.Domain
{
    public interface ICaseEntity
    {
        string ReportedBy { get; set; }

        string PersonsName { get; set; }

        string PersonsEmail { get; set; }

        string PersonsPhone { get; set; }

        string PersonsCellphone { get; set; }

        int? Region_Id { get; set; }

        int? Department_Id { get; set; }

        int? OU_Id { get; set; }

        string UserCode { get; set; }

        string Place { get; set; }

        string CostCentre { get; set; }


        string InventoryNumber { get; set; }

        string InventoryType { get; set; }

        string InventoryLocation { get; set; }
        

        int? ProductArea_Id { get; set; }

        int? System_Id { get; set; }

        string Caption { get; set; }

        string Description { get; set; }

        int? Priority_Id { get; set; }

        int? Project_Id { get; set; }

        int? Urgency_Id { get; set; }

        int? Impact_Id { get; set; }

        int? Category_Id { get; set; }

        int? Supplier_Id { get; set; }



        string InvoiceNumber { get; set; }

        string ReferenceNumber { get; set; }

        string Miscellaneous { get; set; }

        int ContactBeforeAction { get; set; }

        int SMS { get; set; }

        DateTime? AgreedDate { get; set; }

        string Available { get; set; }

        int Cost { get; set; }

        int OtherCost { get; set; }

        string Currency { get; set; }

        int? Performer_User_Id { get; set; }

        int? CausingPartId { get; set; }

        int? WorkingGroup_Id { get; set; }

        int? Problem_Id { get; set; }

        int? Change_Id { get; set; }

        DateTime? PlanDate { get; set; }

        DateTime? WatchDate { get; set; }

        #region IsAbout

        string IsAbout_ReportedBy { get; set; }

        string IsAbout_PersonsName { get; set; }

        string IsAbout_PersonsEmail { get; set; }

        string IsAbout_PersonsPhone { get; set; }

        string IsAbout_PersonsCellPhone { get; set; }

        string IsAbout_CostCentre { get; set; }

        string IsAbout_Place { get; set; }

        string IsAbout_UserCode { get; set; }

        int? IsAbout_Region_Id { get; set; }

        int? IsAbout_Department_Id { get; set; }

        int? IsAbout_OU_Id { get; set; }

        #endregion

        int? Status_Id { get; set; }

        int? StateSecondary_Id { get; set; }

        int Verified { get; set; }

        string VerifiedDescription { get; set; }

        string SolutionRate { get; set; }

        int? RegistrationSourceCustomer_Id { get; set; }

        int CaseType_Id { get; set; }

        string FinishingDescription { get; set; }
    }
}