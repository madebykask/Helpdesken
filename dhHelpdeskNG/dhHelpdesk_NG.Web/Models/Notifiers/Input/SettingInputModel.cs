namespace dhHelpdesk_NG.Web.Models.Notifiers.Input
{
    using System.ComponentModel.DataAnnotations;

    public sealed class SettingInputModel
    {
        [Required]
        [StringLength(50)]
        public string Caption { get; set; }

        public bool Required { get; set; }

        public bool ShowInDetails { get; set; }

        public bool ShowInNotifiers { get; set; }

        [StringLength(50)]
        public string LdapAttribute { get; set; }
    }
}