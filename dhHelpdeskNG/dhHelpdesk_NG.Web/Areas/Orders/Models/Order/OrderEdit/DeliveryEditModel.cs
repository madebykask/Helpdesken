using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System;
    using System.Web.Mvc;

    using Common.ValidationAttributes;
    using FieldModels;

    public sealed class DeliveryEditModel
    {
        public DeliveryEditModel()
        {            
        }

        public DeliveryEditModel(
                ConfigurableFieldModel<DateTime?> deliveryDate,
                ConfigurableFieldModel<DateTime?> installDate,
                ConfigurableFieldModel<SelectList> deliveryDepartment,
                ConfigurableFieldModel<string> deliveryOu,
                ConfigurableFieldModel<string> deliveryAddress,
                ConfigurableFieldModel<string> deliveryPostalCode,
                ConfigurableFieldModel<string> deliveryPostalAddress,
                ConfigurableFieldModel<string> deliveryLocation,
                ConfigurableFieldModel<string> deliveryInfo1,
                ConfigurableFieldModel<string> deliveryInfo2,
                ConfigurableFieldModel<string> deliveryInfo3,
                ConfigurableFieldModel<SelectList> deliveryOuId,
                ConfigurableFieldModel<string> name,
                ConfigurableFieldModel<string> phone)
        {
            DeliveryDate = deliveryDate;
            InstallDate = installDate;
            DeliveryDepartment = deliveryDepartment;
            DeliveryOu = deliveryOu;
            DeliveryAddress = deliveryAddress;
            DeliveryPostalCode = deliveryPostalCode;
            DeliveryPostalAddress = deliveryPostalAddress;
            DeliveryLocation = deliveryLocation;
            DeliveryInfo1 = deliveryInfo1;
            DeliveryInfo2 = deliveryInfo2;
            DeliveryInfo3 = deliveryInfo3;
            DeliveryOuId = deliveryOuId;
            DeliveryName = name;
            DeliveryPhone = phone;
        }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> DeliveryDate { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<DateTime?> InstallDate { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<SelectList> DeliveryDepartment { get; set; } 

        [IsId]
        public int? DeliveryDepartmentId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryOu { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryAddress { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryPostalCode { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryPostalAddress { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryLocation { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryInfo1 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryInfo2 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryInfo3 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<SelectList> DeliveryOuId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryName { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> DeliveryPhone { get; set; }

        [IsId]
        public int? DeliveryOuIdId { get; set; }

        public static DeliveryEditModel CreateEmpty()
        {
            return new DeliveryEditModel(
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<SelectList>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<SelectList>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return DeliveryDate.Show ||
                InstallDate.Show ||
                DeliveryDepartment.Show ||
                DeliveryOu.Show ||
                DeliveryAddress.Show ||
                DeliveryPostalCode.Show ||
                DeliveryPostalAddress.Show ||
                DeliveryLocation.Show ||
                DeliveryInfo1.Show ||
                DeliveryInfo2.Show ||
                DeliveryInfo3.Show ||
                DeliveryOuId.Show ||
                DeliveryName.Show ||
                DeliveryPhone.Show;
        }
    }
}