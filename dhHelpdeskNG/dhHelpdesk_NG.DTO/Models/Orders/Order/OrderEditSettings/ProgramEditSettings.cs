namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramEditSettings
    {
        public ProgramEditSettings(TextFieldEditSettings program, TextFieldEditSettings infoProduct)
        {
            Program = program;
            InfoProduct = infoProduct;
        }

        [NotNull]
        public TextFieldEditSettings Program { get; private set; }


        [NotNull]
        public TextFieldEditSettings InfoProduct { get; private set; }
    }
}