using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class MailTemplateController : BaseController
    {
        private readonly IAccountActivityService _accountActivityService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly ILanguageService _languageService;
        private readonly ICustomerService _customerService;

        public MailTemplateController(
            IAccountActivityService accountActivityService,
            IMailTemplateService mailTemplateService,
            IOrderTypeService orderTypeService,
            ILanguageService languageService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _accountActivityService = accountActivityService;
            _mailTemplateService = mailTemplateService;
            _orderTypeService = orderTypeService;
            _languageService = languageService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            var model = MailTemplateIndexViewModel(customer);

            return View(model);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new MailTemplateLanguage(), null, 1);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(MailTemplateLanguage mailtemplatelanguage)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
             
            if (ModelState.IsValid)
            {
               
                _mailTemplateService.SaveMailTemplateLanguage(mailtemplatelanguage, out errors);

                return RedirectToAction("index", "mailtemplate", new { area = "admin" });
            }

            return View(mailtemplatelanguage);
        }

        public ActionResult Edit(int id, int customerId, int languageId)
        {
            var customer = _customerService.GetCustomer(customerId);
            //var mailTemplateLanguage = _mailTemplateService.GetMailTemplateLanguage(id, customer.Id);
            var mailTemplate = _mailTemplateService.GetMailTemplate(id, customer.Id);

            var mailTemplateLanguage = _mailTemplateService.GetMailTemplateLanguage(mailTemplate.Id, languageId);

            if (mailTemplateLanguage == null)
                return new HttpNotFoundResult("No mail template found...");

            var model = CreateInputViewModel(mailTemplateLanguage, customer, languageId);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, MailTemplateLanguage mailTemplateLanguage, int customerId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();


            var customer = _customerService.GetCustomer(customerId);
            var mailTemplate = _mailTemplateService.GetMailTemplate(id, customerId);

            if (mailTemplateLanguage.MailTemplate_Id == 0)
            {

                mailTemplateLanguage = new MailTemplateLanguage { MailTemplate_Id = 0, Language_Id = mailTemplateLanguage.Language_Id, MailTemplate = mailTemplate, Subject = mailTemplateLanguage.Subject, Body = mailTemplateLanguage.Body };
               
             
            }


            _mailTemplateService.SaveMailTemplateLanguage(mailTemplateLanguage, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "mailtemplate", new { customerId = customer.Id });

            var model = MailTemplateIndexViewModel(customer);
            return View(model);




            //var mailTemplateLanguage = _mailTemplateService.GetMailTemplateLanguage(id, languageid);

            //mailTemplateLanguage.Subject = vmodel.MailTemplateLanguage.Subject;
            //mailTemplateLanguage.Body = vmodel.MailTemplateLanguage.Body;

            //if (ModelState.IsValid)
            //{
            //    _mailTemplateService.SaveMailTemplateLanguage(mailTemplateLanguage, out errors);
               

            //    return RedirectToAction("index", "mailtemplate", new { customerId =  });
            //}

            //var model = CreateInputViewModel(mailTemplateLanguage, null, 1);

            //return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int languageid)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var mailTemplateLanguage = _mailTemplateService.GetMailTemplateLanguage(id, languageid);

            if (mailTemplateLanguage != null)
            {
                _mailTemplateService.DeleteMailTemplateLanguage(mailTemplateLanguage, out errors);
               
            }

            return RedirectToAction("index", "mailtemplate", new { area = "admin" });
        }

        private MailTemplateIndexViewModel MailTemplateIndexViewModel(Customer customer)
        {
            #region RegularCase

            List<SelectListItem> _regularCase = new List<SelectListItem>();

            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Nytt ärende", Enums.TranslationSource.TextTranslation),
                Value = "1",
               
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelat ärende (Handläggare)", Enums.TranslationSource.TextTranslation),
                Value = "2",
               
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelat ärende (Driftgrupp)", Enums.TranslationSource.TextTranslation),
                Value = "7",
             
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Ärendet avslutat", Enums.TranslationSource.TextTranslation),
                Value = "3",
              
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Informera anmälaren om åtgärden", Enums.TranslationSource.TextTranslation),
                Value = "4",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Skicka intern loggpost till", Enums.TranslationSource.TextTranslation),
                Value = "5",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Anmälaren uppdaterat ärende", Enums.TranslationSource.TextTranslation),
                Value = "10",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Anmälaren aktiverat ärende", Enums.TranslationSource.TextTranslation),
                Value = "15",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Bevakningsdatum inträffar", Enums.TranslationSource.TextTranslation),
                Value = "9",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Skicka mail när planerat åtgärdsdatum inträffar", Enums.TranslationSource.TextTranslation),
                Value = "12",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Prioritet", Enums.TranslationSource.TextTranslation),
                Value = "13",
            });

            #endregion

            #region CaseSMS

            List<SelectListItem> _caseSMS = new List<SelectListItem>();

            _caseSMS.Add(new SelectListItem()
            {
                Text = Translation.Get("Ärendet avslutat", Enums.TranslationSource.TextTranslation),
                Value = "14",
            });
            _caseSMS.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelat ärende (Handläggare)", Enums.TranslationSource.TextTranslation),
                Value = "11",
            });

            #endregion

            #region Changes

            List<SelectListItem> _changes = new List<SelectListItem>();

            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelad ändring", Enums.TranslationSource.TextTranslation),
                Value = "50",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Skicka loggpost till", Enums.TranslationSource.TextTranslation),
                Value = "51",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("CAB", Enums.TranslationSource.TextTranslation),
                Value = "52",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("PIR", Enums.TranslationSource.TextTranslation),
                Value = "53",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Statusändring", Enums.TranslationSource.TextTranslation),
                Value = "54",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Förändringsråd", Enums.TranslationSource.TextTranslation),
                Value = "55",
            });

            #endregion

            #region OperationLogs

            List<SelectListItem> _operationLogs = new List<SelectListItem>();

            _operationLogs.Add(new SelectListItem()
            {
                Text = Translation.Get("Driftlogg", Enums.TranslationSource.TextTranslation),
                Value = "60",
            });

            #endregion

            #region Survey

            List<SelectListItem> _survey = new List<SelectListItem>();

            _survey.Add(new SelectListItem()
            {
                Text = Translation.Get("Enkät", Enums.TranslationSource.TextTranslation),
                Value = "6",
            });

            #endregion

            var model = new MailTemplateIndexViewModel
            {
                Customer = customer,
                AccountActivities = _accountActivityService.GetAccountActivities(SessionFacade.CurrentCustomer.Id),
                MailTemplates = _mailTemplateService.GetMailTemplates(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage),
                OrderTypes = _orderTypeService.GetOrderTypesForMailTemplate(SessionFacade.CurrentCustomer.Id),
                ParentOrderTypes = _orderTypeService.GetOrderTypesForMailTemplate(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseSMSs = _caseSMS,
                Changes = _changes,
                OperationLogs = _operationLogs,
                RegularCases = _regularCase,
                Surveys = _survey,
            };

            return model;
        }

        private MailTemplateInputViewModel CreateInputViewModel(MailTemplateLanguage mailTemplateLanguage, Customer customer, int languageId)
        {
            var model = new MailTemplateInputViewModel
            {
                MailTemplateLanguage = mailTemplateLanguage,
                Customer = customer,
                MailTemplateIdentifiers = _mailTemplateService.GetMailTemplateIdentifiers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code.ToString()
                }).ToList(),
                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()

            };

            return model;
        }

        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string UpdateLanguageList(int id, int customerId, int mailTemplateLanguageId, int mailTemplateId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var mailTemplateLanguageToUpdate = _mailTemplateService.GetMailTemplateLanguage(mailTemplateLanguageId, id);
            var mailTemplate = _mailTemplateService.GetMailTemplate(mailTemplateId, customerId);

            if (mailTemplateLanguageToUpdate == null)
                mailTemplateLanguageToUpdate = new MailTemplateLanguage() { Language_Id = id, MailTemplate = mailTemplate };

            var mailTemplateLanguage = new MailTemplateLanguage() { };

            var model = CreateInputViewModel(mailTemplateLanguage, customer, id);

            model.MailTemplateLanguage = mailTemplateLanguageToUpdate;
            model.Customer = customer;

            UpdateModel(model, "mailTemplateLanguage");

            //return View(model);
            var view = "~/areas/admin/views/MailTemplate/_Input.cshtml";
            return RenderRazorViewToString(view, model);
        }
    }
}
