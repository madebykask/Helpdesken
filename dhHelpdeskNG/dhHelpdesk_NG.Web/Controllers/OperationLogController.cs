using System.Web.Mvc;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class OperationLogController : BaseController
    {
        private readonly IOperationLogService _operationLogService;
        private readonly ICustomerService _customerService;
        private readonly IOperationLogCategoryService _operationLogCategoryService;
        private readonly IOperationObjectService _operationObjectService;

        public OperationLogController(
            IOperationLogService operationLogService,
            ICustomerService customerService,
            IOperationObjectService operationObjectService,
            IOperationLogCategoryService operationLogCategoryService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _operationLogService = operationLogService;
            _customerService = customerService;
            _operationLogCategoryService = operationLogCategoryService;
            _operationObjectService = operationObjectService;
        }

        public ActionResult Index()
        {
            var model = GetIndex();

            return View(model);
        }

        private OperationLogIndexViewModel GetIndex()
        {
            var model = new OperationLogIndexViewModel
            {
                OperationLogs = _operationLogService.getAllOpertionLogs(),
                Customers = _customerService.GetAllCustomers(),
                OperationObjects = _operationObjectService.GetOperationObjects(SessionFacade.CurrentCustomer.Id),
                OperationLogList = _operationLogService.getListForIndexPage()
            };

            return model;
        }
    }
}
