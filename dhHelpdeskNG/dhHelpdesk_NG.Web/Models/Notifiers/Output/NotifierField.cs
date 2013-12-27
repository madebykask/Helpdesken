namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NotifierFieldModel
    {
        public NotifierFieldModel(string name, string caption)
        {
            this.Name = name;
            this.Caption = caption;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}