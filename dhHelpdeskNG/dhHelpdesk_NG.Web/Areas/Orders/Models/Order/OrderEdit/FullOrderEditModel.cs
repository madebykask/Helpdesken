namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class FullOrderEditModel
    {
        public FullOrderEditModel()
        {
            this.NewFiles = new List<WebTemporaryFile>();
            this.DeletedFiles = new List<string>();
            this.DeletedLogIds = new List<int>();
        }

        public FullOrderEditModel(
                    DeliveryEditModel delivery,
                    GeneralEditModel general,
                    LogEditModel log,
                    OrdererEditModel orderer,
                    OrderEditModel order,
                    OtherEditModel other,
                    ProgramEditModel program,
                    ReceiverEditModel receiver,
                    SupplierEditModel supplier,
                    UserEditModel user,
                    string id,
                    int customerId,
                    int? orderTypeId, 
                    bool isNew,
                    HistoryModel history)
        {
            this.IsNew = isNew;
            this.Delivery = delivery;
            this.General = general;
            this.Log = log;
            this.Orderer = orderer;
            this.Order = order;
            this.Other = other;
            this.Program = program;
            this.Receiver = receiver;
            this.Supplier = supplier;
            this.User = user;
            this.Id = id;
            this.CustomerId = customerId;
            this.OrderTypeId = orderTypeId;
            this.History = history;

            this.NewFiles = new List<WebTemporaryFile>();
            this.DeletedFiles = new List<string>();
            this.DeletedLogIds = new List<int>();
        }

        public bool IsNew { get; private set; }

        [NotNull]
        public DeliveryEditModel Delivery { get; set; }

        [NotNull]
        public GeneralEditModel General { get; set; }

        [NotNull]
        public LogEditModel Log { get; set; }

        [NotNull]
        public OrdererEditModel Orderer { get; set; }

        [NotNull]
        public OrderEditModel Order { get; set; }

        [NotNull]
        public OtherEditModel Other { get; set; }

        [NotNull]
        public ProgramEditModel Program { get; set; }

        [NotNull]
        public ReceiverEditModel Receiver { get; set; }

        [NotNull]
        public SupplierEditModel Supplier { get; set; }

        [NotNull]
        public UserEditModel User { get; set; }

        [NotNullAndEmpty]
        public string Id { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int? OrderTypeId { get; set; }

        public bool InformOrderer { get; set; }

        public bool InformReceiver { get; set; }

        public bool CreateCase { get; set; }

        [NotNull]
        public List<WebTemporaryFile> NewFiles { get; set; }

        [NotNull]
        public List<string> DeletedFiles { get; set; }

        [NotNull]
        public List<int> DeletedLogIds { get; set; }

        public HistoryModel History { get; set; }
    }
}