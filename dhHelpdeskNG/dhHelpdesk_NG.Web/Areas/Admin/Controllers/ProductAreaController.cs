using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Web.Infrastructure.Helpers;

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

        public JsonResult SetShowOnlyActiveProductAreasInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveProductAreasInAdmin = value;
            return this.Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var productAreas = this._productAreaService.GetAllProductAreas(customer.Id).OrderBy(x => x.Name).ToList();

            var model = new ProductAreaIndexViewModel { ProductAreas = productAreas, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActiveProductAreasInAdmin };

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

            var productArea = new ProductArea { Customer_Id = customer.Id, Parent_ProductArea_Id = parentId, IsActive = 1, ShowOnExternalPage = 1 };
            var model = this.CreateInputViewModel(productArea, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ProductArea productArea, int[] wgSelected, int? caseType_Id)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._productAreaService.SaveProductArea(productArea, wgSelected, caseType_Id, out errors);

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
        public ActionResult Edit(int id, ProductArea productArea, int[] wgSelected, int? caseType_Id)
        {
            var productAreaToSave = this._productAreaService.GetProductArea(id);

            productAreaToSave.Name = productArea.Name;
            productAreaToSave.Description = productArea.Description;
            productAreaToSave.InformUserText = productArea.InformUserText;
            productAreaToSave.WorkingGroup_Id = productArea.WorkingGroup_Id;
            productAreaToSave.Priority_Id = productArea.Priority_Id;
            productAreaToSave.MailID = productArea.MailID;
            productAreaToSave.IsActive = productArea.IsActive;
            productAreaToSave.ShowOnExternalPage = productArea.ShowOnExternalPage;
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._productAreaService.SaveProductArea(productAreaToSave, wgSelected, caseType_Id, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "productarea", new { customerid = productAreaToSave.Customer_Id });

            var customer = this._customerService.GetCustomer(productArea.Customer_Id);
            var model = this.CreateInputViewModel(productAreaToSave, customer);
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, Translation.Get(error.Value));
            }
            model.ProductArea.IsActive = 0;
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
            const int MAX_LEVEL_DEEP = 6;
            var wgSelected = productArea.WorkingGroups ?? new List<WorkingGroupEntity>();
            var wgAvailable = new List<WorkingGroupEntity>();

            foreach (var wg in _workingGroupService.GetWorkingGroups(customer.Id))
            {
                if (!wgSelected.Contains(wg))
                    wgAvailable.Add(wg);
            }
            var pa = productArea;
            var level = 1;
            while (pa.ParentProductArea != null)
            {
                level++;
                pa = pa.ParentProductArea;
            }

            // Get active mail templates 
            var customMailTemplates = _mailTemplateService.GetCustomMailTemplatesFull(customer.Id).ToList();
            var activeMailTemplates = customMailTemplates.Where(m => m.TemplateLanguages
                                                                    .Where(tl => !string.IsNullOrEmpty(tl.Subject) && !string.IsNullOrEmpty(tl.Body)).Any() &&
                                                                    m.MailId >= 100
                                                               )
                                                         .ToList();
                                                      
            var mailTemplateView = new List<SelectListItem>();
            foreach (var mailtemplate in activeMailTemplates)
            {
                var templateId = mailtemplate.MailTemplateId;
                var templateName = string.Empty;
                var activeLanguages = mailtemplate.TemplateLanguages.Where(l=> !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body)).ToList();
                if (activeLanguages.Select(l => l.LanguageId).Contains(SessionFacade.CurrentLanguageId))
                    templateName = activeLanguages.Where(l => l.LanguageId == SessionFacade.CurrentLanguageId).First().TemplateName;
                else
                    templateName = activeLanguages.First().TemplateName;

                mailTemplateView.Add(new SelectListItem { Value = templateId.ToString(), Text = templateName });
            }
            CaseTypeProductArea connectedCaseType = null;
            if (productArea.Id > 0)
            {
                connectedCaseType = productArea.CaseTypeProductAreas?.FirstOrDefault(x => x.ProductArea_Id == productArea.Id);
            }

            var model = new ProductAreaInputViewModel
            {
                ProductArea = productArea,
                Customer = customer,
                MailTemplates = mailTemplateView,                
                WorkingGroups = this._workingGroupService.GetWorkingGroups(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                Priorities = this._priorityService.GetPriorities(customer.Id).Where(x => x.IsActive == 1).Select(x => new SelectListItem
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
                }).ToList(),
                CanAddChild = level < MAX_LEVEL_DEEP,
                CaseType_Id = connectedCaseType?.CaseType_Id ?? 0,
                CaseTypes = _caseTypeService.GetCaseTypes(customer.Id, true),
                ParentPath_CaseType = "--"
            };
            if ( model.CaseType_Id > 0)
            {
                var c = _caseTypeService.GetCaseType(model.CaseType_Id);
                if (c != null)
                {
                    c = Translation.TranslateCaseType(c);
                    model.ParentPath_CaseType = c.getCaseTypeParentPath();
                }
            }

            return model;
        }
    }
}
