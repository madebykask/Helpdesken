namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class ComputerUserGroupController : BaseController
    {
        private readonly IComputerService _computerService;
        private readonly IOUService _ouService;

        public ComputerUserGroupController(
            IComputerService computerService,
            IOUService ouService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._computerService = computerService;
            this._ouService = ouService;
        }

        public ActionResult Index()
        {
            var computerUserGroup = this._computerService.GetComputerUserGroups(SessionFacade.CurrentCustomer.Id);

            return this.View(computerUserGroup);
        }

        public ActionResult New()
        {
            var model = this.CreateInputViewModel(new ComputerUserGroup { Customer_Id = SessionFacade.CurrentCustomer.Id });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ComputerUserGroup computerUserGroup, int[] OUsSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._computerService.SaveComputerUserGroup(computerUserGroup, OUsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "computerusergroup", new { area = "admin" });

            var model = this.CreateInputViewModel(computerUserGroup);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var computerUserGroup = this._computerService.GetComputerUserGroup(id);

            if (computerUserGroup == null)
                return new HttpNotFoundResult("No computer user group found...");

            var model = this.CreateInputViewModel(computerUserGroup);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, ComputerUserGroup computerUserGroup, int[] OUsSelected)
        {
            var computerUserGroupToSave = this._computerService.GetComputerUserGroup(id);
            this.UpdateModel(computerUserGroupToSave, "ComputerUserGroup");

            computerUserGroupToSave.IsDefault = computerUserGroup.IsDefault;
            computerUserGroupToSave.ShowOnStartPage = computerUserGroup.ShowOnStartPage;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._computerService.SaveComputerUserGroup(computerUserGroupToSave, OUsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "computerusergroup", new { area = "admin" });

            var model = this.CreateInputViewModel(computerUserGroupToSave);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._computerService.DeleteComputerUserGroup(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "computerusergroup", new { area = "admin" });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "computerusergroup", new { area = "admin", id = id });
            }
        }

        private ComputerUserGroupInputViewModel CreateInputViewModel(ComputerUserGroup computerUserGroup)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Grupp", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Enhetsadministratör", Enums.TranslationSource.TextTranslation),
                Value = "2",
                Selected = false
            });
            sli.Add(new SelectListItem()
            {
                Text = Translation.Get("Skrivare", Enums.TranslationSource.TextTranslation),
                Value = "3",
                Selected = false
            });

            var ousSelected = computerUserGroup.OUs ?? new List<OU>();
            var ousAvailable = new List<OU>();

            foreach (var ou in this._ouService.GetThemAllOUs(SessionFacade.CurrentCustomer.Id))
            {
                if (!ousSelected.Contains(ou))
                    ousAvailable.Add(ou);
            }

            var model = new ComputerUserGroupInputViewModel
            {
                ComputerUserGroup = computerUserGroup,
                TypeChoices = sli,
                OUsAvailable = ousAvailable.Select(x => new SelectListItem
                {
                    Text = x.Name + " (" + x.Department.DepartmentName + ")",
                    Value = x.Id.ToString()
                }).ToList(),
                OUsSelected = ousSelected.Select(x => new SelectListItem
                {
                    Text = x.Name + " (" + x.Department.DepartmentName + ")",
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
