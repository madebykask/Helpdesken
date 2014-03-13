namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Web.LocalizedAttributes;

    public sealed class TextBoxModel : NotifierInputRequiredFieldModel
    {
         public TextBoxModel(bool show)
            : base(show)
        {
        }

        public TextBoxModel(bool show, string caption, string value, bool required)
            : base(show, caption, required)
        {
            this.Value = value;
        }

        [LocalizedRequiredFrom("Required")]
        public string Value { get; private set; }
    }
}