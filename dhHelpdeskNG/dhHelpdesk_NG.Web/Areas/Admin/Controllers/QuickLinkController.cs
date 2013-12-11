using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
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
            _documentService = documentService;
            _linkService = linkService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var links = _linkService.GetLinks(customer.Id);

            var model = new QuickLinkIndexViewModel { Links = links, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var link = new Link { Customer_Id = customer.Id };

            var model = CreateInputViewModel(link, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Link link)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _linkService.SaveLink(link, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "quicklink", new { customerId = link.Customer_Id });

            var customer = _customerService.GetCustomer(link.Customer_Id.Value);
            var model = CreateInputViewModel(link, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var link = _linkService.GetLink(id);

            if (link == null)
                return new HttpNotFoundResult("No quick link found...");

            var customer = _customerService.GetCustomer(link.Customer_Id.Value);
            var model = CreateInputViewModel(link, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Link link)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _linkService.SaveLink(link, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "quicklink", new { customerId = link.Customer_Id });

            var customer = _customerService.GetCustomer(link.Customer_Id.Value);
            var model = CreateInputViewModel(link, customer);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var link = _linkService.GetLink(id);
            var customer = _customerService.GetCustomer(link.Customer_Id.Value);
            if (_linkService.DeleteLink(id) == DeleteMessage.Success)
                return RedirectToAction("index", "quicklink", new { customerId = customer.Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "quicklink", new { area = "admin", id = link.Id });
            }
        }

        private QuickLinkInputViewModel CreateInputViewModel(Link link, Customer customer)
        {
            var model = new QuickLinkInputViewModel
            {
                Link = link,
                Customer = customer,
                Documents = _documentService.GetDocuments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
