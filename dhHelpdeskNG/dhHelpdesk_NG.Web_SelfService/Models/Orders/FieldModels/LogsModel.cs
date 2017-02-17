using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Enums.Orders;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.SelfService.Models.Shared;

namespace DH.Helpdesk.SelfService.Models.Orders.FieldModels
{
    public sealed class LogsModel
    {
        #region Constructors and Destructors

        public LogsModel()
        {
            this.Logs = new List<LogModel>();
        }

        public LogsModel(SendToDialogModel sendToDialog)
        {
            this.SendToDialog = sendToDialog;
        }

        public LogsModel(int orderId, Subtopic area, List<LogModel> logs, SendToDialogModel sendToDialog)
        {
            this.OrderId = orderId;
            this.Area = area;
            this.Logs = logs;
            this.SendToDialog = sendToDialog;
        }

        #endregion

        #region Public Properties

        public Subtopic Area { get; set; }

        [IsId]
        public int OrderId { get; set; }

        public string Emails { get; set; }

        [NotNull]
        public List<LogModel> Logs { get; set; }

        public SendToDialogModel SendToDialog { get; set; }

        public string Text { get; set; }

        #endregion
    }
}