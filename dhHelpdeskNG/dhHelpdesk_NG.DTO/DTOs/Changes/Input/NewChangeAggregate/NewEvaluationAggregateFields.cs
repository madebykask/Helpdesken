namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NewEvaluationAggregateFields
    {
        public NewEvaluationAggregateFields(
            string changeEvaluation, 
            List<NewChangeFile> attachedFiles,
            bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeEvaluation { get; private set; }

        [NotNull]
        public List<NewChangeFile> AttachedFiles { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
