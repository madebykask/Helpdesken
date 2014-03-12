namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SettingModel
    {
        public SettingModel(
            string name,
            bool showInDetails,
            bool showInNotifiers,
            string caption,
            bool required,
            string ldapAttribute)
            : this(name, showInDetails, showInNotifiers, caption, ldapAttribute)
        {
            this.Required = required;
        }

        public SettingModel(string name, bool showInDetails, bool showInNotifiers, string caption, string ldapAttribute)
        {
            this.Name = name;
            this.ShowInDetails = showInDetails;
            this.ShowInNotifiers = showInNotifiers;
            this.Caption = caption;
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