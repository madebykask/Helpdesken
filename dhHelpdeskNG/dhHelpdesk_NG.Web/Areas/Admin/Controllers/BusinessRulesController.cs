using System.Linq;
using System.Web.Mvc;

using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Changes;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class BusinessRulesController : BaseAdminController
    {
        public BusinessRulesController(IMasterDataService masterDataService) 
            : base(masterDataService)
        {
            
        }

        public ActionResult Index(int customerId)
        {
            return View();
        }

        public ActionResult Edit(int id, int customerId)
        {
            return View();
        }
    }
}
