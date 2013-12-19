namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.Tools;

    public class FieldSettingModel
    {
         public FieldSettingModel(string name, bool showInDetails, bool showInNotifiers, string caption, bool required, string ldapAttribute)
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

        public bool ShowInDetails { get; private set; }

        public string Name { get; private set; }

        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public bool ShowInNotifiers { get; private set; }

        public string LdapAttribute { get; private set; }
    }
}