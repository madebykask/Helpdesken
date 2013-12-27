namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class CaptionModel
    {
        public CaptionModel(string fieldName, string text)
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