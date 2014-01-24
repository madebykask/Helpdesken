namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
