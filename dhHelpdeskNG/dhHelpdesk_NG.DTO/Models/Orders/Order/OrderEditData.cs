namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;
    
    public sealed class OrderEditData
    {
        public OrderEditData(
                FullOrderEditFields order, 
                List<File> files, 
                List<Log> logs, 
                List<HistoriesDifference> histories)
        {
            this.Histories = histories;
            this.Logs = logs;
            this.Files = files;
            this.Order = order;
        }

        [NotNull]
        public FullOrderEditFields Order { get; private set; }

        [NotNull]
        public List<File> Files { get; private set; }

        [NotNull]
        public List<Log> Logs { get; private set; }

        [NotNull]
        public List<HistoriesDifference> Histories { get; private set; }
    }
}