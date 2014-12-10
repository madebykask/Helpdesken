namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
                UserEditFields user)
        {
            this.OrderTypeId = orderTypeId;
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
        public OtherEditFields Other { get; private set; }

        [NotNull]
        public ProgramEditFields Program { get; private set; }

        [NotNull]
        public ReceiverEditFields Receiver { get; private set; }

        [NotNull]
        public SupplierEditFields Supplier { get; private set; }

        [NotNull]
        public UserEditFields User { get; private set; }
    }
}