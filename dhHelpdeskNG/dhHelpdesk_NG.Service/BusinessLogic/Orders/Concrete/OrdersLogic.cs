namespace DH.Helpdesk.Services.BusinessLogic.Orders.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;

    public sealed class OrdersLogic : IOrdersLogic
    {
        private readonly IHistoriesComparator historiesComparator;

        public OrdersLogic(IHistoriesComparator historiesComparator)
        {
            this.historiesComparator = historiesComparator;
        }

        public List<HistoriesDifference> AnalyzeHistoriesDifferences(
                    List<HistoryOverview> histories, 
                    List<LogOverview> logs, 
                    List<EmailLogOverview> emailLogs,
                    FullOrderEditSettings settings)
        {
            var historyDifferences = new List<HistoriesDifference>();

            HistoryOverview previousHistory = null;

            foreach (var history in histories)
            {
                var historyLogs = logs.Where(l => l.HistoryId == history.Id).ToList();
                var historyEmailLogs = emailLogs.Where(l => l.HistoryId == history.Id).ToList();

                var difference = this.historiesComparator.Compare(
                    previousHistory,
                    history,
                    historyLogs,
                    historyEmailLogs,
                    settings);

                if (difference != null)
                {
                    historyDifferences.Add(difference);
                }

                previousHistory = history;
            }

            return historyDifferences;
        }
    }
}