namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FullFieldSettings
    {
        private FullFieldSettings()
        {            
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? OrderTypeId { get; private set; }

        [NotNull]
        public DeliveryFieldSettings Delivery { get; private set; }

        [NotNull]
        public GeneralFieldSettings General { get; private set; }

        [NotNull]
        public LogFieldSettings Log { get; private set; }

        [NotNull]
        public OrdererFieldSettings Orderer { get; private set; }

        [NotNull]
        public OrderFieldSettings Order { get; private set; }

        [NotNull]
        public OtherFieldSettings Other { get; private set; }

        [NotNull]
        public ProgramFieldSettings Program { get; private set; }

        [NotNull]
        public ReceiverFieldSettings Receiver { get; private set; }

        [NotNull]
        public SupplierFieldSettings Supplier { get; private set; }

        [NotNull]
        public UserFieldSettings User { get; private set; }

        [NotNull]
        public AccountInfoFieldSettings AccountInfo { get; private set; }

        [NotNull]
        public ContactFieldSettings Contact { get; private set; }


        public DateTime ChangedDate { get; private set; }

        public static FullFieldSettings CreateForEdit(
                        DeliveryFieldSettings delivery,
                        GeneralFieldSettings general,
                        LogFieldSettings log,
                        OrdererFieldSettings orderer,
                        OrderFieldSettings order,
                        OtherFieldSettings other,
                        ProgramFieldSettings program,
                        ReceiverFieldSettings receiver,
                        SupplierFieldSettings supplier,
                        UserFieldSettings user,
                        AccountInfoFieldSettings accountInfo,
                        ContactFieldSettings contact)
        {
            return new FullFieldSettings
                       {
                           Delivery = delivery,
                           General = general,
                           Log = log,
                           Orderer = orderer,
                           Order = order,
                           Other = other,
                           Program = program,
                           Receiver = receiver,
                           Supplier = supplier,
                           User = user,
                           AccountInfo = accountInfo,
                           Contact = contact
                        };
        }

        public static FullFieldSettings CreateUpdated(
                        int customerId,
                        int? orderTypeId,
                        DeliveryFieldSettings delivery,
                        GeneralFieldSettings general,
                        LogFieldSettings log,
                        OrdererFieldSettings orderer,
                        OrderFieldSettings order,
                        OtherFieldSettings other,
                        ProgramFieldSettings program,
                        ReceiverFieldSettings receiver,
                        SupplierFieldSettings supplier,
                        UserFieldSettings user,
                        AccountInfoFieldSettings accountInfo,
                        ContactFieldSettings contact,
                        DateTime changedDate)
        {
            return new FullFieldSettings
                       {
                           CustomerId = customerId,
                           OrderTypeId = orderTypeId,
                           Delivery = delivery,
                           General = general,
                           Log = log,
                           Orderer = orderer,
                           Order = order,
                           Other = other,
                           Program = program,
                           Receiver = receiver,
                           Supplier = supplier,
                           User = user,
                           AccountInfo = accountInfo,
                           Contact = contact,
                           ChangedDate = changedDate
                       };
        }
    }
}