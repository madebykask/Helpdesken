namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReceiverFieldSettingsOverview
    {
        public ReceiverFieldSettingsOverview(
                FieldOverviewSetting receiverId, 
                FieldOverviewSetting receiverName, 
                FieldOverviewSetting receiverEmail, 
                FieldOverviewSetting receiverPhone, 
                FieldOverviewSetting receiverLocation, 
                FieldOverviewSetting markOfGoods)
        {
            this.MarkOfGoods = markOfGoods;
            this.ReceiverLocation = receiverLocation;
            this.ReceiverPhone = receiverPhone;
            this.ReceiverEmail = receiverEmail;
            this.ReceiverName = receiverName;
            this.ReceiverId = receiverId;
        }

        [NotNull]
        public FieldOverviewSetting ReceiverId { get; private set; }

        [NotNull]
        public FieldOverviewSetting ReceiverName { get; private set; }

        [NotNull]
        public FieldOverviewSetting ReceiverEmail { get; private set; }

        [NotNull]
        public FieldOverviewSetting ReceiverPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting ReceiverLocation { get; private set; }

        [NotNull]
        public FieldOverviewSetting MarkOfGoods { get; private set; }         
    }
}