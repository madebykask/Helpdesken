namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Input
{
    using System;

    public sealed class UpdatedFieldsSettingsDto
    {
        public UpdatedFieldsSettingsDto(
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
            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException("customerId", "Must be more than zero.");
            }

            if (languageId <= 0)
            {
                throw new ArgumentOutOfRangeException("languageId", "Must be more than zero.");
            }

            if (userId == null)
            {
                throw new ArgumentNullException("userId", "Value cannot be null.");
            }

            if (domain == null)
            {
                throw new ArgumentNullException("domain", "Value cannot be null.");
            }

            if (loginName == null)
            {
                throw new ArgumentNullException("loginName", "Value cannot be null.");
            }

            if (firstName == null)
            {
                throw new ArgumentNullException("firstName", "Value cannot be null.");
            }

            if (initials == null)
            {
                throw new ArgumentNullException("initials", "Value cannot be null.");
            }

            if (lastName == null)
            {
                throw new ArgumentNullException("lastName", "Value cannot be null.");
            }

            if (displayName == null)
            {
                throw new ArgumentNullException("displayName", "Value cannot be null.");
            }

            if (place == null)
            {
                throw new ArgumentNullException("place", "Value cannot be null.");
            }

            if (phone == null)
            {
                throw new ArgumentNullException("phone", "Value cannot be null.");
            }

            if (cellPhone == null)
            {
                throw new ArgumentNullException("cellPhone", "Value cannot be null.");
            }

            if (email == null)
            {
                throw new ArgumentNullException("email", "Value cannot be null.");
            }

            if (code == null)
            {
                throw new ArgumentNullException("code", "Value cannot be null.");
            }

            if (postalAddress == null)
            {
                throw new ArgumentNullException("postalAddress", "Value cannot be null.");
            }

            if (postalCode == null)
            {
                throw new ArgumentNullException("postalCode", "Value cannot be null.");
            }

            if (city == null)
            {
                throw new ArgumentNullException("city", "Value cannot be null.");
            }

            if (title == null)
            {
                throw new ArgumentNullException("title", "Value cannot be null.");
            }

            if (department == null)
            {
                throw new ArgumentNullException("department", "Value cannot be null.");
            }

            if (unit == null)
            {
                throw new ArgumentNullException("unit", "Value cannot be null.");
            }

            if (organizationUnit == null)
            {
                throw new ArgumentNullException("organizationUnit", "Value cannot be null.");
            }

            if (division == null)
            {
                throw new ArgumentNullException("division", "Value cannot be null.");
            }

            if (manager == null)
            {
                throw new ArgumentNullException("manager", "Value cannot be null.");
            }

            if (group == null)
            {
                throw new ArgumentNullException("group", "Value cannot be null.");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password", "Value cannot be null.");
            }

            if (other == null)
            {
                throw new ArgumentNullException("other", "Value cannot be null.");
            }

            if (ordered == null)
            {
                throw new ArgumentNullException("ordered", "Value cannot be null.");
            }

            if (createdDate == null)
            {
                throw new ArgumentNullException("createdDate", "Value cannot be null.");
            }

            if (changedDate == null)
            {
                throw new ArgumentNullException("changedDate", "Value cannot be null.");
            }

            if (synchronizationDate == null)
            {
                throw new ArgumentNullException("synchronizationDate", "Value cannot be null.");
            }

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
