namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class LogFieldSettings
    {
        public LogFieldSettings(FieldSetting log)
        {
            this.Log = log;
        }

        [NotNull]
        public FieldSetting Log { get; private set; }
    }
}
