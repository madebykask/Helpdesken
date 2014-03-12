namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldSettings
    {
        public FieldSettings(
            FieldSetting userId,
            FieldSetting domain,
            FieldSetting loginName,
            FieldSetting firstName,
            FieldSetting initials,
            FieldSetting lastName,
            FieldSetting displayName,
            FieldSetting place,
            FieldSetting phone,
            FieldSetting cellPhone,
            FieldSetting email,
            FieldSetting code,
            FieldSetting postalAddress,
            FieldSetting postalCode,
            FieldSetting city,
            FieldSetting title,
            FieldSetting region,
            FieldSetting department,
            FieldSetting unit,
            FieldSetting organizationUnit,
            FieldSetting division,
            FieldSetting manager,
            FieldSetting group,
            FieldSetting other,
            FieldSetting ordered,
            FieldSetting createdDate,
            FieldSetting changedDate,
            FieldSetting synchronizationDate)
        {
            this.UserId = userId;
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
            this.Region = region;
            this.Department = department;
            this.Unit = unit;
            this.OrganizationUnit = organizationUnit;
            this.Division = division;
            this.Manager = manager;
            this.Group = group;
            this.Other = other;
            this.Ordered = ordered;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        [NotNull]
        public FieldSetting UserId { get; private set; }

        [NotNull]
        public FieldSetting Domain { get; private set; }

        [NotNull]
        public FieldSetting LoginName { get; private set; }

        [NotNull]
        public FieldSetting FirstName { get; private set; }

        [NotNull]
        public FieldSetting Initials { get; private set; }

        [NotNull]
        public FieldSetting LastName { get; private set; }

        [NotNull]
        public FieldSetting DisplayName { get; private set; }

        [NotNull]
        public FieldSetting Place { get; private set; }

        [NotNull]
        public FieldSetting Phone { get; private set; }

        [NotNull]
        public FieldSetting CellPhone { get; private set; }

        [NotNull]
        public FieldSetting Email { get; private set; }

        [NotNull]
        public FieldSetting Code { get; private set; }

        [NotNull]
        public FieldSetting PostalAddress { get; private set; }

        [NotNull]
        public FieldSetting PostalCode { get; private set; }

        [NotNull]
        public FieldSetting City { get; private set; }

        [NotNull]
        public FieldSetting Title { get; private set; }

        [NotNull]
        public FieldSetting Region { get; private set; }

        [NotNull]
        public FieldSetting Department { get; private set; }

        [NotNull]
        public FieldSetting Unit { get; private set; }

        [NotNull]
        public FieldSetting OrganizationUnit { get; private set; }

        [NotNull]
        public FieldSetting Division { get; private set; }

        [NotNull]
        public FieldSetting Manager { get; private set; }

        [NotNull]
        public FieldSetting Group { get; private set; }

        [NotNull]
        public FieldSetting Other { get; private set; }

        [NotNull]
        public FieldSetting Ordered { get; private set; }

        [NotNull]
        public FieldSetting CreatedDate { get; private set; }

        [NotNull]
        public FieldSetting ChangedDate { get; private set; }

        [NotNull]
        public FieldSetting SynchronizationDate { get; private set; }
    }
}