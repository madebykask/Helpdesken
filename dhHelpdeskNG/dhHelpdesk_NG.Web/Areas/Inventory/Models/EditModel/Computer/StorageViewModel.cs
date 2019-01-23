namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageViewModel : BaseViewEditWorkstationModel
    {
        public StorageViewModel(int id, List<LogicalDriveOverview> overviews)
            : base(id)
        {
            this.Overviews = overviews;
        }

        [NotNull]
        public List<LogicalDriveOverview> Overviews { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Storage;
            }
        }
    }
}