namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LogFieldSettingsModel
    {
        public LogFieldSettingsModel()
        {
        }

        public LogFieldSettingsModel(FieldSettingModel log)
        {
            this.Log = log;
        }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; set; }
    }
}
