﻿namespace DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting
    {
        public FieldSetting(
            bool showInDetails,
            bool showInNotifiers,
            string caption,
            string lableText,
            bool required,
            string ldapAttribute)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInNotifiers = showInNotifiers;
            this.Caption = caption;
            this.LableText = lableText;  //==null? "" : lableText;
            this.Required = required;
            this.LdapAttribute = ldapAttribute;
        }

        public bool ShowInDetails { get; private set; }

        public bool ShowInNotifiers { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        [NotNullAndEmpty]
        public string LableText { get; private set; }

        public bool Required { get; private set; }

        public string LdapAttribute { get; private set; }

        public bool IsEmpty { get; private set; }

        public void MarkAsEmpty()
        {
            this.IsEmpty = true;
        }
    }
}