namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

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
            DropDownContent regionContent,
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
            NotifierLabelModel createdDate,
            NotifierLabelModel changedDate,
            NotifierLabelModel synchronizationDate)
        {
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
            this.RegionContent = regionContent;
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

        [NotNull]
        public NotifierInputTextBoxModel PostalAddress { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel CellPhone { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel City { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Code { get; private set; }

        [NotNull]
        public NotifierInputDropDownModel Department { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel DisplayName { get; private set; }

        [NotNull]
        public NotifierInputDropDownModel Division { get; private set; }

        [NotNull]
        public NotifierInputDropDownModel Domain { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Email { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel FirstName { get; private set; }

        [NotNull]
        public NotifierInputDropDownModel Group { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Initials { get; private set; }

        public bool IsActive { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel LastName { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Place { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel LoginName { get; private set; }

        [NotNull]
        public NotifierInputCheckBoxModel Ordered { get; private set; }

        public DropDownContent RegionContent { get; private set; }

        [NotNull]
        public NotifierInputDropDownModel OrganizationUnit { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Other { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Password { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Phone { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Title { get; private set; }

        [NotNull]
        public NotifierInputDropDownModel Manager { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel Unit { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel UserId { get; private set; }

        [NotNull]
        public NotifierInputTextBoxModel PostalCode { get; private set; }

        public NotifierLabelModel CreatedDate { get; private set; }

        public NotifierLabelModel ChangedDate { get; private set; }

        public NotifierLabelModel SynchronizationDate { get; private set; }

        #endregion

    }
}