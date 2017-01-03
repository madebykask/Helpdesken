namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using Common.ValidationAttributes;

    public sealed class FullOrderEditSettings
    {
        public FullOrderEditSettings(
                DeliveryEditSettings delivery, 
                GeneralEditSettings general, 
                LogEditSettings log, 
                OrdererEditSettings orderer, 
                OrderEditSettings order, 
                OtherEditSettings other, 
                ProgramEditSettings program, 
                ReceiverEditSettings receiver, 
                SupplierEditSettings supplier, 
                UserEditSettings user,
                AccountInfoEditSettings accountInfo,
                ContactEditSettings contact)
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
        public DeliveryEditSettings Delivery { get; private set; }

        [NotNull]
        public GeneralEditSettings General { get; private set; }

        [NotNull]
        public LogEditSettings Log { get; private set; }

        [NotNull]
        public OrdererEditSettings Orderer { get; private set; }

        [NotNull]
        public OrderEditSettings Order { get; private set; }

        [NotNull]
        public OtherEditSettings Other { get; private set; }

        [NotNull]
        public ProgramEditSettings Program { get; private set; }

        [NotNull]
        public ReceiverEditSettings Receiver { get; private set; }

        [NotNull]
        public SupplierEditSettings Supplier { get; private set; }

        [NotNull]
        public UserEditSettings User { get; private set; }

        [NotNull]
        public AccountInfoEditSettings AccountInfo { get; private set; }

        [NotNull]
        public ContactEditSettings Contact { get; private set; }
    }
}