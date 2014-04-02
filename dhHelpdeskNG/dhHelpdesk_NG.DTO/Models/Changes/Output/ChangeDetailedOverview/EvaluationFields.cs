namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview
{
    using DH.Helpdesk.BusinessData.Enums.Changes;

    public sealed class EvaluationFields
    {
        public EvaluationFields(string changeEvaluation, StepStatus evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        public StepStatus EvaluationReady { get; private set; }
    }
}