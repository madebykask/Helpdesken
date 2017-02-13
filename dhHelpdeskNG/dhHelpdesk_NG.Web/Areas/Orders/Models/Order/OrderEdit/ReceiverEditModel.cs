namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class ReceiverEditModel
    {
        public ReceiverEditModel()
        {            
        }

        public ReceiverEditModel(
            ConfigurableFieldModel<string> receiverId,
            ConfigurableFieldModel<string> receiverName,
            ConfigurableFieldModel<string> receiverEmail,
            ConfigurableFieldModel<string> receiverPhone,
            ConfigurableFieldModel<string> receiverLocation,
            ConfigurableFieldModel<string> markOfGoods)
        {
            this.ReceiverId = receiverId;
            this.ReceiverName = receiverName;
            this.ReceiverEmail = receiverEmail;
            this.ReceiverPhone = receiverPhone;
            this.ReceiverLocation = receiverLocation;
            this.MarkOfGoods = markOfGoods;
        }

        public string Header { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> ReceiverId { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> ReceiverName { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> ReceiverEmail { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> ReceiverPhone { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> ReceiverLocation { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> MarkOfGoods { get; set; }

        public static ReceiverEditModel CreateEmpty()
        {
            return new ReceiverEditModel(
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return this.ReceiverId.Show ||
                this.ReceiverName.Show ||
                this.ReceiverEmail.Show ||
                this.ReceiverPhone.Show ||
                this.ReceiverLocation.Show ||
                this.MarkOfGoods.Show;
        }
    }
}