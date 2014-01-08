using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class ProductAreaController : BaseController
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IProductAreaService _productAreaService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICustomerService _customerService;
        private readonly IPriorityService _priorityService;

        public ProductAreaController(
            ICaseTypeService caseTypeService,
            IMailTemplateService mailTemplateService,
            IProductAreaService productAreaService,
            IWorkingGroupService workingGroupService,
            ICustomerService customerService,
            IPriorityService priorityService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _caseTypeService = caseTypeService;
            _mailTemplateService = mailTemplateService;
            _productAreaService = productAreaService;
            _workingGroupService = workingGroupService;
            _customerService = customerService;
            _priorityService = priorityService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var productAreas = _productAreaService.GetProductAreas(customer.Id);

            var model = new ProductAreaIndexViewModel { ProductAreas = productAreas, Customer = customer };

            return View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (_productAreaService.GetProductArea(parentId.Value) == null)
                    return new HttpNotFoundResult("No parent product area found...");
            }

            var productArea = new ProductArea { Customer_Id = customer.Id, Parent_ProductArea_Id = parentId, IsActive = 1 };
            var model = CreateInputViewModel(productArea, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(ProductArea productArea)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _productAreaService.SaveProductArea(productArea, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "productarea", new { customerid = productArea.Customer_Id });

            var customer = _customerService.GetCustomer(productArea.Customer_Id);
            var model = CreateInputViewModel(productArea, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var productArea = _productAreaService.GetProductArea(id);

            if (productArea == null)
                return new HttpNotFoundResult("No product area found...");

            var customer = _customerService.GetCustomer(productArea.Customer_Id);
            var model = CreateInputViewModel(productArea, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductArea productArea)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _productAreaService.SaveProductArea(productArea, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "productarea", new { customerid = productArea.Customer_Id });

            var customer = _customerService.GetCustomer(productArea.Customer_Id);
            var model = CreateInputViewModel(productArea, customer);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var productArea = _productAreaService.GetProductArea(id);

            if (_productAreaService.DeleteProductArea(id) == DeleteMessage.Success)
                return RedirectToAction("index", "productarea", new { customerid = productArea.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "productarea", new { customerid = productArea.Customer_Id, id = id });
            }
        }

        private ProductAreaInputViewModel CreateInputViewModel(ProductArea productArea, Customer customer)
        {
            //var wgSelected = productArea.WorkingGroups ?? new List<WorkingGroup>();
            //var wgAvailable = new List<WorkingGroup>();

            //if (productArea.Id != 0)
            //{
            //    foreach (var wg in _workingGroupService.GetWorkingGroups(customer.Id))
            //    {
            //        if (!wgSelected.Contains(wg))
            //            wgAvailable.Add(wg);
            //    }
            //}

            var model = new ProductAreaInputViewModel
            {
                ProductArea = productArea,
                Customer = customer,
                MailTemplates = _mailTemplateService.GetMailTemplates(customer.Id, customer.Language_Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                WorkingGroups = _workingGroupService.GetWorkingGroups(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                Priorities = _priorityService.GetPriorities(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
                
            };

            return model;
        }
    }
}
