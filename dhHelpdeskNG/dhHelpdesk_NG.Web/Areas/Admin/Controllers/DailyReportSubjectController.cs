namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class DailyReportSubjectController : BaseController
    {
        private readonly IDailyReportService _dailyReportService;
        private readonly ICustomerService _customerService;

        public DailyReportSubjectController(
            IDailyReportService dailyReportService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._dailyReportService = dailyReportService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var dailyReportSubjects = this._dailyReportService.GetDailyReportSubjects(customer.Id);


            var model = new DailyReportSubjectIndexViewModel { DailyReportSubjects = dailyReportSubjects, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var dailyReportSubejct = new DailyReportSubject { Customer_Id = customer.Id, IsActive = 1 };
            var model = this.CreateInputViewModel(dailyReportSubejct, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(DailyReportSubject dailyReportSubject, DailyReportSubjectInputViewModel vmodel)
        {
            dailyReportSubject.ShowOnStartPage = this.returnDailyReportSubjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._dailyReportService.SaveDailyReportSubject(dailyReportSubject, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "dailyreportsubject", new { customerId = dailyReportSubject.Customer_Id });

            var customer = this._customerService.GetCustomer(dailyReportSubject.Customer_Id);
            var model = this.CreateInputViewModel(dailyReportSubject, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var dailyReportSubject = this._dailyReportService.GetDailyReportSubject(id);

            if (dailyReportSubject == null)
                return new HttpNotFoundResult("No daily report subject found...");

            var customer = this._customerService.GetCustomer(dailyReportSubject.Customer_Id);
            var model = this.CreateInputViewModel(dailyReportSubject, customer);
            model.StartPageShow = model.DailyReportSubject.ShowOnStartPage;

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, DailyReportSubjectInputViewModel vmodel)
        {
            var dailyReportSubjectToSave = this._dailyReportService.GetDailyReportSubject(id);
            var b = this.TryUpdateModel(dailyReportSubjectToSave, "dailyreportsubject");

            dailyReportSubjectToSave.ShowOnStartPage = this.returnDailyReportSubjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._dailyReportService.SaveDailyReportSubject(dailyReportSubjectToSave, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "dailyreportsubject", new { customerId = dailyReportSubjectToSave.Customer_Id });

            var customer = this._customerService.GetCustomer(dailyReportSubjectToSave.Customer_Id);
            var model = this.CreateInputViewModel(dailyReportSubjectToSave, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var dailyReportSubject = this._dailyReportService.GetDailyReportSubject(id);

            if (this._dailyReportService.DeleteDailyReportSubject(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "dailyreportsubject", new { customerId = dailyReportSubject.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "dailyreportsubject", new { area = "admin", id = dailyReportSubject.Id });
            }
        }

        private DailyReportSubjectInputViewModel CreateInputViewModel(DailyReportSubject dailyReportSubject, Customer customer)
        {
            #region selectlistitem

            List<SelectListItem> sl = new List<SelectListItem>();
            for (int i = 1; i < 10; i++)
            {
                sl.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            #endregion

            #region model

            var model = new DailyReportSubjectInputViewModel
            {
                DailyReportSubject = dailyReportSubject,
                Customer = customer,
                NumberToShowOnStartPage = sl
            };

            #endregion

            #region ints

            if (dailyReportSubject.ShowOnStartPage == 0)
            {
                model.ShowYesNo = 0;
            }
            else
            {
                model.ShowYesNo = 1;
            }

            #endregion

            return model;
        }

        private int returnDailyReportSubjectForSave(DailyReportSubjectInputViewModel model)
        {
            var show = model.DailyReportSubject.ShowOnStartPage;

            if (model.ShowYesNo == 0)
            {
                show = 0;
            }
            else
            {
                show = model.StartPageShow;
            }

            return show;
        }
    }
}
