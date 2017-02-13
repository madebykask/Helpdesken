using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain.Orders;

namespace DH.Helpdesk.Domain
{
    public class OrderFieldType : Entity
    {
        public int? OrderType_Id { get; set; }

        public OrderFieldTypes OrderField { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual OrderType OrderType { get; set; }
    }
}
