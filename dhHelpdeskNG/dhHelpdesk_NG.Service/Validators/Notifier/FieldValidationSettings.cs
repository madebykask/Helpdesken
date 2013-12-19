namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class FieldValidationSettings
    {
        public FieldValidationSettings(
            FieldValidationSetting domain,
            StringFieldValidationSetting loginName,
            StringFieldValidationSetting firstName,
            StringFieldValidationSetting initials,
            StringFieldValidationSetting lastName,
            StringFieldValidationSetting displayName,
            StringFieldValidationSetting place,
            StringFieldValidationSetting phone,
            StringFieldValidationSetting cellPhone,
            StringFieldValidationSetting email,
            StringFieldValidationSetting code,
            StringFieldValidationSetting postalAddress,
            StringFieldValidationSetting postalCode,
            StringFieldValidationSetting city,
            StringFieldValidationSetting title,
            FieldValidationSetting department,
            StringFieldValidationSetting unit,
            FieldValidationSetting organizationUnit,
            FieldValidationSetting division,
            FieldValidationSetting manager,
            FieldValidationSetting group,
            StringFieldValidationSetting password,
            StringFieldValidationSetting other,
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

        public StringFieldValidationSetting LoginName { get; private set; }

        public StringFieldValidationSetting FirstName { get; private set; }

        public StringFieldValidationSetting Initials { get; private set; }

        public StringFieldValidationSetting LastName { get; private set; }

        public StringFieldValidationSetting DisplayName { get; private set; }

        public StringFieldValidationSetting Place { get; private set; }

        public StringFieldValidationSetting Phone { get; private set; }

        public StringFieldValidationSetting CellPhone { get; private set; }

        public StringFieldValidationSetting Email { get; private set; }

        public StringFieldValidationSetting Code { get; private set; }

        public StringFieldValidationSetting PostalAddress { get; private set; }

        public StringFieldValidationSetting PostalCode { get; private set; }

        public StringFieldValidationSetting City { get; private set; }

        public StringFieldValidationSetting Title { get; private set; }

        public FieldValidationSetting Department { get; private set; }

        public StringFieldValidationSetting Unit { get; private set; }

        public FieldValidationSetting OrganizationUnit { get; private set; }

        public FieldValidationSetting Division { get; private set; }

        public FieldValidationSetting Manager { get; private set; }

        public FieldValidationSetting Group { get; private set; }

        public StringFieldValidationSetting Password { get; private set; }

        public StringFieldValidationSetting Other { get; private set; }

        public FieldValidationSetting Ordered { get; private set; }
    }
}
