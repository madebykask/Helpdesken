namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogFieldSettings
    {
        public LogFieldSettings(TextFieldSettings log)
        {
            this.Log = log;
        }

        [NotNull]
        public TextFieldSettings Log { get; private set; }
    }
}