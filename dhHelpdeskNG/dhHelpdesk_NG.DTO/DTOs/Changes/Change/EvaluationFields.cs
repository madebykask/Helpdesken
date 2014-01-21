namespace dhHelpdesk_NG.DTO.DTOs.Changes.Change
{
    public sealed class EvaluationFields
    {
        public EvaluationFields(string changeEvaluation, bool ready)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.Ready = ready;

        }

        public string ChangeEvaluation { get; private set; }

        public bool Ready { get; private set; }
    }
}
