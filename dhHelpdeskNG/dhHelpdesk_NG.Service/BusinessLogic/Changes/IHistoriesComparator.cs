namespace dhHelpdesk_NG.Service.BusinessLogic.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public interface IHistoriesComparator
    {
        HistoriesDifference Compare(
            History previousHistory,
            History currentHistory,
            LogOverview currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs);
    }
}