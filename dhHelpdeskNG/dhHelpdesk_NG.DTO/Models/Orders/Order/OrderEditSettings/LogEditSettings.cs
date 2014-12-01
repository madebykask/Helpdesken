namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogEditSettings
    {
        public LogEditSettings(TextFieldEditSettings log)
        {
            this.Log = log;
        }

        [NotNull]
        public TextFieldEditSettings Log { get; private set; }
    }
}