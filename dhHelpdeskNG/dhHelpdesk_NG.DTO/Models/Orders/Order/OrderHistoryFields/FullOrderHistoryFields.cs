namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
                UserHistoryFields user)
        {
            this.User = user;
            this.Supplier = supplier;
            this.Receiver = receiver;
            this.Program = program;
            this.Other = other;
            this.Order = order;
            this.Orderer = orderer;
            this.Log = log;
            this.General = general;
            this.Delivery = delivery;
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
    }
}