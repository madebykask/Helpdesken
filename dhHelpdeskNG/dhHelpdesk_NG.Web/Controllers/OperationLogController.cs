using System;
using System.Web.Mvc;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Models;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Domain;



namespace dhHelpdesk_NG.Web.Controllers
{
    public class OperationLogController : BaseController
    {
        private readonly IOperationLogService _operationLogService;
        private readonly ICustomerService _customerService;
        private readonly IOperationLogCategoryService _operationLogCategoryService;
        private readonly IOperationObjectService _operationObjectService;
        private readonly IWorkingGroupService _workingGroupService;

        public OperationLogController(
            IOperationLogService operationLogService,
            ICustomerService customerService,
            IOperationObjectService operationObjectService,
            IOperationLogCategoryService operationLogCategoryService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _operationLogService = operationLogService;
            _customerService = customerService;
            _operationLogCategoryService = operationLogCategoryService;
            _operationObjectService = operationObjectService;
            _workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = GetIndex();

            OperationLogSearch CS = new OperationLogSearch();
            if (SessionFacade.CurrentOperationLogSearch != null)
            {
                CS = SessionFacade.CurrentOperationLogSearch;
                model.OperationLogList = _operationLogService.SearchAndGenerateOperationLog(SessionFacade.CurrentCustomer.Id, CS);
                model.OLSearch_Filter= CS;                
            }
            else
            {
                model.OperationLogs = _operationLogService.GetOperationLogs(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.CreatedDate).ToList();
                CS.SortBy = "CreatedDate";
                CS.Ascending = true;
                SessionFacade.CurrentOperationLogSearch = CS;
            }
            
            return View(model);
               
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
            CS.Text_Filter = OLSearch_Filter.Text_Filter;
           
            var c = _operationLogService.SearchAndGenerateOperationLog(SessionFacade.CurrentCustomer.Id, CS );
            
            if (OLSearch_Filter != null)
                SessionFacade.CurrentOperationLogSearch = CS;

            var model = GetIndex();

            model.OperationLogList = c;                      
            model.OLSearch_Filter = CS;
           
            return View(model);            
        }
       

        public ActionResult Sort(string FieldName)
        {
            var model = GetIndex();
            
            OperationLogSearch CS = new OperationLogSearch();
            if (SessionFacade.CurrentOperationLogSearch != null)
                CS = SessionFacade.CurrentOperationLogSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = FieldName;
            SessionFacade.CurrentOperationLogSearch = CS;            
            return View(model);
        }

        public ActionResult New()
        {
            var model = OperationLogInputViewModel(new OperationLog { Customer_Id = SessionFacade.CurrentCustomer.Id });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(OperationLog operationlog, int[] WGsSelected, int OperationLogHour, int OperationLogMinute, int chkSecurity)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            operationlog.User_Id = SessionFacade.CurrentUser.Id;
            operationlog.WorkingTime = (OperationLogHour * 60) + OperationLogMinute;
            if (chkSecurity == 0)
                WGsSelected = null;

            _operationLogService.SaveOperationLog(operationlog, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "operationlog");

            var model = OperationLogInputViewModel(operationlog);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var operationlog = _operationLogService.getoperationlog(id);


            if (operationlog == null)
                return new HttpNotFoundResult("No OperationLog found...");

            var model = OperationLogInputViewModel(operationlog);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, OperationLog operationlog, int[] WGsSelected, int OperationLogHour, int OperationLogMinute,int chkSecurity)
        {
            var operationlogToSave = _operationLogService.getoperationlog(id);
            UpdateModel(operationlogToSave, "OperationLog");

            IDictionary<string, string> errors = new Dictionary<string, string>();

            operationlogToSave.User_Id = SessionFacade.CurrentUser.Id;
            operationlogToSave.WorkingTime = (OperationLogHour * 60) + OperationLogMinute;
            
            if (chkSecurity==0) 
               WGsSelected = null;
            _operationLogService.SaveOperationLog(operationlogToSave, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "operationlog");
            
            var model = OperationLogInputViewModel(operationlogToSave);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_operationLogService.DeleteOperationLog(id) == DeleteMessage.Success)
                return RedirectToAction("index", "operationlog");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "operationlog", new { id = id });
            }
        }
   
        private OperationLogInputViewModel OperationLogInputViewModel(OperationLog operationlog)
        {
            var wgsSelected = operationlog.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();

            foreach (var wg in _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id))
            {                
                if (!wgsSelected.Contains(wg))
                    wgsAvailable.Add(wg);
            }

            var model = new OperationLogInputViewModel
            {
                
                OperationLog = operationlog ,

                OperationObjects = _operationObjectService.GetOperationObjects(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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

                OperationLogCategories = _operationLogCategoryService.GetOperationLogCategories (SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.OLCName,
                    Value = x.Id.ToString()
                }).ToList()                 
                                
            };
            
            model.OperationLogHour = model.OperationLog.WorkingTime / 60;
            model.OperationLogMinute = model.OperationLog.WorkingTime - (model.OperationLogHour*60);

            return model;
        }


        private OperationLogIndexViewModel GetIndex()
        {
            
            
            var model = new OperationLogIndexViewModel
            {
                OperationLogs = _operationLogService.getAllOpertionLogs(),
                Customers = _customerService.GetAllCustomers(),
                OperationObjects = _operationObjectService.GetOperationObjects(SessionFacade.CurrentCustomer.Id),
                OperationLogList = _operationLogService.getListForIndexPage(),
                OperationLogCategories = _operationLogCategoryService.GetOperationLogCategories(SessionFacade.CurrentCustomer.Id),
                OLSearch_Filter = new OperationLogSearch ()

            };

            return model;
        }
    }
}
