namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FullOrderOverview
    {
        public FullOrderOverview(
                int id, 
                DeliveryOverview delivery, 
                GeneralOverview general, 
                LogOverview log, 
                OrdererOverview orderer, 
                OrderOverview order, 
                OtherOverview other, 
                ProgramOverview program, 
                ReceiverOverview receiver, 
                SupplierOverview supplier, 
                UserOverview user)
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
            this.Id = id;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public DeliveryOverview Delivery { get; private set; }

        [NotNull]
        public GeneralOverview General { get; private set; }

        [NotNull]
        public LogOverview Log { get; private set; }

        [NotNull]
        public OrdererOverview Orderer { get; private set; }

        [NotNull]
        public OrderOverview Order { get; private set; }

        [NotNull]
        public OtherOverview Other { get; private set; }

        [NotNull]
        public ProgramOverview Program { get; private set; }

        [NotNull]
        public ReceiverOverview Receiver { get; private set; }

        [NotNull]
        public SupplierOverview Supplier { get; private set; }

        [NotNull]
        public UserOverview User { get; private set; }
    }
}