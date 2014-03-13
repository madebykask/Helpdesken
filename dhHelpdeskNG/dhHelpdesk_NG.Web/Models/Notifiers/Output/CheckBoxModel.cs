namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    public sealed class CheckBoxModel : NotifierInputFieldModel
    {
        public CheckBoxModel(bool show) : base(show)
        {
        }

        public CheckBoxModel(bool show, string caption, bool @checked)
            : base(show, caption)
        {
            this.Checked = @checked;
        }

        public bool Checked { get; private set; }
    }
}