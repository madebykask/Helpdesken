namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Web.Infrastructure;

    public class NetWorkAdapterController : ComputerModuleBaseController
    {
        public NetWorkAdapterController(IMasterDataService masterDataService, 
                                        IComputerModulesService computerModulesService,
                                        IUserPermissionsChecker userPermissionChecker)
            : base(masterDataService, computerModulesService, userPermissionChecker)
        {
        }

        public override ModuleTypes ModuleType
        {
            get
            {
                return ModuleTypes.NetworkAdapter;
            }
        }

        protected override List<ItemOverview> Get()
        {
            return this.ComputerModulesService.GetNetAdapters(SessionFacade.CurrentCustomer.Id);
        }

        protected override void Create(ComputerModule computerModule)
        {
            computerModule.Customer_Id = SessionFacade.CurrentCustomer.Id;
            this.ComputerModulesService.AddNetAdapter(computerModule);
        }

        protected override void Update(ComputerModule computerModule)
        {
            computerModule.Customer_Id = SessionFacade.CurrentCustomer.Id;
            this.ComputerModulesService.UpdateNetAdapter(computerModule);
        }

        protected override void Remove(int id)
        {
            this.ComputerModulesService.DeleteNetAdapter(id);
        }
    }
}