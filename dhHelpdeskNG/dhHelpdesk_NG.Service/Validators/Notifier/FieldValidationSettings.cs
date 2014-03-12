namespace DH.Helpdesk.Services.Validators.Notifier
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Validators.Common;

    public sealed class FieldValidationSettings
    {
        public FieldValidationSettings(
            ElementaryValidationRule domain,
            ElementaryValidationRule loginName,
            ElementaryValidationRule firstName,
            ElementaryValidationRule initials,
            ElementaryValidationRule lastName,
            ElementaryValidationRule displayName,
            ElementaryValidationRule place,
            ElementaryValidationRule phone,
            ElementaryValidationRule cellPhone,
            ElementaryValidationRule email,
            ElementaryValidationRule code,
            ElementaryValidationRule postalAddress,
            ElementaryValidationRule postalCode,
            ElementaryValidationRule city,
            ElementaryValidationRule title,
            ElementaryValidationRule department,
            ElementaryValidationRule unit,
            ElementaryValidationRule organizationUnit,
            ElementaryValidationRule division,
            ElementaryValidationRule manager,
            ElementaryValidationRule group,
            ElementaryValidationRule other,
            ElementaryValidationRule ordered)
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
            this.Other = other;
            this.Ordered = ordered;
        }

        [NotNull]
        public ElementaryValidationRule Domain { get; private set; }

        [NotNull]
        public ElementaryValidationRule LoginName { get; private set; }

        [NotNull]
        public ElementaryValidationRule FirstName { get; private set; }

        [NotNull]
        public ElementaryValidationRule Initials { get; private set; }

        [NotNull]
        public ElementaryValidationRule LastName { get; private set; }

        [NotNull]
        public ElementaryValidationRule DisplayName { get; private set; }

        [NotNull]
        public ElementaryValidationRule Place { get; private set; }

        [NotNull]
        public ElementaryValidationRule Phone { get; private set; }

        [NotNull]
        public ElementaryValidationRule CellPhone { get; private set; }

        [NotNull]
        public ElementaryValidationRule Email { get; private set; }

        [NotNull]
        public ElementaryValidationRule Code { get; private set; }

        [NotNull]
        public ElementaryValidationRule PostalAddress { get; private set; }

        [NotNull]
        public ElementaryValidationRule PostalCode { get; private set; }

        [NotNull]
        public ElementaryValidationRule City { get; private set; }

        [NotNull]
        public ElementaryValidationRule Title { get; private set; }

        [NotNull]
        public ElementaryValidationRule Department { get; private set; }

        [NotNull]
        public ElementaryValidationRule Unit { get; private set; }

        [NotNull]
        public ElementaryValidationRule OrganizationUnit { get; private set; }

        [NotNull]
        public ElementaryValidationRule Division { get; private set; }

        [NotNull]
        public ElementaryValidationRule Manager { get; private set; }

        [NotNull]
        public ElementaryValidationRule Group { get; private set; }

        [NotNull]
        public ElementaryValidationRule Other { get; private set; }

        [NotNull]
        public ElementaryValidationRule Ordered { get; private set; }
    }
}
