namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class UpdatedEvaluationFieldSettingGroupDto
    {
        public UpdatedEvaluationFieldSettingGroupDto(
            UpdatedStringFieldSettingDto evaluation,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto log,
            UpdatedFieldSettingDto evaluationReady)
        {
            Evaluation = evaluation;
            AttachedFile = attachedFile;
            Log = log;
            EvaluationReady = evaluationReady;
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
