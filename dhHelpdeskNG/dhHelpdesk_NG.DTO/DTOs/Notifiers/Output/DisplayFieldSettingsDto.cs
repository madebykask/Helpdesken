namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class DisplayFieldSettingsDto
    {
        public DisplayFieldSettingsDto(
            DisplayStringFieldSettingDto userId,
            DisplayFieldSettingDto domain,
            DisplayStringFieldSettingDto loginName,
            DisplayStringFieldSettingDto firstName,
            DisplayStringFieldSettingDto initials,
            DisplayStringFieldSettingDto lastName,
            DisplayStringFieldSettingDto displayName,
            DisplayStringFieldSettingDto place,
            DisplayStringFieldSettingDto phone,
            DisplayStringFieldSettingDto cellPhone,
            DisplayStringFieldSettingDto email,
            DisplayStringFieldSettingDto code,
            DisplayStringFieldSettingDto postalAddress,
            DisplayStringFieldSettingDto postalCode,
            DisplayStringFieldSettingDto city,
            DisplayStringFieldSettingDto title,
            DisplayFieldSettingDto department,
            DisplayStringFieldSettingDto unit,
            DisplayFieldSettingDto organizationUnit,
            DisplayFieldSettingDto division,
            DisplayFieldSettingDto manager,
            DisplayFieldSettingDto group,
            DisplayStringFieldSettingDto password,
            DisplayStringFieldSettingDto other,
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

        public DisplayStringFieldSettingDto UserId { get; private set; }

        public DisplayFieldSettingDto Domain { get; private set; }

        public DisplayStringFieldSettingDto LoginName { get; private set; }

        public DisplayStringFieldSettingDto FirstName { get; private set; }

        public DisplayStringFieldSettingDto Initials { get; private set; }

        public DisplayStringFieldSettingDto LastName { get; private set; }

        public DisplayStringFieldSettingDto DisplayName { get; private set; }

        public DisplayStringFieldSettingDto Place { get; private set; }

        public DisplayStringFieldSettingDto Phone { get; private set; }

        public DisplayStringFieldSettingDto CellPhone { get; private set; }

        public DisplayStringFieldSettingDto Email { get; private set; }

        public DisplayStringFieldSettingDto Code { get; private set; }

        public DisplayStringFieldSettingDto PostalAddress { get; private set; }

        public DisplayStringFieldSettingDto PostalCode { get; private set; }

        public DisplayStringFieldSettingDto City { get; private set; }

        public DisplayStringFieldSettingDto Title { get; private set; }

        public DisplayFieldSettingDto Department { get; private set; }

        public DisplayStringFieldSettingDto Unit { get; private set; }

        public DisplayFieldSettingDto OrganizationUnit { get; private set; }

        public DisplayFieldSettingDto Division { get; private set; }

        public DisplayFieldSettingDto Manager { get; private set; }

        public DisplayFieldSettingDto Group { get; private set; }

        public DisplayStringFieldSettingDto Password { get; private set; }

        public DisplayStringFieldSettingDto Other { get; private set; }

        public DisplayFieldSettingDto Ordered { get; private set; }

        public DisplayFieldSettingDto CreatedDate { get; private set; }

        public DisplayFieldSettingDto ChangedDate { get; private set; }

        public DisplayFieldSettingDto SynchronizationDate { get; private set; }
    }
}
