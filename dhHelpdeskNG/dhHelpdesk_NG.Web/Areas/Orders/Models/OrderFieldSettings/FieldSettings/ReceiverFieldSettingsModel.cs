using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ReceiverFieldSettingsModel
    {
        public ReceiverFieldSettingsModel()
        {            
        }

        public ReceiverFieldSettingsModel(
                TextFieldSettingsModel receiverId, 
                TextFieldSettingsModel receiverName, 
                TextFieldSettingsModel receiverEmail, 
                TextFieldSettingsModel receiverPhone, 
                TextFieldSettingsModel receiverLocation, 
                TextFieldSettingsModel markOfGoods)
        {
            this.MarkOfGoods = markOfGoods;
            this.ReceiverLocation = receiverLocation;
            this.ReceiverPhone = receiverPhone;
            this.ReceiverEmail = receiverEmail;
            this.ReceiverName = receiverName;
            this.ReceiverId = receiverId;
        }

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.ReceiverId)]
        public TextFieldSettingsModel ReceiverId { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.ReceiverName)]
        public TextFieldSettingsModel ReceiverName { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.ReceiverEmail)]
        public TextFieldSettingsModel ReceiverEmail { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.ReceiverPhone)]
        public TextFieldSettingsModel ReceiverPhone { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.ReceiverLocation)]
        public TextFieldSettingsModel ReceiverLocation { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.ReceiverMarkOfGoods)]
        public TextFieldSettingsModel MarkOfGoods { get; set; }     
    }
}