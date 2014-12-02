namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

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
                ConfigurableFieldModel<SelectList> deliveryOuId)
        {
            this.DeliveryDate = deliveryDate;
            this.InstallDate = installDate;
            this.DeliveryDepartment = deliveryDepartment;
            this.DeliveryOu = deliveryOu;
            this.DeliveryAddress = deliveryAddress;
            this.DeliveryPostalCode = deliveryPostalCode;
            this.DeliveryPostalAddress = deliveryPostalAddress;
            this.DeliveryLocation = deliveryLocation;
            this.DeliveryInfo1 = deliveryInfo1;
            this.DeliveryInfo2 = deliveryInfo2;
            this.DeliveryInfo3 = deliveryInfo3;
            this.DeliveryOuId = deliveryOuId;
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

        [IsId]
        public int? DeliveryOuIdId { get; set; }
    }
}