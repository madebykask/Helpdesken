namespace DH.Helpdesk.Services.EntitiesRestorers.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;

    public sealed class ChangeRestorer
    {
//        public void Restore(UpdatedChange updatedChange, Change existingChange, ChangeProcessingSettings settings)
//        {
//            RestoreOrderer(updatedChange.Orderer, existingChange.Orderer, settings.Orderer);
//            RestoreGeneral(updatedChange.General, existingChange.General, settings.General);
//            RestoreRegistration(updatedChange.Registration, existingChange.Registration, settings.Registration);
//            RestoreAnalyze(updatedChange.Analyze, existingChange.Analyze, settings.Analyze);
//            RestoreImplementation(updatedChange.Implementation, existingChange.Implementation, settings.Implementation);
//            RestoreEvaluation(updatedChange.Evaluation, existingChange.Evaluation, settings.Evaluation);
//        }
//
//        private static void RestoreOrderer(
//            UpdatedOrdererFields updated, OrdererFields existing, OrdererProcessingSettings settings)
//        {
//            RestoreFieldIfNeeded(ref updated.Id, existing.Id, settings.Id);
//            RestoreFieldIfNeeded(ref updated.Name, existing.Name, settings.Name);
//            RestoreFieldIfNeeded(ref updated.Phone, existing.Phone, settings.Phone);
//            RestoreFieldIfNeeded(ref updated.CellPhone, existing.CellPhone, settings.CellPhone);
//            RestoreFieldIfNeeded(ref updated.Email, existing.Email, settings.Email);
//            RestoreFieldIfNeeded(ref updated.DepartmentId, existing.DepartmentId, settings.Department);
//        }
//
//        private static void RestoreGeneral(
//            UpdatedGeneralFields updated, GeneralFields existing, GeneralProcessingSettings settings)
//        {
//            RestoreFieldIfNeeded(ref updated.Priority, existing.Priority, settings.Priority);
//            RestoreFieldIfNeeded(ref updated.Title, existing.Title, settings.Title);
//            RestoreFieldIfNeeded(ref updated.StatusId, existing.StatusId, settings.Status);
//            RestoreFieldIfNeeded(ref updated.SystemId, existing.SystemId, settings.System);
//            RestoreFieldIfNeeded(ref updated.ObjectId, existing.ObjectId, settings.Object);
//            RestoreFieldIfNeeded(ref updated.WorkingGroupId, existing.WorkingGroupId, settings.WorkingGroup);
//            RestoreFieldIfNeeded(ref updated.AdministratorId, existing.AdministratorId, settings.Administrator);
//            RestoreFieldIfNeeded(ref updated.FinishingDate, existing.FinishingDate, settings.FinishingDate);
//            RestoreFieldIfNeeded(ref updated.Rss, existing.Rss, settings.Rss);
//        }
//
//        private static void RestoreRegistration(
//            UpdatedRegistrationFields updated, RegistrationFields existing, RegistrationProcessingSettings settings)
//        {
//            RestoreFieldIfNeeded(ref updated.OwnerId, existing.OwnerId, settings.Owner);
//            RestoreFieldIfNeeded(ref updated.Description, existing.Description, settings.Description);
//            RestoreFieldIfNeeded(ref updated.BusinessBenefits, existing.BusinessBenefits, settings.BusinessBenefits);
//            RestoreFieldIfNeeded(ref updated.Consequence, existing.Consequence, settings.Consequence);
//            RestoreFieldIfNeeded(ref updated.Impact, existing.Impact, settings.Impact);
//            RestoreFieldIfNeeded(ref updated.Approval, existing.Approval, settings.Approval);
//            RestoreFieldIfNeeded(ref updated.RejectExplanation, existing.RejectExplanation, settings.RejectExplanation);
//        }
//
//        private static void RestoreAnalyze(
//            UpdatedAnalyzeFields updated, AnalyzeFields existing, AnalyzeProcessingSettings settings)
//        {
//            RestoreFieldIfNeeded(ref updated.CategoryId, existing.CategoryId, settings.Category);
//            RestoreFieldIfNeeded(ref updated.PriorityId, existing.PriorityId, settings.Priority);
//            RestoreFieldIfNeeded(ref updated.ResponsibleId, existing.ResponsibleId, settings.Responsible);
//            RestoreFieldIfNeeded(ref updated.Solution, existing.Solution, settings.Solution);
//            RestoreFieldIfNeeded(ref updated.Cost, existing.Cost, settings.Cost);
//            RestoreFieldIfNeeded(ref updated.YearlyCost, existing.YearlyCost, settings.YearlyCost);
//
//            RestoreFieldIfNeeded(
//                ref updated.EstimatedTimeInHours, existing.EstimatedTimeInHours, settings.EstimatedTimeInHours);
//
//            RestoreFieldIfNeeded(ref updated.Risk, existing.Risk, settings.Risk);
//            RestoreFieldIfNeeded(ref updated.StartDate, existing.StartDate, settings.StartDate);
//            RestoreFieldIfNeeded(ref updated.FinishDate, existing.FinishDate, settings.FinishDate);
//
//            RestoreFieldIfNeeded(
//                ref updated.HasImplementationPlan, existing.HasImplementationPlan, settings.HasImplementationPlan);
//
//            RestoreFieldIfNeeded(ref updated.HasRecoveryPlan, existing.HasRecoveryPlan, settings.HasRecoveryPlan);
//            RestoreFieldIfNeeded(ref updated.Approval, existing.Approval, settings.Approval);
//            RestoreFieldIfNeeded(ref updated.RejectExplanation, existing.RejectExplanation, settings.RejectExplanation);
//        }
//
//        private static void RestoreImplementation(
//            UpdatedImplementationFields updated,
//            ImplementationFields existing,
//            ImplementationProcessingSettings settings)
//        {
//            RestoreFieldIfNeeded(ref updated.StatusId, existing.StatusId, settings.Status);
//            RestoreFieldIfNeeded(ref updated.RealStartDate, existing.RealStartDate, settings.RealStartDate);
//            RestoreFieldIfNeeded(ref updated.FinishingDate, existing.FinishingDate, settings.FinishingDate);
//            RestoreFieldIfNeeded(ref updated.BuildImplemented, existing.BuildImplemented, settings.BuildImplemented);
//
//            RestoreFieldIfNeeded(
//                ref updated.ImplementationPlanUsed, existing.ImplementationPlanUsed, settings.ImplementationPlanUsed);
//
//            RestoreFieldIfNeeded(ref updated.Deviation, existing.Deviation, settings.Deviation);
//            RestoreFieldIfNeeded(ref updated.RecoveryPlanUsed, existing.RecoveryPlanUsed, settings.RecoveryPlanUsed);
//         
//            RestoreFieldIfNeeded(
//                ref updated.ImplementationReady, existing.ImplementationReady, settings.ImplementationReady);
//        }
//
//        private static void RestoreEvaluation(
//            UpdatedEvaluationFields updated,
//            EvaluationFields existing,
//            EvaluationProcessingSettings settings)
//        {
//            RestoreFieldIfNeeded(ref updated.ChangeEvaluation, existing.ChangeEvaluation, settings.ChangeEvaluation);
//            RestoreFieldIfNeeded(ref updated.EvaluationReady, existing.EvaluationReady, settings.EvaluationReady);
//        }
//
//        private static void RestoreFieldIfNeeded<TValue>(Func<TValue> to, TValue from, FieldProcessingSetting setting)
//        {
//            if (!setting.Show)
//            {
//                to.Invoke()
//                to = from;
//            }
//        }
    }
}