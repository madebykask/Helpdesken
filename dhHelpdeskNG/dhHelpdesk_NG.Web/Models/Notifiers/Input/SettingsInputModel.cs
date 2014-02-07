namespace DH.Helpdesk.Web.Models.Notifiers.Input
{
    using System.ComponentModel.DataAnnotations;

    public sealed class SettingsInputModel
    {
        public int LanguageId { get; set; }

        [Required]
        public FieldSettingInputModel UserId { get; set; }

        [Required]
        public FieldSettingInputModel Domain { get; set; }

        [Required]
        public FieldSettingInputModel LoginName { get; set; }

        [Required]
        public FieldSettingInputModel FirstName { get; set; }

        [Required]
        public FieldSettingInputModel Initials { get; set; }

        [Required]
        public FieldSettingInputModel LastName { get; set; }

        [Required]
        public FieldSettingInputModel DisplayName { get; set; }

        [Required]
        public FieldSettingInputModel Place { get; set; }

        [Required]
        public FieldSettingInputModel Phone { get; set; }

        [Required]
        public FieldSettingInputModel CellPhone { get; set; }

        [Required]
        public FieldSettingInputModel Email { get; set; }

        [Required]
        public FieldSettingInputModel Code { get; set; }

        [Required]
        public FieldSettingInputModel PostalAddress { get; set; }

        [Required]
        public FieldSettingInputModel PostalCode { get; set; }

        [Required]
        public FieldSettingInputModel City { get; set; }

        [Required]
        public FieldSettingInputModel Title { get; set; }

        [Required]
        public FieldSettingInputModel Department { get; set; }

        [Required]
        public FieldSettingInputModel Unit { get; set; }

        [Required]
        public FieldSettingInputModel OrganizationUnit { get; set; }

        [Required]
        public FieldSettingInputModel Division { get; set; }

        [Required]
        public FieldSettingInputModel Manager { get; set; }

        [Required]
        public FieldSettingInputModel Group { get; set; }

        [Required]
        public FieldSettingInputModel Password { get; set; }

        [Required]
        public FieldSettingInputModel Other { get; set; }

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