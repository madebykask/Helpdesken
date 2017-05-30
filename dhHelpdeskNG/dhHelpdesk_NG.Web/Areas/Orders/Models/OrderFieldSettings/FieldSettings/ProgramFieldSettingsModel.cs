using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ProgramFieldSettingsModel
    {
        public ProgramFieldSettingsModel()
        {            
        }

        public ProgramFieldSettingsModel(TextFieldSettingsModel program, TextFieldSettingsModel infoProduct)
        {
            Program = program;
            InfoProduct = infoProduct;
        }

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.Program)]
        public TextFieldSettingsModel Program { get; set; }

        [LocalizedDisplay(OrderLabels.ProgramInfoProduct)]
        public TextFieldSettingsModel InfoProduct { get; set; }
    }
}