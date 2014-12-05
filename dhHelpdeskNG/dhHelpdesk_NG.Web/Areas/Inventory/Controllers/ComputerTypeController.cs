namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;

    public class ComputerTypeController : ComputerModuleBaseController
    {
        public ComputerTypeController(IMasterDataService masterDataService, IComputerModulesService computerModulesService)
            : base(masterDataService, computerModulesService)
        {
        }

        public override ModuleTypes ModuleType
        {
            get
            {
                return ModuleTypes.ComputerType;
            }
        }

        protected override List<ItemOverview> Get()
        {
            return this.ComputerModulesService.GetComputerTypes(OperationContext.CustomerId);
        }

        protected override void Create(ComputerModule computerModule)
        {
            this.ComputerModulesService.AddComputerType(computerModule, this.OperationContext);
        }

        protected override void Update(ComputerModule computerModule)
        {
            this.ComputerModulesService.UpdateComputerType(computerModule);
        }

        protected override void Remove(int id)
        {
            this.ComputerModulesService.DeleteComputerType(id);
        }
    }
}