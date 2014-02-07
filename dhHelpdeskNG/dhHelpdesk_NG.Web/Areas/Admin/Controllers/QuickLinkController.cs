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

    public class QuickLinkController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly ILinkService _linkService;
        private readonly ICustomerService _customerService;

        public QuickLinkController(
            IDocumentService documentService,
            ILinkService linkService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._documentService = documentService;
            this._linkService = linkService;
            this._customerService = customerService;
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
            var link = new Link { Customer_Id = customer.Id };

            var model = this.CreateInputViewModel(link, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Link link)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._linkService.SaveLink(link, out errors);

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
        public ActionResult Edit(Link link)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._linkService.SaveLink(link, out errors);

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
            var model = new QuickLinkInputViewModel
            {
                Link = link,
                Customer = customer,
                Documents = this._documentService.GetDocuments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
