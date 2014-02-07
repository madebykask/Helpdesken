namespace DH.Helpdesk.Dal.Dal.Mappers.Changes
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeEntityToChangeDetailedOverviewMapper :
        IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>
    {
        public ChangeDetailedOverview Map(ChangeEntity entity)
        {
            var ordererFields = CreateOrdererFieldGroup(entity);
            var generalFields = CreateGeneralFieldGroup(entity);
            var registrationFields = CreateRegistrationFieldGroup(entity);
            var analyzeFields = CreateAnalyzeFieldGroup(entity);
            var implementationFields = CreateImplementationFieldGroup(entity);
            var evaluationFields = CreateEvaluationFieldGroup(entity);

            return new ChangeDetailedOverview(
                entity.Id,
                ordererFields,
                generalFields,
                registrationFields,
                analyzeFields,
                implementationFields,
                evaluationFields);
        }

        private static OrdererFieldGroupDto CreateOrdererFieldGroup(ChangeEntity change)
        {
            return new OrdererFieldGroupDto(
                change.OrdererId,
                change.OrdererName,
                change.OrdererPhone,
                change.OrdererCellPhone,
                change.OrdererEMail,
                change.OrdererDepartment == null ? string.Empty : change.OrdererDepartment.DepartmentName);
        }

        private static GeneralFieldGroupDto CreateGeneralFieldGroup(ChangeEntity change)
        {
            return new GeneralFieldGroupDto(
                change.Prioritisation,
                change.ChangeTitle,
                change.ChangeStatus != null ? change.ChangeStatus.Name : string.Empty,
                change.System != null ? change.System.SystemName : string.Empty,
                change.ChangeObject != null ? change.ChangeObject.Name : string.Empty,
                change.InventoryNumber,
                string.Empty,
                change.WorkingGroup != null ? change.WorkingGroup.WorkingGroupName : string.Empty,
                string.Empty,
                change.FinishingDate,
                change.RSS != 0);
        }

        private static RegistrationFieldGroupDto CreateRegistrationFieldGroup(ChangeEntity change)
        {
            return new RegistrationFieldGroupDto(
                change.ChangeDescription,
                change.ChangeBenefits,
                change.ChangeConsequence,
                change.ChangeImpact,
                change.DesiredDate,
                change.Verified != 0,
                change.Approval != 0,
                change.ChangeExplanation);
        }

        private static AnalyzeFieldGroupDto CreateAnalyzeFieldGroup(ChangeEntity change)
        {
            return new AnalyzeFieldGroupDto(
                change.ChangeCategory != null ? change.ChangeCategory.Name : string.Empty,
                change.ChangePriority != null ? change.ChangePriority.Name : string.Empty,
                change.ResponsibleUser != null ? change.ResponsibleUser.UserID : string.Empty,
                change.ChangeSolution,
                change.TotalCost,
                change.YearlyCost,
                change.TimeEstimatesHours,
                change.ChangeRisk,
                change.ScheduledStartTime,
                change.ScheduledEndTime,
                change.ImplementationPlan != 0,
                change.RecoveryPlan != 0,
                change.ChangeRecommendation,
                (AnalyzeResult)change.AnalysisApproval);
        }

        private static ImplementationFieldGroupDto CreateImplementationFieldGroup(ChangeEntity change)
        {
            return
                new ImplementationFieldGroupDto(
                    change.ImplementationStatus != null ? change.ImplementationStatus.Name : string.Empty,
                    change.RealStartDate,
                    change.BuildImplemented != 0,
                    change.ImplementationPlanUsed != 0,
                    change.ChangeDeviation,
                    change.RecoveryPlanUsed != 0,
                    change.FinishingDate,
                    change.ImplementationReady != 0);
        }

        private static EvaluationFieldGroupDto CreateEvaluationFieldGroup(ChangeEntity change)
        {
            return new EvaluationFieldGroupDto(change.ChangeEvaluation, change.EvaluationReady != 0);
        }
    }
}