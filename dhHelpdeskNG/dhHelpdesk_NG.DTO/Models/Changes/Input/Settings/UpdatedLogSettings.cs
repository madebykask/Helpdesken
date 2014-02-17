namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedLogSettings
    {
        public UpdatedLogSettings(UpdatedFieldSetting logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public UpdatedFieldSetting Logs { get; private set; }
    }
}
