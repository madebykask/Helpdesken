using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
	public class BusinessRuleController : BaseAdminController
	{
		private readonly ICustomerService _customerService;
		private readonly ISettingService _settingService;
		private readonly IUserService _userService;
        private readonly IProductAreaService _productAreaService;
        private readonly IStateSecondaryService _subStatusService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailGroupService _emailGroupService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IBusinessRuleService _businessRuleService;
        private readonly IStatusService _statusService;

        public BusinessRuleController(IMasterDataService masterDataService,
                                      ICustomerService customerService,
                                      ISettingService settingService,
                                      IUserService userService,
                                      IProductAreaService productAreaService,
                                      IStateSecondaryService subStatusService,
                                      IMailTemplateService mailTemplateService,
                                      IEmailGroupService emailGroupService,
                                      IWorkingGroupService workingGroupService,
                                      IBusinessRuleService businessRuleService,
                                      IStatusService statusService

                                     )
            : base(masterDataService)
        {
            _customerService = customerService;
            _settingService = settingService;
            _userService = userService;
            _productAreaService = productAreaService;
            _subStatusService = subStatusService;
            _mailTemplateService = mailTemplateService;
            _emailGroupService = emailGroupService;
            _workingGroupService = workingGroupService;
            _businessRuleService = businessRuleService;
            _statusService = statusService;

        }

        #region Public Methods 

        [HttpGet]
        public ActionResult Index(int customerId)
		{
			var model = new BusinessRuleIndexModel();
			model.Customer = _customerService.GetCustomer(customerId);

			var rules = _businessRuleService.GetRuleReadlist(customerId);
			model.Rules = rules.Select(x => new BusinessRuleListItemModel
			{
				RuleId = x.Id,
				RuleName = x.RuleName,
				//Action = String.Join(", ", x.Actions.ToArray()),
				//Condition = String.Join(", ", x.Conditions.ToArray()),
				ChangedBy = x.ChangedBy.GetFullName(),
				ChangedOn = x.ChangedTime,
				Event = Enum.GetName(typeof(BREventType), x.Event),
				IsActive = x.RuleActive,
                Sequence = x.RuleSequence
            }).ToList();

			return View(model);
		}

		[HttpGet]
		public ActionResult NewRule(int customerId)
		{
			var model = new BusinessRuleInputModel
			{
				CustomerId = customerId,
				Events = DefineBREvents(),
				Condition = new BRConditionModel
				{
					Sequence = 1
				}
			};

			var currentValue = new List<DdlModel>
			{
				new DdlModel {Value = BRConstItem.CURRENT_VALUE.ToString(), Text = "[" + Translation.Get("Nuvarande värde") +"]", Selected = false}
			};

			var fieldItems = new List<DdlModel>
			{
				new DdlModel {Value = BRConstItem.NULL.ToString(), Text = "[" + Translation.Get("Tomt") +"]", Selected = false},
				new DdlModel {Value = BRConstItem.ANY.ToString(), Text = "[" + Translation.Get("Endera") +"]", Selected = false}
			};

			var productAreas = _productAreaService.GetAll(customerId);
			var lastLevels = new List<ProductArea>();

			foreach (var p in productAreas.Where(p => p.SubProductAreas.Count == 0))
			{
				p.Name = p.ResolveFullName();
				lastLevels.Add(p);
			}

			var processList = GetProcessesList(fieldItems, lastLevels);
			ViewBag.ProcessList = processList;

            var statusList = GetStatusesList(customerId, fieldItems);
            ViewBag.Status = statusList;

            var subStatusList = GetSubStatusesList(customerId, fieldItems);
			ViewBag.SubStatus = subStatusList;

            var emailTemplateList = GetEmailTemplatesList(customerId);
			ViewBag.EMailTemplates = emailTemplateList.OrderBy(x => x.Text).ToList();

			var emailGroupList = GetEmailGroupsList(customerId);
			ViewBag.EMailGroups = emailGroupList;

			var allAdmins = GetAdminsList(customerId, currentValue);
			ViewBag.Administrators = allAdmins;

			var wgs = GetWorkgroupsList(customerId);
			var workingGroups = currentValue.Union(wgs).ToList();
			ViewBag.WorkingGroups = workingGroups;

			model.Action = new BRActionModel
			{
				ActionTypeId = BRActionType.SendEmail,
				EMailGroupIds = emailGroupList.Where(g => g.Selected).Select(g => g.Value).ToList(),
				WorkingGroupIds = workingGroups.Where(g => g.Selected).Select(g => g.Value).ToList(),
				AdministratorIds = allAdmins.Where(g => g.Selected).Select(g => g.Value).ToList(),
				Sequence = 1
			};

			return View(model);
		}

		[HttpGet]
		public ActionResult EditRule(int id)
		{
			var rule = _businessRuleService.GetRule(id);

			var model = new BusinessRuleInputModel
			{
				RuleId = rule.Id,
				CustomerId = rule.CustomerId,
				RuleName = rule.RuleName,
				Events = DefineBREvents(rule.EventId),
				ContinueOnSuccess = rule.ContinueOnSuccess,
				ContinueOnError = rule.ContinueOnError,
				IsActive = rule.RuleActive,
				Sequence = rule.RuleSequence
			};

            var currentValue = new List<DdlModel>
            {
                new DdlModel {Value = BRConstItem.CURRENT_VALUE.ToString(), Text = "[" + Translation.Get("Nuvarande värde") +"]", Selected = false}
            };

            var fieldItems = new List<DdlModel>
            {
                new DdlModel {Value = BRConstItem.NULL.ToString(), Text = "[" + Translation.Get("Tomt") +"]", Selected = false},
                new DdlModel {Value = BRConstItem.ANY.ToString(), Text = "[" + Translation.Get("Endera") +"]", Selected = false}
            };

			var productAreas = _productAreaService.GetAll(rule.CustomerId);
			var lastLevels = new List<ProductArea>();

			foreach (var p in productAreas.Where(p => p.SubProductAreas.Count == 0))
			{
				p.Name = p.ResolveFullName();
				lastLevels.Add(p);
			}

			var processList = GetProcessesList(fieldItems, lastLevels);
			ViewBag.ProcessList = processList;

            var statusList = GetStatusesList(rule.CustomerId, fieldItems);
            ViewBag.Status = statusList;

            var subStatusList = GetSubStatusesList(rule.CustomerId, fieldItems);
			ViewBag.SubStatus = subStatusList;

			model.Condition = new BRConditionModel
			{
				Id = 0,
				RuleId = rule.Id,
				ProcessesFromValue = processList.Where(x => rule.ProcessFrom.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
				ProcessesToValue = processList.Where(x => rule.ProcessTo.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
				SubStatusesFromValue = subStatusList.Where(x => rule.SubStatusFrom.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
				SubStatusesToValue = subStatusList.Where(x => rule.SubStatusTo.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
                StatusesFromValue = statusList.Where(x => rule.StatusFrom.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
                StatusesToValue = statusList.Where(x => rule.StatusTo.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
                Sequence = 1,
                Equals = rule.DomainFrom
            };

			var emailTemplateList = GetEmailTemplatesList(rule.CustomerId);
			ViewBag.EMailTemplates = emailTemplateList.OrderBy(x => x.Text).ToList();

			var emailGroupList = GetEmailGroupsList(rule.CustomerId);
			ViewBag.EMailGroups = emailGroupList;

			var allAdmins = GetAdminsList(rule.CustomerId, currentValue);
			ViewBag.Administrators = allAdmins;

			var wgs = GetWorkgroupsList(rule.CustomerId);
			var workingGroups = currentValue.Union(wgs).ToList();
			ViewBag.WorkingGroups = workingGroups;

			model.Action = new BRActionModel
			{
				Id = 0,
				RuleId = rule.Id,
				ActionTypeId = rule.EventId,
				EmailTemplateId = rule.EmailTemplate,
				EMailGroupIds = emailGroupList.Where(x => rule.EmailGroups.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
				WorkingGroupIds = workingGroups.Where(x => rule.WorkingGroups.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
				AdministratorIds = allAdmins.Where(x => rule.Administrators.Contains(int.Parse(x.Value))).Select(x => x.Value).ToList(),
				Recipients = rule.Recipients != null ? string.Join(BRConstItem.Email_Separator, rule.Recipients) : null,
				CaseCreator = rule.CaseCreator,
				Initiator = rule.Initiator,
				CaseIsAbout = rule.CaseIsAbout,
				DisableFinishingType = rule.DisableFinishingType,
				Sequence = 1
			};

			return View("NewRule", model);
		}

		[HttpGet]
		public JsonResult SaveRule(BusinessRuleJSModel data)
		{
			if (ModelState.IsValid)
			{
				var ruleModel = data.MapToRuleData();

				if (ruleModel.Id == 0)
				{
					ruleModel.CreatedTime = DateTime.UtcNow;
					ruleModel.ChangedTime = DateTime.UtcNow;
					ruleModel.CreatedByUserId = SessionFacade.CurrentUser.Id;
					ruleModel.ChangedByUserId = SessionFacade.CurrentUser.Id;
				}
				else
				{
					ruleModel.ChangedTime = DateTime.UtcNow;
					ruleModel.ChangedByUserId = SessionFacade.CurrentUser.Id;
				}

				if (ruleModel.Recipients == null)
					ruleModel.Recipients = new List<string>().ToArray();


				var res = _businessRuleService.SaveBusinessRule(ruleModel);

				return Json(res == string.Empty ? "OK" : res, JsonRequestBehavior.AllowGet);
			}
			return Json("false", JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var rule = this._businessRuleService.GetRule(id);

            if (this._businessRuleService.DeleteBusinessRule(id) == DeleteMessage.Success)
			{

				return RedirectToAction("Index", new { customerId = rule.CustomerId});

            }

            else
            {
                this.TempData.Add("Error", "");
                return RedirectToAction("EditRule", new { id = rule.Id });
            }
        }

        #endregion

        #region Private

        //private string SerializeRuleAction(string emailTemplate, SelectedItems emailGroups, SelectedItems workingGroups, SelectedItems administrators, string[] recipients)
        //{

        //}

        //private string SerializeRuleCondition(BRActionModel action)
        //{

        //}

        //private string SerializeRuleEvent(BRActionModel action)
        //{

        //}

        private List<DdlModel> GetSubStatusesList(int customerId, List<DdlModel> fieldItems)
		{
			var subStatusList = fieldItems.Union(_subStatusService.GetStateSecondaries(customerId).Select(s => new DdlModel
			{
				Value = s.Id.ToString(),
				Text = s.Name,
				Selected = false,
				Disabled = s.IsActive != 1
			}).OrderBy(x => x.Text)).ToList();
			return subStatusList;
		}

        private List<DdlModel> GetStatusesList(int customerId, List<DdlModel> fieldItems)
        {
            var statusList = fieldItems.Union(_statusService.GetStatuses(customerId).Select(s => new DdlModel
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = false,
                Disabled = s.IsActive != 1
            }).OrderBy(x => x.Text)).ToList();
            return statusList;
        }

        private static List<DdlModel> GetProcessesList(List<DdlModel> fieldItems, List<ProductArea> lastLevels)
		{
			var processList = fieldItems.Union(lastLevels.Select(l => new DdlModel
			{
				Value = l.Id.ToString(),
				Text = l.Name,
				Selected = false,
				Disabled = l.IsActive != 1
			}).OrderBy(x => x.Text)).ToList();
			return processList;
		}

		private List<DdlModel> GetAdminsList(int customerId, List<DdlModel> currentValue)
		{
			var customerSetting = _settingService.GetCustomerSetting(customerId);
			var performers = _userService.GetAvailablePerformersOrUserId(customerId);
			var adminList = performers.MapToCustomSelectList(string.Empty, customerSetting);
			var allAdmins = currentValue.Union(adminList.Items.Select(i => new DdlModel
			{
				Value = i.Id,
				Text = i.Value,
				Selected = false,
				Disabled = !i.IsActive
			}).OrderBy(x => x.Text).ToList()).ToList();
			return allAdmins;
		}

		private List<DdlModel> GetWorkgroupsList(int customerId)
		{
			var wgs = _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false)
                 .Where(wg => wg.IsActive == 1)
                .Select(wg => new DdlModel
				{
					Value = wg.Id.ToString(),
					Text = wg.WorkingGroupName,
					Selected = false
				}).OrderBy(x => x.Text).ToList();
			return wgs;
		}

		private List<DdlModel> GetEmailGroupsList(int customerId)
		{
			var emailGroupList = _emailGroupService.GetEmailGroups(customerId)
				.Select(mg => new DdlModel
				{
					Value = mg.Id.ToString(),
					Text = mg.Name,
					Selected = false,
					Disabled = mg.IsActive != 1
				}).OrderBy(x => x.Text).ToList();
			return emailGroupList;
		}

		private List<SelectListItem> GetEmailTemplatesList(int customerId)
		{
			var customMailTemplates = _mailTemplateService.GetCustomMailTemplatesFull(customerId).ToList();
			var activeMailTemplates =
				customMailTemplates.Where(
					m =>
						m.TemplateLanguages.Any(tl => !string.IsNullOrEmpty(tl.Subject) && !string.IsNullOrEmpty(tl.Body)) &&
						m.MailId >= 100).ToList();

			var emailTemplateList = new List<SelectListItem>();
			foreach (var mailtemplate in activeMailTemplates)
			{
				var templateId = mailtemplate.MailId;
				var activeLanguages =
					mailtemplate.TemplateLanguages.Where(l => !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body)).ToList();
				var templateName = activeLanguages.Select(l => l.LanguageId).Contains(SessionFacade.CurrentLanguageId)
					? activeLanguages.First(l => l.LanguageId == SessionFacade.CurrentLanguageId).TemplateName
					: activeLanguages.First().TemplateName;

				emailTemplateList.Add(new SelectListItem { Value = templateId.ToString(), Text = templateName, Selected = false });
			}
			return emailTemplateList;
		}

		private List<BREvent> DefineBREvents(int selectedId = 1)
        {
            return new List<BREvent>
            {
                new BREvent((int)BREventType.OnSaveCase, "On Save Case", selectedId == (int)BREventType.OnSaveCase),
                new BREvent((int)BREventType.OnCreateCaseM2T, "On Create Case (M2T)", selectedId == (int)BREventType.OnCreateCaseM2T),
                new BREvent((int)BREventType.OnLoadCase, "On Load Case", selectedId == (int)BREventType.OnLoadCase),
                //new BREvent((int)BREventType.OnSaveCaseBefore, "On Save Case (Before)", selectedId == (int)BREventType.OnSaveCaseBefore)
            };

            #endregion Private

        }
    }
}
