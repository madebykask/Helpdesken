namespace DH.Helpdesk.Services.BusinessLogic.Orders.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;

    public sealed class HistoriesComparator : IHistoriesComparator
    {
        public HistoriesDifference Compare(
            HistoryOverview previousHistory,
            HistoryOverview currentHistory,
            List<LogOverview> currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs)
        {
            return previousHistory == null
                ? CreateDifferenceForFirstHistory(currentHistory, currentHistoryLog, currentHistoryEmailLogs)
                : CompareHistories(previousHistory, currentHistory, currentHistoryLog, currentHistoryEmailLogs);
        }

        private static HistoriesDifference CreateDifferenceForFirstHistory(
            HistoryOverview firstHistory,
            List<LogOverview> log,
            List<EmailLogOverview> emailLogs)
        {
            var history = new List<FieldDifference>();

            var emails = emailLogs.Select(l => l.Email).ToList();
            var logs = log.Select(l => l.Text).ToList();

            return new HistoriesDifference(
                firstHistory.DateAndTime,
                firstHistory.RegisteredBy,
                logs,
                history,
                emails);
        }

        private static HistoriesDifference CompareHistories(
            HistoryOverview previousHistory,
            HistoryOverview currentHistory,
            List<LogOverview> currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs)
        {
            var history = new List<FieldDifference>();

            var emails = currentHistoryEmailLogs.Select(l => l.Email).ToList();
            var logs = currentHistoryLog.Select(l => l.Text).ToList();

            if (logs.Any() || history.Any() || emails.Any())
            {
                return new HistoriesDifference(
                currentHistory.DateAndTime,
                currentHistory.RegisteredBy,
                logs,
                history,
                emails);
            }

            return null;
        }
    }
}