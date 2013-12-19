namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.Tools;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class SettingsModel
    {
        public SettingsModel(
            DropDownContent language,
            StringFieldSettingModel userId, 
            FieldSettingModel domain,
            StringFieldSettingModel loginName, 
            StringFieldSettingModel firstName,
            StringFieldSettingModel initials,
            StringFieldSettingModel lastName,
            StringFieldSettingModel displayName,
            StringFieldSettingModel place,
            StringFieldSettingModel phone,
            StringFieldSettingModel cellPhone,
            StringFieldSettingModel email,
            StringFieldSettingModel code,
            StringFieldSettingModel postalAddress,
            StringFieldSettingModel postalCode,
            StringFieldSettingModel city,
            StringFieldSettingModel title,
            FieldSettingModel department,
            StringFieldSettingModel unit,
            FieldSettingModel organizationUnit,
            FieldSettingModel division,
            FieldSettingModel manager,
            FieldSettingModel group,
            StringFieldSettingModel password,
            StringFieldSettingModel other,
            FieldSettingModel ordered,
            FieldSettingModel createdDate,
            FieldSettingModel changedDate,
            FieldSettingModel synchronizationDate)
        {
            ArgumentsValidator.NotNull(language, "language");
            ArgumentsValidator.NotNull(userId, "userId");
            ArgumentsValidator.NotNull(domain, "domain");
            ArgumentsValidator.NotNull(loginName, "loginName");
            ArgumentsValidator.NotNull(firstName, "firstName");
            ArgumentsValidator.NotNull(initials, "initials");
            ArgumentsValidator.NotNull(lastName, "lastName");
            ArgumentsValidator.NotNull(displayName, "displayName");
            ArgumentsValidator.NotNull(place, "place");
            ArgumentsValidator.NotNull(phone, "phone");
            ArgumentsValidator.NotNull(cellPhone, "cellPhone");
            ArgumentsValidator.NotNull(email, "email");
            ArgumentsValidator.NotNull(code, "code");
            ArgumentsValidator.NotNull(postalAddress, "postalAddress");
            ArgumentsValidator.NotNull(postalCode, "postalCode");
            ArgumentsValidator.NotNull(city, "city");
            ArgumentsValidator.NotNull(title, "title");
            ArgumentsValidator.NotNull(department, "department");
            ArgumentsValidator.NotNull(unit, "unit");
            ArgumentsValidator.NotNull(organizationUnit, "organizationUnit");
            ArgumentsValidator.NotNull(division, "division");
            ArgumentsValidator.NotNull(manager, "manager");
            ArgumentsValidator.NotNull(group, "group");
            ArgumentsValidator.NotNull(password, "password");
            ArgumentsValidator.NotNull(other, "other");
            ArgumentsValidator.NotNull(ordered, "ordered");
            ArgumentsValidator.NotNull(createdDate, "createdDate");
            ArgumentsValidator.NotNull(changedDate, "changedDate");
            ArgumentsValidator.NotNull(synchronizationDate, "synchronizationDate");

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

        public DropDownContent Language { get; private set; }

        public StringFieldSettingModel UserId { get; private set; }

        public FieldSettingModel Domain { get; private set; }

        public StringFieldSettingModel LoginName { get; private set; }

        public StringFieldSettingModel FirstName { get; private set; }

        public StringFieldSettingModel Initials { get; private set; }

        public StringFieldSettingModel LastName { get; private set; }

        public StringFieldSettingModel DisplayName { get; private set; }

        public StringFieldSettingModel Place { get; private set; }

        public StringFieldSettingModel Phone { get; private set; }

        public StringFieldSettingModel CellPhone { get; private set; }

        public StringFieldSettingModel Email { get; private set; }

        public StringFieldSettingModel Code { get; private set; }

        public StringFieldSettingModel PostalAddress { get; private set; }

        public StringFieldSettingModel PostalCode { get; private set; }

        public StringFieldSettingModel City { get; private set; }

        public StringFieldSettingModel Title { get; private set; }

        public FieldSettingModel Department { get; private set; }

        public StringFieldSettingModel Unit { get; private set; }

        public FieldSettingModel OrganizationUnit { get; private set; }

        public FieldSettingModel Division { get; private set; }

        public FieldSettingModel Manager { get; private set; }

        public FieldSettingModel Group { get; private set; }

        public StringFieldSettingModel Password { get; private set; }

        public StringFieldSettingModel Other { get; private set; }

        public FieldSettingModel Ordered { get; private set; }

        public FieldSettingModel CreatedDate { get; private set; }

        public FieldSettingModel ChangedDate { get; private set; }

        public FieldSettingModel SynchronizationDate { get; private set; }
    }
}