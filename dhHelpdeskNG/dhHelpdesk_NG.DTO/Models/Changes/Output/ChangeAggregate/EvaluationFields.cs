namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate
{
    public sealed class EvaluationFields
    {
        public EvaluationFields(string changeEvaluation, bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
