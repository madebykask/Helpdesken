using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{
    using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Models.DailyReports;

    public class DailyReportController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IDailyReportService _dailyReportService;
    
        public DailyReportController(      
            IUserService userService,
            IDailyReportService dailyReportService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._userService = userService;
            this._dailyReportService = dailyReportService;
        }
        //
        // GET: /DailyReport/

        /// <summary>
        /// The index view model.
        /// </summary>
        /// <returns>
        /// The <see cref="DailyReportOutputModel"/>.
        /// </returns>
        private DailyReportOutputModel IndexViewModel()
        {
            var model = new DailyReportOutputModel();
           
            return model;
        }


        public ActionResult Index()
        {
            var model = this.IndexViewModel();

            model.Subjects = this._dailyReportService.GetDailyReportSubjects(SessionFacade.CurrentCustomer.Id).ToList();
            model.Reports = this._dailyReportService.GetDailyReports(SessionFacade.CurrentCustomer.Id).Where(d => d.CreatedDate.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();

            return this.View(model);
        }


    }
}
