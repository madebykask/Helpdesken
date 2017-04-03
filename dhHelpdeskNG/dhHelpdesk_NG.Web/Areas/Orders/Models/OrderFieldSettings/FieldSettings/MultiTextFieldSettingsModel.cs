using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    public class MultiTextFieldSettingsModel : TextFieldSettingsModel
    {
        public MultiTextFieldSettingsModel()
        {
            
        }

        public MultiTextFieldSettingsModel(
        bool show,
        bool showInList,
        bool showExternal,
        string label,
        bool required,
        string emailIdentifier,
        string defaultValue,
        string help, 
        bool isMultiple)
                : base(show, showInList, showExternal, label, required, emailIdentifier, defaultValue, help)
        {
            IsMultiple = isMultiple;
        }

        public bool IsMultiple { get; set; }
    }
}