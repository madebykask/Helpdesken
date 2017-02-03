using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain.Orders;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    public class OrderFieldTypeSettingsModel : FieldSettingsModel
    {
        public OrderFieldTypeSettingsModel()
        {
        }

        public OrderFieldTypeSettingsModel(
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier,
                string help,
                List<OrderFieldTypeValueSettingsModel> values,
                OrderFieldTypes type)
                : base(show, showInList, showExternal, label, required, emailIdentifier, help)
        {
            Values = values ?? new List<OrderFieldTypeValueSettingsModel>();
            Type = type;
        }

        public List<OrderFieldTypeValueSettingsModel> Values { get; set; }
        public OrderFieldTypes Type { get; set; }

    }
}