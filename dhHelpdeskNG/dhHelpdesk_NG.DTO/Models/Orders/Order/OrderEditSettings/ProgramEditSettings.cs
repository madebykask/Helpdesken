namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramEditSettings
    {
        public ProgramEditSettings(TextFieldEditSettings program)
        {
            this.Program = program;
        }

        [NotNull]
        public TextFieldEditSettings Program { get; private set; }         
    }
}