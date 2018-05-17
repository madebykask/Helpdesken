namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class RegistrationSourceCustomerController : BaseAdminController
    {
        private readonly Dictionary<int, string> stdRegistrationSources =
            new Dictionary<int, string>()
                {
                    {
                        (int)CaseRegistrationSource.Administrator,
                        "Manuell registrering"
                    },
                    {
                        (int)CaseRegistrationSource.CaseTemplate,
                        "Ärendemall"
                    },
                    {
                        (int)CaseRegistrationSource.Email,
                        "Mail2Ticket"
                    },
                    {
                        (int)CaseRegistrationSource.SelfService,
                        "Självservice"
                    },
                    {
                        (int)CaseRegistrationSource.Contract,
                        "Avtal"
                    },
                    {
                        (int)CaseRegistrationSource.Order,
                        "Beställning"
                    },
                };

        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly ICustomerService _customerService;


        public RegistrationSourceCustomerController(
            IRegistrationSourceCustomerService registrationSourceCustomerService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._registrationSourceCustomerService = registrationSourceCustomerService;
            this._customerService = customerService;
        }

        public JsonResult SetShowOnlyActiveRegistrationSourceCustomerInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveRegistrationSourceCustomerInAdmin = value;
            return this.Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var registrationsources = this._registrationSourceCustomerService.GetRegistrationSources(customer.Id)
                .Select(it => new RegistrationSourceCustomerIndex() 
                {
                    Id = it.Id,
                    SourceName = it.SourceName,
                    SystemCodeName = this.ResolveSystemCodeName(it.SystemCode),
                    IsActiveStr = (it.IsActive == 1) ? Translation.Get("Ja") : Translation.Get("Nej"),
                    IsActive = it.IsActive
                })
                .ToList();

            var model = new RegistrationSourceIndexViewModel { RegistrationSources = registrationsources, 
                                                               Customer = customer,
                                                               IsShowOnlyActive = SessionFacade.ShowOnlyActiveRegistrationSourceCustomerInAdmin
                                                             };

            return this.View(model);
        }

        private string ResolveSystemCodeName(int? systemCode)
        {
            return systemCode.HasValue && this.stdRegistrationSources.ContainsKey(systemCode.Value)
                       ? this.stdRegistrationSources[systemCode.Value]
                       : string.Empty;
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var registrationsourcecustomer = new RegistrationSourceCustomer { Customer_Id = customer.Id, IsActive = 1 };

            var model = this.CreateInputViewModel(registrationsourcecustomer, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(RegistrationSourceInputViewModel registrationsourcecustomerInputViewModel)
        {
            var registrationsourcecustomer = registrationsourcecustomerInputViewModel.RegistrationSourceCustomer;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._registrationSourceCustomerService.SaveRegistrationSourceCustomer(registrationsourcecustomer, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "registrationsourcecustomer", new { customerId = registrationsourcecustomer.Customer_Id });

            var customer = this._customerService.GetCustomer(registrationsourcecustomer.Customer_Id);
            var model = this.CreateInputViewModel(registrationsourcecustomer, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var registrationsourcecustomer = this._registrationSourceCustomerService.GetRegistrationSouceCustomer(id);

            if (registrationsourcecustomer == null)
                return new HttpNotFoundResult("No system found...");

            var customer = this._customerService.GetCustomer(registrationsourcecustomer.Customer_Id);
            var model = this.CreateInputViewModel(registrationsourcecustomer, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(RegistrationSourceCustomer registrationsourcecustomer)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._registrationSourceCustomerService.SaveRegistrationSourceCustomer(registrationsourcecustomer, out errors);
            if (errors.Count == 0) 
            {
                return this.RedirectToAction("index", "registrationsourcecustomer", new { customerid = registrationsourcecustomer.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(registrationsourcecustomer.Customer_Id);
            var model = this.CreateInputViewModel(registrationsourcecustomer, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var registrationsourcecustomer = this._registrationSourceCustomerService.GetRegistrationSouceCustomer(id);

            if (this._registrationSourceCustomerService.DeleteRegistrationSourceCustomer(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "registrationsourcecustomer", new { customerid = registrationsourcecustomer.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "registrationsourcecustomer", new { area = "admin", id = registrationsourcecustomer.Id });
            }
        }

        private RegistrationSourceInputViewModel CreateInputViewModel(RegistrationSourceCustomer registrationsourcecustomer, Customer customer)
        {
            var li =
                this.stdRegistrationSources.Select(it => new SelectListItem() { Value = it.Key.ToString(), Text = Translation.Get(it.Value), Selected = false })
                    .ToList();
            li.Insert(0, new SelectListItem());
            
            var model = new RegistrationSourceInputViewModel
            {
                RegistrationSourceCustomer = registrationsourcecustomer,
                Customer = customer,
                SystemCode = li
            };

            return model;
        }
    }
}
