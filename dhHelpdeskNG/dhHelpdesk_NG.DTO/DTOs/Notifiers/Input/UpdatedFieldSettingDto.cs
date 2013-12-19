namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Input
{
    using System;

    using dhHelpdesk_NG.Common.Tools;

    public class UpdatedFieldSettingDto
    {
        public UpdatedFieldSettingDto(
            bool showInDetails,
            bool showInNotifiers,
            string caption,
            bool required,
            string ldapAttribute,
            DateTime changedDateTime)
        {
            ArgumentsValidator.NotNullAndEmpty(caption, "caption");

            this.ShowInDetails = showInDetails;
            this.ShowInNotifiers = showInNotifiers;
            this.Caption = caption;
            this.Required = required;
            this.LdapAttribute = ldapAttribute;
            this.ChangedDateTime = changedDateTime;
        }


        public bool ShowInDetails { get; private set; }

        public bool ShowInNotifiers { get; private set; }

        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string LdapAttribute { get; private set; }

        public DateTime ChangedDateTime { get; private set; }
    }
}