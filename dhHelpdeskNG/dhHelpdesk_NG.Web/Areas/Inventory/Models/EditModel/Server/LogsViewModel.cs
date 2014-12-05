namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class LogsViewModel : BaseEditServerModel
    {
        public LogsViewModel(int id, List<OperationServerLogOverview> overviews)
            : base(id)
        {
            this.Overviews = overviews;
        }

        [NotNull]
        public List<OperationServerLogOverview> Overviews { get; set; }

        public override ServerEditTabs Tab
        {
            get
            {
                return ServerEditTabs.OperationLog;
            }
        }
    }
}