namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChangeAggregate
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewEvaluationAggregateFields
    {
        public NewEvaluationAggregateFields(
            string changeEvaluation, 
            List<NewFile> attachedFiles,
            bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        [NotNull]
        public List<NewFile> AttachedFiles { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
