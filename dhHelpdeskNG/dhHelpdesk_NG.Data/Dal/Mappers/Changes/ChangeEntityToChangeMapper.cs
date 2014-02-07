namespace DH.Helpdesk.Dal.Dal.Mappers.Changes
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Dal.DbContext;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class ChangeEntityToChangeMapper : IEntityToBusinessModelMapper<Domain.Changes.ChangeEntity, Change>
    {
        private readonly HelpdeskDbContext dbContext;

        public ChangeEntityToChangeMapper(IDatabaseFactory databaseFactory)
        {
            this.dbContext = databaseFactory.Get();
        }

        public Change Map(Domain.Changes.ChangeEntity entity)
        {
            var header = new ChangeHeader(
              entity.OrdererId,
              entity.OrdererName,
              entity.OrdererPhone,
              entity.OrdererCellPhone,
              entity.OrdererEMail,
              entity.OrdererDepartment_Id,
              entity.ChangeTitle,
              entity.ChangeStatus_Id,
              entity.System_Id,
              entity.ChangeObject_Id,
              entity.WorkingGroup_Id,
              entity.User_Id,
              entity.FinishingDate,
              entity.CreatedDate,
              entity.ChangedDate,
              entity.RSS != 0);

            var approvedUser = entity.ApprovedByUser != null
                ? entity.ApprovedByUser.FirstName + entity.ApprovedByUser.SurName
                : null;

            var registration = new RegistrationFields(
                entity.ChangeGroup_Id,
                entity.ChangeDescription,
                entity.ChangeBenefits,
                entity.ChangeConsequence,
                entity.ChangeImpact,
                entity.DesiredDate,
                entity.Verified != 0,
                (RegistrationApprovalResult)entity.Approval,
                approvedUser,
                entity.ApprovalDate,
                entity.ChangeRecommendation);

//            var currencyId = string.IsNullOrEmpty(entity.Currency)
//                ? (int?)null
//                : this.dbContext.Currencies.Single(c => c.Code == entity.Currency).Id;

            var analyzeApprovedByUser = entity.AnalysisApprovedByUser != null
                ? entity.AnalysisApprovedByUser.FirstName + entity.AnalysisApprovedByUser.SurName
                : string.Empty;

            var analyze = new AnalyzeFields(
                entity.ChangeCategory_Id,
                entity.ChangePriority_Id,
                entity.ResponsibleUser_Id,
                entity.ChangeSolution,
                entity.TotalCost,
                entity.YearlyCost,
                1,
                entity.TimeEstimatesHours ?? 0,
                entity.ChangeRisk,
                entity.ScheduledStartTime,
                entity.ScheduledEndTime,
                entity.ImplementationPlan != 0,
                entity.RecoveryPlan != 0,
                (AnalyzeApprovalResult)entity.AnalysisApproval,
                entity.AnalysisApprovalDate,
                analyzeApprovedByUser,
                entity.ChangeRecommendation);

            var implementation = new ImplementationFields(
                entity.ImplementationStatus_Id,
                entity.RealStartDate,
                entity.FinishingDate,
                entity.BuildImplemented != 0,
                entity.ImplementationPlanUsed != 0,
                entity.ChangeDeviation,
                entity.RecoveryPlanUsed != 0,
                entity.ImplementationReady != 0);

            var evaluation = new EvaluationFields(entity.ChangeEvaluation, entity.EvaluationReady != 0);

            return new Change(entity.Id, header, registration, analyze, implementation, evaluation);
        }
    }
}
