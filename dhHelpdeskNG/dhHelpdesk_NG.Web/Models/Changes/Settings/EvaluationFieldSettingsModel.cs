namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class EvaluationFieldSettingsModel
    {
        public EvaluationFieldSettingsModel()
        {
        }

        public EvaluationFieldSettingsModel(
            StringFieldSettingModel evaluation,
            FieldSettingModel attachedFile,
            FieldSettingModel log,
            FieldSettingModel evaluationReady)
        {
            this.Evaluation = evaluation;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        [LocalizedDisplay("Evaluation")]
        public StringFieldSettingModel Evaluation { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; set; }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; set; }

        [NotNull]
        [LocalizedDisplay("Evaluation ready")]
        public FieldSettingModel EvaluationReady { get; set; }
    }
}
