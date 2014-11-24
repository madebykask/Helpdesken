namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FullFieldSettingsModel
    {
        public FullFieldSettingsModel()
        {            
        }

        public FullFieldSettingsModel(
            int? orderTypeId,
            DeliveryFieldSettingsModel delivery,
            GeneralFieldSettingsModel general,
            LogFieldSettingsModel log,
            OrdererFieldSettingsModel orderer,
            OrderFieldSettingsModel order,
            OtherFieldSettingsModel other,
            ProgramFieldSettingsModel program,
            ReceiverFieldSettingsModel receiver,
            SupplierFieldSettingsModel supplier,
            UserFieldSettingsModel user)
        {
            this.OrderTypeId = orderTypeId;
            this.Delivery = delivery;
            this.General = general;
            this.Log = log;
            this.Orderer = orderer;
            this.Order = order;
            this.Other = other;
            this.Program = program;
            this.Receiver = receiver;
            this.Supplier = supplier;
            this.User = user;
        }

        [IsId]
        public int? OrderTypeId { get; set; }

        [NotNull]
        public DeliveryFieldSettingsModel Delivery { get; set; }

        [NotNull]
        public GeneralFieldSettingsModel General { get; set; }

        [NotNull]
        public LogFieldSettingsModel Log { get; set; }

        [NotNull]
        public OrdererFieldSettingsModel Orderer { get; set; }

        [NotNull]
        public OrderFieldSettingsModel Order { get; set; }

        [NotNull]
        public OtherFieldSettingsModel Other { get; set; }

        [NotNull]
        public ProgramFieldSettingsModel Program { get; set; }

        [NotNull]
        public ReceiverFieldSettingsModel Receiver { get; set; }

        [NotNull]
        public SupplierFieldSettingsModel Supplier { get; set; }

        [NotNull]
        public UserFieldSettingsModel User { get; set; }
    }
}