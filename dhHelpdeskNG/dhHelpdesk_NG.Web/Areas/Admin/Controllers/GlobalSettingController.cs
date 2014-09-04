namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

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
            this._globalSettingService = globalSettingService;
            this._holidayService = holidayService;
            this._languageService = languageService;
            this._textTranslationService = textTranslationService;
            this._watchDateCalendarService = watchDateCalendarService;
        }

        public ActionResult Index(FormCollection coll)
        {
            var model = this.GetGSIndexViewModel(1, SessionFacade.CurrentCustomer.Language_Id);

            model.Tabposition = coll["activeTab"];
            return this.View(model);
        }

        public ActionResult Change()
        {
            var gsetting = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            if (gsetting == null)
                return new HttpNotFoundResult("No global settings found...");

            var model = this.SaveGsInputViewModel(gsetting);

            SessionFacade.ActiveTab = "#tab1";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Change(int id, FormCollection coll)
        {
            var changeToSave = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            var b = this.TryUpdateModel(changeToSave, "globalsetting");

            var vmodel = this.SaveGsInputViewModel(changeToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._globalSettingService.SaveGlobalSetting(changeToSave, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting");

            var model = this.SaveGsInputViewModel(changeToSave);
            model.Tabposition = coll["activeTab"];

            return this.View(model);
        }

        public ActionResult NewHoliday()
        {
            var model = this.SaveHolidayViewModel(new Holiday { HolidayDate = DateTime.Now, CreatedDate = DateTime.Now });

            SessionFacade.ActiveTab = "#tab1";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult NewHoliday(GlobalSettingHolidayViewModel viewModel, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var holiday = this.returnWorkingHoursForNewSave(viewModel);

            holiday.HolidayHeader.Name = viewModel.ChangedHeaderName;

            viewModel.Holiday.CreatedDate = DateTime.Now;

            this._holidayService.SaveHoliday(holiday, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting");

            viewModel.Tabposition = coll["activeTab"];

            return this.View(viewModel.Holiday);
        }

        public ActionResult EditHoliday(int id)
        {
            var holiday = this._holidayService.GetHoliday(id);

            if (holiday == null)
                return new HttpNotFoundResult("No holiday found...");

            var model = this.SaveHolidayViewModel(holiday);

            model.ChangedHeaderName = holiday.HolidayHeader.Name;
            SessionFacade.ActiveTab = "#tab2";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditHoliday(int id, GlobalSettingHolidayViewModel vmodel, FormCollection coll)
        {
            var holidayToSave = this._holidayService.GetHoliday(id);
            holidayToSave.TimeFrom = vmodel.TimeFrom;
            holidayToSave.TimeUntil = vmodel.TimeTil;

            if (holidayToSave.HolidayHeader.Name != vmodel.ChangedHeaderName)
            {
                holidayToSave.HolidayHeader.Name = vmodel.ChangedHeaderName;
            }

            var b = this.TryUpdateModel(holidayToSave, "holiday");
            var model = this.SaveHolidayViewModel(holidayToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._holidayService.SaveHoliday(holidayToSave, out errors);

            SessionFacade.ActiveTab = coll["activeTab"];

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting");

            return this.View(model);
        }

        public ActionResult NewWatchDate()
        {
            var model = this.SaveWatchDateViewModel(new WatchDateCalendarValue { CreatedDate = DateTime.Now, WatchDate = DateTime.Now });

            SessionFacade.ActiveTab = "#tab3";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult NewWatchDate(GlobalSettingWatchDateViewModel viewModel, WatchDateCalendarValue watchDateCalendarValue, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            watchDateCalendarValue.WatchDateCalendar.Name = viewModel.ChangedWatchDateName;
            watchDateCalendarValue.CreatedDate = DateTime.Now;
            watchDateCalendarValue.WatchDateCalendar.CreatedDate = DateTime.Now;

            this._watchDateCalendarService.SaveWatchDateCalendarValue(watchDateCalendarValue, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting");

            SessionFacade.ActiveTab = coll["activeTab"];

            return this.View(viewModel.WatchDateCalendarValue);
        }

        public ActionResult EditWatchDate(int id)
        {
            var wdValue = this._watchDateCalendarService.GetWatchDateCalendarValue(id);

            if (wdValue == null)
                return new HttpNotFoundResult("No watch date value found...");

            var model = this.SaveWatchDateViewModel(wdValue);

            SessionFacade.ActiveTab = "#tab3";

            model.ChangedWatchDateName = wdValue.WatchDateCalendar.Name;

            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditWatchDate(int id, GlobalSettingWatchDateViewModel vModel, FormCollection coll)
        {
            var wdcValueToSave = this._watchDateCalendarService.GetWatchDateCalendarValue(id);

            if (wdcValueToSave.WatchDateCalendar.Name != vModel.ChangedWatchDateName)
            {
                wdcValueToSave.WatchDateCalendar.Name = vModel.ChangedWatchDateName;
            }

            var b = this.TryUpdateModel(wdcValueToSave, "watchdatecalendarvalue");
            var model = this.SaveWatchDateViewModel(wdcValueToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._watchDateCalendarService.SaveWatchDateCalendarValue(wdcValueToSave, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting");

            SessionFacade.ActiveTab = coll["activeTab"];

            return this.View(model);
        }

        public ActionResult NewTranslation()
        {
            var model = this.SaveTextTranslationViewModel(new Text { }, SessionFacade.CurrentCustomer.Language_Id);

            SessionFacade.ActiveTab = "#tab4";

            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewTranslation(Text text, GlobalSettingTextTranslationViewModel vmodel, FormCollection coll)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (text == null)
                text = new Text() { };

            text.Type = vmodel.TextType.Id;

            this._textTranslationService.SaveNewText(text, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting");

            var model = this.SaveTextTranslationViewModel(text, 0);

            SessionFacade.ActiveTab = coll["activeTab"];

            return this.View(model);
        }
        //public ActionResult NewTranslation(Text text, List<TextTranslationLanguageList> TTs, GlobalSettingTextTranslationViewModel vmodel, FormCollection coll)
        //{
        //    IDictionary<string, string> errors = new Dictionary<string, string>();

        //    if (text == null)
        //        text = new Text() { };

           
        //    this._textTranslationService.SaveNewText(text, TTs, out errors);

        //    if (errors.Count == 0)
        //        return this.RedirectToAction("index", "globalsetting");

        //    var model = this.SaveTextTranslationViewModel(text, 0);

        //    SessionFacade.ActiveTab = coll["activeTab"];

        //    return this.View(model);
        //}

        public ActionResult EditTranslation(int id)
        {
            var text = this._textTranslationService.GetText(id);
            var language = this._languageService.GetLanguage(SessionFacade.CurrentCustomer.Language_Id);
            if (text == null)
                return new HttpNotFoundResult("No translation found...");

            var model = this.SaveTextTranslationViewModel(text, language.Id);

            SessionFacade.ActiveTab = "#tab4";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditTranslation(int id, List<TextTranslationLanguageList> TTs, FormCollection coll)
        {
            var textToSave = this._textTranslationService.GetText(id);

            if (textToSave == null)
                throw new Exception("No text found...");

            var b = this.TryUpdateModel(textToSave, "text");

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._textTranslationService.SaveEditText(textToSave, TTs, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting");

            var model = this.SaveTextTranslationViewModel(textToSave, 0);

            model.Tabposition = coll["activeTab"];

            return this.View(model);
        }

        //public ActionResult EditTranslation(int id, TextTranslation tt, GlobalSettingTextTranslationViewModel vmodel, FormCollection coll, int languageId)
        //{
        //    var textToSave = this._textTranslationService.GetText(id);
        //    var texttranslationToSave = this._textTranslationService.GetTextTranslationByIdAndLanguage(textToSave.Id, vmodel.Language.Id);

        //    if (textToSave == null)
        //        throw new Exception("No text found...");

        //    var b = this.TryUpdateModel(textToSave, "text");

        //    var update = true;

        //    IDictionary<string, string> errors = new Dictionary<string, string>();

        //    if (texttranslationToSave == null)
        //    {
        //        texttranslationToSave = new TextTranslation
        //        {
        //            TextTranslated = vmodel.TextTranslation.TextTranslated,
        //            Language_Id = vmodel.Language.Id,
        //            Text_Id = textToSave.Id
        //        };

        //        update = false;
        //    }
        //    else
        //    {
        //        texttranslationToSave.TextTranslated = vmodel.TextTranslation.TextTranslated;
        //        texttranslationToSave.Language_Id = vmodel.Language.Id;
        //        texttranslationToSave.Text_Id = textToSave.Id;

        //    }

        //    this._textTranslationService.SaveEditText(textToSave, texttranslationToSave, update, out errors);
        //    //this._textTranslationService.SaveEditText(textToSave, TTs, out errors);

        //    if (errors.Count == 0)
        //        return this.RedirectToAction("index", "globalsetting");

        //    var model = this.SaveTextTranslationViewModel(textToSave, vmodel.Language.Id);

        //    model.Tabposition = coll["activeTab"];

        //    return this.View(model);
        //}

        [HttpPost]
        public ActionResult Delete(int id)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var text = this._textTranslationService.GetText(id);
            var TextTranslations = this._textTranslationService.GetEditListToTextTranslations(text.Id);

            if (TextTranslations != null)
            {
                foreach (var tt in TextTranslations)
                {
                    var texttranslation = this._textTranslationService.GetTextTranslationById(tt.Text_Id);
                    this._textTranslationService.DeleteTextTranslation(texttranslation, out errors);
                }
                

            }

            this._textTranslationService.DeleteText(text, out errors);

            return this.RedirectToAction("index", "globalsetting");
        }

        private GlobalSettingIndexViewModel GetGSIndexViewModel(int texttypeid, int languageId)
        {
            var start = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            var model = new GlobalSettingIndexViewModel
            {
                GlobalSettings = this._globalSettingService.GetGlobalSettings(),
                Holidays = this._holidayService.GetAll().ToList(),
                LanguagesToTranslateInto = this._languageService.GetLanguagesForGlobalSettings(),
                Texts = this._textTranslationService.GetAllTexts(texttypeid).ToList(),
                ListForIndex = this._textTranslationService.GetIndexListToTextTranslations(languageId),
                WatchDateCalendarValues = this._watchDateCalendarService.GetAllWatchDateCalendarValues().ToList(),
                TextType = this._textTranslationService.GetTextType(1),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList()
            };

            model.TextTypes = new List<SelectListItem>();
            model.TextTypes.Add(new SelectListItem { Text = "Master data", Value = "1" });

            foreach (var textype in this._textTranslationService.GetTextTypes())
            {
                model.TextTypes.Add(new SelectListItem { Text = textype.Name, Value = textype.Id.ToString() });
            }

            return model;
        }

        private GlobalSettingInputViewModel SaveGsInputViewModel(GlobalSetting globalSetting)
        {
            var model = new GlobalSettingInputViewModel
            {
                GlobalSetting = globalSetting,
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
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
                HolidayHeaders = this._holidayService.GetHolidayHeaders().Select(x => new SelectListItem
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
                WatchDateCalendars = this._watchDateCalendarService.GetAllWatchDateCalendars().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }

        private GlobalSettingTextTranslationViewModel SaveTextTranslationViewModel(Text text, int languageId)
        {
            var model = new GlobalSettingTextTranslationViewModel
            {
                ListForEdit = this._textTranslationService.GetEditListToTextTranslations(text.Id),
                ListForNew = this._textTranslationService.GetNewListToTextTranslations(),
                Text = text,
                TextTranslations = this._textTranslationService.GetAllTextTranslations(),
                TextTranslation = this._textTranslationService.GetTextTranslationByIdAndLanguage(text.Id, languageId),
                Language = this._languageService.GetLanguage(languageId),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList()
            };

            model.TextTypes = new List<SelectListItem>();
            model.TextTypes.Add(new SelectListItem { Text = "Master data", Value = "1" });

            foreach (var textype in this._textTranslationService.GetTextTypes())
            {
                model.TextTypes.Add(new SelectListItem { Text = textype.Name, Value = textype.Id.ToString() });
            }

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
            var headerNameToChange = this._holidayService.GetHolidayHeader(id);

            if (headerNameToChange == null)
                return string.Empty;

            return headerNameToChange.Name;
        }

        public string ChangeHolidayList(int id)
        {
            var list = this._holidayService.GetAll().Where(x => x.HolidayHeader_Id == id);
            var str = this.RenderRazorViewToString("_HolidayList", list.ToList());

            return str;
        }

        public string ChangeWatchDate(int id)
        {
            var watchDateToChange = this._watchDateCalendarService.GetWatchDateCalendar(id);

            if (watchDateToChange == null)
                return string.Empty;

            return watchDateToChange.Name;
        }

        public string ChangeWatchDateList(int id)
        {
            var list = this._watchDateCalendarService.GetAllWatchDateCalendarValues().Where(x => x.WatchDateCalendar_Id == id);
            var str = this.RenderRazorViewToString("_WatchDateList", list.ToList());

            return str;
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string ChangeLabel(int id, int textId)
        {
            var text = this._textTranslationService.GetText(textId);
            var texttranslation = this._textTranslationService.GetTextTranslationByIdAndLanguage(textId, id);



            var model = this.SaveTextTranslationViewModel(text, id);

            var view = "~/areas/admin/views/GlobalSetting/_Translation.cshtml";
            //var view = "~/areas/admin/views/GlobalSetting/EditTranslation.cshtml";
            return this.RenderRazorViewToString(view, model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string ChangeTextType(int id)
        {

            var model = this.GetGSIndexViewModel(id, SessionFacade.CurrentCustomer.Language_Id);

            var view = "~/areas/admin/views/GlobalSetting/Index.cshtml";
            return this.RenderRazorViewToString(view, model);
        }
    }
}