namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NotifierInputModel
    {        
        #region Constructors and Destructors

        public NotifierInputModel(
            bool isNew,
            TextBoxModel userId, 
            DropDownModel domain,
            TextBoxModel loginName,
            TextBoxModel firstName,
            TextBoxModel initials,
            TextBoxModel lastName,
            TextBoxModel displayName,
            TextBoxModel place,
            TextBoxModel phone,
            TextBoxModel cellPhone,
            TextBoxModel email,
            TextBoxModel code,
            TextBoxModel postalAddress,
            TextBoxModel postalCode,
            TextBoxModel city,
            TextBoxModel title,
            DropDownModel region,
            DropDownModel department,
            TextBoxModel unit, 
            DropDownModel organizationUnit, 
            DropDownModel division, 
            DropDownModel manager,
            DropDownModel group,
            TextBoxModel other, 
            CheckBoxModel ordered, 
            bool isActive,
            LabelModel createdDate,
            LabelModel changedDate,
            LabelModel synchronizationDate)
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
            this.Region = region;
            this.Department = department;
            this.Unit = unit;
            this.OrganizationUnit = organizationUnit;
            this.Division = division;
            this.Manager = manager;
            this.Group = group;
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
        public TextBoxModel PostalAddress { get; private set; }

        [NotNull]
        public TextBoxModel CellPhone { get; private set; }

        [NotNull]
        public TextBoxModel City { get; private set; }

        [NotNull]
        public TextBoxModel Code { get; private set; }

        [NotNull]
        public DropDownModel Region { get; private set; }
        
        [NotNull]
        public DropDownModel Department { get; private set; }

        [NotNull]
        public TextBoxModel DisplayName { get; private set; }

        [NotNull]
        public DropDownModel Division { get; private set; }

        [NotNull]
        public DropDownModel Domain { get; private set; }

        [NotNull]
        public TextBoxModel Email { get; private set; }

        [NotNull]
        public TextBoxModel FirstName { get; private set; }

        [NotNull]
        public DropDownModel Group { get; private set; }

        [NotNull]
        public TextBoxModel Initials { get; private set; }

        public bool IsActive { get; private set; }

        [NotNull]
        public TextBoxModel LastName { get; private set; }

        [NotNull]
        public TextBoxModel Place { get; private set; }

        [NotNull]
        public TextBoxModel LoginName { get; private set; }

        [NotNull]
        public CheckBoxModel Ordered { get; private set; }

        [NotNull]
        public DropDownModel OrganizationUnit { get; private set; }

        [NotNull]
        public TextBoxModel Other { get; private set; }

        [NotNull]
        public TextBoxModel Phone { get; private set; }

        [NotNull]
        public TextBoxModel Title { get; private set; }

        [NotNull]
        public DropDownModel Manager { get; private set; }

        [NotNull]
        public TextBoxModel Unit { get; private set; }

        [NotNull]
        public TextBoxModel UserId { get; private set; }

        [NotNull]
        public TextBoxModel PostalCode { get; private set; }

        public LabelModel CreatedDate { get; private set; }

        public LabelModel ChangedDate { get; private set; }

        public LabelModel SynchronizationDate { get; private set; }

        #endregion

    }
}