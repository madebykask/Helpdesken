using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    public class OrderFieldTypeSettings : FieldSettings
    {
        private  OrderFieldTypeSettings()
        {
        }

        public List<OrderFieldTypeValueSetting> Values { get; set; }


        public static OrderFieldTypeSettings CreateUpdated(
            bool show,
            bool showInList,
            bool showExternal,
            string label,
            bool required,
            string emailIdentifier,
            string help,
            List<OrderFieldTypeValueSetting> values)
        {
            return new OrderFieldTypeSettings
            {
                Show = show,
                ShowInList = showInList,
                ShowExternal = showExternal,
                Label = label,
                Required = required,
                EmailIdentifier = emailIdentifier,
                FieldHelp = !string.IsNullOrEmpty(help) ? help : string.Empty,
                Values = values ?? new List<OrderFieldTypeValueSetting>()
            };
        }

        public static OrderFieldTypeSettings CreateForEdit(
            string orderField,
            bool show,
            bool showInList,
            bool showExternal,
            string label,
            bool required,
            string emailIdentifier,
            string help,
            List<OrderFieldTypeValueSetting> values)
        {
            return new OrderFieldTypeSettings
            {
                OrderField = orderField,
                Show = show,
                ShowInList = showInList,
                ShowExternal = showExternal,
                Label = label,
                Required = required,
                EmailIdentifier = emailIdentifier,
                FieldHelp = help,
                Values = values
            };
        }
    }
}
