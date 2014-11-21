namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramFieldSettings
    {
        public ProgramFieldSettings(TextFieldSettings program)
        {
            this.Program = program;
        }

        [NotNull]
        public TextFieldSettings Program { get; private set; }         
    }
}