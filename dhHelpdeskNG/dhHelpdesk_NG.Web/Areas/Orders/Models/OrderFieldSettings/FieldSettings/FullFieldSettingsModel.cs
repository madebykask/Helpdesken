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
            UserFieldSettingsModel user,
            AccountInfoFieldSettingsModel accountInfo)
        {
            OrderTypeId = orderTypeId;
            Delivery = delivery;
            General = general;
            Log = log;
            Orderer = orderer;
            Order = order;
            Other = other;
            Program = program;
            Receiver = receiver;
            Supplier = supplier;
            User = user;
            AccountInfo = accountInfo;
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

        [NotNull]
        public AccountInfoFieldSettingsModel AccountInfo { get; set; }

    }
}