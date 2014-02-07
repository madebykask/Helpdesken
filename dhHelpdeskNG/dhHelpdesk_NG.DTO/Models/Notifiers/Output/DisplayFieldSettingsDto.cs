namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DisplayFieldSettingsDto
    {
        public DisplayFieldSettingsDto(
            DisplayFieldSettingDto userId,
            DisplayFieldSettingDto domain,
            DisplayFieldSettingDto loginName,
            DisplayFieldSettingDto firstName,
            DisplayFieldSettingDto initials,
            DisplayFieldSettingDto lastName,
            DisplayFieldSettingDto displayName,
            DisplayFieldSettingDto place,
            DisplayFieldSettingDto phone,
            DisplayFieldSettingDto cellPhone,
            DisplayFieldSettingDto email,
            DisplayFieldSettingDto code,
            DisplayFieldSettingDto postalAddress,
            DisplayFieldSettingDto postalCode,
            DisplayFieldSettingDto city,
            DisplayFieldSettingDto title,
            DisplayFieldSettingDto department,
            DisplayFieldSettingDto unit,
            DisplayFieldSettingDto organizationUnit,
            DisplayFieldSettingDto division,
            DisplayFieldSettingDto manager,
            DisplayFieldSettingDto group,
            DisplayFieldSettingDto password,
            DisplayFieldSettingDto other,
            DisplayFieldSettingDto ordered,
            DisplayFieldSettingDto createdDate,
            DisplayFieldSettingDto changedDate,
            DisplayFieldSettingDto synchronizationDate)
        {
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
            this.Department = department;
            this.Unit = unit;
            this.OrganizationUnit = organizationUnit;
            this.Division = division;
            this.Manager = manager;
            this.Group = @group;
            this.Password = password;
            this.Other = other;
            this.Ordered = ordered;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        [NotNull]
        public DisplayFieldSettingDto UserId { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Domain { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto LoginName { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto FirstName { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Initials { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto LastName { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto DisplayName { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Place { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Phone { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto CellPhone { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Email { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Code { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto PostalAddress { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto PostalCode { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto City { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Title { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Department { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Unit { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto OrganizationUnit { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Division { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Manager { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Group { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Password { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Other { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto Ordered { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto CreatedDate { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto ChangedDate { get; private set; }

        [NotNull]
        public DisplayFieldSettingDto SynchronizationDate { get; private set; }
    }
}
