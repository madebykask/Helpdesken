namespace DH.Helpdesk.Services.BusinessLogic.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;

    public interface IHistoriesComparator
    {
        HistoriesDifference Compare(
            History previousHistory,
            History currentHistory,
            LogOverview currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs);
    }
}