namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class OrderStateController : BaseAdminController
    {
        private readonly IOrderStateService _orderStateService;
        private readonly ICustomerService _customerService;

        public OrderStateController(
            IOrderStateService orderStateService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._orderStateService = orderStateService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var orderStates = this._orderStateService.GetOrderStates(customer.Id);

            var model = new OrderStateIndexViewModel { OrderStates = orderStates, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var orderState = new OrderState { Customer_Id = customer.Id, IsActive = 1 };

            var model = new OrderStateInputViewModel { OrderState = orderState, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(OrderState orderState)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._orderStateService.SaveOrderState(orderState, out errors);

            if (errors.Count == 0)               
                return this.RedirectToAction("index", "orderstate", new { customerId = orderState.Customer_Id });

            var customer = this._customerService.GetCustomer(orderState.Customer_Id);
            var model = this.CreateInputViewModel(orderState, customer);
            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var orderState = this._orderStateService.GetOrderState(id);

            if (orderState == null)                
                return new HttpNotFoundResult("No order state found...");

            var customer = this._customerService.GetCustomer(orderState.Customer_Id);
            var model = this.CreateInputViewModel(orderState, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrderState orderState)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._orderStateService.SaveOrderState(orderState, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "orderstate", new { customerId = orderState.Customer_Id });

            var customer = this._customerService.GetCustomer(orderState.Customer_Id);
            var model = this.CreateInputViewModel(orderState, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var orderState = this._orderStateService.GetOrderState(id);

            if (this._orderStateService.DeleteOrderState(id) == DeleteMessage.Success)                
                return this.RedirectToAction("index", "orderstate", new { customerId = orderState.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");                
                return this.RedirectToAction("edit", "orderstate", new { area = "admin", id = orderState.Id });
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
