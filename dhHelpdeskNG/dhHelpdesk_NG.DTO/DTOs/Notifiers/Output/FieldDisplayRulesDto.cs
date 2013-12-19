namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class FieldDisplayRulesDto
    {
        public FieldDisplayRulesDto(
            FieldDisplayRuleDto domain,
            StringFieldDisplayRuleDto loginName,
            StringFieldDisplayRuleDto firstName,
            StringFieldDisplayRuleDto initials,
            StringFieldDisplayRuleDto lastName,
            StringFieldDisplayRuleDto displayName,
            StringFieldDisplayRuleDto place,
            StringFieldDisplayRuleDto phone,
            StringFieldDisplayRuleDto cellPhone,
            StringFieldDisplayRuleDto email,
            StringFieldDisplayRuleDto code,
            StringFieldDisplayRuleDto postalAddress,
            StringFieldDisplayRuleDto postalCode,
            StringFieldDisplayRuleDto city,
            StringFieldDisplayRuleDto title,
            FieldDisplayRuleDto department,
            StringFieldDisplayRuleDto unit,
            FieldDisplayRuleDto organizationUnit,
            FieldDisplayRuleDto division,
            FieldDisplayRuleDto manager,
            FieldDisplayRuleDto group,
            StringFieldDisplayRuleDto password,
            StringFieldDisplayRuleDto other,
            FieldDisplayRuleDto ordered)
        {
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
        }

        public FieldDisplayRuleDto Domain { get; private set; }

        public StringFieldDisplayRuleDto LoginName { get; private set; }
        
        public StringFieldDisplayRuleDto FirstName { get; private set; }

        public StringFieldDisplayRuleDto Initials { get; private set; }

        public StringFieldDisplayRuleDto LastName { get; private set; }

        public StringFieldDisplayRuleDto DisplayName { get; private set; }

        public StringFieldDisplayRuleDto Place { get; private set; }

        public StringFieldDisplayRuleDto Phone { get; private set; }

        public StringFieldDisplayRuleDto CellPhone { get; private set; }

        public StringFieldDisplayRuleDto Email { get; private set; }

        public StringFieldDisplayRuleDto Code { get; private set; }

        public StringFieldDisplayRuleDto PostalAddress { get; private set; }

        public StringFieldDisplayRuleDto PostalCode { get; private set; }

        public StringFieldDisplayRuleDto City { get; private set; }

        public StringFieldDisplayRuleDto Title { get; private set; }

        public FieldDisplayRuleDto Department { get; private set; }

        public StringFieldDisplayRuleDto Unit { get; private set; }

        public FieldDisplayRuleDto OrganizationUnit { get; private set; }

        public FieldDisplayRuleDto Division { get; private set; }

        public FieldDisplayRuleDto Manager { get; private set; }

        public FieldDisplayRuleDto Group { get; private set; }

        public StringFieldDisplayRuleDto Password { get; private set; }

        public StringFieldDisplayRuleDto Other { get; private set; }

        public FieldDisplayRuleDto Ordered { get; private set; }
    }
}
