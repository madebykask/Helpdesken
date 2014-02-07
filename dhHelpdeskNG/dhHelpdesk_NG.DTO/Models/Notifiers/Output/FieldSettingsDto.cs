namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldSettingsDto
    {
        public FieldSettingsDto(
            FieldSettingDto userId,
            FieldSettingDto domain,
            FieldSettingDto loginName,
            FieldSettingDto firstName,
            FieldSettingDto initials,
            FieldSettingDto lastName,
            FieldSettingDto displayName,
            FieldSettingDto place,
            FieldSettingDto phone,
            FieldSettingDto cellPhone,
            FieldSettingDto email,
            FieldSettingDto code,
            FieldSettingDto postalAddress,
            FieldSettingDto postalCode,
            FieldSettingDto city,
            FieldSettingDto title,
            FieldSettingDto department,
            FieldSettingDto unit,
            FieldSettingDto organizationUnit,
            FieldSettingDto division,
            FieldSettingDto manager,
            FieldSettingDto group,
            FieldSettingDto password,
            FieldSettingDto other,
            FieldSettingDto ordered,
            FieldSettingDto createdDate,
            FieldSettingDto changedDate,
            FieldSettingDto synchronizationDate)
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
            this.Group = group;
            this.Password = password;
            this.Other = other;
            this.Ordered = ordered;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        [NotNull]
        public FieldSettingDto UserId { get; private set; }

        [NotNull]
        public FieldSettingDto Domain { get; private set; }

        [NotNull]
        public FieldSettingDto LoginName { get; private set; }

        [NotNull]
        public FieldSettingDto FirstName { get; private set; }

        [NotNull]
        public FieldSettingDto Initials { get; private set; }

        [NotNull]
        public FieldSettingDto LastName { get; private set; }

        [NotNull]
        public FieldSettingDto DisplayName { get; private set; }

        [NotNull]
        public FieldSettingDto Place { get; private set; }

        [NotNull]
        public FieldSettingDto Phone { get; private set; }

        [NotNull]
        public FieldSettingDto CellPhone { get; private set; }

        [NotNull]
        public FieldSettingDto Email { get; private set; }

        [NotNull]
        public FieldSettingDto Code { get; private set; }

        [NotNull]
        public FieldSettingDto PostalAddress { get; private set; }

        [NotNull]
        public FieldSettingDto PostalCode { get; private set; }

        [NotNull]
        public FieldSettingDto City { get; private set; }

        [NotNull]
        public FieldSettingDto Title { get; private set; }

        [NotNull]
        public FieldSettingDto Department { get; private set; }

        [NotNull]
        public FieldSettingDto Unit { get; private set; }

        [NotNull]
        public FieldSettingDto OrganizationUnit { get; private set; }

        [NotNull]
        public FieldSettingDto Division { get; private set; }

        [NotNull]
        public FieldSettingDto Manager { get; private set; }

        [NotNull]
        public FieldSettingDto Group { get; private set; }

        [NotNull]
        public FieldSettingDto Password { get; private set; }

        [NotNull]
        public FieldSettingDto Other { get; private set; }

        [NotNull]
        public FieldSettingDto Ordered { get; private set; }

        [NotNull]
        public FieldSettingDto CreatedDate { get; private set; }

        [NotNull]
        public FieldSettingDto ChangedDate { get; private set; }

        [NotNull]
        public FieldSettingDto SynchronizationDate { get; private set; }
    }
}