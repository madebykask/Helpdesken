namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;

    public sealed class LogsModelFactory : ILogsModelFactory
    {
        public LogsModel Create(int changeId, Subtopic subtopic, List<Log> logs)
        {
            var logModels = logs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
            return new LogsModel(changeId.ToString(CultureInfo.InvariantCulture), subtopic, logModels);
        }
    }
}