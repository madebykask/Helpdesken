namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DisplayFieldSettings
    {
        public DisplayFieldSettings(
            DisplayFieldSetting userId,
            DisplayFieldSetting domain,
            DisplayFieldSetting loginName,
            DisplayFieldSetting firstName,
            DisplayFieldSetting initials,
            DisplayFieldSetting lastName,
            DisplayFieldSetting displayName,
            DisplayFieldSetting place,
            DisplayFieldSetting phone,
            DisplayFieldSetting cellPhone,
            DisplayFieldSetting email,
            DisplayFieldSetting code,
            DisplayFieldSetting postalAddress,
            DisplayFieldSetting postalCode,
            DisplayFieldSetting city,
            DisplayFieldSetting title,
            DisplayFieldSetting region,
            DisplayFieldSetting department,
            DisplayFieldSetting unit,
            DisplayFieldSetting organizationUnit,
            DisplayFieldSetting division,
            DisplayFieldSetting manager,
            DisplayFieldSetting group,
            DisplayFieldSetting other,
            DisplayFieldSetting ordered,
            DisplayFieldSetting createdDate,
            DisplayFieldSetting changedDate,
            DisplayFieldSetting synchronizationDate)
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
            this.Group = @group;
            this.Other = other;
            this.Ordered = ordered;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        [NotNull]
        public DisplayFieldSetting UserId { get; private set; }

        [NotNull]
        public DisplayFieldSetting Domain { get; private set; }

        [NotNull]
        public DisplayFieldSetting LoginName { get; private set; }

        [NotNull]
        public DisplayFieldSetting FirstName { get; private set; }

        [NotNull]
        public DisplayFieldSetting Initials { get; private set; }

        [NotNull]
        public DisplayFieldSetting LastName { get; private set; }

        [NotNull]
        public DisplayFieldSetting DisplayName { get; private set; }

        [NotNull]
        public DisplayFieldSetting Place { get; private set; }

        [NotNull]
        public DisplayFieldSetting Phone { get; private set; }

        [NotNull]
        public DisplayFieldSetting CellPhone { get; private set; }

        [NotNull]
        public DisplayFieldSetting Email { get; private set; }

        [NotNull]
        public DisplayFieldSetting Code { get; private set; }

        [NotNull]
        public DisplayFieldSetting PostalAddress { get; private set; }

        [NotNull]
        public DisplayFieldSetting PostalCode { get; private set; }

        [NotNull]
        public DisplayFieldSetting City { get; private set; }

        [NotNull]
        public DisplayFieldSetting Title { get; private set; }

        [NotNull]
        public DisplayFieldSetting Region { get; private set; }

        [NotNull]
        public DisplayFieldSetting Department { get; private set; }

        [NotNull]
        public DisplayFieldSetting Unit { get; private set; }

        [NotNull]
        public DisplayFieldSetting OrganizationUnit { get; private set; }

        [NotNull]
        public DisplayFieldSetting Division { get; private set; }

        [NotNull]
        public DisplayFieldSetting Manager { get; private set; }

        [NotNull]
        public DisplayFieldSetting Group { get; private set; }

        [NotNull]
        public DisplayFieldSetting Other { get; private set; }

        [NotNull]
        public DisplayFieldSetting Ordered { get; private set; }

        [NotNull]
        public DisplayFieldSetting CreatedDate { get; private set; }

        [NotNull]
        public DisplayFieldSetting ChangedDate { get; private set; }

        [NotNull]
        public DisplayFieldSetting SynchronizationDate { get; private set; }
    }
}
