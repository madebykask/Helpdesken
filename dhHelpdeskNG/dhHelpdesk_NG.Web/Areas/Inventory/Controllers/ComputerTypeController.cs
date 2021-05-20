using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;

    public class ComputerTypeController : ComputerModuleBaseController
    {
        public ComputerTypeController(IMasterDataService masterDataService, 
                                      IComputerModulesService computerModulesService, 
                                      IUserPermissionsChecker userPermissionChecker)
            : base(masterDataService, computerModulesService, userPermissionChecker)
        {
        }

        public override ModuleTypes ModuleType
        {
            get
            {
                return ModuleTypes.ComputerType;
            }
        }

        protected override ComputerModuleEditModel CreateModuleEditModel(int id, string name)
        {
            var computerType = ComputerModulesService.GetComputerType(id);
            return new ComputerModuleEditModel(id, computerType.Name)
            {
                Description = computerType.Description,
                Price = computerType.Price
            };
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

        protected override ComputerModule CreateNewBusinessModel(ComputerModuleEditModel model)
        {
            var businessModel = base.CreateNewBusinessModel(model);
            businessModel.Description = model.Description ?? string.Empty;
            return businessModel;
        }

        protected override ComputerModule CreateUpdatedBusinessModel(ComputerModuleEditModel model)
        {
            var businessModel = base.CreateUpdatedBusinessModel(model);
            businessModel.Description = model.Description ?? string.Empty;
            return businessModel;
        }
    }
}