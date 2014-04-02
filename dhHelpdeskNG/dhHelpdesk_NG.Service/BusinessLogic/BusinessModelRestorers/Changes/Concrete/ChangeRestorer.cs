namespace DH.Helpdesk.Services.Infrastructure.BusinessModelRestorers.Changes.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;

    public sealed class ChangeRestorer : Restorer, IChangeRestorer
    {
        #region Public Methods and Operators

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

            this.RestoreOrderer(updatedChange.Orderer, existingChange.Orderer, settings.Orderer);
            this.RestoreGeneral(updatedChange.General, existingChange.General, settings.General);
            this.RestoreRegistration(updatedChange.Registration, existingChange.Registration, settings.Registration);
            this.RestoreAnalyze(updatedChange.Analyze, existingChange.Analyze, settings.Analyze);
           
            this.RestoreImplementation(
                updatedChange.Implementation,
                existingChange.Implementation,
                settings.Implementation);
            
            this.RestoreEvaluation(updatedChange.Evaluation, existingChange.Evaluation, settings.Evaluation);
        }

        #endregion

        #region Methods

        private void RestoreAnalyze(
            UpdatedAnalyzeFields updated,
            AnalyzeFields existing,
            AnalyzeProcessingSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.CategoryId, existing.CategoryId, settings.Category.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.PriorityId, existing.PriorityId, settings.Priority.Show);
       
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ResponsibleId,
                existing.ResponsibleId,
                settings.Responsible.Show);
            
            this.RestoreFieldIfNeeded(updated, () => updated.Solution, existing.Solution, settings.Solution.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Cost, existing.Cost, settings.Cost.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.YearlyCost, existing.YearlyCost, settings.YearlyCost.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.EstimatedTimeInHours,
                existing.EstimatedTimeInHours,
                settings.EstimatedTimeInHours.Show);

            this.RestoreFieldIfNeeded(updated, () => updated.Risk, existing.Risk, settings.Risk.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.StartDate, existing.StartDate, settings.StartDate.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.FinishDate, existing.FinishDate, settings.FinishDate.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.HasImplementationPlan,
                existing.HasImplementationPlan,
                settings.HasImplementationPlan.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.HasRecoveryPlan,
                existing.HasRecoveryPlan,
                settings.HasRecoveryPlan.Show);

            this.RestoreFieldIfNeeded(updated, () => updated.Approval, existing.Approval, settings.Approval.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RejectExplanation,
                existing.RejectExplanation,
                settings.RejectExplanation.Show);
        }

        private void RestoreEvaluation(
            UpdatedEvaluationFields updated,
            EvaluationFields existing,
            EvaluationProcessingSettings settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ChangeEvaluation,
                existing.ChangeEvaluation,
                settings.ChangeEvaluation.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.EvaluationReady,
                existing.EvaluationReady,
                settings.EvaluationReady.Show);
        }

        private void RestoreGeneral(
            UpdatedGeneralFields updated,
            GeneralFields existing,
            GeneralProcessingSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Priority, existing.Priority, settings.Priority.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Title, existing.Title, settings.Title.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.StatusId, existing.StatusId, settings.Status.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.SystemId, existing.SystemId, settings.System.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.ObjectId, existing.ObjectId, settings.Object.Show);
     
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.WorkingGroupId,
                existing.WorkingGroupId,
                settings.WorkingGroup.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AdministratorId,
                existing.AdministratorId,
                settings.Administrator.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.FinishingDate,
                existing.FinishingDate,
                settings.FinishingDate.Show);
        
            this.RestoreFieldIfNeeded(updated, () => updated.Rss, existing.Rss, settings.Rss.Show);
        }

        private void RestoreImplementation(
            UpdatedImplementationFields updated,
            ImplementationFields existing,
            ImplementationProcessingSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.StatusId, existing.StatusId, settings.Status.Show);
      
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RealStartDate,
                existing.RealStartDate,
                settings.RealStartDate.Show);
            
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.FinishingDate,
                existing.FinishingDate,
                settings.FinishingDate.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.BuildImplemented,
                existing.BuildImplemented,
                settings.BuildImplemented.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ImplementationPlanUsed,
                existing.ImplementationPlanUsed,
                settings.ImplementationPlanUsed.Show);

            this.RestoreFieldIfNeeded(updated, () => updated.Deviation, existing.Deviation, settings.Deviation.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RecoveryPlanUsed,
                existing.RecoveryPlanUsed,
                settings.RecoveryPlanUsed.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ImplementationReady,
                existing.ImplementationReady,
                settings.ImplementationReady.Show);
        }

        private void RestoreOrderer(
            UpdatedOrdererFields updated,
            OrdererFields existing,
            OrdererProcessingSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Id, existing.Id, settings.Id.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Name, existing.Name, settings.Name.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Phone, existing.Phone, settings.Phone.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.CellPhone, existing.CellPhone, settings.CellPhone.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Email, existing.Email, settings.Email.Show);
            
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DepartmentId,
                existing.DepartmentId,
                settings.Department.Show);
        }

        private void RestoreRegistration(
            UpdatedRegistrationFields updated,
            RegistrationFields existing,
            RegistrationProcessingSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.OwnerId, existing.OwnerId, settings.Owner.Show);
          
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Description,
                existing.Description,
                settings.Description.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.BusinessBenefits,
                existing.BusinessBenefits,
                settings.BusinessBenefits.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Consequence,
                existing.Consequence,
                settings.Consequence.Show);
          
            this.RestoreFieldIfNeeded(updated, () => updated.Impact, existing.Impact, settings.Impact.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Approval, existing.Approval, settings.Approval.Show);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RejectExplanation,
                existing.RejectExplanation,
                settings.RejectExplanation.Show);
        }

        #endregion
    }
}