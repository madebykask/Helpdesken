using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/casewatchdate")]
    //TODO: any permissions?
    public class CaseWatchDateController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;

        public CaseWatchDateController(IDepartmentService departmentService, IWatchDateCalendarService watchDateCalendarService)
        {
            _departmentService = departmentService;
            _watchDateCalendarService = watchDateCalendarService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int departmentId) //TODO: async
        {
            var dept = _departmentService.GetDepartment(departmentId);
            DateTime? res = null;
            if (dept?.WatchDateCalendar_Id != null)
                res = _watchDateCalendarService.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, DateTime.UtcNow);

            return Ok(res);
        }
    }
}