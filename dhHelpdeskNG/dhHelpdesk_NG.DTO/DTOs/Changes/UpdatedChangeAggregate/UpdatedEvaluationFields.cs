namespace dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChangeAggregate
{
    public sealed class UpdatedEvaluationFields
    {
        public UpdatedEvaluationFields(string changeEvaluation, bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
