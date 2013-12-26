namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class UpdatedEvaluationFieldSettingGroupDto
    {
        public UpdatedEvaluationFieldSettingGroupDto(
            UpdatedStringFieldSettingDto evaluation,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto log,
            UpdatedFieldSettingDto evaluationReady)
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

        public UpdatedStringFieldSettingDto Evaluation { get; private set; }

        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        public UpdatedFieldSettingDto Log { get; private set; }

        public UpdatedFieldSettingDto EvaluationReady { get; private set; }
    }
}
