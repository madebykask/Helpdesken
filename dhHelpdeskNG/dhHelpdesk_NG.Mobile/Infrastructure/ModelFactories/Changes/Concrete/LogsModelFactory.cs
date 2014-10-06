namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Mobile.Models.Changes;

    public sealed class LogsModelFactory : ILogsModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public LogsModelFactory(ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #region Public Methods and Operators

        public LogsModel Create(int changeId, Subtopic area, List<Log> logs, ChangeEditOptions options)
        {
            var sendToDialog = this.sendToDialogModelFactory.Create(
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.AdministratorsWithEmails);

            var logModels = logs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
            return new LogsModel(changeId, area, logModels, sendToDialog);
        }

        #endregion
    }
}