namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    public sealed class NotifierLabelModel : NotifierInputFieldModel
    {
        public NotifierLabelModel(bool show)
            : base(show)
        {
        }

        public NotifierLabelModel(bool show, string caption, string text)
            : base(show, caption)
        {
            this.Text = text;
        }

        public string Text { get; private set; }
    }
}