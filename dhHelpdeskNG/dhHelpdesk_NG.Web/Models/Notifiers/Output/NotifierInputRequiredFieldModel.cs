namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    public abstract class NotifierInputRequiredFieldModel : NotifierInputFieldModel
    {
        protected NotifierInputRequiredFieldModel(bool show)
            : base(show)
        {
        }

        protected NotifierInputRequiredFieldModel(bool show, string caption, bool required)
            : base(show, caption)
        {
            this.Required = required;
        }

        public bool Required { get; private set; }
    }
}