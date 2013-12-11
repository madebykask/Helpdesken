using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;  
namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class GlobalSettingController : BaseController
    {
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IHolidayService _holidayService;
        private readonly ILanguageService _languageService;
        private readonly ITextTranslationService _textTranslationService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;

        public GlobalSettingController(
            IGlobalSettingService globalSettingService,
            IHolidayService holidayService,
            ILanguageService languageService,
            ITextTranslationService textTranslationService,
            IWatchDateCalendarService watchDateCalendarService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _globalSettingService = globalSettingService;
            _holidayService = holidayService;
            _languageService = languageService;
            _textTranslationService = textTranslationService;
            _watchDateCalendarService = watchDateCalendarService;
        }

        public ActionResult Index()
        {
            var model = GetGSIndexViewModel();


            return View(model);
        }

        public ActionResult Change()
        {
            var gsetting = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            if (gsetting == null)
                return new HttpNotFoundResult("No global settings found...");

            var model = SaveGsInputViewModel(gsetting);

            SessionFacade.ActiveTab = "#tab1";

            return View(model);
        }

        [HttpPost]
        public ActionResult Change(int id, FormCollection coll)
        {
            var changeToSave = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            var b = TryUpdateModel(changeToSave, "globalsetting");

            var vmodel = SaveGsInputViewModel(changeToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _globalSettingService.SaveGlobalSetting(changeToSave, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "globalsetting");

            var model = SaveGsInputViewModel(changeToSave);
            model.Tabposition = coll["activeTab"];

            return View(model);
        }

        public ActionResult NewHoliday()
        {
            var model = SaveHolidayViewModel(new Holiday { HolidayDate = DateTime.Now, CreatedDate = DateTime.Now });

            SessionFacade.ActiveTab = "#tab1";

            return View(model);
        }

        [HttpPost]
        public ActionResult NewHoliday(GlobalSettingHolidayViewModel viewModel, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var holiday = returnWorkingHoursForNewSave(viewModel);

            holiday.HolidayHeader.Name = viewModel.ChangedHeaderName;

            viewModel.Holiday.CreatedDate = DateTime.Now;

            _holidayService.SaveHoliday(holiday, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "globalsetting");

            viewModel.Tabposition = coll["activeTab"];

            return View(viewModel.Holiday);
        }

        public ActionResult EditHoliday(int id)
        {
            var holiday = _holidayService.GetHoliday(id);

            if (holiday == null)
                return new HttpNotFoundResult("No holiday found...");

            var model = SaveHolidayViewModel(holiday);

            model.ChangedHeaderName = holiday.HolidayHeader.Name;
            SessionFacade.ActiveTab = "#tab2";

            return View(model);
        }

        [HttpPost]
        public ActionResult EditHoliday(int id, GlobalSettingHolidayViewModel vmodel, FormCollection coll)
        {
            var holidayToSave = _holidayService.GetHoliday(id);
            holidayToSave.TimeFrom = vmodel.TimeFrom;
            holidayToSave.TimeUntil = vmodel.TimeTil;

            if (holidayToSave.HolidayHeader.Name != vmodel.ChangedHeaderName)
            {
                holidayToSave.HolidayHeader.Name = vmodel.ChangedHeaderName;
            }

            var b = TryUpdateModel(holidayToSave, "holiday");
            var model = SaveHolidayViewModel(holidayToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _holidayService.SaveHoliday(holidayToSave, out errors);

            SessionFacade.ActiveTab = coll["activeTab"];

            if (errors.Count == 0)
                return RedirectToAction("index", "globalsetting");

            return View(model);
        }

        public ActionResult NewWatchDate()
        {
            var model = SaveWatchDateViewModel(new WatchDateCalendarValue { CreatedDate = DateTime.Now, WatchDate = DateTime.Now });

            SessionFacade.ActiveTab = "#tab3";

            return View(model);
        }

        [HttpPost]
        public ActionResult NewWatchDate(GlobalSettingWatchDateViewModel viewModel, WatchDateCalendarValue watchDateCalendarValue, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            watchDateCalendarValue.WatchDateCalendar.Name = viewModel.ChangedWatchDateName;
            watchDateCalendarValue.CreatedDate = DateTime.Now;
            watchDateCalendarValue.WatchDateCalendar.CreatedDate = DateTime.Now;

            _watchDateCalendarService.SaveWatchDateCalendarValue(watchDateCalendarValue, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "globalsetting");

            SessionFacade.ActiveTab = coll["activeTab"];

            return View(viewModel.WatchDateCalendarValue);
        }

        public ActionResult EditWatchDate(int id)
        {
            var wdValue = _watchDateCalendarService.GetWatchDateCalendarValue(id);

            if (wdValue == null)
                return new HttpNotFoundResult("No watch date value found...");

            var model = SaveWatchDateViewModel(wdValue);

            SessionFacade.ActiveTab = "#tab3";

            model.ChangedWatchDateName = wdValue.WatchDateCalendar.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditWatchDate(int id, GlobalSettingWatchDateViewModel vModel, FormCollection coll)
        {
            var wdcValueToSave = _watchDateCalendarService.GetWatchDateCalendarValue(id);

            if (wdcValueToSave.WatchDateCalendar.Name != vModel.ChangedWatchDateName)
            {
                wdcValueToSave.WatchDateCalendar.Name = vModel.ChangedWatchDateName;
            }

            var b = TryUpdateModel(wdcValueToSave, "watchdatecalendarvalue");
            var model = SaveWatchDateViewModel(wdcValueToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _watchDateCalendarService.SaveWatchDateCalendarValue(wdcValueToSave, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "globalsetting");

            SessionFacade.ActiveTab = coll["activeTab"];

            return View(model);
        }

        public ActionResult NewTranslation()
        {
            var model = SaveTextTranslationViewModel(new Text { ChangedDate = DateTime.Now, CreatedDate = DateTime.Now });

            SessionFacade.ActiveTab = "#tab4";

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewTranslation(Text text, List<TextTranslationLanguageList> TTs, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (text == null)
                text = new Text() { };

            _textTranslationService.SaveNewText(text, TTs, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "globalsetting");

            var model = SaveTextTranslationViewModel(text);

            SessionFacade.ActiveTab = coll["activeTab"];

            return View(model);
        }

        public ActionResult EditTranslation(int id)
        {
            var text = _textTranslationService.GetText(id);

            if (text == null)
                return new HttpNotFoundResult("No translation found...");

            var model = SaveTextTranslationViewModel(text);

            SessionFacade.ActiveTab = "#tab4";

            return View(model);
        }

        [HttpPost]
        public ActionResult EditTranslation(int id, List<TextTranslationLanguageList> TTs, FormCollection coll)
        {
            var textToSave = _textTranslationService.GetText(id);

            if (textToSave == null)
                throw new Exception("No text found...");

            var b = TryUpdateModel(textToSave, "text");

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _textTranslationService.SaveEditText(textToSave, TTs, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "globalsetting");

            var model = SaveTextTranslationViewModel(textToSave);

            model.Tabposition = coll["activeTab"];

            return View(model);
        }

        private GlobalSettingIndexViewModel GetGSIndexViewModel()
        {
            var start = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            var model = new GlobalSettingIndexViewModel
            {
                GlobalSettings = _globalSettingService.GetGlobalSettings(),
                Holidays = _holidayService.GetAll().ToList(),
                LanguagesToTranslateInto = _languageService.GetLanguagesForGlobalSettings(),
                Texts = _textTranslationService.GetAllTexts().ToList(),
                ListForIndex = _textTranslationService.GetIndexListToTextTranslations(),
                WatchDateCalendarValues = _watchDateCalendarService.GetAllWatchDateCalendarValues().ToList(),
            };

            return model;
        }

        private GlobalSettingInputViewModel SaveGsInputViewModel(GlobalSetting globalSetting)
        {
            var model = new GlobalSettingInputViewModel
            {
                GlobalSetting = globalSetting,
                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }

        private GlobalSettingHolidayViewModel SaveHolidayViewModel(Holiday holiday)
        {
            #region SelectListItems

            List<SelectListItem> li = new List<SelectListItem>();
            for (int i = 00; i < 24; i++)
            {
                li.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            List<SelectListItem> lis = new List<SelectListItem>();
            for (int i = 00; i < 24; i++)
            {
                lis.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            #endregion

            var model = new GlobalSettingHolidayViewModel
            {
                Holiday = holiday,
                HolidayHeaders = _holidayService.GetHolidayHeaders().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                TimeFromList = li,
                TimeTilList = lis,
            };

            #region SetInts

            if (holiday.TimeFrom == 0)
            {
                model.HalfDay = false;
                model.TimeFrom = 0;
                model.TimeTil = 0;
            }
            else
            {
                model.HalfDay = true;
                model.TimeFrom = holiday.TimeFrom;
                model.TimeTil = holiday.TimeUntil;
            }

            #endregion

            return model;
        }

        private GlobalSettingWatchDateViewModel SaveWatchDateViewModel(WatchDateCalendarValue watchDateCalendarValue)
        {
            var model = new GlobalSettingWatchDateViewModel
            {
                WatchDateCalendarValue = watchDateCalendarValue,
                WatchDateCalendars = _watchDateCalendarService.GetAllWatchDateCalendars().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }

        private GlobalSettingTextTranslationViewModel SaveTextTranslationViewModel(Text text)
        {
            var model = new GlobalSettingTextTranslationViewModel
            {
                ListForEdit = _textTranslationService.GetEditListToTextTranslations(text.Id),
                ListForNew = _textTranslationService.GetNewListToTextTranslations(),
                Text = text,
                TextTranslations = _textTranslationService.GetAllTextTranslations()
            };

            return model;
        }

        private Holiday returnWorkingHoursForNewSave(GlobalSettingHolidayViewModel viewModel)
        {
            var holiday = viewModel.Holiday;

            if (viewModel.TimeFrom == 0)
            {
                holiday.TimeFrom = 0;
                holiday.TimeUntil = 0;
            }
            else
            {
                holiday.TimeFrom = viewModel.TimeFrom;
                holiday.TimeUntil = viewModel.TimeTil;
            }

            return holiday;
        }

        public string ChangeHolidayHeader(int id)
        {
            var headerNameToChange = _holidayService.GetHolidayHeader(id);

            if (headerNameToChange == null)
                return string.Empty;

            return headerNameToChange.Name;
        }

        public string ChangeHolidayList(int id)
        {
            var list = _holidayService.GetAll().Where(x => x.HolidayHeader_Id == id);
            var str = RenderRazorViewToString("_HolidayList", list.ToList());

            return str;
        }

        public string ChangeWatchDate(int id)
        {
            var watchDateToChange = _watchDateCalendarService.GetWatchDateCalendar(id);

            if (watchDateToChange == null)
                return string.Empty;

            return watchDateToChange.Name;
        }

        public string ChangeWatchDateList(int id)
        {
            var list = _watchDateCalendarService.GetAllWatchDateCalendarValues().Where(x => x.WatchDateCalendar_Id == id);
            var str = RenderRazorViewToString("_WatchDateList", list.ToList());

            return str;
        }
    }
}