using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Web.Common.Extensions;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    public class UrgencyController : BaseAdminController
    {
        private readonly IImpactService _impactService;
        private readonly IPriorityService _priorityService;
        private readonly IUrgencyService _urgencyService;
        private readonly ICustomerService _customerService;

        public UrgencyController(
            IImpactService impactService,
            IPriorityService priorityService,
            IUrgencyService urgencyService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._impactService = impactService;
            this._priorityService = priorityService;
            this._urgencyService = urgencyService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var urgencies = this._urgencyService.GetUrgencies(customer.Id);

            var model = new UrgencyIndexViewModel { Urgencies = urgencies, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var urgency = new Urgency { Customer_Id = customer.Id};

            var model = this.CreateInputViewModel(urgency, customer);
            
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Urgency urgency, FormCollection formCollection)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._urgencyService.SaveUrgency(urgency, this.CreatePriorityImpactUrgencyList(formCollection), out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "urgency", new { customerid = urgency.Customer_Id });

            var customer = this._customerService.GetCustomer(urgency.Customer_Id);
            var model = this.CreateInputViewModel(urgency, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var urgency = this._urgencyService.GetUrgency(id);

            if (urgency == null)               
                return new HttpNotFoundResult("No urgency found...");

            var customer = this._customerService.GetCustomer(urgency.Customer_Id);
            var model = this.CreateInputViewModel(urgency, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Urgency urgency, FormCollection formCollection)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._urgencyService.SaveUrgency(urgency, this.CreatePriorityImpactUrgencyList(formCollection), out errors);

            if (errors.Count == 0)               
                return this.RedirectToAction("index", "urgency", new { customerId = urgency.Customer_Id });

            var customer = this._customerService.GetCustomer(urgency.Customer_Id);
            var model = this.CreateInputViewModel(urgency, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var urgency = this._urgencyService.GetUrgency(id);

            if (this._urgencyService.DeleteUrgency(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "urgency", new { customerId = urgency.Customer_Id });          
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "urgency", new { acustomerId = urgency.Customer_Id, id = id });
            }
        }

        private IList<PriorityImpactUrgency> CreatePriorityImpactUrgencyList(FormCollection formCollection)
        {
            var priorityImpactUrgencyList = new List<PriorityImpactUrgency>();
           
            foreach (var priorityImpactUrgency in formCollection.AllKeys.Where(k => k.StartsWith("lstPriority_")).ToDictionary(k => k, k => formCollection[k]))
            {
                if (priorityImpactUrgency.Value == string.Empty)
                    continue;

                

                priorityImpactUrgencyList.Add(new PriorityImpactUrgency
                {
                    Urgency_Id = priorityImpactUrgency.Key.Substring(12, priorityImpactUrgency.Key.IndexOf("_", 12)-12).ToNullableInt32(),
                    Impact_Id = priorityImpactUrgency.Key.Substring(priorityImpactUrgency.Key.IndexOf("_", 12)+1).ToNullableInt32(),
                    Priority_Id = priorityImpactUrgency.Value.ConvertStringToInt() 
                });
            }

            return priorityImpactUrgencyList;
        }

        private UrgencyInputViewModel CreateInputViewModel(Urgency urgency, Customer customer)
        {
            var model = new UrgencyInputViewModel
            {
                Urgency = urgency,
                Customer = customer,
                Impacts = this._impactService.GetImpacts(customer.Id),                
                Priorities = this._priorityService.GetPriorities(customer.Id).Where(x => x.IsActive == 1).ToList(),
                Urgencies = this._urgencyService.GetUrgencies(customer.Id)
            };

            return model;
        }
    }
}
