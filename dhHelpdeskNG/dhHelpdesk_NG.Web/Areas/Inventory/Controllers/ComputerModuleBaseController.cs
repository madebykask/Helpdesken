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

    public abstract class ComputerModuleBaseController : UserInteractionController
    {
        protected readonly IComputerModulesService ComputerModulesService;

        protected ComputerModuleBaseController(IMasterDataService masterDataService, IComputerModulesService computerModulesService)
            : base(masterDataService)
        {
            this.ComputerModulesService = computerModulesService;
        }

        public abstract ModuleTypes ModuleType { get; }

        [HttpGet]
        public ViewResult Index()
        {
            List<ItemOverview> items = this.Get().OrderBy(x => x.Name).ToList();

            var viewModel = new ComputerModuleGridModel(items, this.ModuleType);

            return this.View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, string name)
        {
            var viewModel = new ComputerModuleEditModel(id, name);

            return this.View(viewModel);
        }

        [HttpPost]
        public RedirectToRouteResult Edit(ComputerModuleEditModel model)
        {
            ComputerModule businessModel = ComputerModule.CreateUpdated(model.Id, model.Name, DateTime.Now);

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
            ComputerModule businessModel = ComputerModule.CreateNew(model.Name, DateTime.Now);

            this.Create(businessModel);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult Delete(int id)
        {
            this.Remove(id);

            return this.RedirectToAction("Index");
        }

        protected abstract List<ItemOverview> Get();

        protected abstract void Create(ComputerModule computerModule);

        protected abstract void Update(ComputerModule computerModule);

        protected abstract void Remove(int id);
    }
}