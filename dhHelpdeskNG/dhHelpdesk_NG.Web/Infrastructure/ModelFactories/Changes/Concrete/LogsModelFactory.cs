namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Web.Models.Changes;

    public sealed class LogsModelFactory : ILogsModelFactory
    {
        public LogsModel Create(int changeId, Subtopic subtopic, List<Log> logs)
        {
            var logModels = logs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
            return new LogsModel(changeId.ToString(CultureInfo.InvariantCulture), subtopic, logModels);
        }
    }
}