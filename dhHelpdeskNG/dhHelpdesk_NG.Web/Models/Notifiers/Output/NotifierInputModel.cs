namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifierInputModel
    {        
        #region Constructors and Destructors

        public NotifierInputModel(
            bool isNew,
            NotifierInputTextBoxModel userId, 
            NotifierInputDropDownModel domain,
            NotifierInputTextBoxModel loginName,
            NotifierInputTextBoxModel firstName,
            NotifierInputTextBoxModel initials,
            NotifierInputTextBoxModel lastName,
            NotifierInputTextBoxModel displayName,
            NotifierInputTextBoxModel place,
            NotifierInputTextBoxModel phone,
            NotifierInputTextBoxModel cellPhone,
            NotifierInputTextBoxModel email,
            NotifierInputTextBoxModel code,
            NotifierInputTextBoxModel postalAddress,
            NotifierInputTextBoxModel postalCode,
            NotifierInputTextBoxModel city,
            NotifierInputTextBoxModel title, 
            NotifierInputDropDownModel department,
            NotifierInputTextBoxModel unit, 
            NotifierInputDropDownModel organizationUnit, 
            NotifierInputDropDownModel division, 
            NotifierInputDropDownModel manager,
            NotifierInputDropDownModel group,
            NotifierInputTextBoxModel password,
            NotifierInputTextBoxModel other, 
            NotifierInputCheckBoxModel ordered, 
            bool isActive,
            NotifierInputTextBoxModel createdDate,
            NotifierInputTextBoxModel changedDate,
            NotifierInputTextBoxModel synchronizationDate)
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

            this.IsNew = isNew;
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
            this.IsActive = isActive;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        #endregion

        #region Public Properties

        public bool IsNew { get; private set; }

        public NotifierInputTextBoxModel PostalAddress { get; private set; }

        public NotifierInputTextBoxModel CellPhone { get; private set; }

        public NotifierInputTextBoxModel City { get; private set; }

        public NotifierInputTextBoxModel Code { get; private set; }

        public NotifierInputDropDownModel Department { get; private set; }

        public NotifierInputTextBoxModel DisplayName { get; private set; }

        public NotifierInputDropDownModel Division { get; private set; }

        public NotifierInputDropDownModel Domain { get; private set; }

        public NotifierInputTextBoxModel Email { get; private set; }

        public NotifierInputTextBoxModel FirstName { get; private set; }

        public NotifierInputDropDownModel Group { get; private set; }

        public NotifierInputTextBoxModel Initials { get; private set; }

        public bool IsActive { get; private set; }

        public NotifierInputTextBoxModel LastName { get; private set; }

        public NotifierInputTextBoxModel Place { get; private set; }

        public NotifierInputTextBoxModel LoginName { get; private set; }

        public NotifierInputCheckBoxModel Ordered { get; private set; }

        public NotifierInputDropDownModel OrganizationUnit { get; private set; }

        public NotifierInputTextBoxModel Other { get; private set; }

        public NotifierInputTextBoxModel Password { get; private set; }

        public NotifierInputTextBoxModel Phone { get; private set; }

        public NotifierInputTextBoxModel Title { get; private set; }

        public NotifierInputDropDownModel Manager { get; private set; }

        public NotifierInputTextBoxModel Unit { get; private set; }

        public NotifierInputTextBoxModel UserId { get; private set; }

        public NotifierInputTextBoxModel PostalCode { get; private set; }

        public NotifierInputTextBoxModel CreatedDate { get; private set; }

        public NotifierInputTextBoxModel ChangedDate { get; private set; }

        public NotifierInputTextBoxModel SynchronizationDate { get; private set; }

        #endregion

    }
}