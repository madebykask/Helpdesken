namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    public sealed class LabelModel : NotifierInputFieldModel
    {
        public LabelModel(bool show)
            : base(show)
        {
        }

        public LabelModel(bool show, string caption, string text)
            : base(show, caption)
        {
            this.Text = text;
        }

        public string Text { get; private set; }
    }
}