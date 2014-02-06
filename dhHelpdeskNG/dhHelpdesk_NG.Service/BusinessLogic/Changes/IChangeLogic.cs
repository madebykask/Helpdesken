namespace dhHelpdesk_NG.Service.BusinessLogic.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public interface IChangeLogic
    {
        List<HistoriesDifference> AnalyzeDifference(List<History> histories, List<LogOverview> logs, List<EmailLogOverview> emailLogs);
    }
}