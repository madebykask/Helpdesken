namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public sealed class User
    {
        public User()
        {
        }

        public User(
            ConfigurableFieldModel<List<string>> ids,
            ConfigurableFieldModel<string> firstName,
            ConfigurableFieldModel<string> initials,
            ConfigurableFieldModel<string> lastName,
            ConfigurableFieldModel<List<string>> personalIdentityNumber,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> extension,
            ConfigurableFieldModel<string> eMail,
            ConfigurableFieldModel<string> title,
            ConfigurableFieldModel<string> location,
            ConfigurableFieldModel<string> roomNumber,
            ConfigurableFieldModel<string> postalAddress,
            ConfigurableFieldModel<int?> employmentType,
            ConfigurableFieldModel<int?> departmentId,
            ConfigurableFieldModel<int?> unitId,
            ConfigurableFieldModel<int?> departmentId2,
            ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<string> responsibility,
            ConfigurableFieldModel<string> activity,
            ConfigurableFieldModel<string> manager,
            ConfigurableFieldModel<string> referenceNumber,
            SelectList employmentTypes,
            SelectList departments,
            SelectList units,
            SelectList departments2,
            int? regionId,
            SelectList regions)
        {
            this.Ids = ids;
            this.FirstName = firstName;
            this.Initials = initials;
            this.LastName = lastName;
            this.PersonalIdentityNumber = personalIdentityNumber;
            this.Phone = phone;
            this.Extension = extension;
            this.EMail = eMail;
            this.Title = title;
            this.Location = location;
            this.RoomNumber = roomNumber;
            this.PostalAddress = postalAddress;
            this.EmploymentType = employmentType;
            this.DepartmentId = departmentId;
            this.UnitId = unitId;
            this.DepartmentId2 = departmentId2;
            this.Info = info;
            this.Responsibility = responsibility;
            this.Activity = activity;
            this.Manager = manager;
            this.ReferenceNumber = referenceNumber;
            this.EmploymentTypes = employmentTypes;
            this.Departments = departments;
            this.Units = units;
            this.Departments2 = departments2;
            this.RegionId = regionId;
            this.Regions = regions;
        }

        public ConfigurableFieldModel<List<string>> Ids { get; set; }

        public ConfigurableFieldModel<string> FirstName { get; set; }

        public ConfigurableFieldModel<string> Initials { get; set; }

        public ConfigurableFieldModel<string> LastName { get; set; }

        public ConfigurableFieldModel<List<string>> PersonalIdentityNumber { get; set; }

        public ConfigurableFieldModel<string> Phone { get; set; }

        public ConfigurableFieldModel<string> Extension { get; set; }

        public ConfigurableFieldModel<string> EMail { get; set; }

        public ConfigurableFieldModel<string> Title { get; set; }

        public ConfigurableFieldModel<string> Location { get; set; }

        public ConfigurableFieldModel<string> RoomNumber { get; set; }

        public ConfigurableFieldModel<string> PostalAddress { get; set; }

        public ConfigurableFieldModel<int?> EmploymentType { get; set; }

        public ConfigurableFieldModel<int?> DepartmentId { get; set; }

        public ConfigurableFieldModel<int?> UnitId { get; set; }

        public ConfigurableFieldModel<int?> DepartmentId2 { get; set; }

        public ConfigurableFieldModel<string> Info { get; set; }

        public ConfigurableFieldModel<string> Responsibility { get; set; }

        public ConfigurableFieldModel<string> Activity { get; set; }

        public ConfigurableFieldModel<string> Manager { get; set; }

        public ConfigurableFieldModel<string> ReferenceNumber { get; set; }

        [NotNull]
        public SelectList EmploymentTypes { get; set; }

        [NotNull]
        public SelectList Departments { get; set; }

        [NotNull]
        public SelectList Departments2 { get; set; }

        [NotNull]
        public SelectList Units { get; set; }

        public int? RegionId { get; set; }

        [NotNull]
        public SelectList Regions { get; set; }
    }
}