namespace DH.Helpdesk.Services.BusinessLogic.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;

    public sealed class ChangeLogic : IChangeLogic
    {
        private readonly IHistoriesComparator historiesComparator;

        public ChangeLogic(IHistoriesComparator historiesComparator)
        {
            this.historiesComparator = historiesComparator;
        }

        public List<HistoriesDifference> AnalyzeHistoriesDifferences(List<HistoryOverview> histories, List<LogOverview> logs, List<EmailLogOverview> emailLogs)
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
                    historyEmailLogs);

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
