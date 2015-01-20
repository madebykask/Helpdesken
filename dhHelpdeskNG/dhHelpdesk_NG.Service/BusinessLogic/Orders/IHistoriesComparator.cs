namespace DH.Helpdesk.Services.BusinessLogic.Orders
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;

    public interface IHistoriesComparator
    {
        HistoriesDifference Compare(
           HistoryOverview previousHistory,
           HistoryOverview currentHistory,
           List<LogOverview> currentHistoryLog,
           List<EmailLogOverview> currentHistoryEmailLogs,
           FullOrderEditSettings settings);
    }
}