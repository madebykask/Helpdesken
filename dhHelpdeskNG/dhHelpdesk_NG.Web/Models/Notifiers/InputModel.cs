namespace DH.Helpdesk.Web.Models.Notifiers
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class InputModel
    {
        #region Constructors and Destructors

        public InputModel()
        {
        }

        public InputModel(
            int id,
            ConfigurableFieldModel<string> userId,
            ConfigurableFieldModel<DropDownContent> domain,
            ConfigurableFieldModel<string> loginName,
            ConfigurableFieldModel<string> firstName,
            ConfigurableFieldModel<string> initials,
            ConfigurableFieldModel<string> lastName,
            ConfigurableFieldModel<string> displayName,
            ConfigurableFieldModel<string> place,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> cellPhone,
            ConfigurableFieldModel<string> email,
            ConfigurableFieldModel<string> code,
            ConfigurableFieldModel<string> postalAddress,
            ConfigurableFieldModel<string> postalCode,
            ConfigurableFieldModel<string> city,
            ConfigurableFieldModel<string> title,
            ConfigurableFieldModel<DropDownContent> region,
            ConfigurableFieldModel<DropDownContent> department,
            ConfigurableFieldModel<string> unit,
            ConfigurableFieldModel<DropDownContent> organizationUnit,
            ConfigurableFieldModel<DropDownContent> division,
            ConfigurableFieldModel<DropDownContent> manager,
            ConfigurableFieldModel<DropDownContent> group,
            ConfigurableFieldModel<string> other,
            ConfigurableFieldModel<bool> ordered,
            bool isActive,
            ConfigurableFieldModel<string> createdDate,
            ConfigurableFieldModel<string> changedDate,
            ConfigurableFieldModel<string> synchronizationDate)
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
                unit,
                organizationUnit,
                division,
                manager,
                group,
                other,
                ordered,
                isActive)
        {
            this.Id = id;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
        }

        public InputModel(
            ConfigurableFieldModel<string> userId,
            ConfigurableFieldModel<DropDownContent> domain,
            ConfigurableFieldModel<string> loginName,
            ConfigurableFieldModel<string> firstName,
            ConfigurableFieldModel<string> initials,
            ConfigurableFieldModel<string> lastName,
            ConfigurableFieldModel<string> displayName,
            ConfigurableFieldModel<string> place,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> cellPhone,
            ConfigurableFieldModel<string> email,
            ConfigurableFieldModel<string> code,
            ConfigurableFieldModel<string> postalAddress,
            ConfigurableFieldModel<string> postalCode,
            ConfigurableFieldModel<string> city,
            ConfigurableFieldModel<string> title,
            ConfigurableFieldModel<DropDownContent> region,
            ConfigurableFieldModel<DropDownContent> department,
            ConfigurableFieldModel<string> unit,
            ConfigurableFieldModel<DropDownContent> organizationUnit,
            ConfigurableFieldModel<DropDownContent> division,
            ConfigurableFieldModel<DropDownContent> manager,
            ConfigurableFieldModel<DropDownContent> group,
            ConfigurableFieldModel<string> other,
            ConfigurableFieldModel<bool> ordered,
            bool isActive)
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
            this.Unit = unit;
            this.OrganizationUnit = organizationUnit;
            this.Division = division;
            this.Manager = manager;
            this.Group = group;
            this.Other = other;
            this.Ordered = ordered;
            this.IsActive = isActive;
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }

        public bool IsNew { get; set; }

        public ConfigurableFieldModel<string> PostalAddress { get; set; }

        public ConfigurableFieldModel<string> CellPhone { get; set; }

        public ConfigurableFieldModel<string> City { get; set; }

        public ConfigurableFieldModel<string> Code { get; set; }

        public ConfigurableFieldModel<DropDownContent> Region { get; set; }

        public ConfigurableFieldModel<DropDownContent> Department { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        public ConfigurableFieldModel<string> DisplayName { get; set; }

        public ConfigurableFieldModel<DropDownContent> Division { get; set; }

        [IsId]
        public int? DivisionId { get; set; }

        public ConfigurableFieldModel<DropDownContent> Domain { get; set; }

        public int? DomainId { get; set; }

        public ConfigurableFieldModel<string> Email { get; set; }

        public ConfigurableFieldModel<string> FirstName { get; set; }

        public ConfigurableFieldModel<DropDownContent> Group { get; set; }

        [IsId]
        public int? GroupId { get; set; }

        public ConfigurableFieldModel<string> Initials { get; set; }

        public bool IsActive { get; set; }

        public ConfigurableFieldModel<string> LastName { get; set; }

        public ConfigurableFieldModel<string> Place { get; set; }

        public ConfigurableFieldModel<string> LoginName { get; set; }

        public ConfigurableFieldModel<bool> Ordered { get; set; }

        public ConfigurableFieldModel<DropDownContent> OrganizationUnit { get; set; }

        [IsId]
        public int? OrganizationUnitId { get; set; }

        public ConfigurableFieldModel<string> Other { get; set; }

        public ConfigurableFieldModel<string> Phone { get; set; }

        public ConfigurableFieldModel<string> Title { get; set; }

        public ConfigurableFieldModel<DropDownContent> Manager { get; set; }

        [IsId]
        public int? ManagerId { get; set; }

        public ConfigurableFieldModel<string> Unit { get; set; }

        [IsId]
        public int? UnitId { get; set; }

        public ConfigurableFieldModel<string> UserId { get; set; }

        public ConfigurableFieldModel<string> PostalCode { get; set; }

        public ConfigurableFieldModel<string> CreatedDate { get; set; }

        public ConfigurableFieldModel<string> ChangedDate { get; set; }

        public ConfigurableFieldModel<string> SynchronizationDate { get; set; }

        #endregion

    }
}