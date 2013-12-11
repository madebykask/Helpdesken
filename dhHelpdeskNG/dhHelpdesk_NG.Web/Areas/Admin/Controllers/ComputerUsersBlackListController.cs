using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class ComputerUsersBlackListController : BaseController
    {
        private readonly IComputerService _computerService;

        public ComputerUsersBlackListController(
            IComputerService computerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _computerService = computerService;
        }

        public ActionResult Index()
        {
            var computerUsersBlackLists = _computerService.GetComputerUsersBlackLists();

            return View(computerUsersBlackLists);
        }

        public ActionResult New()
        {
            return View(new ComputerUsersBlackList());
        }

        [HttpPost]
        public ActionResult New(ComputerUsersBlackList computerUsersBlackList)
        {
            if (ModelState.IsValid)
            {
                _computerService.NewComputerUsersBlackList(computerUsersBlackList);
                _computerService.Commit();

                return RedirectToAction("index", "computerusersblacklist", new { area = "admin" });
            }

            return View(computerUsersBlackList);
        }

        public ActionResult Edit(int id)
        {
            var computerUsersBlackList = _computerService.GetComputerUsersBlackList(id);

            if (computerUsersBlackList == null)                
                return new HttpNotFoundResult("No black list found...");

            return View(computerUsersBlackList);
        }

        [HttpPost]
        public ActionResult Edit(ComputerUsersBlackList computerUsersBlackList)
        {
            if (ModelState.IsValid)
            {
                _computerService.UpdateComputerUsersBlackList(computerUsersBlackList);
                _computerService.Commit();

                return RedirectToAction("index", "computerusersblacklist", new { area = "admin" });
            }

            return View(computerUsersBlackList);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var computerUsersBlackList = _computerService.GetComputerUsersBlackList(id);

            if (computerUsersBlackList != null)
            {
                _computerService.DeleteComputerUsersBlackList(computerUsersBlackList);
                _computerService.Commit();
            }

            return RedirectToAction("index", "computerusersblacklist", new { area = "admin" });
        }
    }
}
