namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogEditSettings
    {
        public LogEditSettings(FieldEditSetting logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public FieldEditSetting Logs { get; private set; }
    }
}
