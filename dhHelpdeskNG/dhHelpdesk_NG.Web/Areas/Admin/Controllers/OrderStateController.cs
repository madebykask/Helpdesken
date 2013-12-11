using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class OrderStateController : BaseController
    {
        private readonly IOrderStateService _orderStateService;
        private readonly ICustomerService _customerService;

        public OrderStateController(
            IOrderStateService orderStateService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _orderStateService = orderStateService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var orderStates = _orderStateService.GetOrderStates(customer.Id);

            var model = new OrderStateIndexViewModel { OrderStates = orderStates, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var orderState = new OrderState { Customer_Id = customer.Id, IsActive = 1 };

            var model = new OrderStateInputViewModel { OrderState = orderState, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(OrderState orderState)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _orderStateService.SaveOrderState(orderState, out errors);

            if (errors.Count == 0)               
                return RedirectToAction("index", "orderstate", new { customerId = orderState.Customer_Id });

            var customer = _customerService.GetCustomer(orderState.Customer_Id);
            var model = CreateInputViewModel(orderState, customer);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var orderState = _orderStateService.GetOrderState(id);

            if (orderState == null)                
                return new HttpNotFoundResult("No order state found...");

            var customer = _customerService.GetCustomer(orderState.Customer_Id);
            var model = CreateInputViewModel(orderState, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrderState orderState)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _orderStateService.SaveOrderState(orderState, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "orderstate", new { customerId = orderState.Customer_Id });

            var customer = _customerService.GetCustomer(orderState.Customer_Id);
            var model = CreateInputViewModel(orderState, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var orderState = _orderStateService.GetOrderState(id);

            if (_orderStateService.DeleteOrderState(id) == DeleteMessage.Success)                
                return RedirectToAction("index", "orderstate", new { customerId = orderState.Customer_Id });
            else
            {
                TempData.Add("Error", "");                
                return RedirectToAction("edit", "orderstate", new { area = "admin", id = orderState.Id });
            }
        }

        private OrderStateInputViewModel CreateInputViewModel(OrderState orderState, Customer customer)
        {
            var model = new OrderStateInputViewModel
            {
                OrderState = orderState,
                Customer = customer
            };

            return model;
        }
    }
}
