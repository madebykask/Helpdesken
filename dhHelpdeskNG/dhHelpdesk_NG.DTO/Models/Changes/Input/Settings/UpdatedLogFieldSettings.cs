namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedLogFieldSettings
    {
        public UpdatedLogFieldSettings(UpdatedFieldSetting logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public UpdatedFieldSetting Logs { get; private set; }
    }
}
