namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class EvaluationFieldSettingGroupDto
    {
        public EvaluationFieldSettingGroupDto(StringFieldSettingDto evaluation, FieldSettingDto attachedFile, FieldSettingDto log, FieldSettingDto evaluationReady)
        {
            ArgumentsValidator.NotNull(evaluation, "evaluation");
            ArgumentsValidator.NotNull(attachedFile, "attachedFile");
            ArgumentsValidator.NotNull(log, "log");
            ArgumentsValidator.NotNull(evaluationReady, "evaluationReady");
            
            Evaluation = evaluation;
            AttachedFile = attachedFile;
            Log = log;
            EvaluationReady = evaluationReady;
        }

        public StringFieldSettingDto Evaluation { get; private set; }

        public FieldSettingDto AttachedFile { get; private set; }

        public FieldSettingDto Log { get; private set; }

        public FieldSettingDto EvaluationReady { get; private set; }
    }
}
