using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class BusinessRuleController : BaseAdminController
    {
        private readonly ICustomerService _customerService;
        private readonly IProductAreaService _productAreaService;
        private readonly IStateSecondaryService _subStatusService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailGroupService _emailGroupService;

        public BusinessRuleController(IMasterDataService masterDataService,
                                      ICustomerService customerService,
                                      IProductAreaService productAreaService,
                                      IStateSecondaryService subStatusService,
                                      IMailTemplateService mailTemplateService,
                                      IEmailGroupService emailGroupService
                                     )
            : base(masterDataService)
        {
            _customerService = customerService;
            _productAreaService = productAreaService;
            _subStatusService = subStatusService;
            _mailTemplateService = mailTemplateService;
            _emailGroupService = emailGroupService;
        }

        #region Public Methods 
        
        [HttpGet]
        public ActionResult Index(int customerId)
        {
            var model = new BusinessRuleIndexModel();
            model.Customer = _customerService.GetCustomer(customerId);            
            return View(model);
        }

        [HttpGet]
        public ActionResult NewRule(int customerId)
        {
            var model = new BusinessRuleInputModel();
            model.CustomerId = customerId;
            model.RuleName = string.Empty;
            model.Events = new List<BREvent>();
            model.Events.Add(new BREvent(1, "On Save Case", true));
            model.ContinueOnSuccess = true;
            model.ContinueOnError = true;
            model.IsActive = true;

            var fieldItems = new List<SelectListItem>();

            fieldItems.Add(new SelectListItem(){ Value= "-1", Text = "", Selected = true});
            fieldItems.Add(new SelectListItem(){ Value= "0", Text = "[Null]", Selected = false});
            fieldItems.Add(new SelectListItem(){ Value= int.MaxValue.ToString(), Text = "[Any]", Selected = false});
            
            var productAreas = _productAreaService.GetAll(customerId);
            var lastLevels = new List<ProductArea>();

            foreach (var p in productAreas)
            {
                if (p.SubProductAreas.Count == 0 && p.IsActive != 0)
                {
                    p.Name = p.ResolveFullName();
                    lastLevels.Add(p);
                }
            }  

            var processList = lastLevels.Select(l=> new SelectListItem()
                                                { 
                                                    Value= l.Id.ToString(), 
                                                    Text = l.Name, 
                                                    Selected = false
                                                }).ToList();

            var subStatusList = _subStatusService.GetActiveStateSecondaries(customerId).Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = false
            }).ToList();

            model.Condition = new BRConditionModel()
            {
                Id = 0,
                RuleId = 0,
                Process = processList,
                ProcessFromValue = new List<SelectListItem>(),
                ProcessToValue = new List<SelectListItem>(),

                SubStatus = subStatusList,
                SubStatusFromValue = new List<SelectListItem>(),
                SubStatusToValue = new List<SelectListItem>(),                

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
                var templateId = mailtemplate.MailTemplateId;
                var templateName = string.Empty;
                var activeLanguages = mailtemplate.TemplateLanguages.Where(l => !string.IsNullOrEmpty(l.Subject) && !string.IsNullOrEmpty(l.Body)).ToList();
                if (activeLanguages.Select(l => l.LanguageId).Contains(SessionFacade.CurrentLanguageId))
                    templateName = activeLanguages.Where(l => l.LanguageId == SessionFacade.CurrentLanguageId).First().TemplateName;
                else
                    templateName = activeLanguages.First().TemplateName;

                emailTemplateList.Add(new SelectListItem { Value = templateId.ToString(), Text = templateName, Selected = false });
            }

            var emailGroupList = _emailGroupService.GetEmailGroups(customerId)
                                                   .Select(mg => new SelectListItem
                                                        {
                                                            Value = mg.Id.ToString(),
                                                            Text = mg.Name,
                                                            Selected = false
                                                        }).ToList();
            model.Action = new BRActionModel()
            {
                Id = 0,
                RuleId = 0,
                //Send Email
                ActionTypeId = 1,
                EMailTemplates = emailTemplateList,
                EMailGroups = emailGroupList,
                Sequence = 1
            };
            
            return View(model);
        }

        #endregion



    }
}
