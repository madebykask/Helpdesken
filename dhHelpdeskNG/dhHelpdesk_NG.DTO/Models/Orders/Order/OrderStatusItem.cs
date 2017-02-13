using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    public class OrderStatusItem: ItemOverview
    {
        public OrderStatusItem(string name, string value, bool createCase, bool notifyOrderer, bool notifyReceiver) 
            : base(name, value)
        {
            CreateCase = createCase;
            NotifyOrderer = notifyOrderer;
            NotifyReceiver = notifyReceiver;
        }

        public bool CreateCase { get; private set; }
        public bool NotifyOrderer { get; private set; }
        public bool NotifyReceiver { get; private set; }
    }
}
