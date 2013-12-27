namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class LogFieldSettingGroupDto
    {
        public LogFieldSettingGroupDto(FieldSettingDto log)
        {
            Log = log;
        }

        [NotNull]
        public FieldSettingDto Log { get; private set; }
    }
}
