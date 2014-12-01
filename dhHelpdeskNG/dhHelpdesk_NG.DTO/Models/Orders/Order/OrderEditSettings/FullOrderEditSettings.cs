namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
                UserEditSettings userEditSettings)
        {
            this.UserEditSettings = userEditSettings;
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
        public UserEditSettings UserEditSettings { get; private set; }
    }
}