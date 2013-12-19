namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    public sealed class NotifierInputTextBoxModel : NotifierInputRequiredFieldModel
    {
         public NotifierInputTextBoxModel(bool show)
            : base(show)
        {
        }

        public NotifierInputTextBoxModel(bool show, string caption, string value, bool required, int? minLength)
            : base(show, caption, required)
        {
            this.Value = value;
            this.MinLength = minLength;
        }

        public string Value { get; private set; }

        public int? MinLength { get; private set; }
    }
}