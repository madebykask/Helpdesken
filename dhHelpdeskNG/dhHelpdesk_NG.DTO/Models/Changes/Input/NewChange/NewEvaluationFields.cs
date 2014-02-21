namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    public sealed class NewEvaluationFields
    {
        private NewEvaluationFields(string changeEvaluation, bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        public bool EvaluationReady { get; private set; }

        internal static NewEvaluationFields CreateDefault()
        {
            return new NewEvaluationFields(null, false);
        }
    }
}
