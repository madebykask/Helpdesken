namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Changes.Concrete
{
    using DH.Helpdesk.BusinessData.Enums.Changes.Fields;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;
    using DH.Helpdesk.Services.Requests.Changes;

    public sealed class UpdateChangeRequestValidator : IUpdateChangeRequestValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public UpdateChangeRequestValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        public void Validate(UpdateChangeRequest request, Change existingChange, ChangeProcessingSettings settings)
        {
            this.ValidateOrdererFields(request.Change.Orderer, existingChange.Orderer, settings.Orderer);
            this.ValidateGeneralFields(request.Change.General, existingChange.General, settings.General);

            this.ValidateRegistrationFields(
                request.Change.Registration, existingChange.Registration, settings.Registration);

            this.ValidateAnalyzeFields(request.Change.Analyze, existingChange.Analyze, settings.Analyze);

            this.ValidateImplementationFields(
                request.Change.Implementation, existingChange.Implementation, settings.Implementation);

            this.ValidateEvaluationFields(request.Change.Evaluation, existingChange.Evaluation, settings.Evaluation);
        }

        private void ValidateOrdererFields(
            UpdatedOrdererFields updatedFields, OrdererFields existingFields, OrdererProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Id,
                existingFields.Id,
                OrdererField.Id,
                new ElementaryValidationRule(!settings.Id.Show, settings.Id.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Name,
                existingFields.Name,
                OrdererField.Name,
                new ElementaryValidationRule(!settings.Name.Show, settings.Name.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Phone,
                existingFields.Phone,
                OrdererField.Phone,
                new ElementaryValidationRule(!settings.Phone.Show, settings.Phone.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.CellPhone,
                existingFields.CellPhone,
                OrdererField.CellPhone,
                new ElementaryValidationRule(!settings.CellPhone.Show, settings.CellPhone.Required));
        }

        private void ValidateGeneralFields(
            UpdatedGeneralFields updatedFields, GeneralFields existingFields, GeneralProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.Priority,
                existingFields.Priority,
                GeneralField.Priority,
                new ElementaryValidationRule(!settings.Priority.Show, settings.Priority.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Title,
                existingFields.Title,
                GeneralField.Title,
                new ElementaryValidationRule(!settings.Title.Show, settings.Title.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.StatusId,
                existingFields.StatusId,
                GeneralField.Status,
                new ElementaryValidationRule(!settings.Status.Show, settings.Status.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.SystemId,
                existingFields.SystemId,
                GeneralField.System,
                new ElementaryValidationRule(!settings.System.Show, settings.System.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.ObjectId,
                existingFields.ObjectId,
                GeneralField.Object,
                new ElementaryValidationRule(!settings.Object.Show, settings.Object.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.WorkingGroupId,
                existingFields.WorkingGroupId,
                GeneralField.WorkingGroup,
                new ElementaryValidationRule(!settings.WorkingGroup.Show, settings.WorkingGroup.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.AdministratorId,
                existingFields.AdministratorId,
                GeneralField.Administrator,
                new ElementaryValidationRule(!settings.Administrator.Show, settings.Administrator.Required));

            this.elementaryRulesValidator.ValidateDateTimeField(
                updatedFields.FinishingDate,
                existingFields.FinishingDate,
                GeneralField.FinishingDate,
                new ElementaryValidationRule(!settings.FinishingDate.Show, settings.FinishingDate.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.Rss,
                existingFields.Rss,
                GeneralField.Rss,
                new ElementaryValidationRule(!settings.Rss.Show, settings.Rss.Required));
        }

        private void ValidateRegistrationFields(
            UpdatedRegistrationFields updatedFields,
            RegistrationFields existingFields,
            RegistrationProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.OwnerId,
                existingFields.OwnerId,
                RegistrationField.Owner,
                new ElementaryValidationRule(!settings.Owner.Show, settings.Owner.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Description,
                existingFields.Description,
                RegistrationField.Description,
                new ElementaryValidationRule(!settings.Description.Show, settings.Description.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.BusinessBenefits,
                existingFields.BusinessBenefits,
                RegistrationField.BusinessBenefits,
                new ElementaryValidationRule(!settings.BusinessBenefits.Show, settings.BusinessBenefits.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Consequence,
                existingFields.Consequence,
                RegistrationField.Consequence,
                new ElementaryValidationRule(!settings.Consequence.Show, settings.Consequence.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Impact,
                existingFields.Impact,
                RegistrationField.Impact,
                new ElementaryValidationRule(!settings.Impact.Show, settings.Impact.Required));

            this.elementaryRulesValidator.ValidateDateTimeField(
                updatedFields.DesiredDate,
                existingFields.DesiredDate,
                RegistrationField.DesiredDate,
                new ElementaryValidationRule(!settings.DesiredDate.Show, settings.DesiredDate.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.Verified,
                existingFields.Verified,
                RegistrationField.Verified,
                new ElementaryValidationRule(!settings.Verified.Show, settings.Verified.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                (int)updatedFields.Approval,
                (int)existingFields.Approval,
                RegistrationField.Approval,
                new ElementaryValidationRule(!settings.Approval.Show, settings.Approval.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.RejectExplanation,
                existingFields.RejectExplanation,
                RegistrationField.RejectExplanation,
                new ElementaryValidationRule(!settings.RejectExplanation.Show, settings.RejectExplanation.Required));
        }

        private void ValidateAnalyzeFields(
            UpdatedAnalyzeFields updatedFields, AnalyzeFields existingFields, AnalyzeProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.CategoryId,
                existingFields.CategoryId,
                AnalyzeField.Category,
                new ElementaryValidationRule(!settings.Category.Show, settings.Category.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.PriorityId,
                existingFields.PriorityId,
                AnalyzeField.Priority,
                new ElementaryValidationRule(!settings.Priority.Show, settings.Priority.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.ResponsibleId,
                existingFields.ResponsibleId,
                AnalyzeField.Responsible,
                new ElementaryValidationRule(!settings.Responsible.Show, settings.Responsible.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Solution,
                existingFields.Solution,
                AnalyzeField.Solution,
                new ElementaryValidationRule(!settings.Solution.Show, settings.Solution.Required));

            this.elementaryRulesValidator.ValidateRealField(
                updatedFields.Cost,
                existingFields.Cost,
                AnalyzeField.Cost,
                new ElementaryValidationRule(!settings.Cost.Show, settings.Cost.Required));

            this.elementaryRulesValidator.ValidateRealField(
                updatedFields.YearlyCost,
                existingFields.YearlyCost,
                AnalyzeField.YearlyCost,
                new ElementaryValidationRule(!settings.YearlyCost.Show, settings.YearlyCost.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.EstimatedTimeInHours,
                existingFields.EstimatedTimeInHours,
                AnalyzeField.EstimatedTimeInHours,
                new ElementaryValidationRule(
                    !settings.EstimatedTimeInHours.Show, settings.EstimatedTimeInHours.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Risk,
                existingFields.Risk,
                AnalyzeField.Risk,
                new ElementaryValidationRule(!settings.Risk.Show, settings.Risk.Required));

            this.elementaryRulesValidator.ValidateDateTimeField(
                updatedFields.StartDate,
                existingFields.StartDate,
                AnalyzeField.StartDate,
                new ElementaryValidationRule(!settings.StartDate.Show, settings.StartDate.Required));

            this.elementaryRulesValidator.ValidateDateTimeField(
                updatedFields.FinishDate,
                existingFields.FinishDate,
                AnalyzeField.FinishDate,
                new ElementaryValidationRule(!settings.FinishDate.Show, settings.FinishDate.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.HasImplementationPlan,
                existingFields.HasImplementationPlan,
                AnalyzeField.HasImplementationPlan,
                new ElementaryValidationRule(
                    !settings.HasImplementationPlan.Show, settings.HasImplementationPlan.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.HasRecoveryPlan,
                existingFields.HasRecoveryPlan,
                AnalyzeField.HasRecoveryPlan,
                new ElementaryValidationRule(!settings.HasRecoveryPlan.Show, settings.HasRecoveryPlan.Required));

            this.elementaryRulesValidator.ValidateIntegerField(
                (int)updatedFields.Approval,
                (int)existingFields.Approval,
                AnalyzeField.Approval,
                new ElementaryValidationRule(!settings.Approval.Show, settings.Approval.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.RejectExplanation,
                existingFields.RejectExplanation,
                AnalyzeField.RejectExplanation,
                new ElementaryValidationRule(!settings.RejectExplanation.Show, settings.RejectExplanation.Required));
        }

        private void ValidateImplementationFields(
            UpdatedImplementationFields updatedFields,
            ImplementationFields existingFields,
            ImplementationProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedFields.StatusId,
                existingFields.StatusId,
                ImplementationField.Status,
                new ElementaryValidationRule(!settings.Status.Show, settings.Status.Required));

            this.elementaryRulesValidator.ValidateDateTimeField(
                updatedFields.RealStartDate,
                existingFields.RealStartDate,
                ImplementationField.RealStartDate,
                new ElementaryValidationRule(!settings.RealStartDate.Show, settings.RealStartDate.Required));

            this.elementaryRulesValidator.ValidateDateTimeField(
                updatedFields.FinishingDate,
                existingFields.FinishingDate,
                ImplementationField.FinishingDate,
                new ElementaryValidationRule(!settings.FinishingDate.Show, settings.FinishingDate.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.BuildImplemented,
                existingFields.BuildImplemented,
                ImplementationField.BuildImplemented,
                new ElementaryValidationRule(!settings.BuildImplemented.Show, settings.BuildImplemented.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.ImplementationPlanUsed,
                existingFields.ImplementationPlanUsed,
                ImplementationField.ImplementationPlanUsed,
                new ElementaryValidationRule(
                    !settings.ImplementationPlanUsed.Show, settings.ImplementationPlanUsed.Required));

            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.Deviation,
                existingFields.Deviation,
                ImplementationField.Deviation,
                new ElementaryValidationRule(!settings.Deviation.Show, settings.Deviation.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.RecoveryPlanUsed,
                existingFields.RecoveryPlanUsed,
                ImplementationField.RecoveryPlanUsed,
                new ElementaryValidationRule(!settings.RecoveryPlanUsed.Show, settings.RecoveryPlanUsed.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.ImplementationReady,
                existingFields.ImplementationReady,
                ImplementationField.ImplementationReady,
                new ElementaryValidationRule(!settings.ImplementationReady.Show, settings.ImplementationReady.Required));
        }

        private void ValidateEvaluationFields(
            UpdatedEvaluationFields updatedFields,
            EvaluationFields existingFields,
            EvaluationProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updatedFields.ChangeEvaluation,
                existingFields.ChangeEvaluation,
                EvaluationField.ChangeEvaluation,
                new ElementaryValidationRule(!settings.ChangeEvaluation.Show, settings.ChangeEvaluation.Required));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedFields.EvaluationReady,
                existingFields.EvaluationReady,
                EvaluationField.EvaluationReady,
                new ElementaryValidationRule(!settings.EvaluationReady.Show, settings.EvaluationReady.Required));
        }
    }
}