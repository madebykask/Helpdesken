namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ProgramFieldSettingsModel
    {
        public ProgramFieldSettingsModel()
        {            
        }

        public ProgramFieldSettingsModel(TextFieldSettingsModel program)
        {
            this.Program = program;
        }

        [NotNull]
        [LocalizedDisplay("Program")]
        public TextFieldSettingsModel Program { get; set; } 
    }
}