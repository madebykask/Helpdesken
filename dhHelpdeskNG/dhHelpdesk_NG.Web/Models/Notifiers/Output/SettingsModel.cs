namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class SettingsModel
    {
        public SettingsModel(
            SettingModel userId, 
            SettingModel domain,
            SettingModel loginName, 
            SettingModel firstName,
            SettingModel initials,
            SettingModel lastName,
            SettingModel displayName,
            SettingModel place,
            SettingModel phone,
            SettingModel cellPhone,
            SettingModel email,
            SettingModel code,
            SettingModel postalAddress,
            SettingModel postalCode,
            SettingModel city,
            SettingModel title,
            SettingModel department,
            SettingModel unit,
            SettingModel organizationUnit,
            SettingModel division,
            SettingModel manager,
            SettingModel group,
            SettingModel password,
            SettingModel other,
            SettingModel ordered,
            SettingModel createdDate,
            SettingModel changedDate,
            SettingModel synchronizationDate)
        {
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

        public SettingModel UserId { get; private set; }

        public SettingModel Domain { get; private set; }

        public SettingModel LoginName { get; private set; }

        public SettingModel FirstName { get; private set; }

        public SettingModel Initials { get; private set; }

        public SettingModel LastName { get; private set; }

        public SettingModel DisplayName { get; private set; }

        public SettingModel Place { get; private set; }

        public SettingModel Phone { get; private set; }

        public SettingModel CellPhone { get; private set; }

        public SettingModel Email { get; private set; }

        public SettingModel Code { get; private set; }

        public SettingModel PostalAddress { get; private set; }

        public SettingModel PostalCode { get; private set; }

        public SettingModel City { get; private set; }

        public SettingModel Title { get; private set; }

        public SettingModel Department { get; private set; }

        public SettingModel Unit { get; private set; }

        public SettingModel OrganizationUnit { get; private set; }

        public SettingModel Division { get; private set; }

        public SettingModel Manager { get; private set; }

        public SettingModel Group { get; private set; }

        public SettingModel Password { get; private set; }

        public SettingModel Other { get; private set; }

        public SettingModel Ordered { get; private set; }

        public SettingModel CreatedDate { get; private set; }

        public SettingModel ChangedDate { get; private set; }

        public SettingModel SynchronizationDate { get; private set; }
    }
}