using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class StatisticsController : BaseController
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ICustomerUserService _customerUserService;


        public StatisticsController(IMasterDataService masterDataService,
            IStatisticsService statisticsService, 
            ICustomerUserService customerUserService): base(masterDataService)
        {
            _statisticsService = statisticsService;
            _customerUserService = customerUserService;
        }

        public JsonResult GetStatistics()
        {
            var customerIdsAll = _customerUserService.GetCustomerUsersForHomeIndexPage(SessionFacade.CurrentUser.Id)
                .Select(c => c.Customer.Customer_Id).ToArray();

            var model = _statisticsService.GetStatistics(customerIdsAll, SessionFacade.CurrentUser);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}