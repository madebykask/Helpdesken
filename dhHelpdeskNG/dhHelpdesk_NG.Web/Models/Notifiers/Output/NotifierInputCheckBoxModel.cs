namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    public sealed class NotifierInputCheckBoxModel : NotifierInputFieldModel
    {
        public NotifierInputCheckBoxModel(bool show) : base(show)
        {
        }

        public NotifierInputCheckBoxModel(bool show, string caption, bool @checked)
            : base(show, caption)
        {
            this.Checked = @checked;
        }

        public bool Checked { get; private set; }
    }
}