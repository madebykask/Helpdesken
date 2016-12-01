﻿using DH.Helpdesk.BusinessData.Enums.MailTemplates;
using DH.Helpdesk.BusinessData.OldComponents;
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
        private readonly IOrderService _orderService;
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
            IOrderService orderService,
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
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (this.ModelState.IsValid)
            {

                var mailTemplate = this._mailTemplateService.GetMailTemplate(id, languageId);

                if (mailTemplate == null)
                {
                    mailTemplate = new MailTemplateEntity
                    {                        
                        MailID = id,
                        Customer_Id = customerId,
                    };
                }
                var update = true;


                var mailtemplatelanguageToSave = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customerId, languageId);
               
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

            return this.View(mailtemplatelanguage);
        }

        public ActionResult Edit(int id, int customerId, int languageId, int? ordertypeId, int? accountactivityId)
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
                    MailID = id,
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

            var mailTemplateLanguage = new MailTemplateLanguageEntity();
            if (ordertypeId != null)
                // Search by OrderTypeId
                mailTemplateLanguage = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customer.Id, languageId, ordertypeId.Value);
            else
                // Search by MailID                
                mailTemplateLanguage = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customer.Id, languageId);
            

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

            var mailTemplate = new MailTemplateEntity();

            if (ordertypeId != null)
                // Search by OrderTypeId
                mailTemplate = this._mailTemplateService.GetMailTemplate(id, customer.Id, ordertypeId.Value);
            else
                // Search by MailID                
                mailTemplate = this._mailTemplateService.GetMailTemplate(id, customer.Id);               

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
            var update = true;

            var mailtemplatelanguageToSave = new MailTemplateLanguageEntity();
            if (ordertypeId != null)
                // Search by OrderTypeId
                mailtemplatelanguageToSave =
                    this._mailTemplateService.GetMailTemplateLanguageForCustomerToSave(id, customerId, mailTemplateLanguage.Language_Id, ordertypeId.Value);
            else
                // Search by MailID   
                mailtemplatelanguageToSave = 
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

		public MvcHtmlString MailTemplateFieldIdentifierRow(int customerId, string mailTemplateRowName, string ExtraLabel, string EMailIdentifier)
        {
            //Super quick fix TODO: FIX... Im sorry
            if (mailTemplateRowName == "tblLog_Text_External")
            {
                mailTemplateRowName = "tblLog.Text_External";
            }
            else if (mailTemplateRowName == "tblLog_Text_Internal")
            {
                mailTemplateRowName = "tblLog.Text_Internal";
            }

            var cfs = this._caseFieldSettingService.GetAllCaseFieldSettings().Where(x => x.Name == mailTemplateRowName && x.Customer_Id == customerId && x.ShowOnStartPage == 1);
            
            if (cfs.Any())
            {
                var cfsl = this._caseFieldSettingService.GetCaseFieldSettingLanguage(cfs.FirstOrDefault().Id, SessionFacade.CurrentLanguageId);
                if (!(cfsl == null))
                {
                    if (ExtraLabel != null)
                    {
                        ExtraLabel = Translation.Get(ExtraLabel, Enums.TranslationSource.TextTranslation, customerId);
                        ExtraLabel = ":" + ExtraLabel;
                    }
                    if (cfsl.Label == null || cfsl.Label == "")
                    {
                        cfsl.Label = Translation.Get(mailTemplateRowName, Enums.TranslationSource.CaseTranslation, customerId);

                        if (cfsl.Label == "tblLog.Text_External")
                        {
                            cfsl.Label = Translation.Get("Extern notering", Enums.TranslationSource.TextTranslation);
                        }
                        else if (cfsl.Label == "tblLog.Text_Internal")
                        {
                            cfsl.Label = Translation.Get("Intern notering", Enums.TranslationSource.TextTranslation);
                        }
                    }

                    var emailIdentifier = cfs.FirstOrDefault().EMailIdentifier;
                    if (EMailIdentifier != null)
                    {
                        emailIdentifier = EMailIdentifier;
                    }

                    string row = "";
                    row = "<tr>"
                        + "<td>"
                        + cfsl.Label
                        + ExtraLabel
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
                mailTemplateLanguageToUpdate = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customer.Id, mailTemplateLanguageId, ordertypeId.Value);
            else
                // Search by MailID                
                mailTemplateLanguageToUpdate = this._mailTemplateService.GetMailTemplateLanguageForCustomer(id, customer.Id, mailTemplateLanguageId);


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
            return this.RenderRazorViewToString(view, model);
        }

		#region Private 
		private MailTemplateIndexViewModel MailTemplateIndexViewModel(Customer customer, Setting customersettings)
		{
			#region RegularCase

			List<SelectListItem> _regularCase = new List<SelectListItem>();

			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Nytt ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Anmälare", Enums.TranslationSource.TextTranslation) + ")" + " (" + Translation.Get("Grunddata", Enums.TranslationSource.TextTranslation) + ")",
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
				Text = Translation.Get("Ärendet avslutat", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Anmälare", Enums.TranslationSource.TextTranslation) + ")" + " (" + Translation.Get("Grunddata", Enums.TranslationSource.TextTranslation) + ")",
				Value = "3",

			});
			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Informera anmälaren om åtgärden", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Anmälare", Enums.TranslationSource.TextTranslation) + ")",
				Value = "4",
			});
			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Skicka intern loggpost till", Enums.TranslationSource.TextTranslation),
				Value = "5",
			});
			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Anmälaren uppdaterat ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation) + ")",
				Value = "10",
			});
			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Anmälaren aktiverat ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation) + ")",
				Value = "15",
			});
			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Bevakningsdatum inträffar", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation) + ")",
				Value = "9",
			});
			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Skicka mail när planerat åtgärdsdatum inträffar", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation) + ")",
				Value = "12",
			});
			_regularCase.Add(new SelectListItem()
			{
				Text = Translation.Get("Prioritet", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("Grunddata", Enums.TranslationSource.TextTranslation) + ")",
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
				Text = Translation.Get("Påminnelse", Enums.TranslationSource.TextTranslation) + " " + Translation.Get("Enkät", Enums.TranslationSource.TextTranslation),
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


			var customMailTemplates = _mailTemplateService.GetCustomMailTemplates(customer.Id);

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
					Text = Translation.Get(x.Name, Enums.TranslationSource.TextTranslation),
					Value = x.Id.ToString(),
					Selected = (x.Id == languageId)
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
