namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogHistoryFields
    {
        public LogHistoryFields()
        {
            this.Logs = new List<Log>();    
        }

        public LogHistoryFields(List<Log> logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public List<Log> Logs { get; private set; }
    }
}