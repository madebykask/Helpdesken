using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    public class OrderFieldTypeValueSettingsModel
    {
        public OrderFieldTypeValueSettingsModel()
        {
        }

        public int? Id { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Value { get; set; }

        public OrderFieldTypes Type { get; set; }
    }
}