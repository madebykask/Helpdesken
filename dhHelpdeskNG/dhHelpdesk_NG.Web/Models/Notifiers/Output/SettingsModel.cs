namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class SettingsModel
    {
        public SettingsModel(
            DropDownContent language,
            FieldSettingModel userId, 
            FieldSettingModel domain,
            FieldSettingModel loginName, 
            FieldSettingModel firstName,
            FieldSettingModel initials,
            FieldSettingModel lastName,
            FieldSettingModel displayName,
            FieldSettingModel place,
            FieldSettingModel phone,
            FieldSettingModel cellPhone,
            FieldSettingModel email,
            FieldSettingModel code,
            FieldSettingModel postalAddress,
            FieldSettingModel postalCode,
            FieldSettingModel city,
            FieldSettingModel title,
            FieldSettingModel department,
            FieldSettingModel unit,
            FieldSettingModel organizationUnit,
            FieldSettingModel division,
            FieldSettingModel manager,
            FieldSettingModel group,
            FieldSettingModel password,
            FieldSettingModel other,
            FieldSettingModel ordered,
            FieldSettingModel createdDate,
            FieldSettingModel changedDate,
            FieldSettingModel synchronizationDate)
        {
            this.Language = language;
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
            this.Division = division;
            this.Manager = manager;
            this.Group = group;
            this.Password = password;
            this.Other = other;
            this.Ordered = ordered;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        [NotNull]
        public DropDownContent Language { get; private set; }

        [NotNull]
        public FieldSettingModel UserId { get; private set; }

        [NotNull]
        public FieldSettingModel Domain { get; private set; }

        [NotNull]
        public FieldSettingModel LoginName { get; private set; }

        [NotNull]
        public FieldSettingModel FirstName { get; private set; }

        [NotNull]
        public FieldSettingModel Initials { get; private set; }

        [NotNull]
        public FieldSettingModel LastName { get; private set; }

        [NotNull]
        public FieldSettingModel DisplayName { get; private set; }

        [NotNull]
        public FieldSettingModel Place { get; private set; }

        [NotNull]
        public FieldSettingModel Phone { get; private set; }

        [NotNull]
        public FieldSettingModel CellPhone { get; private set; }

        [NotNull]
        public FieldSettingModel Email { get; private set; }

        [NotNull]
        public FieldSettingModel Code { get; private set; }

        [NotNull]
        public FieldSettingModel PostalAddress { get; private set; }

        [NotNull]
        public FieldSettingModel PostalCode { get; private set; }

        [NotNull]
        public FieldSettingModel City { get; private set; }

        [NotNull]
        public FieldSettingModel Title { get; private set; }

        [NotNull]
        public FieldSettingModel Department { get; private set; }

        [NotNull]
        public FieldSettingModel Unit { get; private set; }

        [NotNull]
        public FieldSettingModel OrganizationUnit { get; private set; }

        [NotNull]
        public FieldSettingModel Division { get; private set; }

        [NotNull]
        public FieldSettingModel Manager { get; private set; }

        [NotNull]
        public FieldSettingModel Group { get; private set; }

        [NotNull]
        public FieldSettingModel Password { get; private set; }

        [NotNull]
        public FieldSettingModel Other { get; private set; }

        [NotNull]
        public FieldSettingModel Ordered { get; private set; }

        [NotNull]
        public FieldSettingModel CreatedDate { get; private set; }

        [NotNull]
        public FieldSettingModel ChangedDate { get; private set; }

        [NotNull]
        public FieldSettingModel SynchronizationDate { get; private set; }
    }
}