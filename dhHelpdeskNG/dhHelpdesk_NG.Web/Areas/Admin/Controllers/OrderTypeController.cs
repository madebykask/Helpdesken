namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class OrderTypeController : BaseAdminController
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly IDocumentService _documentService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly ICustomerService _customerService;

        public OrderTypeController(
            ICaseTypeService caseTypeService,
            IDocumentService documentService,
            IOrderTypeService orderTypeService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._caseTypeService = caseTypeService;
            this._documentService = documentService;
            this._orderTypeService = orderTypeService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var orderTypes = this._orderTypeService.GetOrderTypes(customer.Id);

            var model = new OrderTypeIndexViewModel { OrderTypes = orderTypes, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (this._orderTypeService.GetOrderType(parentId.Value) == null)
                    return new HttpNotFoundResult("No order type found...");
            }

            var orderType = new OrderType { Customer_Id = customer.Id, Parent_OrderType_Id = parentId, IsActive = 1 };
            var model = this.CreateInputViewModel(orderType, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(OrderType orderType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._orderTypeService.SaveOrderType(orderType, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "ordertype", new { customerId = orderType.Customer_Id });

            var customer = this._customerService.GetCustomer(orderType.Customer_Id);
            var model = this.CreateInputViewModel(orderType, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var orderType = this._orderTypeService.GetOrderType(id);

            if (orderType == null)
                return new HttpNotFoundResult("No order type found...");

            var customer = this._customerService.GetCustomer(orderType.Customer_Id);
            var model = this.CreateInputViewModel(orderType, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrderType orderType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._orderTypeService.SaveOrderType(orderType, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "ordertype", new { customerId = orderType.Customer_Id });

            var customer = this._customerService.GetCustomer(orderType.Customer_Id);
            var model = this.CreateInputViewModel(orderType, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var orderType = this._orderTypeService.GetOrderType(id);

            if (this._orderTypeService.DeleteOrderType(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "ordertype", new { customerId = orderType.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "ordertype", new { area = "admin", id = orderType.Id });
            }





            //check if there is subordertypes
            //var subordertypes = this._orderTypeService.GetSubOrderTypes(orderType.Id);

            //if (this._orderTypeService.DeleteOrderType(id) == DeleteMessage.Success)
            //{
            //    if (subordertypes != null)
            //    {
            //        foreach (var sot in subordertypes)
            //        {
            //            this._orderTypeService.DeleteOrderType(sot.Id);
            //        }
            //    }
            //    return this.RedirectToAction("index", "ordertype", new { customerId = orderType.Customer_Id });
            //}
            //else
            //{
            //    this.TempData.Add("Error", "");
            //    return this.RedirectToAction("edit", "ordertype", new { area = "admin", id = orderType.Id });
            //}
        }

        private OrderTypeInputViewModel CreateInputViewModel(OrderType orderType, Customer customer)
        {
            var caseTypes = this._caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id, true);
            var caseTypesInRow = this._caseTypeService.GetChildrenInRow(caseTypes).ToList();

            var model = new OrderTypeInputViewModel
            {
                OrderType = orderType,
                Customer = customer,
                Documents = this._documentService.GetDocuments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CaseTypes = caseTypesInRow.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                ParentOrderType = this._orderTypeService.GetOrderTypes(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
