namespace DH.Helpdesk.BusinessData.Models.Notifiers.Input
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedFieldSettings
    {
        public UpdatedFieldSettings(
            int customerId,
            int languageId,
            UpdatedFieldSetting userId,
            UpdatedFieldSetting domain,
            UpdatedFieldSetting loginName,
            UpdatedFieldSetting firstName,
            UpdatedFieldSetting initials,
            UpdatedFieldSetting lastName,
            UpdatedFieldSetting displayName,
            UpdatedFieldSetting place,
            UpdatedFieldSetting phone,
            UpdatedFieldSetting cellPhone,
            UpdatedFieldSetting email,
            UpdatedFieldSetting code,
            UpdatedFieldSetting postalAddress,
            UpdatedFieldSetting postalCode,
            UpdatedFieldSetting city,
            UpdatedFieldSetting title,
            UpdatedFieldSetting region,
            UpdatedFieldSetting department,
            UpdatedFieldSetting unit,
            UpdatedFieldSetting organizationUnit,
            UpdatedFieldSetting division,
            UpdatedFieldSetting manager,
            UpdatedFieldSetting group,
            UpdatedFieldSetting other,
            UpdatedFieldSetting ordered,
            UpdatedFieldSetting createdDate,
            UpdatedFieldSetting changedDate,
            UpdatedFieldSetting synchronizationDate)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
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

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }

        [NotNull]
        public UpdatedFieldSetting UserId { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Domain { get; private set; }

        [NotNull]
        public UpdatedFieldSetting LoginName { get; private set; }

        [NotNull]
        public UpdatedFieldSetting FirstName { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Initials { get; private set; }

        [NotNull]
        public UpdatedFieldSetting LastName { get; private set; }

        [NotNull]
        public UpdatedFieldSetting DisplayName { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Place { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Phone { get; private set; }

        [NotNull]
        public UpdatedFieldSetting CellPhone { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Email { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Code { get; private set; }

        [NotNull]
        public UpdatedFieldSetting PostalAddress { get; private set; }

        [NotNull]
        public UpdatedFieldSetting PostalCode { get; private set; }

        [NotNull]
        public UpdatedFieldSetting City { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Title { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Region { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Department { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Unit { get; private set; }

        [NotNull]
        public UpdatedFieldSetting OrganizationUnit { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Division { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Manager { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Group { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Other { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Ordered { get; private set; }

        [NotNull]
        public UpdatedFieldSetting CreatedDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting ChangedDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting SynchronizationDate { get; private set; }
    }
}
