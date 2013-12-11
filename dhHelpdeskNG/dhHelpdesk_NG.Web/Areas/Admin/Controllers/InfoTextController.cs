using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class InfoTextController : BaseController
    {
        private readonly IInfoService _infoService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;

        public InfoTextController(
            IInfoService infoService,
            ICustomerService customerService,
            ILanguageService languageService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _infoService = infoService;
            _customerService = customerService;
            _languageService = languageService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var infoTexts = _infoService.GetInfoTexts(customer.Id, customer.Language_Id);

            var model = new InfoTextIndexViewModel { InfoTexts = infoTexts, Customer = customer };
            return View(model);
        }

        public ActionResult Edit(int infoTypeId, int customerId, int languageId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var infoText = _infoService.GetInfoText(infoTypeId, customer.Id, languageId);

            if (infoText == null)
                return new HttpNotFoundResult("No information text found");

            var model = InfoTextInputViewModel(customer);
            model.InfoTextShowViewModel = InfoTextShowViewModel(infoText, customer, languageId);

            return View(model);
            
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(InfoText infoText, int customerId, int languageId, int infoTypeId)
        {

            var customer = _customerService.GetCustomer(customerId);
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (infoText.Id == 0)
            {

                infoText = new InfoText { Customer_Id = customerId, Type = infoTypeId, Id = 0, Language_Id = languageId, Name = infoText.Name };
             
            }

           
            _infoService.SaveInfoText(infoText, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "infotext", new { customerId = customer.Id });

            var model = InfoTextInputViewModel(customer);
            return View(model);
          
            
        }

        private InfoTextInputViewModel CreateInputViewModel(InfoText infoText, Customer customer)
        {
            var model = new InfoTextInputViewModel
            {
                InfoText = infoText,
                Customer = customer,
                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList()
            };

            return model;
        }

        private InfoTextInputViewModel InfoTextInputViewModel(Customer customer)
        {
            if (customer.Id == 0)
            {
                customer.Language_Id = SessionFacade.CurrentLanguage;
            }

            #region Model

            var model = new InfoTextInputViewModel
            {
                InfoTextShowViewModel = new InfoTextShowViewModel(),
                Customer = customer,
                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList()
            };
            #endregion

            return model;
        }

        private InfoTextShowViewModel InfoTextShowViewModel(InfoText infoText, Customer customer, int languageId)
        {
            var model = new InfoTextShowViewModel
            {
                InfoText = infoText,
                Customer = customer
            };

            return model;
        }

        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string UpdateLanguageList(int id, int customerId, int infoTypeId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var infoTextToUpdate = _infoService.GetInfoText(infoTypeId, customer.Id, id);

            //if (infoTextToUpdate == null)
            //    infoTextToUpdate = _caseSettingsService.GetCaseSettings(customer.Id).ToList();

            var infoText = new InfoText() { };

            var model = InfoTextShowViewModel(infoText, customer, id);

            model.InfoText = infoTextToUpdate;
            model.Customer = customer;

            UpdateModel(model, "infoText");

            //return View(model);
            var view = "~/areas/admin/views/Infotext/_InfoTextPartialView.cshtml";
            return RenderRazorViewToString(view, model);
        }

        private InfoTextShowViewModel CreateShowViewModel(InfoText infoText, Customer customer)
        {
            var model = new InfoTextShowViewModel
            {
                InfoText = infoText,
                Customer = customer
            };

            return model;
        }
    }
}
