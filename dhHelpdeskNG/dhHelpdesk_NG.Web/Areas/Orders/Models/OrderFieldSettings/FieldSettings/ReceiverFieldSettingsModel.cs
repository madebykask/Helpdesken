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

        [LocalizedStringLength(30)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay("ID mottagare")]
        public TextFieldSettingsModel ReceiverId { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Namn mottagare")]
        public TextFieldSettingsModel ReceiverName { get; set; }
         
        [NotNull]
        [LocalizedDisplay("E-post mottagare")]
        public TextFieldSettingsModel ReceiverEmail { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Telefon mottagare")]
        public TextFieldSettingsModel ReceiverPhone { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Placering mottagare")]
        public TextFieldSettingsModel ReceiverLocation { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Godsmärke")]
        public TextFieldSettingsModel MarkOfGoods { get; set; }     
    }
}