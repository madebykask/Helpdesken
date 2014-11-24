using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{
    using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
    using DH.Helpdesk.BusinessData.Models.DailyReport.Input;
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

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {            
            this._dailyReportService.DeleteDailyReport(id);
            return this.RedirectToAction("Index");
        }



    //    [HttpPost]
    //    public ActionResult Update(DailyReportOverview ) // DailyReportOutputModel
    //    {

    //        var model = this.CreateInputViewModel(new DailyReportOverview 
    //        (
    //            SessionFacade.CurrentUser.Id,
    //            0,
    //            null,
    //            DateTime.Now,
    //            null,
    //            null
    //        ));

    //        return this.View(model);

        [HttpGet]
        public ActionResult Update(IEnumerable<DailyReportOverview> dailyReport) 
        {
            //foreach (var m in dailyReport)
            //{
            //    if (m.DailyReportSubject.Id != 0)
            //    {
            //        var newDailyReport = new DailyReportUpdate(
            //        SessionFacade.CurrentCustomer.Id,
            //        m.Id,
            //        m.Sent,
            //        m.DailyReportSubject.Id,
            //        m.DailyReportText,
            //        SessionFacade.CurrentUser.Id,
            //        m.CreatedDate,
            //        DateTime.Now
            //        );
            //    }
            //}
            


            return this.RedirectToAction("Index"); 
       }

        //private DailyReportInputViewModel CreateInputViewModel(DailyReportOverview dailyReport)
        //{
        //    var model = new DailyReportInputViewModel
        //    {
        //        DailyReport = dailyReport,
        //        NewDailyreport = null
        //    };

        //    return model;
        //}

    }
}
