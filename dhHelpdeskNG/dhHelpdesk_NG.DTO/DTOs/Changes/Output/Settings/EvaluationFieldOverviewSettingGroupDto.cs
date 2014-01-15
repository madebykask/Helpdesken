namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class EvaluationFieldOverviewSettingGroupDto
    {
        public EvaluationFieldOverviewSettingGroupDto(
            FieldOverviewSettingDto evaluation, FieldOverviewSettingDto evaluationReady)
        {
            Evaluation = evaluation;
            EvaluationReady = evaluationReady;
        }

        [NotNull]
        public FieldOverviewSettingDto Evaluation { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto EvaluationReady { get; private set; }
    }
}
