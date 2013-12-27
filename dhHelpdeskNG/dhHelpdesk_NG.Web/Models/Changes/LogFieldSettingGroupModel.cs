namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
        public FieldSettingModel Log { get; private set; }
    }
}
