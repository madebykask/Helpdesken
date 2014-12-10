namespace DH.Helpdesk.Web.Models.Notifiers
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SettingModel
    {
        public SettingModel()
        {
        }

        public SettingModel(
            bool showInDetails,
            bool showInNotifiers,
            string caption,
            string lableText,
            bool required,
            string ldapAttribute)
            : this(showInDetails, showInNotifiers, caption,lableText, ldapAttribute)
        {
            this.Required = required;
        }

        public SettingModel(bool showInDetails, bool showInNotifiers, string caption, string lableText, string ldapAttribute)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInNotifiers = showInNotifiers;
            this.Caption = caption;
            this.LableText = lableText;
            this.LdapAttribute = ldapAttribute;
        }

        public bool ShowInDetails { get; set; }

        [Required]
        [StringLength(50)]
        [NotNullAndEmpty]
        public string Caption { get; set; }

        [NotNullAndEmpty]
        public string LableText { get; set; }

        public bool Required { get; set; }

        public bool ShowInNotifiers { get; set; }

        [StringLength(50)]
        public string LdapAttribute { get; set; }
    }
}