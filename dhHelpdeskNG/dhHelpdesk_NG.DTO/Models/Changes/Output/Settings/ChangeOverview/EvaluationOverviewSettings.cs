namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EvaluationOverviewSettings
    {
        public EvaluationOverviewSettings(
            FieldOverviewSetting changeEvaluation, FieldOverviewSetting evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public FieldOverviewSetting ChangeEvaluation { get; private set; }

        [NotNull]
        public FieldOverviewSetting EvaluationReady { get; private set; }
    }
}
