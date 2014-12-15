namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;

    public sealed class LogsModelFactory : ILogsModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public LogsModelFactory(ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #region Public Methods and Operators

        public LogsModel Create(int orderId, Subtopic area, List<Log> logs, OrderEditOptions options)
        {
            var sendToDialog = this.sendToDialogModelFactory.Create(
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.AdministratorsWithEmails);

            var logModels = logs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
            return new LogsModel(orderId, area, logModels, sendToDialog);
        }

        #endregion
    }
}