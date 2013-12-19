namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    public sealed class NotifierLabelModel : NotifierInputFieldModel
    {
        public NotifierLabelModel(bool show)
            : base(show)
        {
        }

        public NotifierLabelModel(bool show, string caption)
            : base(show, caption)
        {
        }
    }
}