using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.Domain
{
    using global::System.Collections.Generic;
    using DH.Helpdesk.Domain.Projects;
    using global::System;
    using global::System.Web.Mvc;
    using Common.Enums.CaseSolution;
	using ExtendedCaseEntity;


	public class CaseSolution : Entity
    {

        public CaseSolution()
        {
            this.ExtendedCaseForms = new List<ExtendedCaseEntity.ExtendedCaseFormEntity>();
        }

        public int? CaseSolutionCategory_Id { get; set; }
        public int? CaseWorkingGroup_Id { get; set; }
        public int? CaseType_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? Department_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? FinishingCause_Id { get; set; }
        public int NoMailToNotifier { get; set; }
        public int? PerformerUser_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int? ProductArea_Id { get; set; }
        public int? Project_Id { get; set; }
        public int? WorkingGroup_Id { get; set; }        
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Miscellaneous { get; set; }
        [Required]
        public string Name { get; set; }
        public string ReportedBy { get; set; }
        public string Text_External { get; set; }
        public string Text_Internal { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TemplatePath { get; set; }
        public bool ShowInSelfService { get; set; }
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
        public int Verified { get; set; }
        public string VerifiedDescription { get; set; }
        public string SolutionRate { get; set; }
        public string InventoryNumber { get; set; }
        public string InventoryType { get; set; }
        public string InventoryLocation { get; set; }
        public int? Supplier_Id { get; set; }
        public Guid? FormGUID { get; set; }
        public DateTime? AgreedDate { get; set; }
        public string Available { get; set; }
        public int Cost { get; set; }
        public int OtherCost { get; set; }
        public string Currency { get; set; }
        public int ContactBeforeAction { get; set; }
        public int? Problem_Id { get; set; }
        public int? Change_Id { get; set; }
        public DateTime? WatchDate { get; set; }
        public DateTime? FinishingDate { get; set; }
        public string FinishingDescription { get; set; }
        public int SMS { get; set; }
        public int? UpdateNotifierInformation { get; set; }
        public DateTime? PlanDate { get; set; }
        public int? CausingPartId { get; set; }
        public int? RegistrationSource { get; set; }
        public int Status { get; set; }
        public string CostCentre { get; set; }

        public string IsAbout_ReportedBy { get; set; }
        public string IsAbout_PersonsName { get; set; }
        public string IsAbout_PersonsEmail { get; set; }
        public string IsAbout_PersonsPhone { get; set; }
        public string IsAbout_PersonsCellPhone { get; set; }
        public int? IsAbout_Region_Id { get; set; }
        public int? IsAbout_Department_Id { get; set; }
        public int? IsAbout_OU_Id { get; set; }
        public string IsAbout_CostCentre { get; set; }
        public string IsAbout_Place { get; set; }
        public string IsAbout_UserCode { get; set; }
        
        public int ShowOnCaseOverview { get; set; }
        public int ShowInsideCase { get; set; }
        public int? SetCurrentUserAsPerformer { get; set; }
        public int? SetCurrentUsersWorkingGroup { get; set; }
        public int OverWritePopUp { get; set; }
        
        public int? ConnectedButton { get; set; }
        public int? SaveAndClose { get; set; }

        public string ShortDescription { get; set; }
        
        public string Information { get; set; }
        public string DefaultTab { get; set; }
        public string ValidateOnChange { get; set; }
        public int? NextStepState { get; set; }
        public CaseRelationType CaseRelationType { get; set; }

        public int? SplitToCaseSolution_Id { get; set; }

        public virtual CaseSolutionCategory CaseSolutionCategory { get; set; }
        public virtual CaseSolutionSchedule CaseSolutionSchedule { get; set; }
        public virtual CaseType CaseType { get; set; }
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual FinishingCause FinishingCause { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual ProductArea ProductArea { get; set; }
        public virtual Project Project { get; set; }
        public virtual User PerformerUser { get; set; }
        public virtual WorkingGroupEntity CaseWorkingGroup { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
        //public virtual Problem Problem { get; set; }
        public int SortOrder { get; set; }

		public virtual List<ExtendedCaseFormEntity> ExtendedCaseForms { get; set; }

		public virtual List<CaseSolution_CaseSection_ExtendedCaseForm> CaseSectionsExtendedCaseForm { get; set; }

		public virtual CaseSolution SplitToCaseSolution { get; set; }
        public string CaseSolutionDescription { get; set; }

        public virtual ICollection<CaseSolution_SplitToCaseSolutionEntity> SplitToCaseSolutionAnsestors { get; set; }

        public virtual ICollection<CaseSolution_SplitToCaseSolutionEntity> SplitToCaseSolutionDescendants { get; set; }

    }
}
