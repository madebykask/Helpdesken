namespace dhHelpdesk_NG.Web.Models.Notifiers.Input
{
    using System.ComponentModel.DataAnnotations;

    public sealed class SettingsInputModel
    {
        public int LanguageId { get; set; }

        [Required]
        public StringFieldSettingInputModel UserId { get; set; }

        [Required]
        public FieldSettingInputModel Domain { get; set; }

        [Required]
        public StringFieldSettingInputModel LoginName { get; set; }

        [Required]
        public StringFieldSettingInputModel FirstName { get; set; }

        [Required]
        public StringFieldSettingInputModel Initials { get; set; }

        [Required]
        public StringFieldSettingInputModel LastName { get; set; }

        [Required]
        public StringFieldSettingInputModel DisplayName { get; set; }

        [Required]
        public StringFieldSettingInputModel Place { get; set; }

        [Required]
        public StringFieldSettingInputModel Phone { get; set; }

        [Required]
        public StringFieldSettingInputModel CellPhone { get; set; }

        [Required]
        public StringFieldSettingInputModel Email { get; set; }

        [Required]
        public StringFieldSettingInputModel Code { get; set; }

        [Required]
        public StringFieldSettingInputModel PostalAddress { get; set; }

        [Required]
        public StringFieldSettingInputModel PostalCode { get; set; }

        [Required]
        public StringFieldSettingInputModel City { get; set; }

        [Required]
        public StringFieldSettingInputModel Title { get; set; }

        [Required]
        public FieldSettingInputModel Department { get; set; }

        [Required]
        public StringFieldSettingInputModel Unit { get; set; }

        [Required]
        public FieldSettingInputModel OrganizationUnit { get; set; }

        [Required]
        public FieldSettingInputModel Division { get; set; }

        [Required]
        public FieldSettingInputModel Manager { get; set; }

        [Required]
        public FieldSettingInputModel Group { get; set; }

        [Required]
        public StringFieldSettingInputModel Password { get; set; }

        [Required]
        public StringFieldSettingInputModel Other { get; set; }

        [Required]
        public FieldSettingInputModel Ordered { get; set; }

        [Required]
        public FieldSettingInputModel CreatedDate { get; set; }

        [Required]
        public FieldSettingInputModel ChangedDate { get; set; }

        [Required]
        public FieldSettingInputModel SynchronizationDate { get; set; }
    }
}