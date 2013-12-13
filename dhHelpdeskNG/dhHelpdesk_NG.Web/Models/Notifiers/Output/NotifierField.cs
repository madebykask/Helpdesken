namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifierFieldModel
    {
        public NotifierFieldModel(string name, string caption)
        {
            ArgumentsValidator.NotNullAndEmpty(name, "name");
            ArgumentsValidator.NotNullAndEmpty(caption, "caption");

            this.Name = name;
            this.Caption = caption;
        }

        public string Name { get; private set; }

        public string Caption { get; private set; }
    }
}