namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageViewModel : BaseEditServerModel
    {
        public StorageViewModel(int id, List<LogicalDriveOverview> overviews)
            : base(id)
        {
            this.Overviews = overviews;
        }

        [NotNull]
        public List<LogicalDriveOverview> Overviews { get; set; }

        public override ServerEditTabs Tab
        {
            get
            {
                return ServerEditTabs.Storage;
            }
        }
    }
}