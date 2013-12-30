namespace dhHelpdesk_NG.Web.Controllers
{
    using System.Web.Mvc;

    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.Changes;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;

    public class ChangesController : BaseController
    {
        private readonly IChangeService changeService;

         private readonly ISettingsModelFactory settingsModelFactory;

        public ChangesController(
            IMasterDataService masterDataService,
            IChangeService changeService,
            ISettingsModelFactory settingsModelFactory)
            : base(masterDataService)
        {
            this.changeService = changeService;
            this.settingsModelFactory = settingsModelFactory;
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
    }
}
