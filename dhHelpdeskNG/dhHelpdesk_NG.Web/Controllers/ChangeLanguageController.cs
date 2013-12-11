using System.Web.Mvc;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class ChangeLanguageController : BaseController
    {
        private readonly IMasterDataService _masterDataService;

        public ChangeLanguageController(
            IMasterDataService masterDataService)
            : base (masterDataService)
        {
            _masterDataService = masterDataService;
        }

        public ActionResult Index(int id, string returnUrl)
        {
            var language = _masterDataService.GetLanguage(id);
            if(language != null)
                SessionFacade.CurrentLanguage = language.Id; 

            return Redirect(returnUrl);
        }
    }
}
