namespace DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogFieldSettings
    {
        public LogFieldSettings(FieldSetting logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public FieldSetting Logs { get; private set; }
    }
}
