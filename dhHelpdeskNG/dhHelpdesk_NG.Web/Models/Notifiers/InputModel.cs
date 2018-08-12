namespace DH.Helpdesk.Web.Models.Notifiers
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Notifiers.ConfigurableFields;
    
    public sealed class InputModel
    {
        #region Constructors and Destructors

        public InputModel()
        {
        }

        public InputModel(
            int id,
            StringFieldModel userId,
            DropDownFieldModel domain,
            StringFieldModel loginName,
            StringFieldModel firstName,
            StringFieldModel initials,
            StringFieldModel lastName,
            StringFieldModel displayName,
            StringFieldModel place,
            StringFieldModel phone,
            StringFieldModel cellPhone,
            StringFieldModel email,
            StringFieldModel code,
            StringFieldModel postalAddress,
            StringFieldModel postalCode,
            StringFieldModel city,
            StringFieldModel title,
            DropDownFieldModel region,
            DropDownFieldModel department,
            DropDownFieldModel organizationUnit,
            StringFieldModel costCentre,
            StringFieldModel unit,
            DropDownFieldModel division,
            DropDownFieldModel managers,
            DropDownFieldModel group,
            StringFieldModel other,
            BooleanFieldModel ordered,
            bool isActive,
            StringFieldModel createdDate,
            StringFieldModel changedDate,
            StringFieldModel synchronizationDate,
            DropDownFieldModel language,
            ComputerUserCategoryModel categoryModel)
            : this(
                userId,
                domain,
                loginName,
                firstName,
                initials,
                lastName,
                displayName,
                place,
                phone,
                cellPhone,
                email,
                code,
                postalAddress,
                postalCode,
                city,
                title,
                region,
                department,
                organizationUnit,
                costCentre,
                unit,
                division,
                managers,
                group,
                other,
                ordered,
                isActive,
                language,
                categoryModel)
        {
            this.Id = id;
            this.IsNew = false;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
            this.ComputerUserCategoryModel = categoryModel;

        }

        public InputModel(
            StringFieldModel userId,
            DropDownFieldModel domain,
            StringFieldModel loginName,
            StringFieldModel firstName,
            StringFieldModel initials,
            StringFieldModel lastName,
            StringFieldModel displayName,
            StringFieldModel place,
            StringFieldModel phone,
            StringFieldModel cellPhone,
            StringFieldModel email,
            StringFieldModel code,
            StringFieldModel postalAddress,
            StringFieldModel postalCode,
            StringFieldModel city,
            StringFieldModel title,
            DropDownFieldModel region,
            DropDownFieldModel department,
            DropDownFieldModel organizationUnit,
            StringFieldModel costCentre,
            StringFieldModel unit,
            DropDownFieldModel division,
            DropDownFieldModel managers,
            DropDownFieldModel group,
            StringFieldModel other,
            BooleanFieldModel ordered,
            bool isActive,
            DropDownFieldModel language,
            ComputerUserCategoryModel categoryModel)
        {
            this.IsNew = true;
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
            this.OrganizationUnit = organizationUnit;
            this.CostCentre = costCentre;
            this.Unit = unit;
            this.Division = division;
            this.Managers = managers;
            this.Group = group;
            this.Other = other;
            this.Ordered = ordered;
            this.IsActive = isActive;
            this.Language = language;
            this.ComputerUserCategoryModel = categoryModel;
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }

        public bool IsNew { get; set; }

        public StringFieldModel PostalAddress { get; set; }

        public StringFieldModel CellPhone { get; set; }

        public StringFieldModel City { get; set; }

        public StringFieldModel Code { get; set; }

        public DropDownFieldModel Region { get; set; }

        public DropDownFieldModel Department { get; set; }

        public DropDownFieldModel OrganizationUnit { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        public StringFieldModel DisplayName { get; set; }

        public DropDownFieldModel Division { get; set; }

        [IsId]
        public int? DivisionId { get; set; }

        public DropDownFieldModel Domain { get; set; }

        public int? DomainId { get; set; }

        public StringFieldModel Email { get; set; }

        public StringFieldModel FirstName { get; set; }

        public StringFieldModel CostCentre { get; set; }

        public DropDownFieldModel Group { get; set; }

        [IsId]
        public int? GroupId { get; set; }

        public StringFieldModel Initials { get; set; }

        [LocalizedDisplay("Active")]
        public bool IsActive { get; set; }

        public StringFieldModel LastName { get; set; }

        public StringFieldModel Place { get; set; }

        public StringFieldModel LoginName { get; set; }

        public BooleanFieldModel Ordered { get; set; }

        [IsId]
        public int? OrganizationUnitId { get; set; }

        public StringFieldModel Other { get; set; }

        public StringFieldModel Phone { get; set; }

        public StringFieldModel Title { get; set; }

        public DropDownFieldModel Managers { get; set; }

        [IsId]
        public int? ManagerId { get; set; }

        public StringFieldModel Unit { get; set; }

        [IsId]
        public int? UnitId { get; set; }

        public StringFieldModel UserId { get; set; }

        public StringFieldModel PostalCode { get; set; }

        public StringFieldModel CreatedDate { get; set; }

        public StringFieldModel ChangedDate { get; set; }

        public StringFieldModel SynchronizationDate { get; set; }

        public int LanguageId { get; set; }
        //public virtual Language Language { get; set; }

        public DropDownFieldModel Language { get; set; }

        public int? CategoryId { get; set; }

        public ComputerUserCategoryModel ComputerUserCategoryModel { get; set; }

        #endregion
    }
}