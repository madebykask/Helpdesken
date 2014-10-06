namespace DH.Helpdesk.Mobile.Models.Changes.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public sealed class LogSettingsModel
    {
        public LogSettingsModel()
        {
        }

        public LogSettingsModel(FieldSettingModel logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        [LocalizedDisplay("Logs")]
        public FieldSettingModel Logs { get; set; }
    }
}