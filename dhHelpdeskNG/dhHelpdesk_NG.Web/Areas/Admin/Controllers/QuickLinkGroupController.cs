using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class QuickLinkGroupController : BaseAdminController
    {
        private readonly ILinkService _linkService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public QuickLinkGroupController(ILinkService linkService,
            ICustomerService customerService,
            IUserService userService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _linkService = linkService;
            _customerService = customerService;
            _userService = userService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            if(customer == null)
                return RedirectToAction("Index", "Start");

            var linkGroups = _linkService.GetLinkGroups(customer.Id);
            var model = new QuickLinkGroupIndexViewModel { LinkGroups = linkGroups, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            if(customer == null)
                return RedirectToAction("Index", "Start");

            var linkGroup = new LinkGroup { Customer_Id = customer.Id };

            var model = new QuickLinkGroupInputViewModel { LinkGroup = linkGroup, Customer = customer };

            return View(model);
        }

        [HttpPost]
        public ActionResult New(LinkGroup linkGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _linkService.SaveLinkGroup(linkGroup, out errors);

            if(errors.Count == 0)
                return RedirectToAction("index", "quicklinkgroup", new { customerId = linkGroup.Customer_Id });

            var customer = _customerService.GetCustomer(linkGroup.Customer_Id);

            if(customer == null)
                return RedirectToAction("Index", "Start");

            var model = new QuickLinkGroupInputViewModel { LinkGroup = linkGroup, Customer = customer };

            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var linkGroup = _linkService.GetLinkGroup(id);

            if(linkGroup == null)
                return new HttpNotFoundResult("No group found...");

            var customer = _customerService.GetCustomer(linkGroup.Customer_Id);

            if(customer == null)
                return RedirectToAction("Index", "Start");

            var model = new QuickLinkGroupInputViewModel { LinkGroup = linkGroup, Customer = customer };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(LinkGroup linkGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _linkService.SaveLinkGroup(linkGroup, out errors);

            if(errors.Count == 0)
                return RedirectToAction("index", "quicklinkgroup", new { customerId = linkGroup.Customer_Id });

            var customer = _customerService.GetCustomer(linkGroup.Customer_Id);

            if(customer == null)
                return RedirectToAction("Index", "Start");

            var model = new QuickLinkGroupInputViewModel { LinkGroup = linkGroup, Customer = customer };

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var linkGroup = _linkService.GetLinkGroup(id);
            var customer = _customerService.GetCustomer(linkGroup.Customer_Id);

            if(customer == null)
                return RedirectToAction("Index", "Start");

            if(_linkService.DeleteLinkGroup(id) == DeleteMessage.Success)
                return RedirectToAction("index", "quicklinkgroup", new { customerId = customer.Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "quicklinkgroup", new { area = "admin", id = linkGroup.Id });
            }
        }
    }
}
