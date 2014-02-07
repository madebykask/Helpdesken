namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSettingModel
    {
        public FieldSettingModel(
            string name, bool showInDetails, bool showInNotifiers, string caption, bool required, string ldapAttribute)
        {
            this.Name = name;
            this.ShowInDetails = showInDetails;
            this.ShowInNotifiers = showInNotifiers;
            this.Caption = caption;
            this.Required = required;
            this.LdapAttribute = ldapAttribute;
        }

        public bool ShowInDetails { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public bool ShowInNotifiers { get; private set; }

        public string LdapAttribute { get; private set; }
    }
}