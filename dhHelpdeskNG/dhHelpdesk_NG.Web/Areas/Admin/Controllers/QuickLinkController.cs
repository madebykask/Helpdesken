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
        private readonly IUserService _userService;
        private readonly ICaseSolutionService _casesolutionService;

        public QuickLinkController(
            IDocumentService documentService,
            ILinkService linkService,
            ICustomerService customerService,
            IUserService userService,
            ICaseSolutionService casesolutionService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._documentService = documentService;
            this._linkService = linkService;
            this._customerService = customerService;
            this._userService = userService;
            this._casesolutionService = casesolutionService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var links = this._linkService.GetLinks(customer.Id);

            var model = new QuickLinkIndexViewModel { Links = links, Customer = customer };
            return this.View(model);
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
        public ActionResult New(Link link)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._linkService.SaveLink(link, null, out errors);

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
        public ActionResult Edit(Link link, int[] UsSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._linkService.SaveLink(link, UsSelected, out errors);

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

            foreach (var u in this._userService.GetUsers(customer.Id))
            {
                if (!usSelected.Contains(u))
                    usAvailable.Add(u);
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
                CaseSolutions = this._casesolutionService.GetCaseSolutions(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                LinkGroups = this._linkService.GetLinkGroups(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.LinkGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                UsAvailable = usAvailable.Select(x => new SelectListItem
                {
                    Text = x.FirstName + ' ' + x.SurName,
                    Value = x.Id.ToString()
                }).ToList(),
                UsSelected = usSelected.Select(x => new SelectListItem
                {
                    Text = x.FirstName + ' ' + x.SurName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
