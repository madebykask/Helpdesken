using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain.Orders;

namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    public class OrderFieldTypeValueSetting
    {
        public OrderFieldTypeValueSetting(int? id, string value, OrderFieldTypes type)
        {
            Id = id;
            Value = value;
            Type = type;
        }

        public int? Id { get; private set; }

        public string Value { get; private set; }

        public OrderFieldTypes Type { get; private set; }
    }
}
