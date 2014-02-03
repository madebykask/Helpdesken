namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.Web.Models.Changes;

    public sealed class LogsModelFactory : ILogsModelFactory
    {
        public List<LogModel> Create(List<Log> logs)
        {
            return logs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
        }
    }
}