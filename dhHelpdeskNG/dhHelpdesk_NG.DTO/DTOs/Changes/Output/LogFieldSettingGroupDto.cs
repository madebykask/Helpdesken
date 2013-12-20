namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class LogFieldSettingGroupDto
    {
        public LogFieldSettingGroupDto(FieldSettingDto log)
        {
            ArgumentsValidator.NotNull(log, "log");
            Log = log;
        }

        public FieldSettingDto Log { get; private set; }
    }
}
