namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class ChangeLanguageController : BaseController
    {
        private readonly IMasterDataService _masterDataService;

        public ChangeLanguageController(
            IMasterDataService masterDataService)
            : base (masterDataService)
        {
            this._masterDataService = masterDataService;
        }

        public ActionResult Index(int id, string returnUrl)
        {
            var language = this._masterDataService.GetLanguage(id);
            if(language != null)
                SessionFacade.CurrentLanguageId = language.Id; 

            return this.Redirect(returnUrl);
        }
    }
}
