
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.Settings;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models.FileViewLogs;
using DH.Helpdesk.Web.Models.Gdpr;
//
namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.BusinessData.Enums.BusinessRules;
    using DH.Helpdesk.Services.BusinessLogic.Gdpr;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;

    public class GlobalSettingController : BaseController
    {
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IHolidayService _holidayService;
        private readonly ILanguageService _languageService;
        private readonly ITextTranslationService _textTranslationService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IGDPRDataPrivacyAccessService _gdprDataPrivacyAccessService;
        private readonly IGDPROperationsService _gdprOperationsService;
        private readonly IGDPRFavoritesService _gdprFavoritesService;
        private readonly IGDPRDataPrivacyCasesService _gdprPrivacyCasesService;
        private readonly IUserContext _userContext;
        private readonly IGDPRTasksService _gdprTasksService;
        private readonly IFileViewLogService _fileViewLogService;
        private readonly IDepartmentService _departmentsService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;


        public GlobalSettingController(
            IGlobalSettingService globalSettingService,
            IHolidayService holidayService,
            ILanguageService languageService,
            ITextTranslationService textTranslationService,
            IWatchDateCalendarService watchDateCalendarService,
            ICustomerUserService customerUserService,
            ICaseFieldSettingService caseFieldSettingService,
            IGDPROperationsService gdprOperationsService,
            IGDPRDataPrivacyAccessService gdprDataPrivacyAccessService,
            IMasterDataService masterDataService,
            IGDPRFavoritesService gdprFavoritesService,
            IGDPRTasksService gdprTasksService,
            IUserContext userContext,
            IFileViewLogService fileViewLogService,
            IDepartmentService departmentsService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            IGDPRDataPrivacyCasesService gdprPrivacyCasesService)
            : base(masterDataService)
        {
            _gdprTasksService = gdprTasksService;
            _gdprFavoritesService = gdprFavoritesService;
            _globalSettingService = globalSettingService;
            _holidayService = holidayService;
            _languageService = languageService;
            _textTranslationService = textTranslationService;
            _watchDateCalendarService = watchDateCalendarService;
            _customerUserService = customerUserService;
            _caseFieldSettingService = caseFieldSettingService;
            _gdprDataPrivacyAccessService = gdprDataPrivacyAccessService;
            _userContext = userContext;
            _gdprOperationsService = gdprOperationsService;
            _fileViewLogService = fileViewLogService;
            _departmentsService = departmentsService;
            _caseTypeService = caseTypeService;
            _productAreaService = productAreaService;
            _gdprPrivacyCasesService = gdprPrivacyCasesService;
        }
        [ValidateInput(false)]
        public ActionResult Index(int texttypeid, string textSearch, int compareMethod)
        {
            var searchOpt = new SearchOption { TextType = texttypeid, TextSearch = textSearch, CompareMethod = compareMethod };
            var model = this.GetGSIndexViewModel(1, SessionFacade.CurrentCustomer.Language_Id, searchOpt);

            return this.View(model);
        }

        public ActionResult Change()
        {
            var gsetting = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            if (gsetting == null)
                return new HttpNotFoundResult("No global settings found...");

            var model = this.SaveGsInputViewModel(gsetting);

            SessionFacade.ActiveTab = "#fragment-1";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Change(int id, FormCollection coll)
        {
            var changeToSave = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            var b = this.TryUpdateModel(changeToSave, "globalsetting");

            if (string.IsNullOrEmpty(changeToSave.ApplicationName))
                changeToSave.ApplicationName = "";

            var vmodel = this.SaveGsInputViewModel(changeToSave);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._globalSettingService.SaveGlobalSetting(changeToSave, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting", new { texttypeid = 0, compareMethod = 1 });

            var model = this.SaveGsInputViewModel(changeToSave);
            model.Tabposition = coll["activeTab"];

            return this.View(model);
        }

        public ActionResult NewHoliday()
        {
            var year = DateTime.Today.Year;
            var model = this.SaveHolidayViewModel(new HolidayHeader { }, year);

            SessionFacade.ActiveTab = "#fragment-2";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult NewHoliday(int id, GlobalSettingHolidayViewModel viewModel, FormCollection coll)
        {

            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (id == 0)
            {
                var holidayheader = new HolidayHeader { Name = viewModel.ChangedHeaderName };

                this._holidayService.SaveHolidayHeader(holidayheader, out errors);

                if (errors.Count == 0)
                    return this.RedirectToAction("index", "globalsetting", new { texttypeid = 0, compareMethod = 1 });

                var model = this.SaveHolidayViewModel(holidayheader, viewModel.Year);
                SessionFacade.ActiveTab = coll["activeTab"];

                return this.View(model);
            }
            else
            {
                var holidayheader = this._holidayService.GetHolidayHeader(id);

                holidayheader.Name = viewModel.ChangedHeaderName;
                this._holidayService.SaveHolidayHeader(holidayheader, out errors);

                if (errors.Count == 0)
                    return this.RedirectToAction("editholiday", "globalsetting", new { id = id });

                var model = this.SaveHolidayViewModel(holidayheader, viewModel.Year);
                SessionFacade.ActiveTab = coll["activeTab"];

                return this.View(model);

            }
        }

        public ActionResult EditHoliday(int id, DateTime? holidayDate = null)
        {
            //var holiday = this._holidayService.GetHoliday(id);
            var holidayheader = this._holidayService.GetHolidayHeader(id);

            if (holidayheader == null)
                return new HttpNotFoundResult("No holiday found...");

            var year = holidayDate.HasValue ? holidayDate.Value.Year : DateTime.Today.Year;

            var model = this.SaveHolidayViewModel(holidayheader, year);


            //model.ChangedHeaderName = holiday.HolidayHeader.Name;
            SessionFacade.ActiveTab = "#fragment-2";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditHoliday(int id, GlobalSettingHolidayViewModel vmodel, FormCollection coll)
        {
            var holidayToSave = this._holidayService.GetHoliday(id);
            holidayToSave.TimeFrom = vmodel.TimeFrom;
            holidayToSave.TimeUntil = vmodel.TimeTil;
            holidayToSave.HolidayName = vmodel.HolidayName;

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
            var year = DateTime.Today.Year;
            var model = this.SaveWatchDateViewModel(new WatchDateCalendar { }, year);

            SessionFacade.ActiveTab = "#fragment-3";

            return this.View(model);
        }

        [HttpPost]
        public ActionResult NewWatchDate(int id, GlobalSettingWatchDateViewModel viewModel, FormCollection coll)
        {

            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (id == 0)
            {
                var watchdatecalendar = new WatchDateCalendar { Name = viewModel.WatchDateCalendar.Name, CreatedDate = DateTime.UtcNow };

                this._watchDateCalendarService.SaveWatchDateCalendar(watchdatecalendar, out errors);

                if (errors.Count == 0)
                    return this.RedirectToAction("index", "globalsetting", new { texttypeid = 0, compareMethod = 1 });

                var model = this.SaveWatchDateViewModel(watchdatecalendar, viewModel.Year);

                SessionFacade.ActiveTab = coll["activeTab"];

                return this.View(model);
            }
            else
            {
                var watchdatecalendar = this._watchDateCalendarService.GetWatchDateCalendar(id);

                watchdatecalendar.Name = viewModel.WatchDateCalendar.Name;

                this._watchDateCalendarService.SaveWatchDateCalendar(watchdatecalendar, out errors);

                if (errors.Count == 0)
                    return this.RedirectToAction("index", "globalsetting", new { texttypeid = 0, compareMethod = 1 });

                var model = this.SaveWatchDateViewModel(watchdatecalendar, viewModel.Year);

                SessionFacade.ActiveTab = coll["activeTab"];

                return this.View(model);
            }

        }

        public ActionResult EditWatchDate(int id)
        {
            var wdCalendar = this._watchDateCalendarService.GetWatchDateCalendar(id);
            var year = DateTime.Today.Year;
            //var wdValue = this._watchDateCalendarService.GetWatchDateCalendarValue(id);

            if (wdCalendar == null)
                return new HttpNotFoundResult("No watch date value found...");

            var model = this.SaveWatchDateViewModel(wdCalendar, year);

            SessionFacade.ActiveTab = "#fragment-3";

            //model.ChangedWatchDateName = wdValue.WatchDateCalendar.Name;

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
                return this.RedirectToAction("index", "globalsetting", new { texttypeid = 0 });

            SessionFacade.ActiveTab = coll["activeTab"];

            return this.View(model);
        }

        public ActionResult EditWatchDateCalendarValue(int id)
        {
            var wdCalendarValue = this._watchDateCalendarService.GetWatchDateCalendarValue(id);

            var wdCalendar = this._watchDateCalendarService.GetWatchDateCalendar(wdCalendarValue.WatchDateCalendar_Id);
            var year = DateTime.Today.Year;
            //var wdValue = this._watchDateCalendarService.GetWatchDateCalendarValue(id);

            if (wdCalendar == null)
                return new HttpNotFoundResult("No watch date value found...");

            var model = this.SaveWatchDateViewModel(wdCalendar, year);

            SessionFacade.ActiveTab = "#fragment-3";

            //model.ChangedWatchDateName = wdValue.WatchDateCalendar.Name;

            return this.View(model);
        }

        public ActionResult NewTranslation(int texttypeid)
        {
            var model = this.GetTextTranslationViewModel(new Text { }, texttypeid, SessionFacade.CurrentCustomer.Language_Id, null, 1);

            SessionFacade.ActiveTab = "#fragment-4";

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
            text.ChangedByUser_Id = SessionFacade.CurrentUser.Id;


            this._textTranslationService.SaveNewText(text, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("edittranslation", "globalsetting", new { area = "admin", id = text.Id, textType = 1, compareMethod = 1 });
            //return this.RedirectToAction("index", "globalsetting", new { texttypeid = text.Type });


            var model = this.GetTextTranslationViewModel(text, text.Type, 0, null, 1);

            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, Translation.Get(error.Value));
            }
            SessionFacade.ActiveTab = coll["activeTab"];

            return this.View(model);
        }
        [ValidateInput(false)]
        public ActionResult EditTranslation(int id, int textType, string textSearch, int compareMethod)
        {
            var text = this._textTranslationService.GetText(id);

            if (SessionFacade.CurrentCustomer == null)
                return new HttpNotFoundResult("Customer information is not available...");

            var language = this._languageService.GetLanguage(SessionFacade.CurrentCustomer.Language_Id);
            if (text == null)
                return new HttpNotFoundResult("No translation found...");

            var model = this.GetTextTranslationViewModel(text, text.Type, language.Id, textSearch, compareMethod);

            SessionFacade.ActiveTab = "#fragment-4";

            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditTranslation(int id, int texttypeid, string textSearch, int compareMethod, List<TextTranslationLanguageList> TTs, FormCollection coll)
        {
            var textToSave = this._textTranslationService.GetText(id);

            if (textToSave == null)
                throw new Exception("No text found...");


            var b = this.TryUpdateModel(textToSave, "text");

            foreach (var t in TTs)
            {
                t.ChangedByUser_Id = SessionFacade.CurrentUser.Id;
            }


            IDictionary<string, string> errors = new Dictionary<string, string>();

            this._textTranslationService.SaveEditText(textToSave, TTs, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "globalsetting", new { texttypeid = texttypeid, textSearch = textSearch, compareMethod = compareMethod });

            var model = this.GetTextTranslationViewModel(textToSave, texttypeid, 0, textSearch, compareMethod);

            model.Tabposition = coll["activeTab"];

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int texttypeid, FormCollection coll)
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


            SessionFacade.ActiveTab = coll["activeTab"];
            return this.RedirectToAction("index", "globalsetting", new { texttypeid = texttypeid, compareMethod = 1 });
        }

        private GlobalSettingIndexViewModel GetGSIndexViewModel(int holidayheaderid, int languageId, SearchOption searchOption)
        {
            const int DEFAULT_HOLIDAYS_CALENDAR_ID = 1;

            var gridModel = new TranslationGridModel();
            var allTexts = this._textTranslationService.GetAllTexts(searchOption.TextType, LanguageIds.English).ToList();
            if (string.IsNullOrEmpty(searchOption.TextSearch))
                gridModel.AllTexts = allTexts.OrderBy(a => a.TextToTranslate).ToList();
            else
            {
                switch (searchOption.CompareMethod)
                {
                    case 1:
                        gridModel.AllTexts = allTexts.Where(a => (a.TextToTranslate != null && a.TextToTranslate.ToLower().StartsWith(searchOption.TextSearch.ToLower())) ||
                                                             (a.TextTranslated != null && a.TextTranslated.ToLower().StartsWith(searchOption.TextSearch.ToLower())))
                                                 .OrderBy(a => a.TextToTranslate).ToList();
                        break;

                    case 2:
                        gridModel.AllTexts = allTexts.Where(a => (a.TextToTranslate != null && a.TextToTranslate.ToLower().Contains(searchOption.TextSearch.ToLower())) ||
                                                             (a.TextTranslated != null && a.TextTranslated.ToLower().Contains(searchOption.TextSearch.ToLower())))
                                                 .OrderBy(a => a.TextToTranslate).ToList();
                        break;

                    default:
                        gridModel.AllTexts = allTexts.OrderBy(a => a.TextToTranslate).ToList();
                        break;
                }
            }

            var dataPrivacyAccess = _gdprDataPrivacyAccessService.GetUserWithPrivacyPermissionsByUserId(SessionFacade.CurrentUser.Id);
			var fileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();

            var model = new GlobalSettingIndexViewModel
            {
                GlobalSettings = this._globalSettingService.GetGlobalSettings(),
                Holidays = this._holidayService.GetHolidaysByHeaderId(holidayheaderid),
                LanguagesToTranslateInto = this._languageService.GetLanguages(),
                Texts = this._textTranslationService.GetAllNewTexts(searchOption.TextType).ToList(),
                ListForIndex = this._textTranslationService.GetIndexListToTextTranslations(languageId),
                WatchDateCalendarValues = this._watchDateCalendarService.GetAllWatchDateCalendarValues().ToList(),
                TextType = this._textTranslationService.GetTextType(searchOption.TextType),
                HolidayHeader = this._holidayService.GetHolidayHeader(1),
                GridModel = gridModel,
                TextTypes = this._textTranslationService.GetTextTypes().Select(x => new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),
                HolidayHeaders = this._holidayService.GetHolidayHeaders().Select(x => new SelectListItem
                {
                    Text = x.Id == DEFAULT_HOLIDAYS_CALENDAR_ID ? string.Format("{0} ({1})", Translation.GetCoreTextTranslation(x.Name), Translation.GetCoreTextTranslation("standardkalender")) : Translation.GetCoreTextTranslation(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),
                WatchDateCalendars = this._watchDateCalendarService.GetAllWatchDateCalendars().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                HasDataPrivacyAccess = dataPrivacyAccess != null,
				FileUploadWhiteList = fileUploadWhiteList,
				LimitFileUploadExtensions = fileUploadWhiteList != null
            };

            model.SearchConditions = new List<SelectListItem>();
            model.SearchConditions.Add(new SelectListItem { Text = Translation.GetCoreTextTranslation("Börjar med"), Value = "1" });
            model.SearchConditions.Add(new SelectListItem { Text = Translation.GetCoreTextTranslation("Innehåller"), Value = "2" });

            ViewBag.SelectedSearchCondition = searchOption.CompareMethod.ToString();

            model.SearchTextTr = searchOption.TextSearch;

            model.GridModel.SearchOption = searchOption;

            return model;
        }

        private GlobalSettingInputViewModel SaveGsInputViewModel(GlobalSetting globalSetting)
        {
            var model = new GlobalSettingInputViewModel
            {
                GlobalSetting = globalSetting,
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation(x.Name),
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }

        private GlobalSettingHolidayViewModel SaveHolidayViewModel(HolidayHeader holidayheader, int year)
        {
            #region SelectListItems

            var timeList = new List<SelectListItem>();
            for (var i = 0; i < 24; i++)
            {
                timeList.Add(new SelectListItem
                {
                    Text = Translation.Get(i < 10 ? $"0{i}:00" : $"{i}:00"),
                    Value = i.ToString()
                });
            }
            #endregion

            var model = new GlobalSettingHolidayViewModel
            {
                Holiday = null,
                Holidays = this._holidayService.GetHolidaysByHeaderIdAndYear(year, holidayheader.Id),
                HolidaysForList = this._holidayService.GetHolidaysByHeaderIdAndYearForList(year, holidayheader.Id),
                HolidayHeader = holidayheader,
                HolidayHeaders = this._holidayService.GetHolidayHeaders().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                TimeFromList = timeList,
                TimeTilList = timeList,
                YearList = GetYearsList(2000, DateTime.Now.Year + 5),
                ChangedHeaderName = holidayheader.Name
            };


            #region SetInts

            //if (holiday.TimeFrom == 0)
            //{
            //    model.HalfDay = false;
            //    model.TimeFrom = 0;
            //    model.TimeTil = 0;
            //}
            //else
            //{
            //    model.HalfDay = true;
            //    model.TimeFrom = holiday.TimeFrom;
            //    model.TimeTil = holiday.TimeUntil;
            //}

            model.Year = year;
            #endregion

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

        private GlobalSettingWatchDateViewModel SaveWatchDateViewModel(WatchDateCalendar watchdatecalendar, int year)
        {
            var model = new GlobalSettingWatchDateViewModel
            {
                WatchDateCalendarValue = null,
                WatchDateCalendarValues = this._watchDateCalendarService.GetWDCalendarValuesByWDCIdAndYear(watchdatecalendar.Id, year),
                WatchDateCalendarValuesForList = this._watchDateCalendarService.GetWDCalendarValuesByWDCIdAndYearForList(watchdatecalendar.Id, year),
                WatchDateCalendar = watchdatecalendar,
                YearList = GetYearsList(2010, DateTime.Now.Year + 5)
            };
            model.Year = year;
            return model;
        }

        private List<SelectListItem> GetYearsList(int start, int end)
        {
            var yearList = new List<SelectListItem>();
            for (var j = start; j < end; j++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = j.ToString(),
                    Value = j.ToString()
                });
            }

            return yearList;
        }

        private GlobalSettingWatchDateViewModel SaveWatchDateViewModel(WatchDateCalendarValue watchDateCalendarValue)
        {
            var model = new GlobalSettingWatchDateViewModel
            {
                WatchDateCalendarValue = watchDateCalendarValue,
                YearList = GetYearsList(2000, DateTime.Now.Year + 5),
                WatchDateCalendars = this._watchDateCalendarService.GetAllWatchDateCalendars().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }

        private GlobalSettingTextTranslationViewModel GetTextTranslationViewModel(Text text, int texttypeid, int languageId, string textSearch, int compareMethod)
        {
            var model = new GlobalSettingTextTranslationViewModel
            {
                ListForEdit = this._textTranslationService.GetEditListToTextTranslations(text.Id),
                ListForNew = this._textTranslationService.GetNewListToTextTranslations(),
                Text = text,
                TextTranslations = this._textTranslationService.GetAllTextTranslations(),
                TextTranslation = this._textTranslationService.GetTextTranslationByIdAndLanguage(text.Id, languageId),
                Language = this._languageService.GetLanguage(languageId),
                TextType = this._textTranslationService.GetTextType(text.Type),
                TextTypeName = Translation.Get(this._textTranslationService.GetTextTypeName(texttypeid)),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList()
            };

            if (text.Id == 0)
            {
                model.TextTypes = this._textTranslationService.GetTextTypesForNewText().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList();
            }
            else
            {
                model.TextTypes = this._textTranslationService.GetTextTypes().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList();
            }

            //model.TextTypes = new List<SelectListItem>();
            //model.TextTypes.Add(new SelectListItem { Text = "Master data", Value = "1" });

            //foreach (var textype in this._textTranslationService.GetTextTypes())
            //{
            //    model.TextTypes.Add(new SelectListItem { Text = textype.Name, Value = textype.Id.ToString() });
            //}

            model.SearchOption = new SearchOption { TextType = texttypeid, TextSearch = textSearch, CompareMethod = compareMethod };
            return model;
        }

        public string ChangeHolidayHeader(int id)
        {
            var headerNameToChange = this._holidayService.GetHolidayHeader(id);

            if (headerNameToChange == null)
                return string.Empty;

            return headerNameToChange.Name;
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



            var model = this.GetTextTranslationViewModel(text, text.Type, id, null, 1);

            var view = "~/areas/admin/views/GlobalSetting/_Translation.cshtml";
            //var view = "~/areas/admin/views/GlobalSetting/EditTranslation.cshtml";
            return this.RenderRazorViewToString(view, model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        [HttpGet]
        public PartialViewResult ChangeTextType(int id)
        {
            var model = new TranslationGridModel();
            model.AllTexts = this._textTranslationService.GetAllTexts(id, LanguageIds.English).ToList();
            var view = "~/areas/admin/views/GlobalSetting/_TranslationsList.cshtml";

            var searchOpt = new SearchOption { TextType = id, TextSearch = "", CompareMethod = 1 };
            model.SearchOption = searchOpt;

            return this.PartialView(view, model);
        }

        [HttpGet]
        [ValidateInput(false)]
        public PartialViewResult SearchText(int textTypeId, string searchValue, int searchOption)
        {
            var model = new TranslationGridModel();
            //Before
            //var allTexts = this._textTranslationService.GetAllTexts(textTypeId, SessionFacade.CurrentCustomer.Id).ToList();
            //New 20211109
            var allTexts = this._textTranslationService.GetAllTexts(textTypeId, null).ToList();
            

            switch (searchOption)
            {
                case 1:
                    model.AllTexts = allTexts.Where(a => (a.TextToTranslate != null && a.TextToTranslate.ToLower().StartsWith(searchValue.ToLower())) ||
                                                         (a.TextTranslated != null && a.TextTranslated.ToLower().StartsWith(searchValue.ToLower())))
                                             .OrderBy(a => a.TextToTranslate).ToList();
                    break;

                case 2:
                    model.AllTexts = allTexts.Where(a => (a.TextToTranslate != null && a.TextToTranslate.ToLower().Contains(searchValue.ToLower())) ||
                                                         (a.TextTranslated != null && a.TextTranslated.ToLower().Contains(searchValue.ToLower())))
                                             .OrderBy(a => a.TextToTranslate).ToList();
                    break;

                default:
                    model.AllTexts = allTexts.OrderBy(a => a.TextToTranslate).ToList();
                    break;
            }

            var searchOpt = new SearchOption { TextType = textTypeId, TextSearch = searchValue, CompareMethod = searchOption };
            model.SearchOption = searchOpt;
            var view = "~/areas/admin/views/GlobalSetting/_TranslationsList.cshtml";

            return this.PartialView(view, model);

        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string SearchTranslation()
        {
            var searchOpt = new SearchOption { TextType = 1, TextSearch = string.Empty, CompareMethod = 1 };
            var model = this.GetGSIndexViewModel(1, SessionFacade.CurrentCustomer.Language_Id, searchOpt);

            var view = "~/areas/admin/views/GlobalSetting/_TranslationsList.cshtml";

            return this.RenderRazorViewToString(view, model);

        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string ChangeHoliday(int id)
        {

            var searchOpt = new SearchOption { TextType = 1, TextSearch = string.Empty, CompareMethod = 1 };
            var model = this.GetGSIndexViewModel(id, SessionFacade.CurrentCustomer.Language_Id, searchOpt);

            var view = "~/areas/admin/views/GlobalSetting/_Holidays.cshtml";

            return this.RenderRazorViewToString(view, model);

        }


        public string AddRowToHolidays(int holidayheaderid, DateTime holidaydate, int timefrom, int timeUntil, string holidayname)
        {
            var holiday = new Holiday();
            var year = DateTime.Today.Year;

            var holidayheader = this._holidayService.GetHolidayHeader(holidayheaderid);

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = this.SaveHolidayViewModel(holidayheader, year);

            if (this.ModelState.IsValid)
            {
                holiday.HolidayHeader_Id = holidayheaderid;
                holiday.HolidayDate = holidaydate;
                holiday.TimeFrom = timefrom;
                holiday.TimeUntil = timeUntil;
                holiday.CreatedDate = DateTime.UtcNow;
                holiday.HolidayName = holidayname;

            }

            model.Holiday = holiday;

            this._holidayService.SaveHoliday(holiday, out errors);
            return this.UpdateHolidayList(holidayheader);
        }

        public string SaveRowToHolidays(int id, int holidayheaderid, DateTime holidaydate, string holidayname, int timefrom, int timeuntil)
        {
            var holiday = this._holidayService.GetHoliday(id);
            var year = DateTime.Today.Year;

            var holidayheader = this._holidayService.GetHolidayHeader(holidayheaderid);

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = this.SaveHolidayViewModel(holidayheader, year);

            if (this.ModelState.IsValid)
            {
                holiday.HolidayHeader_Id = holidayheaderid;
                holiday.HolidayDate = holidaydate;
                holiday.TimeFrom = timefrom;
                holiday.TimeUntil = timeuntil;
                holiday.CreatedDate = DateTime.UtcNow;
                holiday.HolidayName = holidayname;

            }

            model.Holiday = holiday;

            this._holidayService.SaveHoliday(holiday, out errors);
            return this.UpdateHolidayList(holidayheader);
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string DeleteHoliday(int id)
        {

            var holiday = this._holidayService.GetHoliday(id);

            var holidayheader = this._holidayService.GetHolidayHeader(holiday.HolidayHeader_Id);

            if (this._holidayService.DeleteHoliday(id) == DeleteMessage.Success)
                return this.UpdateHolidayList(holidayheader);
            else
            {
                this.TempData.Add("Error", "");
                return this.UpdateHolidayList(holidayheader);
            }

        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string UpdateHolidayList(HolidayHeader holidayheader)
        {
            var year = DateTime.Today.Year;

            var model = this.SaveHolidayViewModel(holidayheader, year);


            //model.ChangedHeaderName = holiday.HolidayHeader.Name;
            SessionFacade.ActiveTab = "#fragment-2";

            this.UpdateModel(model, "holiday");

            //return View(model);
            var view = "~/areas/admin/views/GlobalSetting/_Holidays.cshtml";
            return this.RenderRazorViewToString(view, model);

            //return this.View(model);
        }

        public string AddRowToWatchDateCalendarValue(int watchdatecalendarid, DateTime watchdate, string watchdatevaluenname, DateTime? validuntil)
        {
            var wdcv = new WatchDateCalendarValue();
            var year = DateTime.Today.Year;
            var wdc = this._watchDateCalendarService.GetWatchDateCalendar(watchdatecalendarid);

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = this.SaveWatchDateViewModel(wdc, year);

            if (this.ModelState.IsValid)
            {
                wdcv.ValidUntilDate = validuntil;
                wdcv.WatchDate = watchdate;
                wdcv.WatchDateCalendar_Id = watchdatecalendarid;
                wdcv.CreatedDate = DateTime.UtcNow;
                wdcv.WatchDateValueName = watchdatevaluenname;
            }

            model.WatchDateCalendarValue = wdcv;

            this._watchDateCalendarService.SaveWatchDateCalendarValue(wdcv, out errors);

            return this.UpdateWatchDateList(wdc);
        }

        public string SaveRowToWatchDateCalendarValue(int id, int watchdatecalendarId, DateTime watchdate, string watchdatevaluenname, DateTime? validuntil)
        {
            var wdcv = this._watchDateCalendarService.GetWatchDateCalendarValue(id);
            var year = DateTime.Today.Year;
            var wdc = this._watchDateCalendarService.GetWatchDateCalendar(watchdatecalendarId);

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = this.SaveWatchDateViewModel(wdc, year);

            if (this.ModelState.IsValid)
            {
                wdcv.WatchDate = watchdate;
                wdcv.WatchDateCalendar_Id = watchdatecalendarId;
                wdcv.CreatedDate = DateTime.UtcNow;
                wdcv.WatchDateValueName = watchdatevaluenname;
                wdcv.ValidUntilDate = validuntil;
            }

            model.WatchDateCalendarValue = wdcv;

            this._watchDateCalendarService.SaveWatchDateCalendarValue(wdcv, out errors);

            return this.UpdateWatchDateList(wdc);
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string DeleteWatchDateCalendarValue(int id)
        {

            var wdcv = this._watchDateCalendarService.GetWatchDateCalendarValue(id);

            var wdc = this._watchDateCalendarService.GetWatchDateCalendar(wdcv.WatchDateCalendar_Id);

            if (this._watchDateCalendarService.DeleteWDCV(id) == DeleteMessage.Success)
                return this.UpdateWatchDateList(wdc);
            else
            {
                this.TempData.Add("Error", "");
                return this.UpdateWatchDateList(wdc);
            }

        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string UpdateWatchDateList(WatchDateCalendar wdc)
        {
            var year = DateTime.Today.Year;
            var model = this.SaveWatchDateViewModel(wdc, year);


            //model.ChangedHeaderName = holiday.HolidayHeader.Name;
            SessionFacade.ActiveTab = "#fragment-3";

            this.UpdateModel(model, "watchdatecalendarvalue");

            //return View(model);
            var view = "~/areas/admin/views/GlobalSetting/_WatchDate.cshtml";
            return this.RenderRazorViewToString(view, model);

            //return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string ChangeYearHoliday(int year, int holidayheaderId)
        {
            var holidayheader = this._holidayService.GetHolidayHeader(holidayheaderId);

            var model = this.SaveHolidayViewModel(holidayheader, year);

            SessionFacade.ActiveTab = "#fragment-2";

            this.UpdateModel(model, "holiday");

            var view = "~/areas/admin/views/GlobalSetting/_Holidays.cshtml";
            return this.RenderRazorViewToString(view, model);

        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string ChangeYearWatchDate(int year, int watchdatecalendarId)
        {
            var watchdatecalendar = this._watchDateCalendarService.GetWatchDateCalendar(watchdatecalendarId);

            var model = this.SaveWatchDateViewModel(watchdatecalendar, year);


            SessionFacade.ActiveTab = "#fragment-3";

            this.UpdateModel(model, "watchdatecalendarvalue");

            var view = "~/areas/admin/views/GlobalSetting/_WatchDate.cshtml";
            return this.RenderRazorViewToString(view, model);

        }


        public ActionResult EditHolidayHeader(int id)
        {
            var holidayheader = this._holidayService.GetHolidayHeader(id);
            var year = DateTime.Now.Year;
            var model = this.SaveHolidayViewModel(holidayheader, year);

            SessionFacade.ActiveTab = "#fragment-2";


            return View("newholiday", model);

        }

        public ActionResult EditWatchDateCalendar(int id)
        {
            var watchdatecalendar = this._watchDateCalendarService.GetWatchDateCalendar(id);

            var year = DateTime.Now.Year;
            var model = this.SaveWatchDateViewModel(watchdatecalendar, year);

            SessionFacade.ActiveTab = "#fragment-3";


            return View("newwatchdate", model);

        }

        [HttpPost]
        public JsonResult UpdateMultiCustomersSearch(bool val)
        {
            var globalSettings = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            globalSettings.MultiCustomersSearch = val ? 1 : 0;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _globalSettingService.SaveGlobalSetting(globalSettings, out errors);

            if (errors.Any())
            {
                var errRes = new
                {
                    Success = false,
                    ErrorMessage = string.Join(",", errors.Select(kv => kv.Value))
                };
                return Json(errRes);
            }

            var res = new { Success = true };
            return Json(res);
        }

        [NoCache]
        [HttpGet]
        public JsonResult LoadDataPrivacyFavorite(int id)
        {
            var data = _gdprFavoritesService.GetFavorite(id);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDataPrivacyFavorite(int id)
        {
            _gdprFavoritesService.DeleteFavorite(id);

            var items = GetDataPrivacyFavorites();
            return Json(new { Success = true, Favorites = items });
        }

        [HttpPost]
        public JsonResult SaveDataPrivacyFavorites(GdprFavoriteModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Success = false, Error = "Invalid parameters" });

            model.Id = _gdprFavoritesService.SaveFavorite(model, _userContext.UserId);
            var items = GetDataPrivacyFavorites();
            return Json(new { Success = true, FavoriteId = model.Id, Favorites = items });
        }

        private object GetDataPrivacyFavorites()
        {
            //Checking permissions for Deletion/Anonymization
            var dataPrivacyAccess = _gdprDataPrivacyAccessService.GetUserWithPrivacyPermissionsByUserId(SessionFacade.CurrentUser.Id);
            var favorites = _gdprFavoritesService.ListFavorites(dataPrivacyAccess);
            var items = favorites.ToSelectList().Select(x => new
            {
                value = x.Value,
                text = x.Text
            }).ToList();

            return items;
        }

        [GdprAccess]
        [ChildActionOnly]
        public ActionResult DataPrivacy()
        {
            var model = GetDataPrivacyModel();
            return View(model);
        }

        [HttpPost]
        [GdprAccess]
        public ActionResult DataPrivacy(DataPrivacyParameters model)
        {
            SessionFacade.ActiveTab = "#fragment-6";
            if (model.SelectedCustomerId > 0)
            {
                //schedule new data privacy job
                var taskInfo = new GDPRTask
                {
                    CustomerId = model.SelectedCustomerId,
                    UserId = _userContext.UserId,
                    FavoriteId = model.SelectedFavoriteId ?? 0,
                    AddedDate = DateTime.UtcNow,
                    Status = GDPRTaskStatus.None
                };

                var taskId = _gdprTasksService.AddNewTask(taskInfo);
                return Json(new { success = true, taskId = taskId });
            }

            return Json(new { success = false, taskId = 0 });
        }

        private DataPrivacyModel GetDataPrivacyModel()
        {
            var userAccess = _gdprDataPrivacyAccessService.GetUserWithPrivacyPermissionsByUserId(SessionFacade.CurrentUser.Id);
            if (userAccess == null)
                return new DataPrivacyModel();

            var customers = _customerUserService.GetCustomerUsersForUser(SessionFacade.CurrentUser.Id);
            var availableCustomers = customers.OrderBy(x => x.Customer.Name).Select(x => new SelectListItem
            {
                Value = x.Customer.Id.ToString(),
                Text = x.Customer.Name
            }).ToList();

            var favorites = _gdprFavoritesService.ListFavorites(userAccess);

            var model = new DataPrivacyModel
            {
                IsAvailable = true,
                Customers = availableCustomers,
                Favorites = favorites.ToSelectList(new SelectListItem() { Value = "0", Text = Translation.GetCoreTextTranslation("Skapa ny") }),
            };
            
            //Check permissions for Deletion and/or Anonymization
            var gdprTypes = new List<SelectListItem>();
            if (userAccess.DeletionPermission == 1 && userAccess.AnonymizationPermission == 1)
            {
                gdprTypes = Enum.GetValues(typeof(GDPRType)).Cast<GDPRType>().Select(x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = Translation.GetCoreTextTranslation(x.ToString())
                }).ToList();
            }
            else if (userAccess.DeletionPermission == 1)
            {
                gdprTypes = Enum.GetValues(typeof(GDPRType)).Cast<GDPRType>().Select(x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = Translation.GetCoreTextTranslation(x.ToString()),
                }).Where(x => x.Text == Translation.GetCoreTextTranslation("Radering")).ToList();
                model.SelectedGDPRType = (int)GDPRType.Radering;
            }
            else if (userAccess.AnonymizationPermission == 1)
            {
                gdprTypes = Enum.GetValues(typeof(GDPRType)).Cast<GDPRType>().Select(x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = Translation.GetCoreTextTranslation(x.ToString())
                }).Where(x => x.Text == Translation.GetCoreTextTranslation("Avpersonifiering")).ToList();
                model.SelectedGDPRType = (int)GDPRType.Avpersonifiering;
            }

            model.GDPRType = gdprTypes;
            
            return model;
        }

        [HttpGet]
        public JsonResult GetRunningDataPrivacyTasks(int favoriteId)
        {
            var tasks = _gdprTasksService.GetPendingTasksByFavorite(favoriteId);
            if (tasks.Any())
            {
                var taskIds = tasks.Select(t => t.Id).ToArray();
                return Json(new { count = taskIds.Length, ids = taskIds }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [GdprAccess]
        public JsonResult GetDataPrivacyAffectedCases(DataPrivacyParameters p)
        {
            p = _gdprFavoritesService.CreateParameters(p.SelectedFavoriteId ?? 0 );

            var totalCount = _gdprPrivacyCasesService.GetCasesCount(p.SelectedCustomerId, p);
            return Json(new { count = totalCount }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public PartialViewResult DataPrivacyHistory()
        {
            var customers = _gdprOperationsService.GetOperationAuditCustomers();
            var model = new DataPrivacyHistoryViewModel
            {
                SelectedCustomerId = customers.Any() ? customers.First().Key : 0,
                Customers = customers.ToSelectList()
            };

            return PartialView("_DataPrivacyHistory", model);
        }

        [GdprAccess]
        [NoCache]
        public JsonResult GetDataPrivacyHistoryTable(int? customerId)
        {
            var data = _gdprOperationsService.ListGdprOperationsAuditItems(customerId);

            var closedText = Translation.GetCoreTextTranslation("Avslutade");
            var openedText = Translation.GetCoreTextTranslation("Öppna");
            var caseText = Translation.GetCoreTextTranslation("Ärende");
            var logPostsText = Translation.GetCoreTextTranslation("Ärendelogg");

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

            var model = new List<GdprOperationsHistoryListItem>();

            //replace field names with field labels
            foreach (var item in data)
            {
                var attachedFilesFormatted = new StringBuilder();
                if (item.RemoveCaseAttachments)
                    attachedFilesFormatted.AppendFormat(caseText);

                if (item.RemoveLogAttachments)
                    attachedFilesFormatted.AppendFormat(", {0}", logPostsText);

                var formattedFields = item.Fields.Select(x => FormatFieldLabel(x, null, customerId ?? 0)).ToList();

                var modelItem = new GdprOperationsHistoryListItem
                {
                    RegistrationDate = $"{item.RegDateFrom.Value.ToShortDateString()} - {item.RegDateTo.Value.ToShortDateString()}",
                    Cases = item.ClosedOnly ? closedText : $"{closedText}, {openedText}",
                    Data = formattedFields.Any() ? string.Join(", ", formattedFields) : "",
                    AttachedFiles = attachedFilesFormatted.ToString().Trim(',').Trim(),
                    Executed = TimeZoneInfo.ConvertTimeFromUtc(item.ExecutedDate, userTimeZone).ToString(DateFormats.DateTime, CultureInfo.InvariantCulture)
                };

                model.Add(modelItem);
            }

            var viewPath = "~/Areas/Admin/Views/GlobalSetting/_DataPrivacyHistoryTable.cshtml";
            var content = RenderRazorViewToString(viewPath, model);
            return Json(new { Success = true, Content = content }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public PartialViewResult FilesViewLog()
        {
            var customers = _customerUserService.GetCustomerUsersForUser(SessionFacade.CurrentUser.Id).OrderBy(x => x.Customer.Name).ToList();
            var availableCustomers = customers.Select(x => new SelectListItem
            {
                Value = x.Customer.Id.ToString(),
                Text = x.Customer.Name
            }).ToList();
            var selectedCustomerId = customers.Any() ? customers.First().Customer_Id : 0;

            //var userDepartments =
            //    _departmentsService.GetDepartmentsForUser(selectedCustomerId, SessionFacade.CurrentUser.Id);

            var model = new FileViewLogsViewModel
            {
                SelectedCustomerId = selectedCustomerId,
                Customers = availableCustomers,
                SelectedDepartmetsIds = new List<int>(),
                Departments = new List<SelectListItem>(),
                Amount = 500
            };

            return PartialView("_FilesViewLog", model);
        }

		private readonly string[] DEFAULT_FILEUPLOAD_WHITELIST = new string[]
		{
			"7z",
			"avi",
			"bmp",
			"csv",
			"doc",
			"docx",
			"gif",
			"jpeg",
			"jpg",
			"log",
			"mov",
			"mp4",
			"mpeg",
			"mpg",
			"odt",
			"pdf",
			"png",
			"pps",
			"ppt",
			"pptx",
			"rtf",
			"tga",
			"tif",
			"tiff",
			"txt",
			"xls",
			"xlsx",
			"zip"
		};

		[HttpGet]
		public JsonResult GetSystemDefaultWhiteList()
		{
			return Json(DEFAULT_FILEUPLOAD_WHITELIST, JsonRequestBehavior.AllowGet);
		}

		public ActionResult EditFileExtensions()
		{
			var whiteList = _globalSettingService.GetFileUploadWhiteList();

			var whiteListStr = "";

			if (whiteList != null)
			{
				whiteListStr = whiteList.Aggregate((o, p) => o + Environment.NewLine + p);
			}
			else
			{
				whiteListStr = "";
			}
			SessionFacade.ActiveTab = "#fragment-9";
			return View("EditFileExtensions", new FileUploadExtensionsModel
			{
				UseFileExtensionWhiteList = whiteList != null,
				FileExtensions = whiteListStr
			});
		}

		[HttpPost]
		public ActionResult EditFileExtensions(bool useFileExtensionWhiteList, string fileExtensions, int texttypeid, int compareMethod)
		{
			if (useFileExtensionWhiteList)
			{
				var extensions = fileExtensions.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
				_globalSettingService.SetFileUploadWhiteList(extensions);
			}
			else
			{
				// Disable whitelist
				_globalSettingService.SetFileUploadWhiteList(null);
			}

			return this.RedirectToAction("index", "globalsetting", new { texttypeid = texttypeid, compareMethod = compareMethod });

		}

		[NoCache]
        [HttpGet]
        public JsonResult LoadCustomerDepartments(int id)
        {
            var departmets = _departmentsService.GetDepartmentsForUser(id, SessionFacade.CurrentUser.Id);
            var data = departmets != null
                ? departmets.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.DepartmentName.Trim()
                }).ToList()
                : new List<SelectListItem>();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadFileViewLogs(FileViewLogListFilter filter)
        {
            var data = _fileViewLogService.Find(filter, SessionFacade.CurrentUser.TimeZoneId);
            var model = data.Select(f => new FileViewLogItemViewModel
            {
                CaseNumber = f.CaseNumber.ToString(),
                CreatedDate = f.Log.CreatedDate,
                DepartmentName = f.DepartmentName,
                ProductAreaName = f.ProductAreaName,
                FileName = f.Log.FileName,
                FilePath = f.Log.FilePath,
                Id = f.Log.Id,
                UserName = f.UserName,
                Operation = f.Log.Operation.Translate(),
                Source = f.Log.FileSource.Translate()
            }).ToList();

            var viewPath = "~/Areas/Admin/Views/GlobalSetting/_FileViewLogTable.cshtml";
            var content = RenderRazorViewToString(viewPath, model);
            return Json(new { content });
        }

        [HttpPost]
        public ActionResult GetCustomerCaseFields(int? customerId)
        {
            if (customerId.HasValue && customerId > 0)
            {
                var exceptionList = new List<string>
                {
                    //GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString(),
                    GlobalEnums.TranslationCaseFields.AddUserBtn.ToString(),
                    GlobalEnums.TranslationCaseFields.UpdateNotifierInformation.ToString(),
                    GlobalEnums.TranslationCaseFields.Filename.ToString(),
                    "tblLog.Charge",
                    "tblLog.Filename",
                    GlobalEnums.TranslationCaseFields.FinishingDate.ToString(),
                    GlobalEnums.TranslationCaseFields.Verified.ToString(), //mandatory
                    GlobalEnums.TranslationCaseFields.SMS.ToString(),
                    GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString(),
                    //GlobalEnums.TranslationCaseFields.User_Id.ToString(), //because included in registeredBy
                    GlobalEnums.TranslationCaseFields.RegTime.ToString(),
                    GlobalEnums.TranslationCaseFields.ChangeTime.ToString(),
                    GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(), //mandatory
                    GlobalEnums.TranslationCaseFields.CaseNumber.ToString(),
                    GlobalEnums.TranslationCaseFields.Customer_Id.ToString(),
                    GlobalEnums.TranslationCaseFields.Cost.ToString(),
                    "tblLog.Filename_Internal"
                };

                var additionalFields = new List<CaseFieldSettingsWithLanguage>
                {
                    new CaseFieldSettingsWithLanguage
                    {
                        Label = Translation.GetCoreTextTranslation("Ändring"),
                        Name = CaseSolutionFields.Change.ToString()
                    },
                    new CaseFieldSettingsWithLanguage()
                    {
                        Label = "SelfService - RegUser",
                        Name = AdditionalDataPrivacyFields.SelfService_RegUser.ToString()
                    }
                };

                var fields =
                    _caseFieldSettingService.GetCaseFieldSettings(customerId.Value)
                        .Where(f => FieldSettingsUiNames.Names.ContainsKey(f.Name) &&
                                    !exceptionList.Any(o => o.Equals(f.Name, StringComparison.OrdinalIgnoreCase)))
                        .Select(f => new CaseFieldSettingsWithLanguage
                        {
                            Name = f.Name
                        }).ToList();

                // add additional fields
                fields.AddRange(additionalFields);
                //Todo - add fields from customer - CaseType & ProductArea
                var data =
                    fields.Select(f => new SelectListItem
                    {
                        Value = f.Name,
                        Text = FormatFieldLabel(f.Name, f.Label, customerId.Value)
                    })
                    .OrderBy(f => f.Text)
                    .ToList();

                return Json(new { success = true, data });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult GetCustomerCaseTypes(int? customerId)
        {
            if (customerId.HasValue && customerId > 0)
            {

                var caseTypes = _caseTypeService.GetAllCaseTypes(customerId.Value, false, true).ToList();
                var caseTypesInRow = _caseTypeService.GetChildrenInRow(caseTypes).ToList();

                var data =
                    caseTypesInRow.Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Name,
                        Disabled = f.IsActive != 1
                    })
                    .OrderBy(f => f.Text)
                    .ToList();

                return Json(new { success = true, data });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult GetCustomerProductAreas(int? customerId)
        {
            if (customerId.HasValue && customerId > 0)
            {

                var productAreas = _productAreaService.GetTopProductAreasWithChilds(customerId.Value, false);
                var productAreasInRow = _productAreaService.GetChildrenInRow(productAreas).ToList();

                var data =
                    productAreasInRow.Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Name,
                        Disabled = f.IsActive != 1
                    })
                    .OrderBy(f => f.Text)
                    .ToList();

                return Json(new { success = true, data });
            }

            return Json(new { success = false });
        }
        private string FormatFieldLabel(string fieldName, string label, int customerId)
        {
            if (string.IsNullOrEmpty(label) && FieldSettingsUiNames.Names.ContainsKey(fieldName))
            {
                label = Translation.GetForCase(fieldName, customerId);
            }

            // prefix IsAbout section fields ony if they were translated
            if (fieldName.IndexOf("IsAbout_", StringComparison.OrdinalIgnoreCase) != -1 && !string.IsNullOrEmpty(label))
            {
                var regardingHeader = Translation.GetCoreTextTranslation(CaseSections.RegardingHeader);
                label = $"{regardingHeader} - {label}";
            }

            return string.IsNullOrEmpty(label) ? fieldName : label;
        }       
    }
}