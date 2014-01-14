namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class EvaluationFieldSettingGroupDto
    {
        public EvaluationFieldSettingGroupDto(
            StringFieldSettingDto evaluation,
            FieldSettingDto attachedFile,
            FieldSettingDto log,
            FieldSettingDto evaluationReady)
        {
            this.Evaluation = evaluation;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public StringFieldSettingDto Evaluation { get; private set; }

        [NotNull]
        public FieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingDto Log { get; private set; }

        [NotNull]
        public FieldSettingDto EvaluationReady { get; private set; }
    }
}
