namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface ILogsModelFactory
    {
        LogsModel Create(int changeId, Subtopic subtopic, List<Log> logs);
    }
}