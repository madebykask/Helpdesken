namespace dhHelpdesk_NG.Data.Dal.Mappers.Changes
{
    using System;

    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange;

    public sealed class UpdatedChangeToChangeEntityMapper : IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity>
    {
        public void Map(UpdatedChange businessModel, ChangeEntity entity)
        {
            entity.AnalysisApproval = (int)businessModel.Analyze.Approved;
            entity.AnalysisApprovalDate = businessModel.Analyze.ApprovedDateAndTime;
//            entity.AnalysisApprovedByUser_Id = businessModel.Analyze.ApprovedUser;
            entity.Approval = (int)businessModel.Registration.Approved;
            entity.ApprovalDate = businessModel.Registration.ApprovedDateAndTime;
//            entity.ApprovedByUser_Id = businessModel.Registration.ApprovedUser;
            entity.BuildImplemented = businessModel.Implementation.BuildImplemented ? 1 : 0;
            entity.ChangeBenefits = businessModel.Registration.BusinessBenefits;
            entity.ChangeCategory_Id = businessModel.Analyze.CategoryId;
            entity.ChangeConsequence = businessModel.Registration.Consequence;
            entity.ChangeDescription = businessModel.Registration.Description;
            entity.ChangeDeviation = businessModel.Implementation.ChangeDeviation;
            entity.ChangeEvaluation = businessModel.Evaluation.ChangeEvaluation;
            entity.ChangeExplanation = businessModel.Registration.ChangeRecommendation;
            entity.ChangeGroup_Id = businessModel.Registration.OwnerId;
            entity.ChangeImpact = businessModel.Registration.Impact;
            entity.ChangeObject_Id = businessModel.Header.ObjectId;
            entity.ChangePriority_Id = businessModel.Analyze.PriorityId;
            entity.ChangeRecommendation = businessModel.Analyze.ChangeRecommendation;
            entity.ChangeRisk = businessModel.Analyze.Risk;
            entity.ChangeSolution = businessModel.Analyze.Solution;
            entity.ChangeStatus_Id = businessModel.Header.StatusId;
            entity.ChangeTitle = businessModel.Header.Title;
            entity.ChangedDate = businessModel.ChangedDate;
//            entity.Currency = businessModel.Analyze.CurrencyId;
            entity.DesiredDate = businessModel.Registration.DesiredDate;
            entity.EvaluationReady = businessModel.Evaluation.EvaluationReady ? 1 : 0;
            entity.FinishingDate = businessModel.Implementation.FinishingDate;
            entity.ImplementationPlan = businessModel.Analyze.HasImplementationPlan ? 1 : 0;
            entity.ImplementationPlanUsed = businessModel.Implementation.ImplementationPlanUsed ? 1 : 0;
            entity.ImplementationReady = businessModel.Implementation.Ready ? 1 : 0;
            entity.ImplementationStatus_Id = businessModel.Implementation.ImplementationStatusId;
            entity.InventoryNumber = "A";
            entity.OrdererCellPhone = businessModel.Header.CellPhone;
            entity.OrdererDepartment_Id = businessModel.Header.DepartmentId;
            entity.OrdererEMail = businessModel.Header.Email;
            entity.OrdererId = businessModel.Header.Id;
            entity.OrdererName = businessModel.Header.Name;
            entity.OrdererPhone = businessModel.Header.Phone;
            entity.PlannedReadyDate = DateTime.Now;
            entity.Prioritisation = 1;
            entity.RSS = businessModel.Header.Rss ? 1 : 0;
            entity.RealStartDate = businessModel.Implementation.RealStartDate;
            entity.RecoveryPlan = businessModel.Analyze.HasRecoveryPlan ? 1 : 0;
            entity.RecoveryPlanUsed = businessModel.Implementation.RecoveryPlanUsed ? 1 : 0;
            //entity.RegLanguage_Id 
            entity.ResponsibleUser_Id = businessModel.Header.AdministratorId;
            entity.ScheduledEndTime = businessModel.Analyze.StartDate;
            entity.ScheduledStartTime = businessModel.Analyze.EndDate;
//            entity.SourceCase_Id 
            entity.System_Id = businessModel.Header.SystemId;
            entity.TimeEstimatesHours = businessModel.Analyze.TimeEstimatesHours;
            entity.TotalCost = businessModel.Analyze.Cost;
//            entity.User_Id
            entity.Verified = businessModel.Registration.Verified ? 1 : 0;
            entity.WorkingGroup_Id = businessModel.Header.WorkingGroupId;
            entity.YearlyCost = businessModel.Analyze.YearlyCost;
        }
    }
}
