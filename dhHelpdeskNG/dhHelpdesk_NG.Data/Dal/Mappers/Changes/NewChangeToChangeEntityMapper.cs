namespace DH.Helpdesk.Dal.Dal.Mappers.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.Domain.Changes;

    public sealed class NewChangeToChangeEntityMapper : INewBusinessModelToEntityMapper<NewChange, ChangeEntity>
    {
        public ChangeEntity Map(NewChange businessModel)
        {
            return new ChangeEntity
                   {
                       AnalysisApproval = (int)businessModel.Analyze.Approved,
                       AnalysisApprovalDate = DateTime.Now,
//                       AnalysisApprovedByUser_Id = businessModel.Analyze.ApprovedUser,
                       Approval = (int)businessModel.Registration.Approved,
                       ApprovalDate = DateTime.Now,
//                       ApprovedByUser_Id = businessModel.Registration.ApprovedUser,
                       BuildImplemented = businessModel.Implementation.BuildImplemented ? 1 : 0,
                       ChangeBenefits = businessModel.Registration.BusinessBenefits,
                       ChangeCategory_Id = businessModel.Analyze.CategoryId,
                       ChangeConsequence = businessModel.Registration.Consequence,
                       ChangeDescription = businessModel.Registration.Description,
                       ChangeDeviation = businessModel.Implementation.ChangeDeviation,
                       ChangeEvaluation = businessModel.Evaluation.ChangeEvaluation,
                       ChangeExplanation = businessModel.Registration.ChangeRecommendation,
                       ChangeGroup_Id = businessModel.Registration.OwnerId,
                       ChangeImpact = businessModel.Registration.Impact,
                       ChangeObject_Id = businessModel.Header.ObjectId,
                       ChangePriority_Id = businessModel.Analyze.PriorityId,
                       ChangeRecommendation = businessModel.Registration.ChangeRecommendation,
                       ChangeRisk = businessModel.Analyze.Risk,
                       ChangeSolution = businessModel.Analyze.Solution,
                       ChangeStatus_Id = businessModel.Header.StatusId,
                       ChangeTitle = businessModel.Header.Title,
                       ChangedByUser_Id = 0,
                       CreatedDate = DateTime.Now,
                       Currency = "EUR",
                       Customer_Id = businessModel.CustomerId,
                       DesiredDate = DateTime.Now,
                       EvaluationReady = businessModel.Evaluation.EvaluationReady ? 1 : 0,
                       FinishingDate = DateTime.Now,
                       ImplementationPlan = businessModel.Analyze.HasImplementationPlan ? 1 : 0,
                       ImplementationPlanUsed =
                           businessModel.Implementation.ImplementationPlanUsed ? 1 : 0,
                       ImplementationReady = businessModel.Implementation.ImplementaionReady ? 1 : 0,
                       ImplementationStatus_Id = businessModel.Header.StatusId,
//                       InventoryNumber = 0,
                       OrdererCellPhone = businessModel.Header.CellPhone,
                       OrdererDepartment_Id = businessModel.Header.DepartmentId,
                       OrdererEMail = businessModel.Header.Email,
                       OrdererId = businessModel.Header.Id,
                       OrdererName = businessModel.Header.Name,
                       OrdererPhone = businessModel.Header.Phone,
//                       PlannedReadyDate = DateTime.Now,
                       RSS = businessModel.Header.Rss ? 1 : 0,
                       RealStartDate = DateTime.Now,
                       RecoveryPlan = businessModel.Analyze.HasRecoveryPlan ? 1 : 0,
                       RecoveryPlanUsed = businessModel.Implementation.RecoveryPlanUsed ? 1 : 0,
                       ResponsibleUser_Id = businessModel.Analyze.ResponsibleUserId,
                       ScheduledEndTime = DateTime.Now,
                       ScheduledStartTime = DateTime.Now,
                       System_Id = businessModel.Header.SystemId,
                       TimeEstimatesHours = businessModel.Analyze.TimeEstimatesHours,
                       TotalCost = businessModel.Analyze.Cost,
                       User_Id = businessModel.Header.AdministratorId,
                       Verified = businessModel.Registration.Verified ? 1 : 0,
                       WorkingGroup_Id = businessModel.Header.WorkingGroupId,
                       YearlyCost = businessModel.Analyze.YearlyCost,
                       PlannedReadyDate = DateTime.Now,
                       ChangedDate = DateTime.Now,
                       RegLanguage_Id = 1
                   };
        }
    }
}
