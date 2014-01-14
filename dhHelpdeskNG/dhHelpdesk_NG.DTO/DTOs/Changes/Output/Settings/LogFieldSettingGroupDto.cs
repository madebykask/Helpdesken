namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class LogFieldSettingGroupDto
    {
        public LogFieldSettingGroupDto(FieldSettingDto log)
        {
            this.Log = log;
        }

        [NotNull]
        public FieldSettingDto Log { get; private set; }
    }
}
