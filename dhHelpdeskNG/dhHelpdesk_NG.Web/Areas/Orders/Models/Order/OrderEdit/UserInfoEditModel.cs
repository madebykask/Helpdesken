using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    public class UserInfoEditModel
    {
        public UserInfoEditModel()
        {
        }

        public UserInfoEditModel(ConfigurableFieldModel<string> personalIdentityNumber, ConfigurableFieldModel<string> initials, ConfigurableFieldModel<string> extension, ConfigurableFieldModel<string> title,
            ConfigurableFieldModel<string> location, ConfigurableFieldModel<string> roomNumber,
            ConfigurableFieldModel<string> postalAddress, ConfigurableFieldModel<int?> employmentTypeId,
            ConfigurableFieldModel<int?> departmentId1, ConfigurableFieldModel<int?> unitId,
            ConfigurableFieldModel<int?> departmentId2, ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<string> responsibility, ConfigurableFieldModel<string> activity,
            ConfigurableFieldModel<string> manager, ConfigurableFieldModel<string> referenceNumber)
        {
            PersonalIdentityNumber = personalIdentityNumber;
            Initials = initials;
            Extension = extension;
            Title = title;
            Location = location;
            RoomNumber = roomNumber;
            PostalAddress = postalAddress;
            EmploymentTypeId = employmentTypeId;
            DepartmentId1 = departmentId1;
            UnitId = unitId;
            DepartmentId2 = departmentId2;
            Info = info;
            Responsibility = responsibility;
            Activity = activity;
            Manager = manager;
            ReferenceNumber = referenceNumber;
        }

        [NotNull]
        public ConfigurableFieldModel<string> PersonalIdentityNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Initials { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Extension { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Title { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Location { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> RoomNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> PostalAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> EmploymentTypeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> DepartmentId1 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> UnitId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> DepartmentId2 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Responsibility { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Activity { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Manager { get; set; }

        [NotNull]
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


        public static UserInfoEditModel CreateEmpty()
        {
            return new UserInfoEditModel(
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return PersonalIdentityNumber.Show ||
                Initials.Show ||
                Extension.Show ||
                Title.Show ||
                Location.Show ||
                RoomNumber.Show ||
                PostalAddress.Show ||
                Info.Show ||
                Responsibility.Show ||
                Activity.Show ||
                Manager.Show ||
                ReferenceNumber.Show ||
                EmploymentTypeId.Show ||
                DepartmentId1.Show ||
                UnitId.Show ||
                DepartmentId2.Show;
        }

    }
}