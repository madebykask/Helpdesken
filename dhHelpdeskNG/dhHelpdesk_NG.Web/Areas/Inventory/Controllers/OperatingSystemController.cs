namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;

    public class OperatingSystemController : ComputerModuleBaseController
    {
        public OperatingSystemController(IMasterDataService masterDataService, IComputerModulesService computerModulesService)
            : base(masterDataService, computerModulesService)
        {
        }

        public override ModuleTypes ModuleType
        {
            get
            {
                return ModuleTypes.OperatingSystem;
            }
        }

        protected override List<ItemOverview> Get()
        {
            return this.ComputerModulesService.GetOperatingSystems();
        }

        protected override void Create(ComputerModule computerModule)
        {
            this.ComputerModulesService.AddOperatingSystem(computerModule);
        }

        protected override void Update(ComputerModule computerModule)
        {
            this.ComputerModulesService.UpdateOperatingSystem(computerModule);
        }

        protected override void Remove(int id)
        {
            this.ComputerModulesService.DeleteOperatingSystem(id);
        }
    }
}