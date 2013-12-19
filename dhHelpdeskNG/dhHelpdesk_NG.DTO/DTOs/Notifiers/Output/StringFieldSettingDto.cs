namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    public sealed class StringFieldSettingDto : FieldSettingDto
    {
        public StringFieldSettingDto(
            string name, bool showInDetails, bool showInNotifiers, string caption, bool required, int? minLength, string ldapAttribute)
            : base(name, showInDetails, showInNotifiers, caption, required, ldapAttribute)
        {
            this.MinLength = minLength;
        }

        public int? MinLength { get; private set; }
    }
}
