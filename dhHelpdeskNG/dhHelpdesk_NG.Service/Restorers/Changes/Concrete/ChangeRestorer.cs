namespace DH.Helpdesk.Services.Restorers.Changes.Concrete
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;

    public sealed class ChangeRestorer : IChangeRestorer
    {
        public void Restore(UpdatedChange updatedChange, Change existingChange, ChangeProcessingSettings settings)
        {
            if (updatedChange.Orderer == null)
            {
                updatedChange.Orderer = UpdatedOrdererFields.CreateDefault();
            }

            if (updatedChange.Evaluation == null)
            {
                updatedChange.Evaluation = UpdatedEvaluationFields.CreateDefault();
            }

            RestoreOrderer(updatedChange.Orderer, existingChange.Orderer, settings.Orderer);
            RestoreGeneral(updatedChange.General, existingChange.General, settings.General);
            RestoreRegistration(updatedChange.Registration, existingChange.Registration, settings.Registration);
            RestoreAnalyze(updatedChange.Analyze, existingChange.Analyze, settings.Analyze);
            RestoreImplementation(updatedChange.Implementation, existingChange.Implementation, settings.Implementation);
            RestoreEvaluation(updatedChange.Evaluation, existingChange.Evaluation, settings.Evaluation);
        }

        private static void RestoreOrderer(
            UpdatedOrdererFields updated, OrdererFields existing, OrdererProcessingSettings settings)
        {
            RestoreFieldIfNeeded(updated, () => updated.Id, existing.Id, settings.Id);
            RestoreFieldIfNeeded(updated, () => updated.Name, existing.Name, settings.Name);
            RestoreFieldIfNeeded(updated, () => updated.Phone, existing.Phone, settings.Phone);
            RestoreFieldIfNeeded(updated, () => updated.CellPhone, existing.CellPhone, settings.CellPhone);
            RestoreFieldIfNeeded(updated, () => updated.Email, existing.Email, settings.Email);
            RestoreFieldIfNeeded(updated, () => updated.DepartmentId, existing.DepartmentId, settings.Department);
        }

        private static void RestoreGeneral(
            UpdatedGeneralFields updated, GeneralFields existing, GeneralProcessingSettings settings)
        {
            RestoreFieldIfNeeded(updated, () => updated.Priority, existing.Priority, settings.Priority);
            RestoreFieldIfNeeded(updated, () => updated.Title, existing.Title, settings.Title);
            RestoreFieldIfNeeded(updated, () => updated.StatusId, existing.StatusId, settings.Status);
            RestoreFieldIfNeeded(updated, () => updated.SystemId, existing.SystemId, settings.System);
            RestoreFieldIfNeeded(updated, () => updated.ObjectId, existing.ObjectId, settings.Object);
            RestoreFieldIfNeeded(updated, () => updated.WorkingGroupId, existing.WorkingGroupId, settings.WorkingGroup);

            RestoreFieldIfNeeded(
                updated, () => updated.AdministratorId, existing.AdministratorId, settings.Administrator);

            RestoreFieldIfNeeded(updated, () => updated.FinishingDate, existing.FinishingDate, settings.FinishingDate);
            RestoreFieldIfNeeded(updated, () => updated.Rss, existing.Rss, settings.Rss);
        }

        private static void RestoreRegistration(
            UpdatedRegistrationFields updated, RegistrationFields existing, RegistrationProcessingSettings settings)
        {
            RestoreFieldIfNeeded(updated, () => updated.OwnerId, existing.OwnerId, settings.Owner);
            RestoreFieldIfNeeded(updated, () => updated.Description, existing.Description, settings.Description);

            RestoreFieldIfNeeded(
                updated, () => updated.BusinessBenefits, existing.BusinessBenefits, settings.BusinessBenefits);

            RestoreFieldIfNeeded(updated, () => updated.Consequence, existing.Consequence, settings.Consequence);
            RestoreFieldIfNeeded(updated, () => updated.Impact, existing.Impact, settings.Impact);
            RestoreFieldIfNeeded(updated, () => updated.Approval, existing.Approval, settings.Approval);

            RestoreFieldIfNeeded(
                updated, () => updated.RejectExplanation, existing.RejectExplanation, settings.RejectExplanation);
        }

        private static void RestoreAnalyze(
            UpdatedAnalyzeFields updated, AnalyzeFields existing, AnalyzeProcessingSettings settings)
        {
            RestoreFieldIfNeeded(updated, () => updated.CategoryId, existing.CategoryId, settings.Category);
            RestoreFieldIfNeeded(updated, () => updated.PriorityId, existing.PriorityId, settings.Priority);
            RestoreFieldIfNeeded(updated, () => updated.ResponsibleId, existing.ResponsibleId, settings.Responsible);
            RestoreFieldIfNeeded(updated, () => updated.Solution, existing.Solution, settings.Solution);
            RestoreFieldIfNeeded(updated, () => updated.Cost, existing.Cost, settings.Cost);
            RestoreFieldIfNeeded(updated, () => updated.YearlyCost, existing.YearlyCost, settings.YearlyCost);

            RestoreFieldIfNeeded(
                updated,
                () => updated.EstimatedTimeInHours,
                existing.EstimatedTimeInHours,
                settings.EstimatedTimeInHours);

            RestoreFieldIfNeeded(updated, () => updated.Risk, existing.Risk, settings.Risk);
            RestoreFieldIfNeeded(updated, () => updated.StartDate, existing.StartDate, settings.StartDate);
            RestoreFieldIfNeeded(updated, () => updated.FinishDate, existing.FinishDate, settings.FinishDate);

            RestoreFieldIfNeeded(
                updated,
                () => updated.HasImplementationPlan,
                existing.HasImplementationPlan,
                settings.HasImplementationPlan);

            RestoreFieldIfNeeded(
                updated, () => updated.HasRecoveryPlan, existing.HasRecoveryPlan, settings.HasRecoveryPlan);

            RestoreFieldIfNeeded(updated, () => updated.Approval, existing.Approval, settings.Approval);

            RestoreFieldIfNeeded(
                updated, () => updated.RejectExplanation, existing.RejectExplanation, settings.RejectExplanation);
        }

        private static void RestoreImplementation(
            UpdatedImplementationFields updated,
            ImplementationFields existing,
            ImplementationProcessingSettings settings)
        {
            RestoreFieldIfNeeded(updated, () => updated.StatusId, existing.StatusId, settings.Status);
            RestoreFieldIfNeeded(updated, () => updated.RealStartDate, existing.RealStartDate, settings.RealStartDate);
            RestoreFieldIfNeeded(updated, () => updated.FinishingDate, existing.FinishingDate, settings.FinishingDate);

            RestoreFieldIfNeeded(
                updated, () => updated.BuildImplemented, existing.BuildImplemented, settings.BuildImplemented);

            RestoreFieldIfNeeded(
                updated,
                () => updated.ImplementationPlanUsed,
                existing.ImplementationPlanUsed,
                settings.ImplementationPlanUsed);

            RestoreFieldIfNeeded(updated, () => updated.Deviation, existing.Deviation, settings.Deviation);

            RestoreFieldIfNeeded(
                updated, () => updated.RecoveryPlanUsed, existing.RecoveryPlanUsed, settings.RecoveryPlanUsed);

            RestoreFieldIfNeeded(
                updated, () => updated.ImplementationReady, existing.ImplementationReady, settings.ImplementationReady);
        }

        private static void RestoreEvaluation(
            UpdatedEvaluationFields updated, EvaluationFields existing, EvaluationProcessingSettings settings)
        {
            RestoreFieldIfNeeded(
                updated, () => updated.ChangeEvaluation, existing.ChangeEvaluation, settings.ChangeEvaluation);

            RestoreFieldIfNeeded(
                updated, () => updated.EvaluationReady, existing.EvaluationReady, settings.EvaluationReady);
        }

        private static void RestoreFieldIfNeeded<TValue>(
            object sourceObject, Expression<Func<TValue>> property, object existingValue, FieldProcessingSetting setting)
        {
            if (setting.Show)
            {
                return;
            }

            var expressionMember = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)expressionMember.Member;
            propertyInfo.SetValue(sourceObject, existingValue, null);
        }
    }
}