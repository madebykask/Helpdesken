using System;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Enums.MailTemplates;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services.Orders;
using DH.Helpdesk.Web.Common.Constants.Case;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models.Questionnaire.Output;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.BusinessData.Models;

    public class MailTemplateController : BaseAdminController
    {
        private readonly IAccountActivityService _accountActivityService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly ILanguageService _languageService;
        private readonly ICustomerService _customerService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IAccountFieldSettingsService _accountFieldSettingsService;
        private readonly IOrdersService _orderService;
        private readonly ISettingService _settingService;
        private readonly IFeedbackService _feedbackService;

        public MailTemplateController(
            IAccountActivityService accountActivityService,
            IMailTemplateService mailTemplateService,
            IOrderTypeService orderTypeService,
            ILanguageService languageService,
            ICustomerService customerService,
            ICaseFieldSettingService caseFieldSettingService,
            IAccountFieldSettingsService accountFieldSettingsService,
            IOrdersService orderService,
            ISettingService settingSetvice,
            IFeedbackService feedbackService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _accountActivityService = accountActivityService;
            _mailTemplateService = mailTemplateService;
            _orderTypeService = orderTypeService;
            _languageService = languageService;
            _customerService = customerService;
            _accountFieldSettingsService = accountFieldSettingsService;
            _caseFieldSettingService = caseFieldSettingService;
            _orderService = orderService;
            _settingService = settingSetvice;
            _feedbackService = feedbackService;
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

            var mailTemplate = new MailTemplateEntity();

            var existingmailId = this._mailTemplateService.GetNewMailTemplateMailId();

            if (existingmailId >= MailTemplate.UserTemplatesMinID)
            {
                mailTemplate = new MailTemplateEntity { MailID = existingmailId + 1, Customer_Id = customer.Id, };
            }
            else
            {
                mailTemplate = new MailTemplateEntity { MailID = MailTemplate.UserTemplatesMinID, Customer_Id = customer.Id, };
            }
            
            var mailTemplateLanguage = new MailTemplateLanguageEntity() { Language_Id = SessionFacade.CurrentLanguageId, MailTemplate = mailTemplate };

            var model = this.CreateInputViewModel(mailTemplateLanguage, customer, customer.Language_Id, null, null);

            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(int id, MailTemplateLanguageEntity mailtemplatelanguage, int languageId, int customerId)
        {
            var mailTemplateId = id;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (this.ModelState.IsValid)
            {

                var mailTemplate = this._mailTemplateService.GetMailTemplate(id, languageId);

                if (mailTemplate == null)
                {
                    mailTemplate = new MailTemplateEntity
                    {
                        MailID = id,
                        Customer_Id = customerId
                    };
                }

                mailTemplate.SendMethod =
                    mailtemplatelanguage?.MailTemplate?.SendMethod ?? EmailSendMethod.SeparateEmails;
                var update = true;
                
                var mailtemplatelanguageToSave = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(customerId, languageId, mailTemplateId);
               
                if (mailtemplatelanguageToSave == null)
                {
                    mailtemplatelanguageToSave = new MailTemplateLanguageEntity
                    {
                        MailTemplate_Id = mailTemplate.Id,
                        Language_Id = mailtemplatelanguage.Language_Id,
                        MailTemplate = mailTemplate,
                        Subject = mailtemplatelanguage.Subject,
                        Body = mailtemplatelanguage.Body,
                        MailTemplateName = mailtemplatelanguage.MailTemplateName,
                    };

                    update = false;
                }

                this._mailTemplateService.SaveMailTemplateLanguage(mailtemplatelanguageToSave, update, out errors);

                return this.RedirectToAction("index", "mailtemplate", new { customerId = customerId });
            }

            var customer = this._customerService.GetCustomer(customerId);
            var model = this.CreateInputViewModel(mailtemplatelanguage, customer, customer.Language_Id, null, null);
            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId, int languageId, int? ordertypeId, int? accountactivityId)
        {            
            var customer = this._customerService.GetCustomer(customerId);

            var mailTemplate = ordertypeId != null ? 
                this._mailTemplateService.GetMailTemplate(id, customer.Id, ordertypeId.Value) : 
                this._mailTemplateService.GetMailTemplate(id, customer.Id);

            if (mailTemplate == null)
            {
                mailTemplate = new MailTemplateEntity
                {
                    MailID = id
                };
            }

            // Get first active language for Template
            var customMailTemplate = this._mailTemplateService.GetCustomMailTemplate(mailTemplate.Id);
            if (customMailTemplate != null)
            {
                var activeLanguages = customMailTemplate.TemplateLanguages
                                                        .Where(l=> !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body))
                                                        .Select(l => l.LanguageId)
                                                        .ToList();
                if (activeLanguages.Any())
                {
                    if (!activeLanguages.Contains(languageId))
                        languageId = activeLanguages.First();
                }
            }

            var mailTemplateLanguage = ordertypeId != null ?
                this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(customer.Id, languageId, id, ordertypeId.Value) :
                this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(customer.Id, languageId, id);

            if (mailTemplateLanguage == null)
            {
                mailTemplateLanguage = new MailTemplateLanguageEntity
                {
                    MailTemplate = mailTemplate,
                    Language_Id = languageId,
                    Subject = string.Empty,
                    Body = string.Empty
                };
            }
            else
            {
                mailTemplateLanguage.MailTemplate = mailTemplate;
            }

            var model = this.CreateInputViewModel(mailTemplateLanguage, customer, languageId, ordertypeId, accountactivityId);

            if (model.MailTemplateLanguage.MailTemplate.MailID == (int)GlobalEnums.MailTemplates.ClosedCase)
            {
                SetFeedbacks();
            }

            // 1 to 99  System case template
            if (id <= 99)
                model.IsStandardTemplate = true;
            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, MailTemplateLanguageEntity mailTemplateLanguage, int customerId, int? ordertypeId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var customer = this._customerService.GetCustomer(customerId);

            var mailTemplate = ordertypeId != null ?
                this._mailTemplateService.GetMailTemplate(id, customer.Id, ordertypeId.Value) :
                this._mailTemplateService.GetMailTemplate(id, customer.Id);

            var customersettings = this._settingService.GetCustomerSetting(customer.Id);

            if (mailTemplate == null)
            {
                mailTemplate = new MailTemplateEntity
                {
                    //Id = id,
                    MailID = id,
                    Customer_Id = customer.Id,
                    OrderType_Id = ordertypeId
                };
            }

            mailTemplate.SendMethod = mailTemplateLanguage?.MailTemplate?.SendMethod ?? EmailSendMethod.SeparateEmails;
            mailTemplate.IncludeLogText_External = mailTemplateLanguage?.MailTemplate?.IncludeLogText_External ?? false;  
            var update = true;

            var mailtemplatelanguageToSave = ordertypeId != null ?
                this._mailTemplateService.GetMailTemplateLanguageForCustomerToSave(id, customerId, mailTemplateLanguage.Language_Id, ordertypeId.Value) :
                this._mailTemplateService.GetMailTemplateLanguageForCustomerToSave(id, customerId, mailTemplateLanguage.Language_Id);

            if (mailtemplatelanguageToSave == null)
            {
                mailtemplatelanguageToSave = new MailTemplateLanguageEntity
                {
                    MailTemplate_Id = mailTemplate.Id,
                    Language_Id = mailTemplateLanguage.Language_Id,
                    MailTemplate = mailTemplate,
                    Subject = mailTemplateLanguage.Subject,
                    Body = mailTemplateLanguage.Body,
                    MailTemplateName = mailTemplateLanguage.MailTemplateName
                };
                update = false;
            }
            else
            {
                mailtemplatelanguageToSave.MailTemplate = mailTemplate;
                mailtemplatelanguageToSave.Subject = mailTemplateLanguage.Subject;
                mailtemplatelanguageToSave.Body = mailTemplateLanguage.Body;
                if (mailTemplateLanguage.MailTemplateName != null)
                    mailtemplatelanguageToSave.MailTemplateName = mailTemplateLanguage.MailTemplateName;
            }
            //Textarea maxlenth does not work as expected, this is to check the actual characterlengt 
            //int bodyLength = mailTemplateLanguage.Body.Length;
            this._mailTemplateService.SaveMailTemplateLanguage(mailtemplatelanguageToSave, update, out errors);
            
            if (errors.Count == 0)
                // return this.RedirectToAction("edit", "mailtemplate", new { customerId = customer.Id, id = id, languageId = mailTemplateLanguage.Language_Id });
                return this.RedirectToAction("index", "mailtemplate", new { customerId = customerId });


            var model = this.MailTemplateIndexViewModel(customer, customersettings);//TODO: should be CreateInputViewModel instead of MailTemplateIndexViewModel
            //if (model.MailTemplateLanguage.MailTemplate.MailID == (int)GlobalEnums.MailTemplates.ClosedCase)
            //{
            //	SetFeedbacks();
            //}
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int languageid, int customerId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var mailTemplateLanguage = this._mailTemplateService.GetMailTemplateLanguage(id, languageid);

            if (mailTemplateLanguage != null)
            {
                this._mailTemplateService.DeleteMailTemplateLanguage(mailTemplateLanguage, out errors);

            }

            return this.RedirectToAction("index", "mailtemplate", new { customerId = customerId });
        }

        public MvcHtmlString MailTemplateFieldIdentifierRow(int customerId, string mailTemplateRowName, string extraLabel, string eMailIdentifier)
        {
            var fieldName = mailTemplateRowName.getCaseFieldName();
            var cfs = _caseFieldSettingService.GetCaseFieldSettingsByName(customerId, fieldName).FirstOrDefault(x => x.ShowOnStartPage == 1);
            
            if (cfs != null)
            {
                var cfsl = _caseFieldSettingService.GetCaseFieldSettingLanguage(cfs.Id, SessionFacade.CurrentLanguageId);
                if (cfsl != null)
                {
                    if (extraLabel != null)
                    {
                        extraLabel = Translation.Get(extraLabel, Enums.TranslationSource.TextTranslation, customerId);
                        extraLabel = ":" + extraLabel;
                    }
                    if (string.IsNullOrEmpty(cfsl.Label))
                    {
                        cfsl.Label = Translation.Get(mailTemplateRowName, Enums.TranslationSource.CaseTranslation, customerId);

                        if (cfsl.Label ==  "tblLog.Text_External")
                        {
                            cfsl.Label = Translation.GetCoreTextTranslation("Extern notering");
                        }
                        else if (cfsl.Label == "tblLog.Text_Internal")
                        {
                            cfsl.Label = Translation.GetCoreTextTranslation("Intern notering");
                        }
                    }

                    var emailIdentifier = cfs.EMailIdentifier;
                    if (eMailIdentifier != null)
                    {
                        emailIdentifier = eMailIdentifier;
                    }

                    string row = "";
                    row = "<tr>"
                        + "<td>"
                        + cfsl.Label
                        + extraLabel
                        + "</td>"
                        + "<td>"
                        + emailIdentifier
                        + "</td>"
                        + "</tr>";
                    var html = new MvcHtmlString(row);
                    return html;
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }



        //commented out because: redmine case #10053
        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string UpdateLanguageList(int id, int customerId, int mailTemplateLanguageId, int mailTemplateId, int? accountactivityId, int? ordertypeId, int mailId, string mailTemplateName)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var mailTemplate = new MailTemplateEntity();

            if (ordertypeId != null)
                // Search by OrderTypeId
                mailTemplate = this._mailTemplateService.GetMailTemplate(id, customer.Id, ordertypeId.Value);
            else
                // Search by MailID                
                mailTemplate = this._mailTemplateService.GetMailTemplate(id, customer.Id);             

            if (mailTemplate == null)
            {
                mailTemplate = new MailTemplateEntity
                {
                    Id = mailTemplateId,
                    MailID = mailId,
                };
            }

            var mailTemplateLanguageToUpdate = new MailTemplateLanguageEntity();
            if (ordertypeId != null)
                // Search by OrderTypeId
                mailTemplateLanguageToUpdate = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(customer.Id, mailTemplateLanguageId, id, ordertypeId.Value);
            else
                // Search by MailID                
                mailTemplateLanguageToUpdate = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(customer.Id, mailTemplateLanguageId, id);


            //var mailTemplateLanguageToUpdate = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customer.Id, mailTemplateLanguageId);

            if (mailTemplateLanguageToUpdate == null)
                mailTemplateLanguageToUpdate = new MailTemplateLanguageEntity
                {

                    MailTemplate_Id = mailTemplate.Id,
                    Language_Id = mailTemplateLanguageId,
                    Subject = string.Empty,
                    Body = string.Empty,
                    MailTemplate = mailTemplate,
                    MailTemplateName = mailTemplateName
                };            


            var mailTemplateLanguage = new MailTemplateLanguageEntity() 
                { Language_Id = mailTemplateLanguageId, 
                  MailTemplate = mailTemplate 
                };

   
            var model = this.CreateInputViewModel(mailTemplateLanguage, customer, mailTemplateLanguageId, ordertypeId, accountactivityId);

            model.MailTemplateLanguage = mailTemplateLanguageToUpdate;
            model.Customer = customer;

            this.UpdateModel(model, "mailTemplateLanguage");

            //return View(model);
            var view = "~/areas/admin/views/MailTemplate/_Input.cshtml";
            if (model.MailTemplateLanguage.MailTemplate.MailID == (int)GlobalEnums.MailTemplates.ClosedCase)
            {
                SetFeedbacks();
            }
            return this.RenderRazorViewToString(view, model);
        }

        #region Private 
        private MailTemplateIndexViewModel MailTemplateIndexViewModel(Customer customer, Setting customersettings)
        {
            #region RegularCase

            List<SelectListItem> _regularCase = new List<SelectListItem>();

            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Nytt ärende") + " (" + Translation.Get("Anmälare") + ")"
                       + " (" + Translation.Get("Grunddata") + ")",
                Value = "1",

            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelat ärende") + " (" + Translation.Get("Handläggare") + ")",
                Value = "2",

            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelat ärende") + " (" + Translation.Get("Driftgrupp") + ")",
                Value = "7",

            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Ärendet avslutat") + " (" + Translation.Get("Anmälare") + ")" 
                       + " (" + Translation.Get("Grunddata") + ")",
                Value = "3",

            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Sammanfogat ärende") + " (" + Translation.Get("Anmälare") + ")",
                Value = "18",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Informera anmälaren om åtgärden") + " (" + Translation.Get("Anmälare") + ")",
                Value = "4",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Skicka intern loggpost till"),
                Value = "5",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Anmälaren uppdaterat ärende") + " (" + Translation.Get("Handläggare") + ")",
                Value = "10",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Anmälaren aktiverat ärende") + " (" + Translation.Get("Handläggare") + ")",
                Value = "15",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Bevakningsdatum inträffar") + " (" + Translation.Get("Handläggare") + ")",
                Value = "9",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Skicka mail när planerat åtgärdsdatum inträffar") + " (" + Translation.Get("Handläggare") + ")",
                Value = "12",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Prioritet") + " (" + Translation.Get("Grunddata") + ")",
                Value = "13",
            });
            _regularCase.Add(new SelectListItem()
            {
                Text = Translation.Get("Påminnelse") + " (" + Translation.Get("Anmälare") + ")",
                Value = "17",
            });

            #endregion

            #region CaseSMS

            List<SelectListItem> _caseSMS = new List<SelectListItem>();

            _caseSMS.Add(new SelectListItem()
            {
                Text = Translation.Get("Ärendet avslutat"),
                Value = "14",
            });
            _caseSMS.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelat ärende") + " (" + Translation.Get("Handläggare") + ")",
                Value = "11",
            });

            #endregion

            #region Changes

            List<SelectListItem> _changes = new List<SelectListItem>();

            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Tilldelad ändring"),
                Value = "50",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Skicka loggpost till"),
                Value = "51",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("CAB"),
                Value = "52",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("PIR"),
                Value = "53",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Statusändring"),
                Value = "54",
            });
            _changes.Add(new SelectListItem()
            {
                Text = Translation.Get("Ändring"),
                Value = "55",
            });

            #endregion

            #region OperationLogs

            List<SelectListItem> _operationLogs = new List<SelectListItem>();

            _operationLogs.Add(new SelectListItem()
            {
                Text = Translation.Get("Driftlogg"),
                Value = "60",
            });

            #endregion

            #region Survey

            List<SelectListItem> _survey = new List<SelectListItem>();

            _survey.Add(new SelectListItem()
            {
                Text = Translation.Get("Enkät"),
                Value = "6",
            });
            _survey.Add(new SelectListItem()
            {
                Text = Translation.Get("Påminnelse") + " " + Translation.Get("Enkät"),
                Value = "16",
            });
            #endregion

            var mailTemplates = new List<MailTemplateList>();
            var languages = _languageService.GetActiveLanguages();
            foreach (var lang in languages)
            {
                mailTemplates.AddRange(this._mailTemplateService
                                           .GetMailTemplates(customer.Id, lang.Id)
                                           .Where(x => !mailTemplates.Select(m => m.MailID)
                                                                    .Contains(x.MailID))
                                           .ToList());
            }


            var customMailTemplates = _mailTemplateService.GetCustomMailTemplatesFull(customer.Id);

            var activeMailTemplateLanguages = new List<ActiveMailTemplateLanguage>();
            foreach (var customMailTemplate in customMailTemplates)
            {
                var languageNames = customMailTemplate.TemplateLanguages
                                                      .Where(l => !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body))
                                                      .Select(l => Translation.Get(l.Language.Name))
                                                      .ToList();

                var activeMailTemplateLanguage =
                    new ActiveMailTemplateLanguage()
                    {
                        Id = customMailTemplate.MailId,
                        LanguageNames = string.Join(", ", languageNames)
                    };

                activeMailTemplateLanguages.Add(activeMailTemplateLanguage);
            }

            var activeOrderMailTemplateLanguages = new List<ActiveMailTemplateLanguage>();
            foreach (var customMailTemplate in customMailTemplates.Where(x => x.OrderTypeId != null))
            {
                var languageNames = customMailTemplate.TemplateLanguages
                                                      .Where(l => !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body))
                                                      .Select(l => Translation.Get(l.Language.Name))
                                                      .ToList();

                var activeOrderMailTemplateLanguage =
                    new ActiveMailTemplateLanguage()
                    {
                        Id = customMailTemplate.OrderTypeId.Value,
                        LanguageNames = string.Join(", ", languageNames)
                    };

                activeOrderMailTemplateLanguages.Add(activeOrderMailTemplateLanguage);
            }

            // *TODO: ViewModel should be change. shouldn't pass Entity to the view
            var model = new MailTemplateIndexViewModel
            {
                Customer = customer,
                AccountActivities = this._accountActivityService.GetAccountActivities(customer.Id),
                MailTemplates = mailTemplates,
                OrderTypes = this._orderTypeService.GetOrderTypesForMailTemplate(customer.Id),
                Settings = customersettings,
                ParentOrderTypes = this._orderTypeService.GetParentOrderTypesForMailTemplateIndexPage(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseSMSs = _caseSMS,
                Changes = _changes,
                OperationLogs = _operationLogs,
                RegularCases = _regularCase,
                Surveys = _survey,
                ActiveMailTemplateLanguages = activeMailTemplateLanguages,
                ActiveOrderMailTemplateLanguages = activeOrderMailTemplateLanguages
            };

            return model;
        }

        private MailTemplateInputViewModel CreateInputViewModel(MailTemplateLanguageEntity mailTemplateLanguage, Customer customer, int languageId, int? ordertypeId, int? accountactivityId)
        {
            var model = new MailTemplateInputViewModel
            {
                IsStandardTemplate = false,
                MailTemplateLanguage = mailTemplateLanguage,
                Customer = customer,
                CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customer.Id, SessionFacade.CurrentLanguageId),
                AccountFieldSettings = this._accountFieldSettingsService.GetAccountFieldSettings(customer.Id, accountactivityId),
                OrderFieldSettings = this._orderService.GetOrderFieldSettingsForMailTemplate(customer.Id, ordertypeId),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation(x.Name),
                    Value = x.Id.ToString(),
                    Selected = (x.Id == languageId)
                }).ToList(),
                SendMethods = Enum.GetValues(typeof(EmailSendMethod)).Cast<EmailSendMethod>().ToList()
                    .Select(x => new SelectListItem
                {
                    Text = x.Translate(),
                    Value = ((int)x).ToString(),
                }).ToList()
            };

            return model;
        }

        private void SetFeedbacks()
        {
            var feedbacks = _feedbackService.FindFeedbackOverviews(SessionFacade.CurrentCustomer.Id);
            ViewBag.Feedbacks = feedbacks.Any()
                ? feedbacks.Select(f => new FeedbackOverviewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Identifier = FeedbackTemplate.FeedbackIdentifierPredicate + f.Identifier,
                    Description = f.Description
                }).ToList()
                : new List<FeedbackOverviewModel>();
        }
        #endregion
    }
}
