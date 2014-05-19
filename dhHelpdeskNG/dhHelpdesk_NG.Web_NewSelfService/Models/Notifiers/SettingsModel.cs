namespace DH.Helpdesk.NewSelfService.Models.Notifiers
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.NewSelfService.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class SettingsModel
    {
        public SettingsModel()
        {
        }

        public SettingsModel(
            DropDownContent language,
            SettingModel userId, 
            SettingModel domain,
            SettingModel loginName, 
            SettingModel firstName,
            SettingModel initials,
            SettingModel lastName,
            SettingModel displayName,
            SettingModel place,
            SettingModel phone,
            SettingModel cellPhone,
            SettingModel email,
            SettingModel code,
            SettingModel postalAddress,
            SettingModel postalCode,
            SettingModel city,
            SettingModel title,
            SettingModel region,
            SettingModel department,
            SettingModel unit,
            SettingModel organizationUnit,
            SettingModel division,
            SettingModel manager,
            SettingModel group,
            SettingModel other,
            SettingModel ordered,
            SettingModel createdDate,
            SettingModel changedDate,
            SettingModel synchronizationDate)
        {
            this.Language = language;
            this.UserId = userId;
            this.Domain = domain;
            this.LoginName = loginName;
            this.FirstName = firstName;
            this.Initials = initials;
            this.LastName = lastName;
            this.DisplayName = displayName;
            this.Place = place;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Code = code;
            this.PostalAddress = postalAddress;
            this.PostalCode = postalCode;
            this.City = city;
            this.Title = title;
            this.Region = region;
            this.Department = department;
            this.Unit = unit;
            this.OrganizationUnit = organizationUnit;
            this.Division = division;
            this.Manager = manager;
            this.Group = group;
            this.Other = other;
            this.Ordered = ordered;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        [NotNull]
        public DropDownContent Language { get; set; }

        [IsId]
        public int LanguageId { get; set; }

        [Required]
        [NotNull]
        public SettingModel UserId { get; set; }

        [Required]
        [NotNull]
        public SettingModel Domain { get; set; }

        [Required]
        [NotNull]
        public SettingModel LoginName { get; set; }

        [Required]
        [NotNull]
        public SettingModel FirstName { get; set; }

        [Required]
        [NotNull]
        public SettingModel Initials { get; set; }

        [Required]
        [NotNull]
        public SettingModel LastName { get; set; }

        [Required]
        [NotNull]
        public SettingModel DisplayName { get; set; }

        [Required]
        [NotNull]
        public SettingModel Place { get; set; }

        [Required]
        [NotNull]
        public SettingModel Phone { get; set; }

        [Required]
        [NotNull]
        public SettingModel CellPhone { get; set; }

        [Required]
        [NotNull]
        public SettingModel Email { get; set; }

        [Required]
        [NotNull]
        public SettingModel Code { get; set; }

        [Required]
        [NotNull]
        public SettingModel PostalAddress { get; set; }

        [Required]
        [NotNull]
        public SettingModel PostalCode { get; set; }

        [Required]
        [NotNull]
        public SettingModel City { get; set; }

        [Required]
        [NotNull]
        public SettingModel Title { get; set; }

        [Required]
        [NotNull]
        public SettingModel Region { get; set; }

        [Required]
        [NotNull]
        public SettingModel Department { get; set; }

        [Required]
        [NotNull]
        public SettingModel Unit { get; set; }

        [Required]
        [NotNull]
        public SettingModel OrganizationUnit { get; set; }

        [Required]
        [NotNull]
        public SettingModel Division { get; set; }

        [Required]
        [NotNull]
        public SettingModel Manager { get; set; }

        [Required]
        [NotNull]
        public SettingModel Group { get; set; }

        [Required]
        [NotNull]
        public SettingModel Other { get; set; }

        [Required]
        [NotNull]
        public SettingModel Ordered { get; set; }

        [Required]
        [NotNull]
        public SettingModel CreatedDate { get; set; }

        [Required]
        [NotNull]
        public SettingModel ChangedDate { get; set; }

        [Required]
        [NotNull]
        public SettingModel SynchronizationDate { get; set; }
    }
}