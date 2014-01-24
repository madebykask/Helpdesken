namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate
{
    public sealed class NewEvaluationAggregateFields
    {
        public NewEvaluationAggregateFields(string changeEvaluation, bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
