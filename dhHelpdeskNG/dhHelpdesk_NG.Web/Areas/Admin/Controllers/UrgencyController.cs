using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Infrastructure.Extensions;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class UrgencyController : BaseController
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
            _impactService = impactService;
            _priorityService = priorityService;
            _urgencyService = urgencyService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var urgencies = _urgencyService.GetUrgencies(customer.Id);

            var model = new UrgencyIndexViewModel { Urgencies = urgencies, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var urgency = new Urgency { Customer_Id = customer.Id};

            var model = CreateInputViewModel(urgency, customer);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult New(Urgency urgency, FormCollection formCollection)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _urgencyService.SaveUrgency(urgency, CreatePriorityImpactUrgencyList(formCollection), out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "urgency", new { customerid = urgency.Customer_Id });

            var customer = _customerService.GetCustomer(urgency.Customer_Id);
            var model = CreateInputViewModel(urgency, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var urgency = _urgencyService.GetUrgency(id);

            if (urgency == null)               
                return new HttpNotFoundResult("No urgency found...");

            var customer = _customerService.GetCustomer(urgency.Customer_Id);
            var model = CreateInputViewModel(urgency, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Urgency urgency, FormCollection formCollection)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _urgencyService.SaveUrgency(urgency, CreatePriorityImpactUrgencyList(formCollection), out errors);

            if (errors.Count == 0)               
                return RedirectToAction("index", "urgency", new { customerId = urgency.Customer_Id });

            var customer = _customerService.GetCustomer(urgency.Customer_Id);
            var model = CreateInputViewModel(urgency, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var urgency = _urgencyService.GetUrgency(id);

            if (_urgencyService.DeleteUrgency(id) == DeleteMessage.Success)
                return RedirectToAction("index", "urgency", new { customerId = urgency.Customer_Id });          
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "urgency", new { acustomerId = urgency.Customer_Id, id = id });
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
                    Urgency_Id = priorityImpactUrgency.Key.Substring(12, 1).ToNullableInt32(),
                    Impact_Id = priorityImpactUrgency.Key.Substring(14).ToNullableInt32(),
                    Priority_Id = priorityImpactUrgency.Value.convertStringToInt() 
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
                Impacts = _impactService.GetImpacts(SessionFacade.CurrentCustomer.Id),                
                Priorities = _priorityService.GetPriorities(SessionFacade.CurrentCustomer.Id).Where(x => x.IsActive == 1).ToList(),
                Urgencies = _urgencyService.GetUrgencies(SessionFacade.CurrentCustomer.Id)
            };

            return model;
        }
    }
}
