using System;
using DH.Helpdesk.BusinessData.Models.Orders.Order;

namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using FieldModels;
    using Web.Infrastructure.Tools;

    public sealed class FullOrderEditModel
    {
        public FullOrderEditModel()
        {
            NewFiles = new List<WebTemporaryFile>();
            DeletedFiles = new List<string>();
            DeletedLogIds = new List<int>();
        }

        public FullOrderEditModel(
                    DeliveryEditModel delivery,
                    GeneralEditModel general,
                    LogEditModel log,
                    OrderEditModel order,
                    OtherEditModel other,
                    ProgramEditModel program,
                    ReceiverEditModel receiver,
                    SupplierEditModel supplier,
                    UserEditModel user,
                    UserInfoEditModel userInfo,
                    string id,
                    int customerId,
                    int? orderTypeId, 
                    bool isNew,
                    HistoryModel history)
        {
            IsNew = isNew;
            Delivery = delivery;
            General = general;
            Log = log;
            Order = order;
            Other = other;
            Program = program;
            Receiver = receiver;
            Supplier = supplier;
            User = user;
            UserInfo = userInfo;
            Id = id;
            CustomerId = customerId;
            OrderTypeId = orderTypeId;
            History = history;

            NewFiles = new List<WebTemporaryFile>();
            DeletedFiles = new List<string>();
            DeletedLogIds = new List<int>();
        }

        public bool IsNew { get; private set; }

        public bool IsReturnToCase { get; set; }

        public string OrderTypeDescription { get; set; }

        [NotNull]
        public DeliveryEditModel Delivery { get; set; }

        [NotNull]
        public GeneralEditModel General { get; set; }

        [NotNull]
        public LogEditModel Log { get; set; }


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

        [NotNull]
        public UserInfoEditModel UserInfo { get; set; }

        [NotNullAndEmpty]
        public string Id { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int? OrderTypeId { get; set; }

        public bool InformOrderer { get; set; }

        public bool InformReceiver { get; set; }

        public bool CreateCase { get; set; }

        public bool UserHasAdminOrderPermission { get; set; }

        [NotNull]
        public List<WebTemporaryFile> NewFiles { get; set; }

        [NotNull]
        public List<string> DeletedFiles { get; set; }

        [NotNull]
        public List<int> DeletedLogIds { get; set; }

        public HistoryModel History { get; set; }
        public OrderStatusItem[] Statuses { get; set; }

        public Tuple<int?, string> OrderTypeDocument { get; set; }
    }
}