namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate
{
    public sealed class UpdatedEvaluationFields
    {
        public UpdatedEvaluationFields(string changeEvaluation, NewLog newLog, bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.NewLog = newLog;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        public NewLog NewLog { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
