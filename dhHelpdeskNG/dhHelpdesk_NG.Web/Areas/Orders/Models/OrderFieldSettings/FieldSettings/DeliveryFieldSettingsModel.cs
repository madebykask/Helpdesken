using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using Web.Infrastructure.LocalizedAttributes;

    public sealed class DeliveryFieldSettingsModel
    {
        public DeliveryFieldSettingsModel()
        {            
        }

        public DeliveryFieldSettingsModel(
                TextFieldSettingsModel deliveryDate, 
                TextFieldSettingsModel installDate, 
                TextFieldSettingsModel deliveryDepartment, 
                TextFieldSettingsModel deliveryOu, 
                TextFieldSettingsModel deliveryAddress, 
                TextFieldSettingsModel deliveryPostalCode, 
                TextFieldSettingsModel deliveryPostalAddress, 
                TextFieldSettingsModel deliveryLocation, 
                TextFieldSettingsModel deliveryInfo1, 
                TextFieldSettingsModel deliveryInfo2, 
                TextFieldSettingsModel deliveryInfo3, 
                TextFieldSettingsModel deliveryOuId,
                TextFieldSettingsModel name,
                TextFieldSettingsModel phone)
        {
            DeliveryOuId = deliveryOuId;
            DeliveryInfo3 = deliveryInfo3;
            DeliveryInfo2 = deliveryInfo2;
            DeliveryInfo1 = deliveryInfo1;
            DeliveryLocation = deliveryLocation;
            DeliveryPostalAddress = deliveryPostalAddress;
            DeliveryPostalCode = deliveryPostalCode;
            DeliveryAddress = deliveryAddress;
            DeliveryOu = deliveryOu;
            DeliveryDepartment = deliveryDepartment;
            InstallDate = installDate;
            DeliveryDate = deliveryDate;
            Name = name;
            Phone = phone;
        }

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryDate)]
        public TextFieldSettingsModel DeliveryDate { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryInstallDate)]
        public TextFieldSettingsModel InstallDate { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryDepartment)]
        public TextFieldSettingsModel DeliveryDepartment { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryOu)]
        public TextFieldSettingsModel DeliveryOu { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryAddress)]
        public TextFieldSettingsModel DeliveryAddress { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryPostalCode)]
        public TextFieldSettingsModel DeliveryPostalCode { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryPostalAddress)]
        public TextFieldSettingsModel DeliveryPostalAddress { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryLocation)]
        public TextFieldSettingsModel DeliveryLocation { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryInfo1)]
        public TextFieldSettingsModel DeliveryInfo1 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryInfo2)]
        public TextFieldSettingsModel DeliveryInfo2 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryInfo3)]
        public TextFieldSettingsModel DeliveryInfo3 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryOuId)]
        public TextFieldSettingsModel DeliveryOuId { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryName)]
        public TextFieldSettingsModel Name { get; set; }


        [NotNull]
        [LocalizedDisplay(OrderLabels.DeliveryPhone)]
        public TextFieldSettingsModel Phone { get; set; }
    }
}