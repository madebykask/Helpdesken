using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    using Common.ValidationAttributes;

    public sealed class FullOrderOverview
    {
        public FullOrderOverview(
                int id, 
                OrderType orderType,
                DeliveryOverview delivery, 
                GeneralOverview general, 
                LogOverview log, 
                OrdererOverview orderer, 
                OrderOverview order, 
                OtherOverview other, 
                ProgramOverview program, 
                ReceiverOverview receiver, 
                SupplierOverview supplier, 
                UserOverview user,
                AccountInfoOverview accountInfo)
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
            Id = id;
            OrderType = orderType.Name;
            AccountInfo = accountInfo;
        }

        [IsId]
        public int Id { get; private set; }

        public string OrderType { get; private set; }

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

        [NotNull]
        public AccountInfoOverview AccountInfo { get; private set; }


        
    }
}