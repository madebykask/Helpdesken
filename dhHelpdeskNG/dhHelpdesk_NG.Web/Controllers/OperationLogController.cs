﻿using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Shared;
    using DHDomain = DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models;
    using System.Text;
    using DH.Helpdesk.BusinessData.Models.WorkingGroup;


    public class OperationLogController : BaseController
    {
        private readonly IOperationLogService _operationLogService;
        private readonly ICustomerService _customerService;
        private readonly IOperationLogCategoryService _operationLogCategoryService;
        private readonly IOperationObjectService _operationObjectService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IEmailGroupService _emailGroupService;
        private readonly IEmailService _emailService;
        private readonly IOperationLogEmailLogService _operationLogEmailLogService;
        private readonly ISystemService _systemService;
        private readonly IGlobalSettingService _globalService;

        public OperationLogController(
            IOperationLogService operationLogService,
            ICustomerService customerService,
            IOperationObjectService operationObjectService,
            IOperationLogCategoryService operationLogCategoryService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IUserService userService,
            ISettingService settingService,
            IEmailGroupService emailGroupService,
            IEmailService emailService,
            IOperationLogEmailLogService operationLogEmailLogService,
            ISystemService systemService,
            IGlobalSettingService globalService)
            : base(masterDataService)
        {
            this._operationLogService = operationLogService;
            this._customerService = customerService;
            this._operationLogCategoryService = operationLogCategoryService;
            this._operationObjectService = operationObjectService;
            this._workingGroupService = workingGroupService;
            this._userService = userService;
            this._settingService = settingService;
            this._emailGroupService = emailGroupService;
            this._emailService = emailService;
            this._operationLogEmailLogService = operationLogEmailLogService;
            this._systemService = systemService;
            this._globalService = globalService;
        }

        public ActionResult Index()
        {
            var model = this.GetIndex();

            OperationLogSearch CS = new OperationLogSearch();
            if (SessionFacade.CurrentOperationLogSearch != null)
            {
                CS = SessionFacade.CurrentOperationLogSearch;
                model.OperationLogList = this._operationLogService.SearchAndGenerateOperationLog(SessionFacade.CurrentCustomer.Id, CS);
                model.OLSearch_Filter= CS;                
            }
            else
            {
                model.OperationLogs = this._operationLogService.GetOperationLogs(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.CreatedDate).ToList();
                CS.SortBy = "CreatedDate";
                CS.Ascending = false;
                SessionFacade.CurrentOperationLogSearch = CS;
                model.OperationLogList = this._operationLogService.SearchAndGenerateOperationLog(SessionFacade.CurrentCustomer.Id, CS);
            }
            
            return this.View(model);
               
        }

        [HttpPost]
        public ActionResult Index(OperationLogSearch OLSearch_Filter)
        {
            OperationLogSearch CS = new OperationLogSearch();            
            if (SessionFacade.CurrentOperationLogSearch != null)            
                CS = SessionFacade.CurrentOperationLogSearch;

            CS.OperationCategory_Filter = OLSearch_Filter.OperationCategory_Filter;
            CS.OperationObject_Filter = OLSearch_Filter.OperationObject_Filter;
            CS.CustomerId = OLSearch_Filter.CustomerId;
            CS.PeriodFrom = OLSearch_Filter.PeriodFrom;
            CS.PeriodTo = OLSearch_Filter.PeriodTo;
            if (CS.PeriodTo != null)
                CS.PeriodTo = OLSearch_Filter.PeriodTo.Value.AddDays(1);
            CS.Text_Filter = OLSearch_Filter.Text_Filter;
           
            var c = this._operationLogService.SearchAndGenerateOperationLog(SessionFacade.CurrentCustomer.Id, CS );

            if (OLSearch_Filter != null)
            {
                SessionFacade.CurrentOperationLogSearch = CS;
                if (CS.PeriodTo != null)
                    CS.PeriodTo = CS.PeriodTo.Value.AddDays(-1);
            }

            var model = this.GetIndex();

            model.OperationLogList = c;                      
            model.OLSearch_Filter = CS;
           
            return this.View(model);            
        }
       

        public void Sort(string FieldName)
        {
            var model = this.GetIndex();
            
            OperationLogSearch CS = new OperationLogSearch();
            if (SessionFacade.CurrentOperationLogSearch != null)
                CS = SessionFacade.CurrentOperationLogSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = FieldName;
            SessionFacade.CurrentOperationLogSearch = CS;                        
        }

        public ActionResult New()
        {
            var operationLog = new OperationLog { Customer_Id = SessionFacade.CurrentCustomer.Id };
            var model = GetOperationLogInputViewModel(operationLog, true);
            AddViewDataValues();

            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        //todo: create single input viewmodel class
        public ActionResult New(OperationLog operationlog, OperationLogList operationLogList, int[] WGsSelected, string[] SRsSelected, string[] UsersSelected, int OperationLogHour, int OperationLogMinute, int chkSecurity, int? chkOperationLogSMS, string txtSMS)
        {
            IDictionary<string, string> errors = null; 
            operationlog.User_Id = SessionFacade.CurrentUser.Id;
            operationlog.WorkingTime = (OperationLogHour * 60) + OperationLogMinute;
            if (chkSecurity == 0)
                WGsSelected = null;

            //var operationLogList = new OperationLogList();
            var customer = _customerService.GetCustomer(operationlog.Customer_Id);

            var operationObject = _operationObjectService.GetOperationObject(operationlog.OperationObject_Id);
            var operationCategory = new OperationLogCategory();

            if (operationlog.OperationLogCategory_Id != null)
            {
                operationCategory =
                    _operationLogCategoryService.GetOperationLogCategory(operationlog.OperationLogCategory_Id, customer.Id);
            }

            operationLogList.Language_Id = customer.Language_Id;
            operationLogList.OperationLogAction = operationlog.LogAction;
            operationLogList.OperationLogDescription = operationlog.LogText;
            operationLogList.OperationObjectName = operationObject.Name;
            operationLogList.OperationLogCategoryName = operationCategory.OLCName;

            _operationLogService.SaveOperationLog(operationlog, WGsSelected, out errors);

            //get the id for new operationlog
            var newOperationLogId = _operationLogService.GetOperationLogId();
            operationlog.Id = newOperationLogId;

            // send emails
            if (operationLogList.EmailRecepientsOperationLog != null)
                _operationLogService.SendOperationLogEmail(operationlog, operationLogList, customer);

            // send sms
            if (chkOperationLogSMS == 1)
            {
                var systemRepsSelected = string.Empty;
                var administratorsSelected = string.Empty;

                if (SRsSelected != null)
                    systemRepsSelected = SRsSelected.JoinToString();

                if (UsersSelected != null)
                    administratorsSelected = UsersSelected.JoinToString();

                var smsRecipients = systemRepsSelected + administratorsSelected;

                _operationLogService.SendOperationLogSMS(operationlog, smsRecipients, txtSMS, customer);
            }

            SaveRssFeed(operationlog.Customer_Id);

            if (errors.Count == 0)
                return RedirectToAction("index", "operationlog");

            var model = GetOperationLogInputViewModel(operationlog);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var operationlog = _operationLogService.GetOperationLog(id);
            
            if (operationlog == null)
                return new HttpNotFoundResult("No OperationLog found...");

            var model = GetOperationLogInputViewModel(operationlog);

            AddViewDataValues();

            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        //todo: create single input viewmodel class
        public ActionResult Edit(int id, OperationLog operationlog, OperationLogList operationLogList, int[] WGsSelected, string[] SRsSelected, string[] WGsSMSSelected, string[] UsersSelected, int OperationLogHour, int OperationLogMinute, int chkSecurity, int? chkOperationLogSMS, string txtSMS)
        {
            IDictionary<string, string> errors;
            var operationlogToSave = _operationLogService.GetOperationLog(id);
            UpdateModel(operationlogToSave, "OperationLog");
            var customer = _customerService.GetCustomer(operationlog.Customer_Id);

            var operationObject = _operationObjectService.GetOperationObject(operationlog.OperationObject_Id);
            var operationCategory = new OperationLogCategory();

            if (operationlog.OperationLogCategory_Id != null)
            {
                operationCategory = 
                    _operationLogCategoryService.GetOperationLogCategory(operationlog.OperationLogCategory_Id, customer.Id);
            }

            operationLogList.Language_Id = customer.Language_Id;
            operationLogList.OperationLogAction = operationlog.LogAction;
            operationLogList.OperationLogDescription = operationlog.LogText;
            operationLogList.OperationObjectName = operationObject.Name;
            operationLogList.OperationLogCategoryName = operationCategory.OLCName;

            operationlogToSave.User_Id = SessionFacade.CurrentUser.Id;
            operationlogToSave.WorkingTime = (OperationLogHour * 60) + OperationLogMinute;

            if (chkSecurity==0) 
               WGsSelected = null;

            _operationLogService.SaveOperationLog(operationlogToSave, WGsSelected, out errors);

            // send emails
            if (operationLogList.EmailRecepientsOperationLog != null)
                 _operationLogService.SendOperationLogEmail(operationlogToSave, operationLogList, customer);

            // send sms
            if (chkOperationLogSMS == 1)
            {
                var systemRepsSelected = string.Empty;
                var administratorsSelected = string.Empty;
                var wgsForSmsSelected = string.Empty;

                if (SRsSelected != null)
                    systemRepsSelected = SRsSelected.JoinToString();

                if (UsersSelected != null)
                    administratorsSelected = UsersSelected.JoinToString();

                if (WGsSMSSelected != null)
                    wgsForSmsSelected = WGsSMSSelected.JoinToString();

                var smsRecipients = systemRepsSelected + administratorsSelected + wgsForSmsSelected;
                _operationLogService.SendOperationLogSMS(operationlogToSave, smsRecipients, txtSMS, customer);
            }

            SaveRssFeed(operationlogToSave.Customer_Id);

            if (errors.Count == 0)
                return RedirectToAction("index", "operationlog");


            var model = GetOperationLogInputViewModel(operationlogToSave);
            
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._operationLogService.DeleteOperationLog(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "operationlog");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "operationlog", new { id = id });
            }
        }
   
        [HttpGet]
        public JsonResult GetOperationObjectAttr(int id)
        {
            var res = "";
            var curObj = this._operationObjectService.GetOperationObject(id);
            if (curObj != null)            
                res = curObj.ShowOnStartPage.ToString();            

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private OperationLogInputViewModel GetOperationLogInputViewModel(OperationLog operationlog, bool isNewModel = false)
        {
            var wgsSelected = operationlog.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();
            var customerId = SessionFacade.CurrentCustomer.Id;
            var cs = _settingService.GetCustomerSetting(customerId);

            const bool isAddEmpty = true;

            var wgsAvailableSMS = new List<WorkingGroupForSMS>();
            wgsAvailableSMS = _workingGroupService.GetWorkingGroupsForSMS(customerId).ToList();

            var usAvailable = new List<User>();
            usAvailable = _userService.GetAdminstratorsForSMS(customerId).ToList();

            var systemRespAvailable = new List<System>();
            systemRespAvailable = _systemService.GetSystemResponsibles(customerId).ToList();

            var smsEmailDomain = "";
            var operationObject = this._operationObjectService.GetOperationObject(operationlog.OperationObject_Id);
            var operationObjectShow = 0;

            if (operationObject != null)
                operationObjectShow = operationObject.ShowOnStartPage;

            if (cs.SMSEMailDomain != null)
                smsEmailDomain = cs.SMSEMailDomain;

            var workingGroups = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            if (isNewModel)
            {
                var defaultWg = workingGroups.FirstOrDefault(x => x.IsDefaultOperationLog.ToBool());
                if (defaultWg != null && !wgsSelected.Contains(defaultWg))
                {
                    wgsSelected.Add(defaultWg);
                }
            }

            foreach (var wg in workingGroups)
            {                
                if (!wgsSelected.Select(w => w.Id).Contains(wg.Id))
                    wgsAvailable.Add(wg);
            }

            var responsibleUsersAvailable = this._userService.GetAvailablePerformersOrUserId(customerId, operationlog.User_Id);
            var operationLogHours = operationlog.WorkingTime / 60;

            var model = new OperationLogInputViewModel
            {
                
                OperationLog = operationlog,

                OperationObjects = _operationObjectService.GetActiveOperationObjects(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                OperationObjectsAvailable = wgsAvailable.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                OperationObjectsSelected = wgsSelected.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                OperationLogCategories = _operationLogCategoryService.GetActiveOperationLogCategories(customerId).Select(x => new SelectListItem
                {
                    Text = x.OLCName,
                    Value = x.Id.ToString()
                }).ToList(),

                SMSWorkingGroupAvailable = wgsAvailableSMS.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.PhoneNumbers
                }).ToList(),

                SMSWorkingGroupSelected = new List<SelectListItem>(),
                 
                AdministratorsAvailable = usAvailable.Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.CellPhone + "@" + smsEmailDomain
                }).ToList(),

                AdministratorsSelected = new List<SelectListItem>(),

                SystemResponsiblesAvailable = systemRespAvailable.Select(x => new SelectListItem
                {
                    Text = x.ContactName,
                    Value = x.ContactPhone + "@" + smsEmailDomain
                }).ToList(),

                SystemResponsiblesSelected = new List<SelectListItem>(),

                OperationObjectShow = operationObjectShow,
            
                OperationLogEmailLog = this._operationLogEmailLogService.GetOperationLogEmailLogs(operationlog.Id),
            
                ResponsibleUsersAvailable = responsibleUsersAvailable.MapToSelectList(cs, isAddEmpty),

                SendToDialogModel = CreateNewSendToDialogModel(customerId, responsibleUsersAvailable.ToList(), cs),

                OperationLogHour = operationLogHours,

                OperationLogMinute = (operationlog.WorkingTime - operationLogHours),

                CustomerSettings = cs
            };
            return model;
        }

        private OperationLogIndexViewModel GetIndex()
        {                        
            var model = new OperationLogIndexViewModel
            {
                OperationLogs = this._operationLogService.GetAllOpertionLogs(),
                Customers = this._customerService.GetAllCustomers(),
                OperationObjects = this._operationObjectService.GetActiveOperationObjects(SessionFacade.CurrentCustomer.Id),
                OperationLogList = this._operationLogService.GetListForIndexPage(),
                OperationLogCategories = this._operationLogCategoryService.GetActiveOperationLogCategories(SessionFacade.CurrentCustomer.Id),
                OLSearch_Filter = new OperationLogSearch ()
            };

            return model;
        }

        private SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<CustomerUserInfo> users, Setting customerSetting)
        {
            var emailGroups = _emailGroupService.GetEmailGroupsWithEmails(customerId);
            var workingGroups = _workingGroupService.GetWorkingGroupsWithActiveEmails(customerId);
            var administrators = new List<ItemOverview>();

            if (users != null)
            {
                var validUsers = users.Where(u => u.IsActive != 0 &&
                                                 u.Performer == 1 &&
                                                 _emailService.IsValidEmail(u.Email) &&
                                                 !String.IsNullOrWhiteSpace(u.Email)).ToList();

                if (customerSetting.IsUserFirstLastNameRepresentation == 1)
                {
                    foreach (var u in users.OrderBy(it => it.FirstName).ThenBy(it => it.SurName))
                        if (u.IsActive == 1 && u.Performer == 1 && _emailService.IsValidEmail(u.Email) && !String.IsNullOrWhiteSpace(u.Email))
                            administrators.Add(new ItemOverview(string.Format("{0} {1}", u.FirstName, u.SurName), u.Email));
                }
                else
                {
                    foreach (var u in users.OrderBy(it => it.SurName).ThenBy(it => it.FirstName))
                        if (u.IsActive == 1 && u.Performer == 1 && _emailService.IsValidEmail(u.Email) && !String.IsNullOrWhiteSpace(u.Email))
                            administrators.Add(new ItemOverview(string.Format("{0} {1}", u.SurName, u.FirstName), u.Email));
                }
            }

            var emailGroupList = new MultiSelectList(emailGroups, "Id", "Name");
            var emailGroupEmails = emailGroups.Select(g => new GroupEmailsModel(g.Id, g.Emails)).ToList();
            var workingGroupList = new MultiSelectList(workingGroups, "Id", "Name");
            var workingGroupEmails = workingGroups.Select(g => new GroupEmailsModel(g.Id, g.Emails)).ToList();
            var administratorList = new MultiSelectList(administrators, "Value", "Name");

            return new SendToDialogModel(
                emailGroupList,
                emailGroupEmails,
                workingGroupList,
                workingGroupEmails,
                administratorList);
        }

        private void AddViewDataValues()
        {
            ViewData["Callback"] = "SendToDialogOperationLogCallback";
            ViewData["Id"] = "divSendToDialogCase";
        }

        private void SaveRssFeed(int customerId)
        {
            var logs = _operationLogService.GetRssOperationLogs(customerId);
            var title = _globalService.GetGlobalSettings().FirstOrDefault()?.ApplicationName;
            var rss = _operationLogService.CreateRssFeed(RequestExtension.GetAbsoluteUrl(), title, logs);
            _operationLogService.SaveRssFeed(rss, HttpContext.Request.PhysicalApplicationPath);
        }

    }
}
