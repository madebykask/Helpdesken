namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LogFieldSettingsModel
    {
        public LogFieldSettingsModel()
        {
        }

        public LogFieldSettingsModel(FieldSettingModel logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        [LocalizedDisplay("Logs")]
        public FieldSettingModel Logs { get; set; }
    }
}
