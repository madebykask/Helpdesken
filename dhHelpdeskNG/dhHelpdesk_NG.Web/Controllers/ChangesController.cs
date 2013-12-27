namespace dhHelpdesk_NG.Web.Controllers
{
    using System.Web.Mvc;

    using dhHelpdesk_NG.Service.Changes;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Output;

    public class ChangesController : Controller
    {
        private readonly IChangeService changeService;

        // private readonly ISettingsModelFactory settingsModelFactory;

        public ChangesController(IChangeService changeService)
        {
            this.changeService = changeService;
            //this.settingsModelFactory = settingsModelFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View(new TextModel());
        }

        [HttpGet]
        public PartialViewResult Settings()
        {
            var fieldSettings = this.changeService.FindSettings(
                SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage);

            //var model = this.settingsModelFactory.Create(fieldSettings);
            return this.PartialView();
        }
    }
}
