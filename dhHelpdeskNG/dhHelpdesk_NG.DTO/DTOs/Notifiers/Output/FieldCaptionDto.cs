namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldCaptionDto
    {
        public FieldCaptionDto(string fieldName, string text)
        {
            this.FieldName = fieldName;
            this.Text = text;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        [NotNullAndEmpty]
        public string Text { get; private set; }
    }
}
