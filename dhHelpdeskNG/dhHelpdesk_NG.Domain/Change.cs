using System;

namespace dhHelpdesk_NG.Domain
{
    public class Change : Entity
    {
        public int? AnalysisApproval { get; set; }
        public int? AnalysisApprovedByUser_Id { get; set; }
        public int? Approval { get; set; }
        public int? ApprovedByUser_Id { get; set; }
        public int? BuildImplemented { get; set; }
        public int? ChangedByUser_Id { get; set; }
        public int? ChangeCategory_Id { get; set; }
        public int? ChangeGroup_Id { get; set; }
        public int? ChangeObject_Id { get; set; }
        public int? ChangePriority_Id { get; set; }
        public int? ChangeStatus_Id { get; set; }
        public int? Customer_Id { get; set; }
        public int? ImplementationPlan { get; set; }
        public int? ImplementationPlanUsed { get; set; }
        public int? ImplementationReady { get; set; }
        public int? ImplementationStatus_Id { get; set; }
        public int? OrdererDepartment_Id { get; set; }
        public int? Prioritisation { get; set; }
        public int? RecoveryPlan { get; set; }
        public int? RecoveryPlanUsed { get; set; }
        public int? RegLanguage_Id { get; set; }
        public int? ResponsibleUser_Id { get; set; }
        public int? RSS { get; set; }
        public int? SourceCase_Id { get; set; }
        public int? System_Id { get; set; }
        public int? TimeEstimatesHours { get; set; }
        public int? TotalCost { get; set; }
        public int? User_Id { get; set; }
        public int? Verified { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public int? YearlyCost { get; set; }
        public string ChangeSolution { get; set; }
        public string ChangeBenefits { get; set; }
        public string ChangeConsequence { get; set; }
        public string ChangeDescription { get; set; }
        public string ChangeDeviation { get; set; }
        public string ChangeEvaluation { get; set; }
        public string ChangeExplanation { get; set; }
        public string ChangeImpact { get; set; }
        public string ChangeRecommendation { get; set; }
        public string ChangeRisk { get; set; }
        public string ChangeTitle { get; set; }
        public string Currency { get; set; }
        public string InventoryNumber { get; set; }
        public string OrdererCellPhone { get; set; }
        public string OrdererEmail { get; set; }
        public string OrdererId { get; set; }
        public string OrdererName { get; set; }
        public string OrdererPhone { get; set; }
        public DateTime? AnalysisApprovalDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DesiredDate { get; set; }
        public DateTime? FinishingDate { get; set; }
        public DateTime? RealStartDate { get; set; }
        public DateTime? ScheduledEndTime { get; set; }
        public DateTime? ScheduledStartTime { get; set; }
        public Guid ChangeGUID { get; set; }

        //public virtual ChangeCategory ChangeCategory { get; set; }
        //public virtual ChangeGroup ChangeGroup { get; set; }
        //public virtual ChangeObject ChangeObject { get; set; }
        //public virtual ChangePriority ChangePriority { get; set; }
        //public virtual ChangeStatus ChangeStatus { get; set; }
        //public virtual Case SourceCase { get; set; }
        //public virtual Customer Customer { get; set; }
        //public virtual Department OrdererDepartment { get; set; }
        //public virtual Language RegLanguage { get; set; }
        //public virtual Status ImplementationStatus { get; set; }
        //public virtual User ApprovedByUser { get; set; }
        //public virtual User AnalysisApprovedByUser { get; set; }
        //public virtual WorkingGroup WorkingGroup { get; set; }
    }
}
