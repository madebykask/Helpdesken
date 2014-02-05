namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class LogFieldEditSettings
    {
        public LogFieldEditSettings(FieldEditSetting logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public FieldEditSetting Logs { get; private set; }
    }
}
