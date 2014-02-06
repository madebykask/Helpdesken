namespace dhHelpdesk_NG.Service.BusinessLogic.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public sealed class ChangeLogic : IChangeLogic
    {
        private readonly IHistoriesComparator historiesComparator;

        public ChangeLogic(IHistoriesComparator historiesComparator)
        {
            this.historiesComparator = historiesComparator;
        }

        public List<HistoriesDifference> AnalyzeDifference(List<History> histories, List<LogOverview> logs, List<EmailLogOverview> emailLogs)
        {
            var historyDifferences = new List<HistoriesDifference>();

            History previousHistory = null;

            foreach (var history in histories)
            {
                var historyLog = logs.SingleOrDefault(l => l.HistoryId == history.Id);
                var historyEmailLogs = emailLogs.Where(l => l.HistoryId == history.Id).ToList();

                var difference = this.historiesComparator.Compare(
                    previousHistory,
                    history,
                    historyLog,
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
