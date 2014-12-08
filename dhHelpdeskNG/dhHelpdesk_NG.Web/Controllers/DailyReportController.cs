using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure.Extensions;

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


        public ActionResult Index()
        {
            var model = CreateDailyReportModel();
                    
            return this.View(model);
        }


        [HttpPost]
        public ActionResult Save(List<DailyReportInputModel> DailyReports) 
        {
            for (int i = 0; i < DailyReports.Count(); i++)
            {
              
                var updatedDailyReport = new DailyReportUpdate
                (
                    SessionFacade.CurrentCustomer.Id,
                    DailyReports[i].Id,
                    DailyReports[i].Sent,
                    DailyReports[i].DailyReportSubjectId,
                    (DailyReports[i].DailyReportText == null ? " " : DailyReports[i].DailyReportText),
                    SessionFacade.CurrentUser.Id,
                    DailyReports[i].CreatedDate,
                    DateTime.Now
               );

                if (updatedDailyReport.DailyReportSubjectId != 0)
                                        
                    this._dailyReportService.SaveDailyReport(updatedDailyReport);                
            }

            return this.RedirectToAction("Index");          
        }

   

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {            
            this._dailyReportService.DeleteDailyReport(id);
            return this.RedirectToAction("Index");
        }


      
        [HttpPost]
        public PartialViewResult Search(DailyReportHistoryModel historyDailyReports)
        {
            
            var toDate = (historyDailyReports.ReportsTo == null ? DateTime.Now : historyDailyReports.ReportsTo);

            toDate = toDate.Value.AddDays(1);

            var searchedReports = this._dailyReportService.GetDailyReports(SessionFacade.CurrentCustomer.Id)
                                 .Where(d => d.CreatedDate >= historyDailyReports.ReportsFrom &&  d.CreatedDate <= (toDate))
                                 .OrderBy(d => d.CreatedDate).ToList();


            return this.PartialView("_HistoryOverviewData", searchedReports);                              
        }


        private DailyReportModel IndexViewModel()
        {
            var model = new DailyReportModel();

            return model;
        }

        private DailyReportModel CreateDailyReportModel()
        {
            var model = new DailyReportModel();

            var allSubjects = this._dailyReportService.GetAllDailyReportSubjects(SessionFacade.CurrentCustomer.Id).ToList();

            var todayReports = this._dailyReportService.GetDailyReports(SessionFacade.CurrentCustomer.Id)
                                      .Where(d => d.CreatedDate.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderBy(d=> d.CreatedDate).ToList();

            var inputModel = todayReports.Select(r => new DailyReportInputModel
                      (   r.Id,
                          r.Sent,
                          r.UserName,
                          r.CreatedDate,
                          r.DailyReportSubject.Id,
                          r.DailyReportText,
                          r.FirstName,
                          r.LastName,
                          allSubjects
                      )).ToList();

            model.InputModels = inputModel;

            var emptyChoice = new DailyReportSubjectBM(0,0,1,0,"",DateTime.Now,DateTime.Now);
            allSubjects.Insert(0, emptyChoice);
  
            var newInput = new DailyReportInputModel ( 0, 0, SessionFacade.CurrentUser.UserId,
                                                        DateTime.Now, 0, " ", "", "", allSubjects);
            model.InputModels.Insert(0, newInput);


            //var activeTab = SessionFacade.FindActiveTab("DailyReport");
            //activeTab = (activeTab == null) ? "DailyTab" : activeTab;


            model.HistoryModel = new DailyReportHistoryModel()
            {
                Reports = this._dailyReportService.GetDailyReports(SessionFacade.CurrentCustomer.Id).Where(d => d.CreatedDate.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList(),
                ReportsFrom = DateTime.Now,
                ReportsTo = null
            };
           
            return model;
        }

    }
}
