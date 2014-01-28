namespace dhHelpdesk_NG.Web.Models.Changes.InputModel
{
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class EvaluationModel
    {
        public EvaluationModel()
        {
        }

        public EvaluationModel(
            string changeEvaluation,
            AttachedFilesContainerModel attachedFilesContainer,
            bool evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFilesContainer = attachedFilesContainer;
            this.EvaluationReady = evaluationReady;
        }

        [LocalizedDisplay("Change evaluation")]
        public string ChangeEvaluation { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        [LocalizedDisplay("Evaluation ready")]
        public bool EvaluationReady { get; set; }
    }
}