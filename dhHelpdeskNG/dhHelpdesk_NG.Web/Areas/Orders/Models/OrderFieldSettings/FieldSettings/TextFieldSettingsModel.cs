using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    public class TextFieldSettingsModel : FieldSettingsModel
    {
        public TextFieldSettingsModel()
        {            
        }

        public TextFieldSettingsModel(
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier,
                string defaultValue,
                string help)
                : base(show, showInList, showExternal, label, required, emailIdentifier, help)
        {
            DefaultValue = defaultValue;
        }

        [LocalizedStringLength(50)]
        public string DefaultValue { get; set; }
    }
}