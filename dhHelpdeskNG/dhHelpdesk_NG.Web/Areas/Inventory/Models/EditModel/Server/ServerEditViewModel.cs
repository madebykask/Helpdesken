namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using System.Collections.Generic;

    public class ServerEditViewModel : BaseEditServerModel
    {
        public List<string> FileUploadWhiteList { get; set; }

        public ServerEditViewModel(int id, ServerViewModel serverViewModel)
            : base(id)
        {
            this.ServerViewModel = serverViewModel;
        }

        public ServerViewModel ServerViewModel { get; set; }
        public bool UserHasInventoryAdminPermission { get; set; }

        public override ServerEditTabs Tab
        {
            get
            {
                return ServerEditTabs.Server;
            }
        }
    }
}

