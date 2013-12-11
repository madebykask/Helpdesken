namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    public abstract class NotifierInputRequiredFieldModel : NotifierInputFieldModel
    {
        public bool Required { get; private set; }

        protected NotifierInputRequiredFieldModel(bool show)
            : base(show)
        {
        }

        protected NotifierInputRequiredFieldModel(bool show, string caption, bool required)
            : base(show, caption)
        {
            this.Required = required;
        }
    }
}