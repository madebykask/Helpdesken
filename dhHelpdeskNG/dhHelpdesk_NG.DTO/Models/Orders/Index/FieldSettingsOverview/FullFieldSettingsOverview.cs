namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using Common.ValidationAttributes;

    public sealed class FullFieldSettingsOverview
    {
        public FullFieldSettingsOverview(
                DeliveryFieldSettingsOverview delivery, 
                GeneralFieldSettingsOverview general, 
                LogFieldSettingsOverview log, 
                OrdererFieldSettingsOverview orderer, 
                OrderFieldSettingsOverview order, 
                OtherFieldSettingsOverview other, 
                ProgramFieldSettingsOverview program, 
                ReceiverFieldSettingsOverview receiver, 
                SupplierFieldSettingsOverview supplier, 
                UserFieldSettingsOverview user,
                AccountInfoFieldSettingsOverview accountInfo)
        {
            User = user;
            Supplier = supplier;
            Receiver = receiver;
            Program = program;
            Other = other;
            Order = order;
            Orderer = orderer;
            Log = log;
            General = general;
            Delivery = delivery;
            AccountInfo = accountInfo;
        }

        [NotNull]
        public DeliveryFieldSettingsOverview Delivery { get; private set; }

        [NotNull]
        public GeneralFieldSettingsOverview General { get; private set; }

        [NotNull]
        public LogFieldSettingsOverview Log { get; private set; }

        [NotNull]
        public OrdererFieldSettingsOverview Orderer { get; private set; }

        [NotNull]
        public OrderFieldSettingsOverview Order { get; private set; }

        [NotNull]
        public OtherFieldSettingsOverview Other { get; private set; }

        [NotNull]
        public ProgramFieldSettingsOverview Program { get; private set; }

        [NotNull]
        public ReceiverFieldSettingsOverview Receiver { get; private set; }

        [NotNull]
        public SupplierFieldSettingsOverview Supplier { get; private set; }

        [NotNull]
        public UserFieldSettingsOverview User { get; private set; }

        [NotNull]
        public AccountInfoFieldSettingsOverview AccountInfo { get; private set; }
    }
}