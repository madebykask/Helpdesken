namespace DH.Helpdesk.Web.Models.Changes.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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