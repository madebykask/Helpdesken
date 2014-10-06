namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Services.Services;

    public class ComputerUsersBlackListController : BaseAdminController
    {
        private readonly IComputerService _computerService;

        public ComputerUsersBlackListController(
            IComputerService computerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._computerService = computerService;
        }

        public ActionResult Index()
        {
            var computerUsersBlackLists = this._computerService.GetComputerUsersBlackLists();

            return this.View(computerUsersBlackLists);
        }

        public ActionResult New()
        {
            return this.View(new ComputerUsersBlackList());
        }

        [HttpPost]
        public ActionResult New(ComputerUsersBlackList computerUsersBlackList)
        {
            if (this.ModelState.IsValid)
            {
                this._computerService.NewComputerUsersBlackList(computerUsersBlackList);
                this._computerService.Commit();

                return this.RedirectToAction("index", "computerusersblacklist", new { area = "admin" });
            }

            return this.View(computerUsersBlackList);
        }

        public ActionResult Edit(int id)
        {
            var computerUsersBlackList = this._computerService.GetComputerUsersBlackList(id);

            if (computerUsersBlackList == null)                
                return new HttpNotFoundResult("No black list found...");

            return this.View(computerUsersBlackList);
        }

        [HttpPost]
        public ActionResult Edit(ComputerUsersBlackList computerUsersBlackList)
        {
            if (this.ModelState.IsValid)
            {
                this._computerService.UpdateComputerUsersBlackList(computerUsersBlackList);
                this._computerService.Commit();

                return this.RedirectToAction("index", "computerusersblacklist", new { area = "admin" });
            }

            return this.View(computerUsersBlackList);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var computerUsersBlackList = this._computerService.GetComputerUsersBlackList(id);

            if (computerUsersBlackList != null)
            {
                this._computerService.DeleteComputerUsersBlackList(computerUsersBlackList);
                this._computerService.Commit();
            }

            return this.RedirectToAction("index", "computerusersblacklist", new { area = "admin" });
        }
    }
}
