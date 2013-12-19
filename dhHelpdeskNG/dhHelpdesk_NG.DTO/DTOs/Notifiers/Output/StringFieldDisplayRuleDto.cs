namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    public sealed class StringFieldDisplayRuleDto : FieldDisplayRuleDto
    {
        public StringFieldDisplayRuleDto(bool show, bool required, int? minLength)
            : base(show, required)
        {
            this.MinLength = minLength;
        }

        public int? MinLength { get; private set; }
    }
}