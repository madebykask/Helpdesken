using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class OrderTypeController : BaseController
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
            _caseTypeService = caseTypeService;
            _documentService = documentService;
            _orderTypeService = orderTypeService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var orderTypes = _orderTypeService.GetOrderTypes(customer.Id);

            var model = new OrderTypeIndexViewModel { OrderTypes = orderTypes, Customer = customer };
            return View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (_orderTypeService.GetOrderType(parentId.Value) == null)
                    return new HttpNotFoundResult("No order type found...");
            }

            var orderType = new OrderType { Customer_Id = customer.Id, Parent_OrderType_Id = parentId, IsActive = 1 };
            var model = CreateInputViewModel(orderType, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(OrderType orderType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _orderTypeService.SaveOrderType(orderType, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "ordertype", new { customerId = orderType.Customer_Id });

            var customer = _customerService.GetCustomer(orderType.Customer_Id);
            var model = CreateInputViewModel(orderType, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var orderType = _orderTypeService.GetOrderType(id);

            if (orderType == null)
                return new HttpNotFoundResult("No order type found...");

            var customer = _customerService.GetCustomer(orderType.Customer_Id);
            var model = CreateInputViewModel(orderType, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrderType orderType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _orderTypeService.SaveOrderType(orderType, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "ordertype", new { customerId = orderType.Customer_Id });

            var customer = _customerService.GetCustomer(orderType.Customer_Id);
            var model = CreateInputViewModel(orderType, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var orderType = _orderTypeService.GetOrderType(id);

            if (_orderTypeService.DeleteOrderType(id) == DeleteMessage.Success)
                return RedirectToAction("index", "ordertype", new { customerId = orderType.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "ordertype", new { area = "admin", id = orderType.Id });
            }
        }

        private OrderTypeInputViewModel CreateInputViewModel(OrderType orderType, Customer customer)
        {
            var model = new OrderTypeInputViewModel
            {
                OrderType = orderType,
                Customer = customer,
                Documents = _documentService.GetDocuments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CaseTypes = _caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                ParentOrderType = _orderTypeService.GetOrderTypes(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
