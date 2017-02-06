namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramFieldSettings : HeaderSettings
    {
        public ProgramFieldSettings(TextFieldSettings program, TextFieldSettings infoProduct)
        {
            Program = program;
            InfoProduct = infoProduct;
        }

        [NotNull]
        public TextFieldSettings Program { get; private set; }

        [NotNull]
        public TextFieldSettings InfoProduct { get; private set; }

    }
}