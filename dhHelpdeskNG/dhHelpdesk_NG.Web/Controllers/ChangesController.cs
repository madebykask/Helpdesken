namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.Changes;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;

    public class ChangesController : BaseController
    {
        private readonly IChangeService changeService;

        private readonly ISettingsModelFactory settingsModelFactory;

        private readonly IUpdatedFieldSettingsFactory updatedFieldSettingsFactory;

        public ChangesController(
            IMasterDataService masterDataService,
            IChangeService changeService,
            ISettingsModelFactory settingsModelFactory, 
            IUpdatedFieldSettingsFactory updatedFieldSettingsFactory)
            : base(masterDataService)
        {
            this.changeService = changeService;
            this.settingsModelFactory = settingsModelFactory;
            this.updatedFieldSettingsFactory = updatedFieldSettingsFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public PartialViewResult Settings()
        {
            var fieldSettings = this.changeService.FindSettings(
                SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage);

            var model = this.settingsModelFactory.Create(fieldSettings);
            return this.PartialView(model);
        }

        [HttpPost]
        public void Settings(SettingsModel model)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var updatedFieldSettings = this.updatedFieldSettingsFactory.Create(
                model, SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage, DateTime.Now);

            this.changeService.UpdateSettings(updatedFieldSettings);
        }

        [HttpGet]
        public ViewResult Change()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public PartialViewResult ChangesGrid(SearchModel model)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            // var changes = this.changesRepository.SearchOverviews();
            // this.changesGridModelFactory.Create(changes)

            return null;
        }
    }
}
