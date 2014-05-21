namespace DH.Helpdesk.Domain.Changes
{
    using global::System;

    public class ChangeEntity : Entity
    {
        #region Public Properties

        public int AnalysisApproval { get; set; }

        public DateTime? AnalysisApprovalDate { get; set; }

        public int? AnalysisApprovedByUser_Id { get; set; }

        public virtual User AnalysisApprovedByUser { get; set; }

        public int Approval { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public int? ApprovedByUser_Id { get; set; }

        public virtual User ApprovedByUser { get; set; }

        public int BuildImplemented { get; set; }

        public string ChangeBenefits { get; set; }

        public int? ChangeCategory_Id { get; set; }

        public virtual ChangeCategoryEntity ChangeCategory { get; set; }

        public string ChangeConsequence { get; set; }

        public string ChangeDescription { get; set; }

        public string ChangeDeviation { get; set; }

        public string ChangeEvaluation { get; set; }

        public string ChangeExplanation { get; set; }

        public Guid ChangeGUID { get; set; }

        public int? ChangeGroup_Id { get; set; }

        public virtual ChangeGroupEntity ChangeGroup { get; set; }

        public string ChangeImpact { get; set; }

        public int? ChangeObject_Id { get; set; }

        public virtual ChangeObjectEntity ChangeObject { get; set; }

        public int? ChangePriority_Id { get; set; }

        public virtual ChangePriorityEntity ChangePriority { get; set; }

        public string ChangeRecommendation { get; set; }

        public string ChangeRisk { get; set; }

        public string ChangeSolution { get; set; }

        public int? ChangeStatus_Id { get; set; }

        public virtual ChangeStatusEntity ChangeStatus { get; set; }

        public string ChangeTitle { get; set; }

        public int? ChangedByUser_Id { get; set; }

        public virtual User ChangedByUser { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Currency { get; set; }

        public int? Customer_Id { get; set; }

        public virtual Customer Customer { get; set; }

        public DateTime? DesiredDate { get; set; }

        public DateTime? FinishingDate { get; set; }

        public int ImplementationPlan { get; set; }

        public int ImplementationPlanUsed { get; set; }

        public int ImplementationReady { get; set; }

        public int? ImplementationStatus_Id { get; set; }

        public virtual ChangeImplementationStatusEntity ImplementationStatus { get; set; }

        public string InventoryNumber { get; set; }

        public string OrdererCellPhone { get; set; }

        public int? OrdererDepartment_Id { get; set; }

        public virtual Department OrdererDepartment { get; set; }

        public string OrdererEMail { get; set; }

        public string OrdererId { get; set; }

        public string OrdererName { get; set; }

        public string OrdererPhone { get; set; }

        public int? Prioritisation { get; set; }

        public int RSS { get; set; }

        public DateTime? RealStartDate { get; set; }

        public int RecoveryPlan { get; set; }

        public int RecoveryPlanUsed { get; set; }

        public int RegLanguage_Id { get; set; }

        public virtual RegionLanguage RegLanguage { get; set; }

        public int? ResponsibleUser_Id { get; set; }

        public virtual User ResponsibleUser { get; set; }

        public DateTime? ScheduledEndTime { get; set; }

        public DateTime? ScheduledStartTime { get; set; }

        public int? SourceCase_Id { get; set; }

        public virtual Case SourceCase { get; set; }

        public int? System_Id { get; set; }

        public virtual System System { get; set; }

        public int? TimeEstimatesHours { get; set; }

        public int TotalCost { get; set; }

        public int? User_Id { get; set; }

        public int Verified { get; set; }

        public int? WorkingGroup_Id { get; set; }

        public virtual WorkingGroupEntity WorkingGroup { get; set; }

        public int YearlyCost { get; set; }

        public DateTime? PlannedReadyDate { get; set; }

        public int EvaluationReady { get; set; }

        #endregion
    }
}