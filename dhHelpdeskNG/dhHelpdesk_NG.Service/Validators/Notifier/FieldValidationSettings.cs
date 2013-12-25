namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class FieldValidationSettings
    {
        public FieldValidationSettings(
            FieldValidationSetting domain,
            FieldValidationSetting loginName,
            FieldValidationSetting firstName,
            FieldValidationSetting initials,
            FieldValidationSetting lastName,
            FieldValidationSetting displayName,
            FieldValidationSetting place,
            FieldValidationSetting phone,
            FieldValidationSetting cellPhone,
            FieldValidationSetting email,
            FieldValidationSetting code,
            FieldValidationSetting postalAddress,
            FieldValidationSetting postalCode,
            FieldValidationSetting city,
            FieldValidationSetting title,
            FieldValidationSetting department,
            FieldValidationSetting unit,
            FieldValidationSetting organizationUnit,
            FieldValidationSetting division,
            FieldValidationSetting manager,
            FieldValidationSetting group,
            FieldValidationSetting password,
            FieldValidationSetting other,
            FieldValidationSetting ordered)
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
            ArgumentsValidator.NotNull(ordered, "ordered");

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
        }

        public FieldValidationSetting Domain { get; private set; }

        public FieldValidationSetting LoginName { get; private set; }

        public FieldValidationSetting FirstName { get; private set; }

        public FieldValidationSetting Initials { get; private set; }

        public FieldValidationSetting LastName { get; private set; }

        public FieldValidationSetting DisplayName { get; private set; }

        public FieldValidationSetting Place { get; private set; }

        public FieldValidationSetting Phone { get; private set; }

        public FieldValidationSetting CellPhone { get; private set; }

        public FieldValidationSetting Email { get; private set; }

        public FieldValidationSetting Code { get; private set; }

        public FieldValidationSetting PostalAddress { get; private set; }

        public FieldValidationSetting PostalCode { get; private set; }

        public FieldValidationSetting City { get; private set; }

        public FieldValidationSetting Title { get; private set; }

        public FieldValidationSetting Department { get; private set; }

        public FieldValidationSetting Unit { get; private set; }

        public FieldValidationSetting OrganizationUnit { get; private set; }

        public FieldValidationSetting Division { get; private set; }

        public FieldValidationSetting Manager { get; private set; }

        public FieldValidationSetting Group { get; private set; }

        public FieldValidationSetting Password { get; private set; }

        public FieldValidationSetting Other { get; private set; }

        public FieldValidationSetting Ordered { get; private set; }
    }
}
