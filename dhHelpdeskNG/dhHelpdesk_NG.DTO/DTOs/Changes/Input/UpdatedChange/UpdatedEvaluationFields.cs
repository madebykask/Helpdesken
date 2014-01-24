namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChange
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
