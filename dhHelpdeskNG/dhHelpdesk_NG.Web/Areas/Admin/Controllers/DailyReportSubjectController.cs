using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
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
            _dailyReportService = dailyReportService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var dailyReportSubjects = _dailyReportService.GetDailyReportSubjects(customer.Id);


            var model = new DailyReportSubjectIndexViewModel { DailyReportSubjects = dailyReportSubjects, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var dailyReportSubejct = new DailyReportSubject { Customer_Id = customer.Id, IsActive = 1 };
            var model = CreateInputViewModel(dailyReportSubejct, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(DailyReportSubject dailyReportSubject, DailyReportSubjectInputViewModel vmodel)
        {
            dailyReportSubject.ShowOnStartPage = returnDailyReportSubjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _dailyReportService.SaveDailyReportSubject(dailyReportSubject, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "dailyreportsubject", new { customerId = dailyReportSubject.Customer_Id });

            var customer = _customerService.GetCustomer(dailyReportSubject.Customer_Id);
            var model = CreateInputViewModel(dailyReportSubject, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var dailyReportSubject = _dailyReportService.GetDailyReportSubject(id);

            if (dailyReportSubject == null)
                return new HttpNotFoundResult("No daily report subject found...");

            var customer = _customerService.GetCustomer(dailyReportSubject.Customer_Id);
            var model = CreateInputViewModel(dailyReportSubject, customer);
            model.StartPageShow = model.DailyReportSubject.ShowOnStartPage;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, DailyReportSubjectInputViewModel vmodel)
        {
            var dailyReportSubjectToSave = _dailyReportService.GetDailyReportSubject(id);
            var b = TryUpdateModel(dailyReportSubjectToSave, "dailyreportsubject");

            dailyReportSubjectToSave.ShowOnStartPage = returnDailyReportSubjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _dailyReportService.SaveDailyReportSubject(dailyReportSubjectToSave, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "dailyreportsubject", new { customerId = dailyReportSubjectToSave.Customer_Id });

            var customer = _customerService.GetCustomer(dailyReportSubjectToSave.Customer_Id);
            var model = CreateInputViewModel(dailyReportSubjectToSave, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var dailyReportSubject = _dailyReportService.GetDailyReportSubject(id);

            if (_dailyReportService.DeleteDailyReportSubject(id) == DeleteMessage.Success)
                return RedirectToAction("index", "dailyreportsubject", new { customerId = dailyReportSubject.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "dailyreportsubject", new { area = "admin", id = dailyReportSubject.Id });
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
