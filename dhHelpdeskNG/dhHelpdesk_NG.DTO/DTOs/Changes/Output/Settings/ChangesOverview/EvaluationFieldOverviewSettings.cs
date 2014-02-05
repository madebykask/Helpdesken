namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class EvaluationFieldOverviewSettings
    {
        public EvaluationFieldOverviewSettings(
            FieldOverviewSetting evaluation, FieldOverviewSetting evaluationReady)
        {
            this.Evaluation = evaluation;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public FieldOverviewSetting Evaluation { get; private set; }

        [NotNull]
        public FieldOverviewSetting EvaluationReady { get; private set; }
    }
}
