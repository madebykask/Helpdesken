namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using DH.Helpdesk.Web.Areas.Inventory.Models;

    public class ServerEditViewModel : BaseEditServerModel
    {
        public ServerEditViewModel(int id, ServerViewModel serverViewModel)
            : base(id)
        {
            this.ServerViewModel = serverViewModel;
        }

        public ServerViewModel ServerViewModel { get; set; }

        public override ServerEditTabs Tab
        {
            get
            {
                return ServerEditTabs.Server;
            }
        }
    }
}

