namespace DH.Helpdesk.Services.BusinessLogic.Orders
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;

    public interface IOrdersLogic
    {
        List<HistoriesDifference> AnalyzeHistoriesDifferences(List<HistoryOverview> histories, List<LogOverview> logs, List<EmailLogOverview> emailLogs);
    }
}