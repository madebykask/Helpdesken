namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    public sealed class UpdatedEvaluationFields
    {
        public UpdatedEvaluationFields(string changeEvaluation, bool evaluationReady, string logNote)
        {
            this.LogNote = logNote;
            this.ChangeEvaluation = changeEvaluation;
            this.EvaluationReady = evaluationReady;

        }

        public string ChangeEvaluation { get; private set; }

        public bool EvaluationReady { get; private set; }

        public string LogNote { get; private set; }

        public static UpdatedEvaluationFields CreateEmpty()
        {
            return new UpdatedEvaluationFields(null, false, string.Empty);
        }
    }
}
