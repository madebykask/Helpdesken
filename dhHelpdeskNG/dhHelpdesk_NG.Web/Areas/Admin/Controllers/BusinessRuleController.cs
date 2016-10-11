using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.BusinessRule;

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

        public BusinessRuleController(IMasterDataService masterDataService,
                                      ICustomerService customerService,
                                      ISettingService settingService,
                                      IUserService userService,  
                                      IProductAreaService productAreaService,
                                      IStateSecondaryService subStatusService,
                                      IMailTemplateService mailTemplateService,
                                      IEmailGroupService emailGroupService,
                                      IWorkingGroupService workingGroupService,
                                      IBusinessRuleService businessRuleService

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
                Event = "On Save Case",
                IsActive = x.RuleActive
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult NewRule(int customerId)
        {
            var model = new BusinessRuleInputModel();
            model.RuleId = 0;
            model.CustomerId = customerId;
            model.RuleName = string.Empty;
            model.Events = new List<BREvent>();
            model.Events.Add(new BREvent((int)BREventType.OnSaveCase, "On Save Case", true));
            model.ContinueOnSuccess = true;
            model.ContinueOnError = true;
            model.IsActive = true;

            var currentValue = new List<DdlModel>();
            currentValue.Add(new DdlModel() { Value = BRConstItem.CURRENT_VALUE.ToString(), Text = "[CurrentValue]", Selected = false });

            var fieldItems = new List<DdlModel>();            
            //fieldItems.Add(new SelectListItem(){ Value= "-1", Text = "", Selected = true});
            fieldItems.Add(new DdlModel() { Value = BRConstItem.NULL.ToString(), Text = "[Null]", Selected = false });
            fieldItems.Add(new DdlModel() { Value = BRConstItem.ANY.ToString(), Text = "[Any]", Selected = false });
            
            var productAreas = _productAreaService.GetAll(customerId);
            var lastLevels = new List<ProductArea>();

            foreach (var p in productAreas)
            {
                if (p.SubProductAreas.Count == 0)
                {
                    p.Name = p.ResolveFullName();
                    lastLevels.Add(p);
                }
            }

            var processList = fieldItems.Union(lastLevels.Select(l => new DdlModel()
                                                { 
                                                    Value= l.Id.ToString(), 
                                                    Text = l.Name, 
                                                    Selected = false,
                                                    Disabled = l.IsActive != 1
                                                }).OrderBy(x => x.Text)).ToList();

            var subStatusList = fieldItems.Union(_subStatusService.GetActiveStateSecondaries(customerId).Select(s => new DdlModel()
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = false,
                Disabled = s.IsActive != 1
            }).OrderBy(x => x.Text)).ToList();

            model.Condition = new BRConditionModel()
            {
                Id = 0,
                RuleId = 0,
                Process = processList,
                ProcessFromValue = new List<DdlModel>(),
                ProcessToValue = new List<DdlModel>(),

                SubStatus = subStatusList,
                SubStatusFromValue = new List<DdlModel>(),
                SubStatusToValue = new List<DdlModel>(),                

                Sequence = 1                
            };

            var customMailTemplates = _mailTemplateService.GetCustomMailTemplates(customerId).ToList();
            var activeMailTemplates = customMailTemplates.Where(m => m.TemplateLanguages
                                                                    .Where(tl => !string.IsNullOrEmpty(tl.Subject) && !string.IsNullOrEmpty(tl.Body)).Any() &&
                                                                    m.MailId >= 100
                                                               )
                                                         .ToList();

            var emailTemplateList = new List<SelectListItem>();
            foreach (var mailtemplate in activeMailTemplates)
            {
                var templateId = mailtemplate.MailId;
                var templateName = string.Empty;
                var activeLanguages = mailtemplate.TemplateLanguages.Where(l => !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body)).ToList();
                if (activeLanguages.Select(l => l.LanguageId).Contains(SessionFacade.CurrentLanguageId))
                    templateName = activeLanguages.Where(l => l.LanguageId == SessionFacade.CurrentLanguageId).First().TemplateName;
                else
                    templateName = activeLanguages.First().TemplateName;

                emailTemplateList.Add(new SelectListItem { Value = templateId.ToString(), Text = templateName, Selected = false });
            }

            var emailGroupList = _emailGroupService.GetEmailGroups(customerId)
                                                   .Select(mg => new DdlModel()
                                                        {
                                                            Value = mg.Id.ToString(),
                                                            Text = mg.Name,
                                                            Selected = false,
                                                            Disabled = mg.IsActive != 1
                                                        }).OrderBy(x => x.Text).ToList();

            var wgs = _workingGroupService.GetAllWorkingGroupsForCustomer(customerId)
                                          .Select(wg => new DdlModel
                                                        {
                                                            Value = wg.Id.ToString(),
                                                            Text = wg.WorkingGroupName,
                                                            Selected = false,
                                                            Disabled = wg.IsActive != 1
                                                        }).OrderBy(x => x.Text).ToList();
    
            var customerSetting = _settingService.GetCustomerSetting(customerId);
            var performers = this._userService.GetAvailablePerformersOrUserId(customerId);                        
            var adminList = performers.MapToCustomSelectList(string.Empty, customerSetting);
            var allAdmins = currentValue.Union(adminList.Items.Select(i => new DdlModel
                                                        {
                                                            Value = i.Id,
                                                            Text = i.Value,
                                                            Selected = false,
                                                            Disabled = !i.IsActive
                                                        }).OrderBy(x => x.Text).ToList());

            model.Action = new BRActionModel()
            {
                Id = 0,
                RuleId = 0,                
                ActionTypeId = BRActionType.SendEmail,
                EMailTemplates = emailTemplateList.OrderBy(x => x.Text).ToList(),
                EMailGroups = emailGroupList,
                WorkingGroups = currentValue.Union(wgs).ToList(),
                Administrators = allAdmins.ToList(),
                Recipients = string.Empty,
                Sequence = 1
            };
            
            return View(model);
        }

        [HttpGet]
        public ActionResult EditRule(int id)
        {
            var rule = _businessRuleService.GetRule(id);

            var model = new BusinessRuleInputModel();
            model.RuleId = rule.Id;
            model.CustomerId = rule.CustomerId;
            model.RuleName = rule.RuleName;
            model.Events = new List<BREvent>();
            model.Events.Add(new BREvent((int)BREventType.OnSaveCase, "On Save Case", true));
            model.ContinueOnSuccess = rule.ContinueOnSuccess;
            model.ContinueOnError = rule.ContinueOnError;
            model.IsActive = rule.RuleActive;
            model.Sequence = rule.RuleSequence;

            var currentValue = new List<DdlModel>();
            currentValue.Add(new DdlModel() { Value = BRConstItem.CURRENT_VALUE.ToString(), Text = "[CurrentValue]", Selected = false });

            var fieldItems = new List<DdlModel>();            
            //fieldItems.Add(new SelectListItem(){ Value= "-1", Text = "", Selected = true});
            fieldItems.Add(new DdlModel() { Value = BRConstItem.NULL.ToString(), Text = "[Null]", Selected = false });
            fieldItems.Add(new DdlModel() { Value = BRConstItem.ANY.ToString(), Text = "[Any]", Selected = false });
            
            var productAreas = _productAreaService.GetAll(rule.CustomerId);
            var lastLevels = new List<ProductArea>();

            foreach (var p in productAreas)
            {
                if (p.SubProductAreas.Count == 0)
                {
                    p.Name = p.ResolveFullName();
                    lastLevels.Add(p);
                }
            }

            var processList = fieldItems.Union(lastLevels.Select(l => new DdlModel()
                                                { 
                                                    Value= l.Id.ToString(), 
                                                    Text = l.Name, 
                                                    Selected = false,
                                                    Disabled = l.IsActive != 1
                                                }).OrderBy(x => x.Text)).ToList();

            var subStatusList = fieldItems.Union(_subStatusService.GetActiveStateSecondaries(rule.CustomerId).Select(s => new DdlModel()
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = false,
                Disabled = s.IsActive != 1
            }).OrderBy(x => x.Text)).ToList();

            model.Condition = new BRConditionModel()
            {
                Id = 0,
                RuleId = rule.Id,
                Process = processList,
                ProcessFromValue = processList.Where(x => rule.ProcessFrom.Contains(Int32.Parse(x.Value))).Select(x => new DdlModel { Value = x.Value, Selected = true }).ToList(),
                ProcessToValue = processList.Where(x => rule.ProcessTo.Contains(Int32.Parse(x.Value))).Select(x => new DdlModel { Value = x.Value, Selected = true }).ToList(),

                SubStatus = subStatusList,
                SubStatusFromValue = subStatusList.Where(x => rule.SubStatusFrom.Contains(Int32.Parse(x.Value))).Select(x => new DdlModel { Value = x.Value, Selected = true }).ToList(),
                SubStatusToValue = subStatusList.Where(x => rule.SubStatusTo.Contains(Int32.Parse(x.Value))).Select(x => new DdlModel { Value = x.Value, Selected = true }).ToList(),

                Sequence = 1
            };

            var customMailTemplates = _mailTemplateService.GetCustomMailTemplates(rule.CustomerId).ToList();
            var activeMailTemplates = customMailTemplates.Where(m => m.TemplateLanguages
                                                                    .Where(tl => !string.IsNullOrEmpty(tl.Subject) && !string.IsNullOrEmpty(tl.Body)).Any() &&
                                                                    m.MailId >= 100
                                                               )
                                                         .ToList();

            var emailTemplateList = new List<SelectListItem>();
            foreach (var mailtemplate in activeMailTemplates)
            {
                var templateId = mailtemplate.MailId;
                var templateName = string.Empty;
                var activeLanguages = mailtemplate.TemplateLanguages.Where(l => !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body)).ToList();
                if (activeLanguages.Select(l => l.LanguageId).Contains(SessionFacade.CurrentLanguageId))
                    templateName = activeLanguages.Where(l => l.LanguageId == SessionFacade.CurrentLanguageId).First().TemplateName;
                else
                    templateName = activeLanguages.First().TemplateName;

                emailTemplateList.Add(new SelectListItem { Value = templateId.ToString(), Text = templateName, Selected = false });
            }

            var emailGroupList = _emailGroupService.GetEmailGroups(rule.CustomerId)
                                                   .Select(mg => new DdlModel()
                                                   {
                                                       Value = mg.Id.ToString(),
                                                       Text = mg.Name,
                                                       Selected = false,
                                                       Disabled = mg.IsActive != 1
                                                   }).OrderBy(x => x.Text).ToList();

            var wgs = _workingGroupService.GetAllWorkingGroupsForCustomer(rule.CustomerId)
                                          .Select(wg => new DdlModel
                                          {
                                              Value = wg.Id.ToString(),
                                              Text = wg.WorkingGroupName,
                                              Selected = false,
                                              Disabled = wg.IsActive != 1
                                          }).OrderBy(x => x.Text).ToList();

            var customerSetting = _settingService.GetCustomerSetting(rule.CustomerId);
            var performers = this._userService.GetAvailablePerformersOrUserId(rule.CustomerId);
            var adminList = performers.MapToCustomSelectList(string.Empty, customerSetting);
            var allAdmins = currentValue.Union(adminList.Items.Select(i => new DdlModel
            {
                Value = i.Id,
                Text = i.Value,
                Selected = false,
                Disabled = !i.IsActive
            }).OrderBy(x => x.Text).ToList());

            model.Action = new BRActionModel()
            {
                Id = 0,
                RuleId = rule.Id,
                ActionTypeId = BRActionType.SendEmail,
                EMailTemplates = emailTemplateList.Select(x => new SelectListItem { Value = x.Value, Selected = rule.EmailTemplate == (Int32.Parse(x.Value)), Text = x.Text }).OrderBy(x => x.Text).ToList(),
                EMailGroups = emailGroupList.Select(x => new DdlModel { Value = x.Value, Selected = rule.EmailGroups.Contains(Int32.Parse(x.Value)), Text = x.Text, Disabled = x.Disabled}).ToList(),
                WorkingGroups = currentValue.Union(wgs).ToList()
                    .Select(x => new DdlModel { Value = x.Value, Selected = rule.WorkingGroups.Contains(Int32.Parse(x.Value)), Text = x.Text, Disabled = x.Disabled }).ToList(),
                Administrators = allAdmins.ToList()
                    .Select(x => new DdlModel { Value = x.Value, Selected = rule.Administrators.Contains(Int32.Parse(x.Value)), Text = x.Text, Disabled = x.Disabled }).ToList(),
                Recipients = String.Join(BRConstItem.Email_Separator, rule.Recipients),
                CaseCreator = rule.CaseCreator,
                Initiator = rule.Initiator,
                CaseIsAbout = rule.CaseIsAbout,
                Sequence = 1
            };

            return View("NewRule", model);
        }

        [HttpGet]
        public JsonResult SaveRule(BusinessRuleJSModel data)
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

            ruleModel.EventId = (int)BREventType.OnSaveCase;
            if (ruleModel.Recipients == null)
                ruleModel.Recipients = new List<string>().ToArray();


            var res = _businessRuleService.SaveBusinessRule(ruleModel);

            if (res == string.Empty)
                return Json("OK", JsonRequestBehavior.AllowGet);
            else
                return Json(res, JsonRequestBehavior.AllowGet);
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

        #endregion Private

    }
}
