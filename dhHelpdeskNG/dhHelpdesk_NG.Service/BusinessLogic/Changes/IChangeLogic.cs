namespace DH.Helpdesk.Services.BusinessLogic.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;

    public interface IChangeLogic
    {
        List<HistoriesDifference> AnalyzeHistoriesDifferences(List<HistoryOverview> histories, List<LogOverview> logs, List<EmailLogOverview> emailLogs);
    }
}