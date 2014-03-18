namespace DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NotifierOverviewSettings
    {
        public NotifierOverviewSettings(
            FieldOverviewSetting userId,
            FieldOverviewSetting domain,
            FieldOverviewSetting loginName,
            FieldOverviewSetting firstName,
            FieldOverviewSetting initials,
            FieldOverviewSetting lastName,
            FieldOverviewSetting displayName,
            FieldOverviewSetting place,
            FieldOverviewSetting phone,
            FieldOverviewSetting cellPhone,
            FieldOverviewSetting email,
            FieldOverviewSetting code,
            FieldOverviewSetting postalAddress,
            FieldOverviewSetting postalCode,
            FieldOverviewSetting city,
            FieldOverviewSetting title,
            FieldOverviewSetting region,
            FieldOverviewSetting department,
            FieldOverviewSetting unit,
            FieldOverviewSetting organizationUnit,
            FieldOverviewSetting division,
            FieldOverviewSetting manager,
            FieldOverviewSetting group,
            FieldOverviewSetting other,
            FieldOverviewSetting ordered,
            FieldOverviewSetting createdDate,
            FieldOverviewSetting changedDate,
            FieldOverviewSetting synchronizationDate)
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
        public FieldOverviewSetting UserId { get; private set; }

        [NotNull]
        public FieldOverviewSetting Domain { get; private set; }

        [NotNull]
        public FieldOverviewSetting LoginName { get; private set; }

        [NotNull]
        public FieldOverviewSetting FirstName { get; private set; }

        [NotNull]
        public FieldOverviewSetting Initials { get; private set; }

        [NotNull]
        public FieldOverviewSetting LastName { get; private set; }

        [NotNull]
        public FieldOverviewSetting DisplayName { get; private set; }

        [NotNull]
        public FieldOverviewSetting Place { get; private set; }

        [NotNull]
        public FieldOverviewSetting Phone { get; private set; }

        [NotNull]
        public FieldOverviewSetting CellPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting Email { get; private set; }

        [NotNull]
        public FieldOverviewSetting Code { get; private set; }

        [NotNull]
        public FieldOverviewSetting PostalAddress { get; private set; }

        [NotNull]
        public FieldOverviewSetting PostalCode { get; private set; }

        [NotNull]
        public FieldOverviewSetting City { get; private set; }

        [NotNull]
        public FieldOverviewSetting Title { get; private set; }

        [NotNull]
        public FieldOverviewSetting Region { get; private set; }

        [NotNull]
        public FieldOverviewSetting Department { get; private set; }

        [NotNull]
        public FieldOverviewSetting Unit { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrganizationUnit { get; private set; }

        [NotNull]
        public FieldOverviewSetting Division { get; private set; }

        [NotNull]
        public FieldOverviewSetting Manager { get; private set; }

        [NotNull]
        public FieldOverviewSetting Group { get; private set; }

        [NotNull]
        public FieldOverviewSetting Other { get; private set; }

        [NotNull]
        public FieldOverviewSetting Ordered { get; private set; }

        [NotNull]
        public FieldOverviewSetting CreatedDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting ChangedDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting SynchronizationDate { get; private set; }
    }
}
