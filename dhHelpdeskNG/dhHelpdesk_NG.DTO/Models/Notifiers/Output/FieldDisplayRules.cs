namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldDisplayRules
    {
        public FieldDisplayRules(
            FieldDisplayRule domain,
            FieldDisplayRule loginName,
            FieldDisplayRule firstName,
            FieldDisplayRule initials,
            FieldDisplayRule lastName,
            FieldDisplayRule displayName,
            FieldDisplayRule place,
            FieldDisplayRule phone,
            FieldDisplayRule cellPhone,
            FieldDisplayRule email,
            FieldDisplayRule code,
            FieldDisplayRule postalAddress,
            FieldDisplayRule postalCode,
            FieldDisplayRule city,
            FieldDisplayRule title,
            FieldDisplayRule department,
            FieldDisplayRule unit,
            FieldDisplayRule organizationUnit,
            FieldDisplayRule division,
            FieldDisplayRule manager,
            FieldDisplayRule group,
            FieldDisplayRule other,
            FieldDisplayRule ordered,
            FieldDisplayRule changedDate)
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
            this.Group = group;
            this.Other = other;
            this.Ordered = ordered;
            this.ChangedDate = changedDate;
        }

        [NotNull]
        public FieldDisplayRule Domain { get; private set; }

        [NotNull]
        public FieldDisplayRule LoginName { get; private set; }

        [NotNull]
        public FieldDisplayRule FirstName { get; private set; }

        [NotNull]
        public FieldDisplayRule Initials { get; private set; }

        [NotNull]
        public FieldDisplayRule LastName { get; private set; }

        [NotNull]
        public FieldDisplayRule DisplayName { get; private set; }

        [NotNull]
        public FieldDisplayRule Place { get; private set; }

        [NotNull]
        public FieldDisplayRule Phone { get; private set; }

        [NotNull]
        public FieldDisplayRule CellPhone { get; private set; }

        [NotNull]
        public FieldDisplayRule Email { get; private set; }

        [NotNull]
        public FieldDisplayRule Code { get; private set; }

        [NotNull]
        public FieldDisplayRule PostalAddress { get; private set; }

        [NotNull]
        public FieldDisplayRule PostalCode { get; private set; }

        [NotNull]
        public FieldDisplayRule City { get; private set; }

        [NotNull]
        public FieldDisplayRule Title { get; private set; }

        [NotNull]
        public FieldDisplayRule Department { get; private set; }

        [NotNull]
        public FieldDisplayRule Unit { get; private set; }

        [NotNull]
        public FieldDisplayRule OrganizationUnit { get; private set; }

        [NotNull]
        public FieldDisplayRule Division { get; private set; }

        [NotNull]
        public FieldDisplayRule Manager { get; private set; }

        [NotNull]
        public FieldDisplayRule Group { get; private set; }

        [NotNull]
        public FieldDisplayRule Other { get; private set; }

        [NotNull]
        public FieldDisplayRule Ordered { get; private set; }

        [NotNull]
        public FieldDisplayRule ChangedDate { get; private set; }
    }
}
