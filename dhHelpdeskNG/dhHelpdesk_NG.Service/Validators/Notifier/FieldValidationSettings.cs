namespace DH.Helpdesk.Services.Validators.Notifier
{
    using DH.Helpdesk.Common.ValidationAttributes;

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

        [NotNull]
        public FieldValidationSetting Domain { get; private set; }

        [NotNull]
        public FieldValidationSetting LoginName { get; private set; }

        [NotNull]
        public FieldValidationSetting FirstName { get; private set; }

        [NotNull]
        public FieldValidationSetting Initials { get; private set; }

        [NotNull]
        public FieldValidationSetting LastName { get; private set; }

        [NotNull]
        public FieldValidationSetting DisplayName { get; private set; }

        [NotNull]
        public FieldValidationSetting Place { get; private set; }

        [NotNull]
        public FieldValidationSetting Phone { get; private set; }

        [NotNull]
        public FieldValidationSetting CellPhone { get; private set; }

        [NotNull]
        public FieldValidationSetting Email { get; private set; }

        [NotNull]
        public FieldValidationSetting Code { get; private set; }

        [NotNull]
        public FieldValidationSetting PostalAddress { get; private set; }

        [NotNull]
        public FieldValidationSetting PostalCode { get; private set; }

        [NotNull]
        public FieldValidationSetting City { get; private set; }

        [NotNull]
        public FieldValidationSetting Title { get; private set; }

        [NotNull]
        public FieldValidationSetting Department { get; private set; }

        [NotNull]
        public FieldValidationSetting Unit { get; private set; }

        [NotNull]
        public FieldValidationSetting OrganizationUnit { get; private set; }

        [NotNull]
        public FieldValidationSetting Division { get; private set; }

        [NotNull]
        public FieldValidationSetting Manager { get; private set; }

        [NotNull]
        public FieldValidationSetting Group { get; private set; }

        [NotNull]
        public FieldValidationSetting Password { get; private set; }

        [NotNull]
        public FieldValidationSetting Other { get; private set; }

        [NotNull]
        public FieldValidationSetting Ordered { get; private set; }
    }
}
