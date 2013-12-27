namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
            UserId = userId;
            Domain = domain;
            LoginName = loginName;
            FirstName = firstName;
            Initials = initials;
            LastName = lastName;
            DisplayName = displayName;
            Place = place;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Code = code;
            PostalAddress = postalAddress;
            PostalCode = postalCode;
            City = city;
            Title = title;
            Department = department;
            Unit = unit;
            OrganizationUnit = organizationUnit;
            Division = division;
            Manager = manager;
            Group = @group;
            Password = password;
            Other = other;
            Ordered = ordered;
            CreatedDate = createdDate;
            ChangedDate = changedDate;
            SynchronizationDate = synchronizationDate;
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
