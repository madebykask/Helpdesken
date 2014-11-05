namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class ProductAreaController : BaseAdminController
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
            this._caseTypeService = caseTypeService;
            this._mailTemplateService = mailTemplateService;
            this._productAreaService = productAreaService;
            this._workingGroupService = workingGroupService;
            this._customerService = customerService;
            this._priorityService = priorityService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var productAreas = this._productAreaService.GetProductAreas(customer.Id);

            var model = new ProductAreaIndexViewModel { ProductAreas = productAreas, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (this._productAreaService.GetProductArea(parentId.Value) == null)
                    return new HttpNotFoundResult("No parent product area found...");
            }

            var productArea = new ProductArea { Customer_Id = customer.Id, Parent_ProductArea_Id = parentId, IsActive = 1 };
            var model = this.CreateInputViewModel(productArea, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ProductArea productArea, int[] wgSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._productAreaService.SaveProductArea(productArea, wgSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "productarea", new { customerid = productArea.Customer_Id });

            var customer = this._customerService.GetCustomer(productArea.Customer_Id);
            var model = this.CreateInputViewModel(productArea, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var productArea = this._productAreaService.GetProductArea(id);

            if (productArea == null)
                return new HttpNotFoundResult("No product area found...");

            var customer = this._customerService.GetCustomer(productArea.Customer_Id);
            var model = this.CreateInputViewModel(productArea, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, ProductArea productArea, int[] wgSelected)
        {
            var productAreaToSave = this._productAreaService.GetProductArea(id);

            productAreaToSave.Name = productArea.Name;
            productAreaToSave.Description = productArea.Description;
            productAreaToSave.InformUserText = productArea.InformUserText;
            productAreaToSave.WorkingGroup_Id = productArea.WorkingGroup_Id;
            productAreaToSave.Priority_Id = productArea.Priority_Id;
            productAreaToSave.MailID = productArea.MailID;
            productAreaToSave.IsActive = productArea.IsActive;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._productAreaService.SaveProductArea(productAreaToSave, wgSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "productarea", new { customerid = productAreaToSave.Customer_Id });

            var customer = this._customerService.GetCustomer(productArea.Customer_Id);
            var model = this.CreateInputViewModel(productAreaToSave, customer);

            return this.View(model);
        }

        public ActionResult Delete(int id)
        {
            var productArea = this._productAreaService.GetProductArea(id);

            if (this._productAreaService.DeleteProductArea(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "productarea", new { customerid = productArea.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "productarea", new { customerid = productArea.Customer_Id, id = id });
            }
        }

        private ProductAreaInputViewModel CreateInputViewModel(ProductArea productArea, Customer customer)
        {
            var wgSelected = productArea.WorkingGroups ?? new List<WorkingGroupEntity>();
            var wgAvailable = new List<WorkingGroupEntity>();

            //if (productArea.Id != 0)
            //{
            //    foreach (var wg in _workingGroupService.GetWorkingGroups(customer.Id))
            //    {
            //        if (!wgSelected.Contains(wg))
            //            wgAvailable.Add(wg);
            //    }
            //}

            foreach (var wg in _workingGroupService.GetWorkingGroups(customer.Id))
            {
                if (!wgSelected.Contains(wg))
                    wgAvailable.Add(wg);
            }

            var model = new ProductAreaInputViewModel
            {
                ProductArea = productArea,
                Customer = customer,
                MailTemplates = this._mailTemplateService.GetMailTemplates(customer.Id, customer.Language_Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                WorkingGroups = this._workingGroupService.GetWorkingGroups(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                Priorities = this._priorityService.GetPriorities(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                WgAvailable = wgAvailable.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                WgSelected = wgSelected.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList()
                
            };

            return model;
        }
    }
}
