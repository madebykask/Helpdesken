namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogFieldSettings
    {
        public LogFieldSettings(FieldSettings log)
        {
            this.Log = log;
        }

        [NotNull]
        public FieldSettings Log { get; private set; }
    }
}