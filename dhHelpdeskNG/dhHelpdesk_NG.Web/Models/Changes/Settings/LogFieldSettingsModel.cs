namespace dhHelpdesk_NG.Web.Models.Changes.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

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
