namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class FieldDisplayRulesDto
    {
        public FieldDisplayRulesDto(
            FieldDisplayRuleDto domain,
            FieldDisplayRuleDto loginName,
            FieldDisplayRuleDto firstName,
            FieldDisplayRuleDto initials,
            FieldDisplayRuleDto lastName,
            FieldDisplayRuleDto displayName,
            FieldDisplayRuleDto place,
            FieldDisplayRuleDto phone,
            FieldDisplayRuleDto cellPhone,
            FieldDisplayRuleDto email,
            FieldDisplayRuleDto code,
            FieldDisplayRuleDto postalAddress,
            FieldDisplayRuleDto postalCode,
            FieldDisplayRuleDto city,
            FieldDisplayRuleDto title,
            FieldDisplayRuleDto department,
            FieldDisplayRuleDto unit,
            FieldDisplayRuleDto organizationUnit,
            FieldDisplayRuleDto division,
            FieldDisplayRuleDto manager,
            FieldDisplayRuleDto group,
            FieldDisplayRuleDto password,
            FieldDisplayRuleDto other,
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

        public FieldDisplayRuleDto LoginName { get; private set; }
        
        public FieldDisplayRuleDto FirstName { get; private set; }

        public FieldDisplayRuleDto Initials { get; private set; }

        public FieldDisplayRuleDto LastName { get; private set; }

        public FieldDisplayRuleDto DisplayName { get; private set; }

        public FieldDisplayRuleDto Place { get; private set; }

        public FieldDisplayRuleDto Phone { get; private set; }

        public FieldDisplayRuleDto CellPhone { get; private set; }

        public FieldDisplayRuleDto Email { get; private set; }

        public FieldDisplayRuleDto Code { get; private set; }

        public FieldDisplayRuleDto PostalAddress { get; private set; }

        public FieldDisplayRuleDto PostalCode { get; private set; }

        public FieldDisplayRuleDto City { get; private set; }

        public FieldDisplayRuleDto Title { get; private set; }

        public FieldDisplayRuleDto Department { get; private set; }

        public FieldDisplayRuleDto Unit { get; private set; }

        public FieldDisplayRuleDto OrganizationUnit { get; private set; }

        public FieldDisplayRuleDto Division { get; private set; }

        public FieldDisplayRuleDto Manager { get; private set; }

        public FieldDisplayRuleDto Group { get; private set; }

        public FieldDisplayRuleDto Password { get; private set; }

        public FieldDisplayRuleDto Other { get; private set; }

        public FieldDisplayRuleDto Ordered { get; private set; }
    }
}
