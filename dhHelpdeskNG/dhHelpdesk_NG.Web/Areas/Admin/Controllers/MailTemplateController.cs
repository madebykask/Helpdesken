namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class MailTemplateController : BaseController
    {
        private readonly IAccountActivityService _accountActivityService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly ILanguageService _languageService;
        private readonly ICustomerService _customerService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IAccountFieldSettingsService _accountFieldSettingsService;
        private readonly IOrderService _orderService;
        private readonly ISettingService _settingService;       

        public MailTemplateController(
            IAccountActivityService accountActivityService,
            IMailTemplateService mailTemplateService,
            IOrderTypeService orderTypeService,
            ILanguageService languageService,
            ICustomerService customerService,
            ICaseFieldSettingService caseFieldSettingService,
            IAccountFieldSettingsService accountFieldSettingsService,
            IOrderService orderService,
            ISettingService settingSetvice,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._accountActivityService = accountActivityService;
            this._mailTemplateService = mailTemplateService;
            this._orderTypeService = orderTypeService;
            this._languageService = languageService;
            this._customerService = customerService;
            this._accountFieldSettingsService = accountFieldSettingsService;
            this._caseFieldSettingService = caseFieldSettingService;
            this._orderService = orderService;
            this._settingService = settingSetvice;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var customersettings = this._settingService.GetCustomerSetting(customer.Id);

            var model = this.MailTemplateIndexViewModel(customer, customersettings);

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            var mailTemplate = new MailTemplate();

            var existingmailId = this._mailTemplateService.GetNewMailTemplateMailId();

            if (existingmailId > 99)
            {
                mailTemplate = new MailTemplate
                {
                    //Id = id,
                    MailID = existingmailId + 1,
                    Customer_Id = customer.Id,

                };
            }
            
            var mailTemplateLanguage = new MailTemplateLanguage() { Language_Id = customer.Language_Id, MailTemplate = mailTemplate };

            var model = this.CreateInputViewModel(mailTemplateLanguage, customer, customer.Language_Id, null, null);

            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(int id, MailTemplateLanguage mailtemplatelanguage, int languageId, int customerId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (this.ModelState.IsValid)
            {

                var mailTemplate = this._mailTemplateService.GetMailTemplate(id, languageId);

                if (mailTemplate == null)
                {
                    mailTemplate = new MailTemplate
                    {
                        //Id = id,
                        MailID = id,
                        Customer_Id = customerId,

                    };
                }
                var update = true;


                var mailtemplatelanguageToSave = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customerId, languageId);

                //if (mailtemplatelanguageToSave == null)
                //    update = false;

                //mailtemplatelanguageToSave.MailTemplate = mailTemplate;

                //if (mailtemplatelanguageToSave.MailTemplate_Id == 0)
                if (mailtemplatelanguageToSave == null)
                {

                    mailtemplatelanguageToSave = new MailTemplateLanguage
                    {
                        MailTemplate_Id = mailTemplate.Id,
                        Language_Id = mailtemplatelanguage.Language_Id,
                        MailTemplate = mailTemplate,
                        Subject = mailtemplatelanguage.Subject,
                        Body = mailtemplatelanguage.Body,
                        Name = mailtemplatelanguage.Name,
                    };

                    update = false;
                }

                this._mailTemplateService.SaveMailTemplateLanguage(mailtemplatelanguageToSave, update, out errors);

                return this.RedirectToAction("index", "mailtemplate", new { customerId = customerId });

                
            }

            return this.View(mailtemplatelanguage);
        }

        public ActionResult Edit(int id, int customerId, int languageId, int? ordertypeId, int? accountactivityId)
        {
          
            var customer = this._customerService.GetCustomer(customerId);

            var mailTemplate = new MailTemplateEntity();

            mailTemplate = this._mailTemplateService.GetMailTemplate(id, customer.Id);

            if (mailTemplate == null)
            {
                mailTemplate = new MailTemplateEntity
                {
                    //Id = id,
                    MailID = id,
                    
                };
            }
                //return new HttpNotFoundResult("No mail template found...");

            //var mailTemplateLanguage = this._mailTemplateService.GetMailTemplateLanguage(mailTemplate.Id, languageId);
            var mailTemplateLanguage = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customer.Id, languageId);

            if (mailTemplateLanguage == null)
            {
                mailTemplateLanguage = new MailTemplateLanguage
                {
                    MailTemplate = mailTemplate,
                    //MailTemplate_Id = id,
                    Language_Id = languageId,
                    Subject = string.Empty,
                    Body = string.Empty

                };
            }
            else
            {
                mailTemplateLanguage.MailTemplate = mailTemplate;
            }

               // return new HttpNotFoundResult("No mail template found...");

            var model = this.CreateInputViewModel(mailTemplateLanguage, customer, languageId, ordertypeId, accountactivityId);
            return this.View(model);

            
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, MailTemplateLanguageEntity mailTemplateLanguage, int customerId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var customer = this._customerService.GetCustomer(customerId);
            var mailTemplate = this._mailTemplateService.GetMailTemplate(id, customerId);
            
            var customersettings = this._settingService.GetCustomerSetting(customer.Id);

            if (mailTemplate == null)
            {
                mailTemplate = new MailTemplateEntity
                {
                    //Id = id,
                    MailID = id,
                    Customer_Id = customer.Id,
                     
                };
            }
            var update = true;

            var mailtemplatelanguageToSave = this._mailTemplateService.GetMailTemplateLanguageForCustomerToSave(id, customerId, mailTemplateLanguage.Language_Id);

            //if (mailtemplatelanguageToSave == null)
            //    update = false;

            //mailtemplatelanguageToSave.MailTemplate = mailTemplate;

            //if (mailtemplatelanguageToSave.MailTemplate_Id == 0)
            if (mailtemplatelanguageToSave == null)
            {

                mailtemplatelanguageToSave = new MailTemplateLanguage
                {
                    MailTemplate_Id = mailTemplate.Id,
                    Language_Id = mailTemplateLanguage.Language_Id,
                    MailTemplate = mailTemplate,
                    Subject = mailTemplateLanguage.Subject,
                    Body = mailTemplateLanguage.Body
                };

                update = false;
            }
            else
            {
                mailtemplatelanguageToSave.MailTemplate = mailTemplate;
                mailtemplatelanguageToSave.Subject = mailTemplateLanguage.Subject;
                mailtemplatelanguageToSave.Body = mailTemplateLanguage.Body;

            }

            this._mailTemplateService.SaveMailTemplateLanguage(mailtemplatelanguageToSave, update, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "mailtemplate", new { customerId = customer.Id });

            var model = this.MailTemplateIndexViewModel(customer, customersettings);
            return this.View(model);

        }

        [HttpPost]
        public ActionResult Delete(int id, int languageid)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var mailTemplateLanguage = this._mailTemplateService.GetMailTemplateLanguage(id, languageid);

            if (mailTemplateLanguage != null)
            {
                this._mailTemplateService.DeleteMailTemplateLanguage(mailTemplateLanguage, out errors);

            }

            return this.RedirectToAction("index", "mailtemplate", new { area = "admin" });
        }

        private MailTemplateIndexViewModel MailTemplateIndexViewModel(Customer customer, Setting customersettings)
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
                Text = Translation.Get("Tilldelat ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation) + ")",
                Value = "2",

            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelat ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Driftgrupp", Enums.TranslationSource.TextTranslation) + ")",
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
                Text = Translation.Get("Tilldelat ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation) + ")",
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
                Text = Translation.Get("Ändring", Enums.TranslationSource.TextTranslation),
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
            _survey.Add(new SelectListItem()
            {
                Text =Translation.Get("Påminnelse", Enums.TranslationSource.TextTranslation) + " " + Translation.Get("Enkät", Enums.TranslationSource.TextTranslation),
                Value = "16",
            });
            #endregion

            var model = new MailTemplateIndexViewModel
            {
                Customer = customer,
                AccountActivities = this._accountActivityService.GetAccountActivities(customer.Id),
                MailTemplates = this._mailTemplateService.GetMailTemplates(customer.Id, customer.Language_Id),
                OrderTypes = this._orderTypeService.GetOrderTypesForMailTemplate(customer.Id),
                Settings = customersettings,
                ParentOrderTypes = this._orderTypeService.GetParentOrderTypesForMailTemplate(customer.Id).Select(x => new SelectListItem
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

        private MailTemplateInputViewModel CreateInputViewModel(MailTemplateLanguageEntity mailTemplateLanguage, Customer customer, int languageId, int? ordertypeId, int? accountactivityId)
        {
            var model = new MailTemplateInputViewModel
            {
                MailTemplateLanguage = mailTemplateLanguage,
                Customer = customer,
                CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customer.Id, languageId),
                AccountFieldSettings = this._accountFieldSettingsService.GetAccountFieldSettings(customer.Id, accountactivityId),
                OrderFieldSettings = this._orderService.GetOrderFieldSettingsForMailTemplate(customer.Id, ordertypeId),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name, Enums.TranslationSource.TextTranslation),
                    Value = x.Id.ToString()
                }).ToList()

            };

            return model;
        }

        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string UpdateLanguageList(int id, int customerId, int mailTemplateLanguageId, int mailTemplateId, int? accountactivityId, int? ordertypeId, int mailId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            //var mailTemplateLanguageToUpdate = this._mailTemplateService.GetMailTemplateLanguage(mailTemplateLanguageId, id);
           
            var mailTemplate = this._mailTemplateService.GetMailTemplate(mailTemplateId, customerId);

            if (mailTemplate == null)
            {
                mailTemplate = new MailTemplateEntity
                {
                    Id = mailTemplateId,
                    MailID = mailId,
                };
            }

            var mailTemplateLanguageToUpdate = this._mailTemplateService.GetMailTemplateLanguageForCustomer(mailTemplate.MailID, customer.Id, id);
            if (mailTemplateLanguageToUpdate == null)
                mailTemplateLanguageToUpdate = new MailTemplateLanguageEntity
                {
                    
                    MailTemplate_Id = mailTemplate.Id,
                    Language_Id = id,
                    Subject = string.Empty,
                    Body = string.Empty,
                    MailTemplate = mailTemplate
                    
                };
                //mailTemplateLanguageToUpdate = new MailTemplateLanguage() { Language_Id = id, MailTemplate = mailTemplate };


            var mailTemplateLanguage = new MailTemplateLanguageEntity() { Language_Id = id, MailTemplate = mailTemplate };
            
            var model = this.CreateInputViewModel(mailTemplateLanguage, customer, id, ordertypeId, accountactivityId);

            model.MailTemplateLanguage = mailTemplateLanguageToUpdate;
            model.Customer = customer;

            this.UpdateModel(model, "mailTemplateLanguage");

            //return View(model);
            var view = "~/areas/admin/views/MailTemplate/_Input.cshtml";
            return this.RenderRazorViewToString(view, model);
        }
    }
}
