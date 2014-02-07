namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
