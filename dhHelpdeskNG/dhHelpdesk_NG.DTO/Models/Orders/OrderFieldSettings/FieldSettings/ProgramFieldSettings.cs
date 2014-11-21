namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramFieldSettings
    {
        public ProgramFieldSettings(FieldSettings program)
        {
            this.Program = program;
        }

        [NotNull]
        public FieldSettings Program { get; private set; }         
    }
}