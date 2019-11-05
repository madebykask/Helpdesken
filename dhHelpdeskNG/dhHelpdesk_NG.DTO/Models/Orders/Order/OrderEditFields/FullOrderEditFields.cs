namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using Common.ValidationAttributes;

    public sealed class FullOrderEditFields : Shared.Input.BusinessModel
    {
        public FullOrderEditFields(
                int id, 
                int? orderTypeId,
                DeliveryEditFields delivery, 
                GeneralEditFields general, 
                LogEditFields log, 
                OrdererEditFields orderer, 
                OrderEditFields order, 
                OtherEditFields other, 
                ProgramEditFields program, 
                ReceiverEditFields receiver, 
                SupplierEditFields supplier, 
                UserEditFields user,
                AccountInfoEditFields accountInfo)
        {
            OrderTypeId = orderTypeId;
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
            AccountInfo = accountInfo;
        }

        [IsId]
        public int? OrderTypeId { get; private set; }

        [NotNull]
        public DeliveryEditFields Delivery { get; private set; }

        [NotNull]
        public GeneralEditFields General { get; private set; }

        [NotNull]
        public LogEditFields Log { get; private set; }

        [NotNull]
        public OrdererEditFields Orderer { get; private set; }

        [NotNull]
        public OrderEditFields Order { get; private set; }

		[NotNull]
		public ProgramEditFields Program { get; private set; }

		[NotNull]
        public OtherEditFields Other { get; private set; }

        [NotNull]
        public ReceiverEditFields Receiver { get; private set; }

        [NotNull]
        public SupplierEditFields Supplier { get; private set; }

        [NotNull]
        public UserEditFields User { get; private set; }

        [NotNull]
        public AccountInfoEditFields AccountInfo { get; private set; }

    }
}