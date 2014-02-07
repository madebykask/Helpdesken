namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedLogFieldSettingGroupDto
    {
        public UpdatedLogFieldSettingGroupDto(UpdatedFieldSettingDto log)
        {
            this.Log = log;
        }

        [NotNull]
        public UpdatedFieldSettingDto Log { get; private set; }
    }
}
