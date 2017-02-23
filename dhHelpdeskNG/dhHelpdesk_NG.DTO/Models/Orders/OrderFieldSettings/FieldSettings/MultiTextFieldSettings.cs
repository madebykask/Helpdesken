using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    public class MultiTextFieldSettings : TextFieldSettings
    {

        private MultiTextFieldSettings()
        {
        }

        public static MultiTextFieldSettings CreateUpdated(
                 bool show,
                 bool showInList,
                 bool showExternal,
                 string label,
                 bool required,
                 string emailIdentifier,
                 string defaultValue,
                 string help,
                 bool multiValue)
        {
            return new MultiTextFieldSettings
            {
                Show = show,
                ShowInList = showInList,
                ShowExternal = showExternal,
                Label = label,
                Required = required,
                EmailIdentifier = emailIdentifier,
                DefaultValue = !string.IsNullOrEmpty(defaultValue) ? defaultValue : string.Empty,
                FieldHelp = !string.IsNullOrEmpty(help) ? help : string.Empty,
                MultiValue = multiValue
            };
        }

        public static MultiTextFieldSettings CreateForEdit(
                string orderField,
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier,
                string defaultValue,
                string help,
                bool multiValue)
        {
            return new MultiTextFieldSettings
            {
                OrderField = orderField,
                Show = show,
                ShowInList = showInList,
                ShowExternal = showExternal,
                Label = label,
                Required = required,
                EmailIdentifier = emailIdentifier,
                DefaultValue = defaultValue,
                FieldHelp = help,
                MultiValue = multiValue
            };
        }

        [NotNull]
        public bool MultiValue { get; private set; }
    }
}
