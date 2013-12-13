namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class CaptionModel
    {
        public CaptionModel(string fieldName, string text)
        {
            ArgumentsValidator.NotNullAndEmpty(fieldName, "fieldName");
            ArgumentsValidator.NotNullAndEmpty(text, "text");

            this.FieldName = fieldName;
            this.Text = text;
        }

        public string FieldName { get; private set; }

        public string Text { get; private set; }
    }
}