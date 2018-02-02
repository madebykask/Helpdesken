namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class LogsViewModel : BaseEditWorkstationModel
    {
        public LogsViewModel(int id, List<ComputerLogOverview> computerLogs, bool dialog = false, string userId = null)
            : base(id)
        {
            this.ComputerLogs = computerLogs;
            this.ComputerLogModel = new ComputerLogModel { ComputerId = id, IsForDialog = dialog, UserId = userId };
        }

        [NotNull]
        public ComputerLogModel ComputerLogModel { get; set; }

        [NotNull]
        public List<ComputerLogOverview> ComputerLogs { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Log;
            }
        }
    }
}