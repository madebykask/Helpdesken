namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public class FieldSettingDto
    {
        public FieldSettingDto(string name, bool showInDetails, bool showInNotifiers, string caption, bool required, string ldapAttribute)
        {
            ArgumentsValidator.NotNullAndEmpty(name, "name");
            ArgumentsValidator.NotNullAndEmpty(caption, "caption");

            this.Name = name;
            this.ShowInDetails = showInDetails;
            this.ShowInNotifiers = showInNotifiers;
            this.Caption = caption;
            this.Required = required;
            this.LdapAttribute = ldapAttribute;
        }

        public string Name { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInNotifiers { get; private set; }

        public string LdapAttribute { get; private set; }

        public string Caption { get; private set; }

        public bool Required { get; private set; }
    }
}
