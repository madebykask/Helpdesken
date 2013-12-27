namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class LogFieldSettingGroupModel
    {
        public LogFieldSettingGroupModel(FieldSettingModel log)
        {
            ArgumentsValidator.NotNull(log, "log");
            this.Log = log;
        }

        public FieldSettingModel Log { get; private set; }
    }
}
