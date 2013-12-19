namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class FieldSettingsDto
    {
        public FieldSettingsDto(
            StringFieldSettingDto userId,
            FieldSettingDto domain,
            StringFieldSettingDto loginName,
            StringFieldSettingDto firstName,
            StringFieldSettingDto initials,
            StringFieldSettingDto lastName,
            StringFieldSettingDto displayName,
            StringFieldSettingDto place,
            StringFieldSettingDto phone,
            StringFieldSettingDto cellPhone,
            StringFieldSettingDto email,
            StringFieldSettingDto code,
            StringFieldSettingDto postalAddress,
            StringFieldSettingDto postalCode,
            StringFieldSettingDto city,
            StringFieldSettingDto title,
            FieldSettingDto department,
            StringFieldSettingDto unit,
            FieldSettingDto organizationUnit,
            FieldSettingDto division,
            FieldSettingDto manager,
            FieldSettingDto group,
            StringFieldSettingDto password,
            StringFieldSettingDto other,
            FieldSettingDto ordered,
            FieldSettingDto createdDate,
            FieldSettingDto changedDate,
            FieldSettingDto synchronizationDate)
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
            ArgumentsValidator.NotNull(ordered, "ordered");
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
            Group = group;
            Password = password;
            Other = other;
            Ordered = ordered;
            CreatedDate = createdDate;
            ChangedDate = changedDate;
            SynchronizationDate = synchronizationDate;
        }

        public StringFieldSettingDto UserId { get; private set; }

        public FieldSettingDto Domain { get; private set; }

        public StringFieldSettingDto LoginName { get; private set; }

        public StringFieldSettingDto FirstName { get; private set; }

        public StringFieldSettingDto Initials { get; private set; }

        public StringFieldSettingDto LastName { get; private set; }

        public StringFieldSettingDto DisplayName { get; private set; }

        public StringFieldSettingDto Place { get; private set; }

        public StringFieldSettingDto Phone { get; private set; }

        public StringFieldSettingDto CellPhone { get; private set; }

        public StringFieldSettingDto Email { get; private set; }

        public StringFieldSettingDto Code { get; private set; }

        public StringFieldSettingDto PostalAddress { get; private set; }

        public StringFieldSettingDto PostalCode { get; private set; }

        public StringFieldSettingDto City { get; private set; }

        public StringFieldSettingDto Title { get; private set; }

        public FieldSettingDto Department { get; private set; }

        public StringFieldSettingDto Unit { get; private set; }

        public FieldSettingDto OrganizationUnit { get; private set; }

        public FieldSettingDto Division { get; private set; }

        public FieldSettingDto Manager { get; private set; }

        public FieldSettingDto Group { get; private set; }

        public StringFieldSettingDto Password { get; private set; }

        public StringFieldSettingDto Other { get; private set; }

        public FieldSettingDto Ordered { get; private set; }

        public FieldSettingDto CreatedDate { get; private set; }

        public FieldSettingDto ChangedDate { get; private set; }

        public FieldSettingDto SynchronizationDate { get; private set; }
    }
}