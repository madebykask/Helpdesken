namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class SettingsModel
    {
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
        public DropDownContent Language { get; private set; }

        [NotNull]
        public SettingModel UserId { get; private set; }

        [NotNull]
        public SettingModel Domain { get; private set; }

        [NotNull]
        public SettingModel LoginName { get; private set; }

        [NotNull]
        public SettingModel FirstName { get; private set; }

        [NotNull]
        public SettingModel Initials { get; private set; }

        [NotNull]
        public SettingModel LastName { get; private set; }

        [NotNull]
        public SettingModel DisplayName { get; private set; }

        [NotNull]
        public SettingModel Place { get; private set; }

        [NotNull]
        public SettingModel Phone { get; private set; }

        [NotNull]
        public SettingModel CellPhone { get; private set; }

        [NotNull]
        public SettingModel Email { get; private set; }

        [NotNull]
        public SettingModel Code { get; private set; }

        [NotNull]
        public SettingModel PostalAddress { get; private set; }

        [NotNull]
        public SettingModel PostalCode { get; private set; }

        [NotNull]
        public SettingModel City { get; private set; }

        [NotNull]
        public SettingModel Title { get; private set; }

        [NotNull]
        public SettingModel Region { get; private set; }

        [NotNull]
        public SettingModel Department { get; private set; }

        [NotNull]
        public SettingModel Unit { get; private set; }

        [NotNull]
        public SettingModel OrganizationUnit { get; private set; }

        [NotNull]
        public SettingModel Division { get; private set; }

        [NotNull]
        public SettingModel Manager { get; private set; }

        [NotNull]
        public SettingModel Group { get; private set; }

        [NotNull]
        public SettingModel Other { get; private set; }

        [NotNull]
        public SettingModel Ordered { get; private set; }

        [NotNull]
        public SettingModel CreatedDate { get; private set; }

        [NotNull]
        public SettingModel ChangedDate { get; private set; }

        [NotNull]
        public SettingModel SynchronizationDate { get; private set; }
    }
}