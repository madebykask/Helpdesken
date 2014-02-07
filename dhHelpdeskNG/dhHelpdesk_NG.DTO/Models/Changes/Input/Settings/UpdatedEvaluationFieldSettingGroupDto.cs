namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedEvaluationFieldSettingGroupDto
    {
        public UpdatedEvaluationFieldSettingGroupDto(
            UpdatedStringFieldSettingDto evaluation,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto log,
            UpdatedFieldSettingDto evaluationReady)
        {
            this.Evaluation = evaluation;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public UpdatedStringFieldSettingDto Evaluation { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Log { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto EvaluationReady { get; private set; }
    }
}
