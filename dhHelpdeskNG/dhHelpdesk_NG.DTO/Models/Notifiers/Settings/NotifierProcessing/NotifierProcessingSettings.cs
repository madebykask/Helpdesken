namespace DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NotifierProcessingSettings
    {
        public NotifierProcessingSettings(
            FieldProcessingSetting userId,
            FieldProcessingSetting domain,
            FieldProcessingSetting loginName,
            FieldProcessingSetting firstName,
            FieldProcessingSetting initials,
            FieldProcessingSetting lastName,
            FieldProcessingSetting displayName,
            FieldProcessingSetting place,
            FieldProcessingSetting phone,
            FieldProcessingSetting cellPhone,
            FieldProcessingSetting email,
            FieldProcessingSetting code,
            FieldProcessingSetting postalAddress,
            FieldProcessingSetting postalCode,
            FieldProcessingSetting city,
            FieldProcessingSetting title,
            FieldProcessingSetting department,
            FieldProcessingSetting unit,
            FieldProcessingSetting organizationUnit,
            FieldProcessingSetting costCentre,
            FieldProcessingSetting division,
            FieldProcessingSetting manager,
            FieldProcessingSetting group,
            FieldProcessingSetting other,
            FieldProcessingSetting ordered,
            FieldProcessingSetting changedDate,
            FieldProcessingSetting language)
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
            this.Department = department;
            this.Unit = unit;
            this.OrganizationUnit = organizationUnit;
            this.CostCentre = costCentre;
            this.Division = division;
            this.Manager = manager;
            this.Group = group;
            this.Other = other;
            this.Ordered = ordered;
            this.ChangedDate = changedDate;
            this.Language = language;
        }

        [NotNull]
        public FieldProcessingSetting UserId { get; private set; }

        [NotNull]
        public FieldProcessingSetting Domain { get; private set; }

        [NotNull]
        public FieldProcessingSetting LoginName { get; private set; }

        [NotNull]
        public FieldProcessingSetting FirstName { get; private set; }

        [NotNull]
        public FieldProcessingSetting Initials { get; private set; }

        [NotNull]
        public FieldProcessingSetting LastName { get; private set; }

        [NotNull]
        public FieldProcessingSetting DisplayName { get; private set; }

        [NotNull]
        public FieldProcessingSetting Place { get; private set; }

        [NotNull]
        public FieldProcessingSetting Phone { get; private set; }

        [NotNull]
        public FieldProcessingSetting CellPhone { get; private set; }

        [NotNull]
        public FieldProcessingSetting Email { get; private set; }

        [NotNull]
        public FieldProcessingSetting Code { get; private set; }

        [NotNull]
        public FieldProcessingSetting PostalAddress { get; private set; }

        [NotNull]
        public FieldProcessingSetting PostalCode { get; private set; }

        [NotNull]
        public FieldProcessingSetting City { get; private set; }

        [NotNull]
        public FieldProcessingSetting Title { get; private set; }

        [NotNull]
        public FieldProcessingSetting Department { get; private set; }

        [NotNull]
        public FieldProcessingSetting Unit { get; private set; }

        [NotNull]
        public FieldProcessingSetting OrganizationUnit { get; private set; }

        public FieldProcessingSetting CostCentre { get; private set; }

        [NotNull]
        public FieldProcessingSetting Division { get; private set; }

        [NotNull]
        public FieldProcessingSetting Manager { get; private set; }


        [NotNull]
        public FieldProcessingSetting Language { get; private set; }

        [NotNull]
        public FieldProcessingSetting Group { get; private set; }

        [NotNull]
        public FieldProcessingSetting Other { get; private set; }

        [NotNull]
        public FieldProcessingSetting Ordered { get; private set; }

        [NotNull]
        public FieldProcessingSetting ChangedDate { get; private set; }
    }
}
