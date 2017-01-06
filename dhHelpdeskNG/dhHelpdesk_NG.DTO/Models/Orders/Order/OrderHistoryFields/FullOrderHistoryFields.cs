namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using Common.ValidationAttributes;

    public sealed class FullOrderHistoryFields
    {
        public FullOrderHistoryFields(
                DeliveryHistoryFields delivery, 
                GeneralHistoryFields general, 
                LogHistoryFields log, 
                OrdererHistoryFields orderer, 
                OrderHistoryFields order, 
                OtherHistoryFields other, 
                ProgramHistoryFields program, 
                ReceiverHistoryFields receiver, 
                SupplierHistoryFields supplier, 
                UserHistoryFields user,
                AccountInfoHistoryFields accountInfo,
                ContactHistoryFields contact)
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
            Contact = contact;
        }

        [NotNull]
        public DeliveryHistoryFields Delivery { get; private set; }

        [NotNull]
        public GeneralHistoryFields General { get; private set; }

        [NotNull]
        public LogHistoryFields Log { get; private set; }

        [NotNull]
        public OrdererHistoryFields Orderer { get; private set; }

        [NotNull]
        public OrderHistoryFields Order { get; private set; }

        [NotNull]
        public OtherHistoryFields Other { get; private set; }

        [NotNull]
        public ProgramHistoryFields Program { get; private set; }

        [NotNull]
        public ReceiverHistoryFields Receiver { get; private set; }

        [NotNull]
        public SupplierHistoryFields Supplier { get; private set; }

        [NotNull]
        public UserHistoryFields User { get; private set; }

        [NotNull]
        public AccountInfoHistoryFields AccountInfo { get; private set; }

        [NotNull]
        public ContactHistoryFields Contact { get; private set; }
    }
}