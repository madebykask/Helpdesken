namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
