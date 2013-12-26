namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class UpdatedFieldSettingsDto
    {
        public UpdatedFieldSettingsDto(
            int customerId,
            int languageId,
            UpdatedOrdererFieldSettingGroupDto ordererFieldSettingGroup,
            UpdatedGeneralFieldSettingGroupDto generalFieldSettingGroup,
            UpdatedRegistrationFieldSettingGroupDto registrationFieldSettingGroup,
            UpdatedAnalyzeFieldSettingGroupDto analyzeFieldSettingGroup,
            UpdatedImplementationFieldSettingGroupDto implementationFieldSettingGroup,
            UpdatedEvaluationFieldSettingGroupDto evaluationFieldSettingGroup,
            UpdatedLogFieldSettingGroupDto logFieldSettingGroup)
        {
            ArgumentsValidator.IsId(customerId, "customerId");
            ArgumentsValidator.IsId(languageId, "languageId");
            ArgumentsValidator.NotNull(ordererFieldSettingGroup, "ordererFieldSettingGroup");
            ArgumentsValidator.NotNull(generalFieldSettingGroup, "generalFieldSettingGroup");
            ArgumentsValidator.NotNull(registrationFieldSettingGroup, "registrationFieldSettingGroup");
            ArgumentsValidator.NotNull(analyzeFieldSettingGroup, "analyzeFieldSettingGroup");
            ArgumentsValidator.NotNull(implementationFieldSettingGroup, "implementationFieldSettingGroup");
            ArgumentsValidator.NotNull(evaluationFieldSettingGroup, "evaluationFieldSettingGroup");
            ArgumentsValidator.NotNull(logFieldSettingGroup, "logFieldSettingGroup");

            CustomerId = customerId;
            LanguageId = languageId;
            OrdererFieldSettingGroup = ordererFieldSettingGroup;
            GeneralFieldSettingGroup = generalFieldSettingGroup;
            RegistrationFieldSettingGroup = registrationFieldSettingGroup;
            AnalyzeFieldSettingGroup = analyzeFieldSettingGroup;
            ImplementationFieldSettingGroup = implementationFieldSettingGroup;
            EvaluationFieldSettingGroup = evaluationFieldSettingGroup;
            LogFieldSettingGroup = logFieldSettingGroup;
        }

        public int CustomerId { get; private set; }

        public int LanguageId { get; private set; }

        public UpdatedOrdererFieldSettingGroupDto OrdererFieldSettingGroup { get; private set; }

        public UpdatedGeneralFieldSettingGroupDto GeneralFieldSettingGroup { get; private set; }

        public UpdatedRegistrationFieldSettingGroupDto RegistrationFieldSettingGroup { get; private set; }

        public UpdatedAnalyzeFieldSettingGroupDto AnalyzeFieldSettingGroup { get; private set; }

        public UpdatedImplementationFieldSettingGroupDto ImplementationFieldSettingGroup { get; private set; }

        public UpdatedEvaluationFieldSettingGroupDto EvaluationFieldSettingGroup { get; private set; }

        public UpdatedLogFieldSettingGroupDto LogFieldSettingGroup { get; private set; }
    }
}
