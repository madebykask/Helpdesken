using System.Web.Mvc;

namespace dhHelpdesk_NG.Web.Controllers
{
    using System;

    using dhHelpdesk_NG.Data.Repositories;

    public class ChangesController : Controller
    {
        private readonly IChangeFieldSettingRepository changeFieldSettingRepository;

        public ChangesController(IChangeFieldSettingRepository changeFieldSettingRepository)
        {
            this.changeFieldSettingRepository = changeFieldSettingRepository;
        }

        public ActionResult Index()
        {
            var fieldSettings = this.changeFieldSettingRepository.FindByCustomerIdAndLanguageId(1, 2);
            return View();
        }

        public ActionResult New()
        {
            throw new NotImplementedException();
        }
    }
}
