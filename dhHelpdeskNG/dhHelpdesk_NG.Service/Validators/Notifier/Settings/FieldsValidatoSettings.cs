namespace dhHelpdesk_NG.Service.Validators.Notifier.Settings
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class FieldsValidatoSettings
    {
        public FieldsValidatoSettings(FieldValidationSetting userId, FieldValidationSetting domain, FieldValidationSetting loginName, FieldValidationSetting firstName, FieldValidationSetting initials, FieldValidationSetting lastName, FieldValidationSetting name, FieldValidationSetting place, FieldValidationSetting phone, FieldValidationSetting cellPhone, FieldValidationSetting email, FieldValidationSetting code, FieldValidationSetting address, FieldValidationSetting postalCode, FieldValidationSetting city, FieldValidationSetting title, FieldValidationSetting region, FieldValidationSetting department, FieldValidationSetting unit, FieldValidationSetting organizationUnit, FieldValidationSetting division, FieldValidationSetting manager, FieldValidationSetting extendedInfo, FieldValidationSetting @group, FieldValidationSetting password, FieldValidationSetting other, FieldValidationSetting orderer, FieldValidationSetting createdDate, FieldValidationSetting changedDate, FieldValidationSetting synchronizationDate)
        {
            ArgumentsValidator.NotNull(userId, "userId");
            ArgumentsValidator.NotNull(domain, "domain");
            ArgumentsValidator.NotNull(loginName, "loginName");
            ArgumentsValidator.NotNull(firstName, "firstName");
            ArgumentsValidator.NotNull(initials, "initials");
            ArgumentsValidator.NotNull(lastName, "lastName");
            ArgumentsValidator.NotNull(name, "name");
            ArgumentsValidator.NotNull(place, "place");
            ArgumentsValidator.NotNull(phone, "phone");
            ArgumentsValidator.NotNull(cellPhone, "cellPhone");
            ArgumentsValidator.NotNull(email, "email");
            ArgumentsValidator.NotNull(code, "code");
            ArgumentsValidator.NotNull(address, "address");
            ArgumentsValidator.NotNull(postalCode, "postalCode");
            ArgumentsValidator.NotNull(city, "city");
            ArgumentsValidator.NotNull(title, "title");
            ArgumentsValidator.NotNull(region, "region");
            ArgumentsValidator.NotNull(department, "department");
            ArgumentsValidator.NotNull(unit, "unit");
            ArgumentsValidator.NotNull(organizationUnit, "organizationUnit");
            ArgumentsValidator.NotNull(division, "division");
            ArgumentsValidator.NotNull(manager, "manager");
            ArgumentsValidator.NotNull(extendedInfo, "extendedInfo");
            ArgumentsValidator.NotNull(group, "group");
            ArgumentsValidator.NotNull(password, "password");
            ArgumentsValidator.NotNull(other, "other");
            ArgumentsValidator.NotNull(orderer, "orderer");

            UserId = userId;
            Domain = domain;
            LoginName = loginName;
            FirstName = firstName;
            Initials = initials;
            LastName = lastName;
            Name = name;
            Place = place;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Code = code;
            Address = address;
            PostalCode = postalCode;
            City = city;
            Title = title;
            Region = region;
            Department = department;
            Unit = unit;
            OrganizationUnit = organizationUnit;
            Division = division;
            Manager = manager;
            ExtendedInfo = extendedInfo;
            Group = @group;
            Password = password;
            Other = other;
            Orderer = orderer;
        }

        public FieldValidationSetting UserId { get; private set; }

        public FieldValidationSetting Domain { get; private set; }

        public FieldValidationSetting LoginName { get; private set; }

        public FieldValidationSetting FirstName { get; private set; }

        public FieldValidationSetting Initials { get; private set; }

        public FieldValidationSetting LastName { get; private set; }

        public FieldValidationSetting Name { get; private set; }

        public FieldValidationSetting Place { get; private set; }

        public FieldValidationSetting Phone { get; private set; }

        public FieldValidationSetting CellPhone { get; private set; }

        public FieldValidationSetting Email { get; private set; }

        public FieldValidationSetting Code { get; private set; }

        public FieldValidationSetting Address { get; private set; }

        public FieldValidationSetting PostalCode { get; private set; }

        public FieldValidationSetting City { get; private set; }

        public FieldValidationSetting Title { get; private set; }

        public FieldValidationSetting Region { get; private set; }

        public FieldValidationSetting Department { get; private set; }

        public FieldValidationSetting Unit { get; private set; }

        public FieldValidationSetting OrganizationUnit { get; private set; }

        public FieldValidationSetting Division { get; private set; }

        public FieldValidationSetting Manager { get; private set; }

        public FieldValidationSetting ExtendedInfo { get; private set; }

        public FieldValidationSetting Group { get; private set; }

        public FieldValidationSetting Password { get; private set; }

        public FieldValidationSetting Other { get; private set; }

        public FieldValidationSetting Orderer { get; private set; }
    }
}
