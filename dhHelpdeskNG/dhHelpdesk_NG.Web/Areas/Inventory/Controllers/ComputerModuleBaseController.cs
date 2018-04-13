namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    public abstract class ComputerModuleBaseController : UserInteractionController
    {
        protected readonly IComputerModulesService ComputerModulesService;
        protected readonly IUserPermissionsChecker UserPermissionsChecker;

        protected ComputerModuleBaseController(
                  IMasterDataService masterDataService, 
                  IComputerModulesService computerModulesService,
                  IUserPermissionsChecker userPermissionChecker
            )
            : base(masterDataService)
        {
            this.ComputerModulesService = computerModulesService;
            this.UserPermissionsChecker = userPermissionChecker;
        }

        public abstract ModuleTypes ModuleType { get; }

        [HttpGet]
        public ViewResult Index()
        {
            List<ItemOverview> items = this.Get().OrderBy(x => x.Name).ToList();

            var inventoryPermission = this.UserPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);
            var viewModel = new ComputerModuleGridModel(items, this.ModuleType, inventoryPermission);

            return this.View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, string name)
        {
            var inventoryPermission = this.UserPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            var viewModel = CreateModuleEditModel(id, name);
            viewModel.UserHasInventoryAdminPermission = inventoryPermission;

            return this.View(viewModel);
        }

        [HttpPost]
        public RedirectToRouteResult Edit(ComputerModuleEditModel model)
        {
            var businessModel = CreateUpdatedBusinessModel(model);
            this.Update(businessModel);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult New()
        {
            var viewModel = new ComputerModuleEditModel();

            return this.View(viewModel);
        }

        [HttpPost]
        public RedirectToRouteResult New(ComputerModuleEditModel model)
        {
            var businessModel = CreateNewBusinessModel(model);
            this.Create(businessModel);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult Delete(int id)
        {
            this.Remove(id);

            return this.RedirectToAction("Index");
        }

        protected virtual ComputerModuleEditModel CreateModuleEditModel(int id, string name)
        {
            var viewModel = new ComputerModuleEditModel(id, name);
            return viewModel;
        }

        protected virtual ComputerModule CreateNewBusinessModel(ComputerModuleEditModel model)
        {
            var businessModel = ComputerModule.CreateNew(model.Name, DateTime.Now);
            return businessModel;
        }

        protected virtual ComputerModule CreateUpdatedBusinessModel(ComputerModuleEditModel model)
        {
            var businessModel = ComputerModule.CreateUpdated(model.Id, model.Name, DateTime.Now);
            return businessModel;
        }

        protected abstract List<ItemOverview> Get();

        protected abstract void Create(ComputerModule computerModule);

        protected abstract void Update(ComputerModule computerModule);

        protected abstract void Remove(int id);
    }
}