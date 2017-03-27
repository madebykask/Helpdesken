using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    public class MultiTextFieldEditSettings : TextFieldEditSettings
    {
        public MultiTextFieldEditSettings(bool show, string caption,
           bool required, string emailIdentifier, string defaultValue, string help, bool isMultiple)
            : base(show, caption, required, emailIdentifier, defaultValue, help)
        {
            IsMultiple = isMultiple;
        }

        public bool IsMultiple { get; private set; }

    }
}
