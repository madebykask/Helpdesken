using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class QuickLinkController : BaseAdminController
    {
        private readonly IDocumentService _documentService;
        private readonly ILinkService _linkService;
        private readonly ICustomerService _customerService;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;
        private readonly ICaseSolutionService _casesolutionService;
        private readonly IWorkingGroupService _workgroupService;

        public QuickLinkController(
            IDocumentService documentService,
            ILinkService linkService,
            ICustomerService customerService,
            IUserService userService,
            ICaseSolutionService casesolutionService,
            ISettingService settingService,
            IMasterDataService masterDataService,
            IWorkingGroupService workgroupService)
            : base(masterDataService)
        {
            this._documentService = documentService;
            this._linkService = linkService;
            this._customerService = customerService;
            this._userService = userService;
            this._casesolutionService = casesolutionService;
            this._settingService = settingService;
            this._workgroupService = workgroupService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var links = this._linkService.GetLinks(customer.Id);
            var linkGroups = _linkService.GetLinkGroups(customer.Id);

            var model = new QuickLinkIndexViewModel { Links = links, Customer = customer, LinkGroups = linkGroups };
            return this.View(model);
        }

        [HttpPost]
        public PartialViewResult Search(string searchText, int customerId, List<int> groupIds)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var links = this._linkService.SearchLinks(customer.Id, searchText, groupIds);

            var model = new QuickLinkIndexViewModel { Links = links, Customer = customer };
            return PartialView("~/Areas/Admin/Views/QuickLink/_QuickLinksRows.cshtml", model.Links);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            // Height and width is handled via default value in database in helpdesk 4. This is a fix to satisfy KOREA IKEA requirements. TODO: Add possibility to change them for the user?
            var link = new Link { Customer_Id = customer.Id, NewWindowHeight = 725, NewWindowWidth = 1225 };

            var model = this.CreateInputViewModel(link, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Link link, int[] UsSelected, int[] WgSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._linkService.SaveLink(link, UsSelected, WgSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "quicklink", new { customerId = link.Customer_Id });

            var customer = this._customerService.GetCustomer(link.Customer_Id.Value);
            var model = this.CreateInputViewModel(link, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var link = this._linkService.GetLink(id);

            if (link == null)
                return new HttpNotFoundResult("No quick link found...");

            var customer = this._customerService.GetCustomer(link.Customer_Id.Value);
            var model = this.CreateInputViewModel(link, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Link link, int[] UsSelected, int[] WgSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._linkService.SaveLink(link, UsSelected, WgSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "quicklink", new { customerId = link.Customer_Id });

            var customer = this._customerService.GetCustomer(link.Customer_Id.Value);
            var model = this.CreateInputViewModel(link, customer);

            return this.View(model);
        }

        public ActionResult Delete(int id)
        {
            var link = this._linkService.GetLink(id);
            var customer = this._customerService.GetCustomer(link.Customer_Id.Value);
            if (this._linkService.DeleteLink(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "quicklink", new { customerId = customer.Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "quicklink", new { area = "admin", id = link.Id });
            }
        }

        private QuickLinkInputViewModel CreateInputViewModel(Link link, Customer customer)
        {
            var usSelected = link.Us ?? new List<User>();
            var usAvailable = new List<User>();

            var wgSelected = link.Wg ?? new List<WorkingGroupEntity>();
            var wgAvailable = new List<WorkingGroupEntity>();

            var cs = this._settingService.GetCustomerSetting(customer.Id);
            var isFirstName = (cs.IsUserFirstLastNameRepresentation == 1);

            foreach (var u in this._userService.GetUsers(customer.Id))
            {
                if (!usSelected.Contains(u))
                    usAvailable.Add(u);
            }

            foreach (var w in this._workgroupService.GetWorkingGroups(customer.Id, true))
            {
                if (!wgSelected.Contains(w))
                    wgAvailable.Add(w);
            }

            var model = new QuickLinkInputViewModel
            {

                Link = link,
                Customer = customer,
                Documents = this._documentService.GetDocuments(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseSolutions = this._casesolutionService.GetCustomerCaseSolutionsOverview(customer.Id).Where(x => x.ConnectedButton != 0).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CaseSolutionId.ToString()
                }).ToList(),

                LinkGroups = this._linkService.GetLinkGroups(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.LinkGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                UsAvailable = usAvailable.Select(x => new SelectListItem
                {
                    Text = (isFirstName ? string.Format("{0} {1}", x.FirstName, x.SurName) : string.Format("{0} {1}", x.SurName, x.FirstName)),
                    Value = x.Id.ToString()
                }).OrderBy(a => a.Text).ToList(),

                UsSelected = usSelected.Select(x => new SelectListItem
                {
                    Text = (isFirstName ? string.Format("{0} {1}", x.FirstName, x.SurName) : string.Format("{0} {1}", x.SurName, x.FirstName)),
                    Value = x.Id.ToString()
                }).OrderBy(s => s.Text).ToList(),

                WgAvailable = wgAvailable.Select(x => new SelectListItem
                {
                    Text = (x.WorkingGroupName),
                    Value = x.Id.ToString()
                }).OrderBy(a => a.Text).ToList(),

                WgSelected = wgSelected.Select(x => new SelectListItem
                {
                    Text = (x.WorkingGroupName),
                    Value = x.Id.ToString()
                }).OrderBy(s => s.Text).ToList()

            };

            return model;
        }
    }
}
