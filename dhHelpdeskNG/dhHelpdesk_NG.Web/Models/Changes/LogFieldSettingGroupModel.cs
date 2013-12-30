namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class LogFieldSettingGroupModel
    {
        public LogFieldSettingGroupModel()
        {
        }

        public LogFieldSettingGroupModel(FieldSettingModel log)
        {
            this.Log = log;
        }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; private set; }
    }
}
