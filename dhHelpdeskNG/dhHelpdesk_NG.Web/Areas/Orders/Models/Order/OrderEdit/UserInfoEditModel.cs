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
            ConfigurableFieldModel<string> manager, ConfigurableFieldModel<string> referenceNumber,
            ConfigurableFieldModel<string> ordererId,
            ConfigurableFieldModel<string> ordererName,
            ConfigurableFieldModel<string> ordererLocation,
            ConfigurableFieldModel<string> ordererEmail,
            ConfigurableFieldModel<string> ordererPhone,
            ConfigurableFieldModel<string> ordererCode,
            ConfigurableFieldModel<string> ordererAddress,
            ConfigurableFieldModel<string> ordererInvoiceAddress,
            ConfigurableFieldModel<string> ordererReferenceNumber,
            ConfigurableFieldModel<string> accountingDimension1,
            ConfigurableFieldModel<string> accountingDimension2,
            ConfigurableFieldModel<string> accountingDimension3,
            ConfigurableFieldModel<string> accountingDimension4,
            ConfigurableFieldModel<string> accountingDimension5)
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
            Unit = unitId;
            DepartmentId2 = departmentId2;
            Info = info;
            Responsibility = responsibility;
            Activity = activity;
            Manager = manager;
            ReferenceNumber = referenceNumber;
            OrdererId = ordererId;
            OrdererName = ordererName;
            OrdererLocation = ordererLocation;
            OrdererEmail = ordererEmail;
            OrdererCode = ordererCode;
            OrdererAddress = ordererAddress;
            OrdererInvoiceAddress = ordererInvoiceAddress;
            OrdererReferenceNumber = ordererReferenceNumber;
            AccountingDimension1 = accountingDimension1;
            AccountingDimension2 = accountingDimension2;
            AccountingDimension3 = accountingDimension3;
            AccountingDimension4 = accountingDimension4;
            AccountingDimension5 = accountingDimension5;
            OrdererPhone = ordererPhone;
        }

        [NotNull]
        public ConfigurableFieldModel<string> PersonalIdentityNumber { get; set; }

        //[NotNull]
        public ConfigurableFieldModel<string> Initials { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Extension { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Title { get; set; }

        //[NotNull]
        public ConfigurableFieldModel<string> Location { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> RoomNumber { get; set; }

        //[NotNull]
        public ConfigurableFieldModel<string> PostalAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> EmploymentTypeId { get; set; }

        //[NotNull]
        public ConfigurableFieldModel<int?> DepartmentId1 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> DepartmentId2 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }

        //[NotNull]
        public ConfigurableFieldModel<string> Responsibility { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Activity { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Manager { get; set; }

        //[NotNull]
        public ConfigurableFieldModel<string> ReferenceNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererName { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererLocation { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererEmail { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererPhone { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererCode { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> Unit { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererInvoiceAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererReferenceNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension1 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension2 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension3 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension4 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension5 { get; set; }

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
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
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
                DepartmentId2.Show ||
                OrdererId.Show ||
                OrdererName.Show ||
                OrdererLocation.Show ||
                OrdererEmail.Show ||
                OrdererPhone.Show ||
                OrdererCode.Show ||
                Unit.Show ||
                OrdererAddress.Show ||
                OrdererInvoiceAddress.Show ||
                OrdererReferenceNumber.Show ||
                AccountingDimension1.Show ||
                AccountingDimension2.Show ||
                AccountingDimension3.Show ||
                AccountingDimension4.Show ||
                AccountingDimension5.Show;
        }

    }
}