namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class UpdatedLogFieldSettingGroupDto
    {
        public UpdatedLogFieldSettingGroupDto(UpdatedFieldSettingDto log)
        {
            ArgumentsValidator.NotNull(log, "log");
            Log = log;
        }

        public UpdatedFieldSettingDto Log { get; private set; }
    }
}
