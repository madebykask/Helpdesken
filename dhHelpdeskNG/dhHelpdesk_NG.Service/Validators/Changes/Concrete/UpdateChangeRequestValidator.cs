namespace DH.Helpdesk.Services.Validators.Changes.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;
    using DH.Helpdesk.BusinessData.Requests.Changes;
    using DH.Helpdesk.Common.Exceptions;

    public sealed class UpdateChangeRequestValidator : ElementaryRulesValidator, IUpdateChangeRequestValidator
    {
        public void Validate(UpdateChangeRequest request, Change existingChange, ChangeProcessingSettings settings)
        {
            var errors = new List<FieldValidationError>();

            this.ValidateOrdererFields(request.Change.Orderer, existingChange.Orderer, settings.Orderer, errors);
            this.ValidateGeneralFields(request.Change.General, existingChange.General, settings.General, errors);

            this.ValidateRegistrationFields(
                request.Change.Registration, existingChange.Registration, settings.Registration, errors);

            this.ValidateAnalyzeFields(request.Change.Analyze, existingChange.Analyze, settings.Analyze, errors);

            this.ValidateImplementationFields(
                request.Change.Implementation, existingChange.Implementation, settings.Implementation, errors);

            this.ValidateEvaluationFields(
                request.Change.Evaluation, existingChange.Evaluation, settings.Evaluation, errors);
        }

        private void ValidateOrdererFields(
            UpdatedOrdererFields updatedFields,
            OrdererFields existingFields,
            OrdererProcessingSettings settings,
            List<FieldValidationError> errors)
        {
            this.ValidateStringField(
                updatedFields.Id,
                existingFields.Id,
                "",
                new FieldValidationSetting(!settings.Id.Show, settings.Name.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Name,
                existingFields.Name,
                "",
                new FieldValidationSetting(!settings.Name.Show, settings.Name.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Phone,
                existingFields.Phone,
                "",
                new FieldValidationSetting(!settings.Phone.Show, settings.Phone.Required),
                errors);

            this.ValidateStringField(
                updatedFields.CellPhone,
                existingFields.CellPhone,
                "",
                new FieldValidationSetting(!settings.CellPhone.Show, settings.CellPhone.Required),
                errors);
        }

        private void ValidateGeneralFields(
            UpdatedGeneralFields updatedFields,
            GeneralFields existingFields,
            GeneralProcessingSettings settings,
            List<FieldValidationError> errors)
        {
            this.ValidateIntegerField(
                updatedFields.Priority,
                existingFields.Priority,
                "",
                new FieldValidationSetting(!settings.Priority.Show, settings.Priority.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Title,
                existingFields.Title,
                "",
                new FieldValidationSetting(!settings.Title.Show, settings.Title.Required),
                errors);

            this.ValidateIntegerField(
                updatedFields.StatusId,
                existingFields.StatusId,
                "",
                new FieldValidationSetting(!settings.Status.Show, settings.Status.Required),
                errors);

            this.ValidateIntegerField(
                updatedFields.SystemId,
                existingFields.SystemId,
                "",
                new FieldValidationSetting(!settings.System.Show, settings.System.Required),
                errors);

            this.ValidateIntegerField(
                updatedFields.ObjectId,
                existingFields.ObjectId,
                "",
                new FieldValidationSetting(!settings.Object.Show, settings.Object.Required),
                errors);

            this.ValidateIntegerField(
                updatedFields.WorkingGroupId,
                existingFields.WorkingGroupId,
                "",
                new FieldValidationSetting(!settings.WorkingGroup.Show, settings.WorkingGroup.Required),
                errors);

            this.ValidateIntegerField(
                updatedFields.AdministratorId,
                existingFields.AdministratorId,
                "",
                new FieldValidationSetting(!settings.Administrator.Show, settings.Administrator.Required),
                errors);

            // Validate finishing date

            this.ValidateBooleanField(
                updatedFields.Rss,
                existingFields.Rss,
                "",
                new FieldValidationSetting(!settings.Rss.Show, settings.Rss.Required),
                errors);
        }

        private void ValidateRegistrationFields(
            UpdatedRegistrationFields updatedFields,
            RegistrationFields existingFields,
            RegistrationProcessingSettings settings,
            List<FieldValidationError> errors)
        {
            this.ValidateIntegerField(
                updatedFields.OwnerId,
                existingFields.OwnerId,
                "",
                new FieldValidationSetting(!settings.Owner.Show, settings.Owner.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Description,
                existingFields.Description,
                "",
                new FieldValidationSetting(!settings.Description.Show, settings.Description.Required),
                errors);

            this.ValidateStringField(
                updatedFields.BusinessBenefits,
                existingFields.BusinessBenefits,
                "",
                new FieldValidationSetting(!settings.BusinessBenefits.Show, settings.BusinessBenefits.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Consequence,
                existingFields.Consequence,
                "",
                new FieldValidationSetting(!settings.Consequence.Show, settings.Consequence.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Impact,
                existingFields.Impact,
                "",
                new FieldValidationSetting(!settings.Impact.Show, settings.Impact.Required),
                errors);

            // Validate desired date

            this.ValidateBooleanField(
                updatedFields.Verified,
                existingFields.Verified,
                "",
                new FieldValidationSetting(!settings.Verified.Show, settings.Verified.Required),
                errors);

            // Validate approval

            this.ValidateStringField(
                updatedFields.RejectExplanation,
                existingFields.RejectExplanation,
                "",
                new FieldValidationSetting(!settings.RejectExplanation.Show, settings.RejectExplanation.Required),
                errors);
        }

        private void ValidateAnalyzeFields(
            UpdatedAnalyzeFields updatedFields,
            AnalyzeFields existingFields,
            AnalyzeProcessingSettings settings,
            List<FieldValidationError> errors)
        {
            this.ValidateIntegerField(
                updatedFields.CategoryId,
                existingFields.CategoryId,
                "",
                new FieldValidationSetting(!settings.Category.Show, settings.Category.Required),
                errors);

            this.ValidateIntegerField(
                updatedFields.PriorityId,
                existingFields.PriorityId,
                "",
                new FieldValidationSetting(!settings.Priority.Show, settings.Priority.Required),
                errors);

            this.ValidateIntegerField(
                updatedFields.ResponsibleUserId,
                existingFields.ResponsibleId,
                "",
                new FieldValidationSetting(!settings.Responsible.Show, settings.Responsible.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Solution,
                existingFields.Solution,
                "",
                new FieldValidationSetting(!settings.Solution.Show, settings.Solution.Required),
                errors);

            // Validate cost
            // Validate yearly cost

            this.ValidateIntegerField(
                updatedFields.EstimatedTimeInHours,
                existingFields.EstimatedTimeInHours,
                "",
                new FieldValidationSetting(!settings.EstimatedTimeInHours.Show, settings.EstimatedTimeInHours.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Risk,
                existingFields.Risk,
                "",
                new FieldValidationSetting(!settings.Risk.Show, settings.Risk.Required),
                errors);

            // Validate start date
            // Validate finish date

            this.ValidateBooleanField(
                updatedFields.HasImplementationPlan,
                existingFields.HasImplementationPlan,
                "",
                new FieldValidationSetting(
                    !settings.HasImplementationPlan.Show, settings.HasImplementationPlan.Required),
                errors);

            this.ValidateBooleanField(
                updatedFields.HasRecoveryPlan,
                existingFields.HasRecoveryPlan,
                "",
                new FieldValidationSetting(!settings.HasRecoveryPlan.Show, settings.HasRecoveryPlan.Required),
                errors);

            // Validate approval

            this.ValidateStringField(
                updatedFields.RejectExplanation,
                existingFields.RejectExplanation,
                "",
                new FieldValidationSetting(!settings.RejectExplanation.Show, settings.RejectExplanation.Required),
                errors);
        }

        private void ValidateImplementationFields(
            UpdatedImplementationFields updatedFields,
            ImplementationFields existingFields,
            ImplementationProcessingSettings settings,
            List<FieldValidationError> errors)
        {
            this.ValidateIntegerField(
                updatedFields.StatusId,
                existingFields.StatusId,
                "",
                new FieldValidationSetting(!settings.Status.Show, settings.Status.Required),
                errors);

            // Validate real start date
            // Validate finishing date

            this.ValidateBooleanField(
                updatedFields.BuildImplemented,
                existingFields.BuildImplemented,
                "",
                new FieldValidationSetting(!settings.BuildImplemented.Show, settings.BuildImplemented.Required),
                errors);

            this.ValidateBooleanField(
                updatedFields.ImplementationPlanUsed,
                existingFields.ImplementationPlanUsed,
                "",
                new FieldValidationSetting(
                    !settings.ImplementationPlanUsed.Show, settings.ImplementationPlanUsed.Required),
                errors);

            this.ValidateStringField(
                updatedFields.Deviation,
                existingFields.Deviation,
                "",
                new FieldValidationSetting(!settings.Deviation.Show, settings.Deviation.Required),
                errors);

            this.ValidateBooleanField(
                updatedFields.RecoveryPlanUsed,
                existingFields.RecoveryPlanUsed,
                "",
                new FieldValidationSetting(!settings.RecoveryPlanUsed.Show, settings.RecoveryPlanUsed.Required),
                errors);

            this.ValidateBooleanField(
                updatedFields.ImplementationReady,
                existingFields.ImplementationReady,
                "",
                new FieldValidationSetting(!settings.ImplementationReady.Show, settings.ImplementationReady.Required),
                errors);
        }

        private void ValidateEvaluationFields(
            UpdatedEvaluationFields updatedFields,
            EvaluationFields existingFields,
            EvaluationProcessingSettings settings,
            List<FieldValidationError> errors)
        {
            this.ValidateStringField(
                updatedFields.ChangeEvaluation,
                existingFields.ChangeEvaluation,
                "",
                new FieldValidationSetting(!settings.ChangeEvaluation.Show, settings.ChangeEvaluation.Required),
                errors);

            this.ValidateBooleanField(
                updatedFields.EvaluationReady,
                existingFields.EvaluationReady,
                "",
                new FieldValidationSetting(!settings.EvaluationReady.Show, settings.EvaluationReady.Required),
                errors);
        }
    }
}
