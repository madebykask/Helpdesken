namespace DH.Helpdesk.BusinessData.Models.Notifiers.Input
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedFieldSettingsDto
    {
        public UpdatedFieldSettingsDto(
            int customerId,
            int languageId,
            UpdatedFieldSettingDto userId,
            UpdatedFieldSettingDto domain,
            UpdatedFieldSettingDto loginName,
            UpdatedFieldSettingDto firstName,
            UpdatedFieldSettingDto initials,
            UpdatedFieldSettingDto lastName,
            UpdatedFieldSettingDto displayName,
            UpdatedFieldSettingDto place,
            UpdatedFieldSettingDto phone,
            UpdatedFieldSettingDto cellPhone,
            UpdatedFieldSettingDto email,
            UpdatedFieldSettingDto code,
            UpdatedFieldSettingDto postalAddress,
            UpdatedFieldSettingDto postalCode,
            UpdatedFieldSettingDto city,
            UpdatedFieldSettingDto title,
            UpdatedFieldSettingDto department,
            UpdatedFieldSettingDto unit,
            UpdatedFieldSettingDto organizationUnit,
            UpdatedFieldSettingDto division,
            UpdatedFieldSettingDto manager,
            UpdatedFieldSettingDto group,
            UpdatedFieldSettingDto password,
            UpdatedFieldSettingDto other,
            UpdatedFieldSettingDto ordered,
            UpdatedFieldSettingDto createdDate,
            UpdatedFieldSettingDto changedDate,
            UpdatedFieldSettingDto synchronizationDate)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
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

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto UserId { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Domain { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto LoginName { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto FirstName { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Initials { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto LastName { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto DisplayName { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Place { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Phone { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto CellPhone { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Email { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Code { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto PostalAddress { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto PostalCode { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto City { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Title { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Department { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Unit { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto OrganizationUnit { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Division { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Manager { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Group { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Password { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Other { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Ordered { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto CreatedDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto ChangedDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto SynchronizationDate { get; private set; }
    }
}
