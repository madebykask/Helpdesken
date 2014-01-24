namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.OrdererFieldSettingGroup = ordererFieldSettingGroup;
            this.GeneralFieldSettingGroup = generalFieldSettingGroup;
            this.RegistrationFieldSettingGroup = registrationFieldSettingGroup;
            this.AnalyzeFieldSettingGroup = analyzeFieldSettingGroup;
            this.ImplementationFieldSettingGroup = implementationFieldSettingGroup;
            this.EvaluationFieldSettingGroup = evaluationFieldSettingGroup;
            this.LogFieldSettingGroup = logFieldSettingGroup;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }

        [NotNull]
        public UpdatedOrdererFieldSettingGroupDto OrdererFieldSettingGroup { get; private set; }

        [NotNull]
        public UpdatedGeneralFieldSettingGroupDto GeneralFieldSettingGroup { get; private set; }

        [NotNull]
        public UpdatedRegistrationFieldSettingGroupDto RegistrationFieldSettingGroup { get; private set; }

        [NotNull]
        public UpdatedAnalyzeFieldSettingGroupDto AnalyzeFieldSettingGroup { get; private set; }

        [NotNull]
        public UpdatedImplementationFieldSettingGroupDto ImplementationFieldSettingGroup { get; private set; }

        [NotNull]
        public UpdatedEvaluationFieldSettingGroupDto EvaluationFieldSettingGroup { get; private set; }

        [NotNull]
        public UpdatedLogFieldSettingGroupDto LogFieldSettingGroup { get; private set; }
    }
}
