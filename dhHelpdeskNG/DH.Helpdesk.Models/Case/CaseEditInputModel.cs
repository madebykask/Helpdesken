using System;

namespace DH.Helpdesk.Models.Case
{
    public class CaseEditInputModel
    {
        public int? CaseId { get; set; }
        public string CaseGuid { get; set; }

        public string ReportedBy { get; set; }
        public string PersonName { get; set; }
        public string PersonEmail { get; set; }
        public string PersonPhone { get; set; }
        public string PersonCellPhone { get; set; }
        public int? RegionId { get; set; }
        public int? DepartmentId { get; set; }
        public int? OrganizationUnitId { get; set; }
        public string CostCentre { get; set; }
        public string Place { get; set; }
        public string UserCode { get; set; }

        public string IsAbout_ReportedBy { get; set; }
        public string IsAbout_PersonName { get; set; }
        public string IsAbout_PersonCellPhone { get; set; }
        public int? IsAbout_RegionId { get; set; }
        public int? IsAbout_DepartmentId { get; set; }
        public int? IsAbout_OrganizationUnitId { get; set; }
        public string IsAbout_CostCentre { get; set; }
        public string IsAbout_Place { get; set; }
        public string IsAbout_UserCode { get; set; }

        public string InventoryNumber { get; set; }
        public string ComputerTypeId { get; set; }
        public string InventoryLocation { get; set; }


        public decimal CaseNumber { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime ChangeTime { get; set; }
        public int? UserId { get; set; }
        public int? RegistrationSourceCustomerId { get; set; }
        public int CaseTypeId { get; set; }
        public int? ProductAreaId { get; set; }
        public int? SystemId { get; set; }
        public int? UrgencyId { get; set; }
        public int? ImpactId { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? SupplierCountryId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string Miscellaneous { get; set; }
        public string Caption { get; set; }
        public bool ContactBeforeAction { get; set; }
        public bool Sms { get; set; }
        public DateTime? AgreedDate { get; set; }
        public string Available { get; set; }
        public int Cost { get; set; }
        public int OtherCost { get; set; }
        public string CostCurrency { get; set; }

        public int? WorkingGroupId { get; set; }
        public int? ResponsibleUserId { get; set; }
        public int? PerformerId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? StateSecondaryId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProblemId { get; set; }
        public int? CausingPartId { get; set; }
        public int? ChangeId { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? WatchDate { get; set; }
        public bool Verified { get; set; }
        public string VerifiedDescription { get; set; }
        public string SolutionRate { get; set; }

        public string FinishingDescription { get; set; }
        public int? ClosingReason { get; set; }
        public string FinishingDate { get; set; }
        public string LogInternalText { get; set; }
        public string LogExternalText { get; set; }
    }
}

