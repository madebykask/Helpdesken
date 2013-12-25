namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Input
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class UpdatedFieldSettingsDto
    {
        public UpdatedFieldSettingsDto(
            int customerId,
            int languageId,
            UpdatedFieldSettingDto userId,
            UpdatedFieldSettingDto domain,
            UpdatedFieldSettingDto loginName,
            UpdatedFieldSettingDto firstName,
            UpdatedFieldSettingDto initials,
            UpdatedFieldSettingDto lastName,
            UpdatedFieldSettingDto displayName,
            UpdatedFieldSettingDto place,
            UpdatedFieldSettingDto phone,
            UpdatedFieldSettingDto cellPhone,
            UpdatedFieldSettingDto email,
            UpdatedFieldSettingDto code,
            UpdatedFieldSettingDto postalAddress,
            UpdatedFieldSettingDto postalCode,
            UpdatedFieldSettingDto city,
            UpdatedFieldSettingDto title,
            UpdatedFieldSettingDto department,
            UpdatedFieldSettingDto unit,
            UpdatedFieldSettingDto organizationUnit,
            UpdatedFieldSettingDto division,
            UpdatedFieldSettingDto manager,
            UpdatedFieldSettingDto group,
            UpdatedFieldSettingDto password,
            UpdatedFieldSettingDto other,
            UpdatedFieldSettingDto ordered,
            UpdatedFieldSettingDto createdDate,
            UpdatedFieldSettingDto changedDate,
            UpdatedFieldSettingDto synchronizationDate)
        {
            ArgumentsValidator.IsId(customerId, "customerId");
            ArgumentsValidator.IsId(languageId, "languageId");
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

        public int CustomerId { get; private set; }

        public int LanguageId { get; private set; }

        public UpdatedFieldSettingDto UserId { get; private set; }

        public UpdatedFieldSettingDto Domain { get; private set; }

        public UpdatedFieldSettingDto LoginName { get; private set; }

        public UpdatedFieldSettingDto FirstName { get; private set; }

        public UpdatedFieldSettingDto Initials { get; private set; }

        public UpdatedFieldSettingDto LastName { get; private set; }

        public UpdatedFieldSettingDto DisplayName { get; private set; }

        public UpdatedFieldSettingDto Place { get; private set; }

        public UpdatedFieldSettingDto Phone { get; private set; }

        public UpdatedFieldSettingDto CellPhone { get; private set; }

        public UpdatedFieldSettingDto Email { get; private set; }

        public UpdatedFieldSettingDto Code { get; private set; }

        public UpdatedFieldSettingDto PostalAddress { get; private set; }

        public UpdatedFieldSettingDto PostalCode { get; private set; }

        public UpdatedFieldSettingDto City { get; private set; }

        public UpdatedFieldSettingDto Title { get; private set; }

        public UpdatedFieldSettingDto Department { get; private set; }

        public UpdatedFieldSettingDto Unit { get; private set; }

        public UpdatedFieldSettingDto OrganizationUnit { get; private set; }

        public UpdatedFieldSettingDto Division { get; private set; }

        public UpdatedFieldSettingDto Manager { get; private set; }

        public UpdatedFieldSettingDto Group { get; private set; }

        public UpdatedFieldSettingDto Password { get; private set; }

        public UpdatedFieldSettingDto Other { get; private set; }

        public UpdatedFieldSettingDto Ordered { get; private set; }

        public UpdatedFieldSettingDto CreatedDate { get; private set; }

        public UpdatedFieldSettingDto ChangedDate { get; private set; }

        public UpdatedFieldSettingDto SynchronizationDate { get; private set; }
    }
}
