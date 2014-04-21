namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Web.Models.Changes;

    public interface ILogsModelFactory
    {
        #region Public Methods and Operators

        LogsModel Create(int changeId, ChangeArea area, List<Log> logs, ChangeEditOptions options);

        #endregion
    }
}