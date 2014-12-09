namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogEditFields
    {
        public LogEditFields()
        {
            this.Logs = new List<Log>();    
        }

        public LogEditFields(List<Log> logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public List<Log> Logs { get; private set; }
    }
}