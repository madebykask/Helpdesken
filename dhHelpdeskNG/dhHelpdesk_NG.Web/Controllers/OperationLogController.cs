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
            this._operationLogService = operationLogService;
            this._customerService = customerService;
            this._operationLogCategoryService = operationLogCategoryService;
            this._operationObjectService = operationObjectService;
            this._workingGroupService = workingGroupService;
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
                CS.Ascending = true;
                SessionFacade.CurrentOperationLogSearch = CS;
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
            CS.Text_Filter = OLSearch_Filter.Text_Filter;
           
            var c = this._operationLogService.SearchAndGenerateOperationLog(SessionFacade.CurrentCustomer.Id, CS );
            
            if (OLSearch_Filter != null)
                SessionFacade.CurrentOperationLogSearch = CS;

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
            var model = this.OperationLogInputViewModel(new OperationLog { Customer_Id = SessionFacade.CurrentCustomer.Id });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(OperationLog operationlog, int[] WGsSelected, int OperationLogHour, int OperationLogMinute, int chkSecurity)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            operationlog.User_Id = SessionFacade.CurrentUser.Id;
            operationlog.WorkingTime = (OperationLogHour * 60) + OperationLogMinute;
            if (chkSecurity == 0)
                WGsSelected = null;

            this._operationLogService.SaveOperationLog(operationlog, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "operationlog");

            var model = this.OperationLogInputViewModel(operationlog);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var operationlog = this._operationLogService.GetOperationLog(id);


            if (operationlog == null)
                return new HttpNotFoundResult("No OperationLog found...");

            var model = this.OperationLogInputViewModel(operationlog);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, OperationLog operationlog, int[] WGsSelected, int OperationLogHour, int OperationLogMinute,int chkSecurity)
        {
            var operationlogToSave = this._operationLogService.GetOperationLog(id);
            this.UpdateModel(operationlogToSave, "OperationLog");

            IDictionary<string, string> errors = new Dictionary<string, string>();

            operationlogToSave.User_Id = SessionFacade.CurrentUser.Id;
            operationlogToSave.WorkingTime = (OperationLogHour * 60) + OperationLogMinute;
            
            if (chkSecurity==0) 
               WGsSelected = null;
            this._operationLogService.SaveOperationLog(operationlogToSave, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "operationlog");
            
            var model = this.OperationLogInputViewModel(operationlogToSave);
            
            return this.View(model);
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
   
        private OperationLogInputViewModel OperationLogInputViewModel(OperationLog operationlog)
        {
            var wgsSelected = operationlog.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();

            foreach (var wg in this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id))
            {                
                if (!wgsSelected.Contains(wg))
                    wgsAvailable.Add(wg);
            }

            var model = new OperationLogInputViewModel
            {
                
                OperationLog = operationlog ,

                OperationObjects = this._operationObjectService.GetOperationObjects(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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

                OperationLogCategories = this._operationLogCategoryService.GetOperationLogCategories (SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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
                OperationLogs = this._operationLogService.GetAllOpertionLogs(),
                Customers = this._customerService.GetAllCustomers(),
                OperationObjects = this._operationObjectService.GetOperationObjects(SessionFacade.CurrentCustomer.Id),
                OperationLogList = this._operationLogService.GetListForIndexPage(),
                OperationLogCategories = this._operationLogCategoryService.GetOperationLogCategories(SessionFacade.CurrentCustomer.Id),
                OLSearch_Filter = new OperationLogSearch ()
            };

            return model;
        }
    }
}
