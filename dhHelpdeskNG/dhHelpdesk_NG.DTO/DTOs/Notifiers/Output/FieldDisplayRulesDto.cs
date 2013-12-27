namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
            FieldDisplayRuleDto ordered,
            FieldDisplayRuleDto changedDate)
        {
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
            ChangedDate = changedDate;
        }

        [NotNull]
        public FieldDisplayRuleDto Domain { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto LoginName { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto FirstName { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Initials { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto LastName { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto DisplayName { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Place { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Phone { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto CellPhone { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Email { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Code { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto PostalAddress { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto PostalCode { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto City { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Title { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Department { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Unit { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto OrganizationUnit { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Division { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Manager { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Group { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Password { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Other { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto Ordered { get; private set; }

        [NotNull]
        public FieldDisplayRuleDto ChangedDate { get; private set; }
    }
}
