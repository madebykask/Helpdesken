namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

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
            ArgumentsValidator.NotNull(userId, "userId");
            ArgumentsValidator.NotNull(domain, "domain");
            ArgumentsValidator.NotNull(loginName, "loginName");
            ArgumentsValidator.NotNull(firstName, "firstName");
            ArgumentsValidator.NotNull(initials, "initials");
            ArgumentsValidator.NotNull(lastName, "lastName");
            ArgumentsValidator.NotNull(displayName, "displayName");
            ArgumentsValidator.NotNull(place, "place");
            ArgumentsValidator.NotNull(phone, "phone");
            ArgumentsValidator.NotNull(cellPhone, "cellPhone");
            ArgumentsValidator.NotNull(email, "email");
            ArgumentsValidator.NotNull(code, "code");
            ArgumentsValidator.NotNull(postalAddress, "postalAddress");
            ArgumentsValidator.NotNull(postalCode, "postalCode");
            ArgumentsValidator.NotNull(city, "city");
            ArgumentsValidator.NotNull(title, "title");
            ArgumentsValidator.NotNull(department, "department");
            ArgumentsValidator.NotNull(unit, "unit");
            ArgumentsValidator.NotNull(organizationUnit, "organizationUnit");
            ArgumentsValidator.NotNull(division, "division");
            ArgumentsValidator.NotNull(manager, "manager");
            ArgumentsValidator.NotNull(group, "group");
            ArgumentsValidator.NotNull(password, "password");
            ArgumentsValidator.NotNull(other, "other");
            ArgumentsValidator.NotNull(ordered, "ordred");
            ArgumentsValidator.NotNull(createdDate, "createdDate");
            ArgumentsValidator.NotNull(changedDate, "changedDate");
            ArgumentsValidator.NotNull(synchronizationDate, "synchronizationDate");

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

        public DisplayFieldSettingDto UserId { get; private set; }

        public DisplayFieldSettingDto Domain { get; private set; }

        public DisplayFieldSettingDto LoginName { get; private set; }

        public DisplayFieldSettingDto FirstName { get; private set; }

        public DisplayFieldSettingDto Initials { get; private set; }

        public DisplayFieldSettingDto LastName { get; private set; }

        public DisplayFieldSettingDto DisplayName { get; private set; }

        public DisplayFieldSettingDto Place { get; private set; }

        public DisplayFieldSettingDto Phone { get; private set; }

        public DisplayFieldSettingDto CellPhone { get; private set; }

        public DisplayFieldSettingDto Email { get; private set; }

        public DisplayFieldSettingDto Code { get; private set; }

        public DisplayFieldSettingDto PostalAddress { get; private set; }

        public DisplayFieldSettingDto PostalCode { get; private set; }

        public DisplayFieldSettingDto City { get; private set; }

        public DisplayFieldSettingDto Title { get; private set; }

        public DisplayFieldSettingDto Department { get; private set; }

        public DisplayFieldSettingDto Unit { get; private set; }

        public DisplayFieldSettingDto OrganizationUnit { get; private set; }

        public DisplayFieldSettingDto Division { get; private set; }

        public DisplayFieldSettingDto Manager { get; private set; }

        public DisplayFieldSettingDto Group { get; private set; }

        public DisplayFieldSettingDto Password { get; private set; }

        public DisplayFieldSettingDto Other { get; private set; }

        public DisplayFieldSettingDto Ordered { get; private set; }

        public DisplayFieldSettingDto CreatedDate { get; private set; }

        public DisplayFieldSettingDto ChangedDate { get; private set; }

        public DisplayFieldSettingDto SynchronizationDate { get; private set; }
    }
}
