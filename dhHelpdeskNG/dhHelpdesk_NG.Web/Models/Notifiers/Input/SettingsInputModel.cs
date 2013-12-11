namespace dhHelpdesk_NG.Web.Models.Notifiers.Input
{
    using System.ComponentModel.DataAnnotations;

    public sealed class SettingsInputModel
    {
        public int LanguageId { get; set; }

        [Required]
        public SettingInputModel UserId { get; set; }

        [Required]
        public SettingInputModel Domain { get; set; }

        [Required]
        public SettingInputModel LoginName { get; set; }

        [Required]
        public SettingInputModel FirstName { get; set; }

        [Required]
        public SettingInputModel Initials { get; set; }

        [Required]
        public SettingInputModel LastName { get; set; }

        [Required]
        public SettingInputModel DisplayName { get; set; }

        [Required]
        public SettingInputModel Place { get; set; }

        [Required]
        public SettingInputModel Phone { get; set; }

        [Required]
        public SettingInputModel CellPhone { get; set; }

        [Required]
        public SettingInputModel Email { get; set; }

        [Required]
        public SettingInputModel Code { get; set; }

        [Required]
        public SettingInputModel PostalAddress { get; set; }

        [Required]
        public SettingInputModel PostalCode { get; set; }

        [Required]
        public SettingInputModel City { get; set; }

        [Required]
        public SettingInputModel Title { get; set; }

        [Required]
        public SettingInputModel Department { get; set; }

        [Required]
        public SettingInputModel Unit { get; set; }

        [Required]
        public SettingInputModel OrganizationUnit { get; set; }

        [Required]
        public SettingInputModel Division { get; set; }

        [Required]
        public SettingInputModel Manager { get; set; }

        [Required]
        public SettingInputModel Group { get; set; }

        [Required]
        public SettingInputModel Password { get; set; }

        [Required]
        public SettingInputModel Other { get; set; }

        [Required]
        public SettingInputModel Ordered { get; set; }

        [Required]
        public SettingInputModel CreatedDate { get; set; }

        [Required]
        public SettingInputModel ChangedDate { get; set; }

        [Required]
        public SettingInputModel SynchronizationDate { get; set; }
    }
}