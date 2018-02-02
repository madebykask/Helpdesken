
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
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Common.Enums;

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
            else {
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

        
        private GlobalSettingIndexViewModel GetGSIndexViewModel( int holidayheaderid, int languageId, SearchOption searchOption)
        {
            const int DEFAULT_HOLIDAYS_CALENDAR_ID = 1;
            var start = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            //int texttypeid,
            //string textSearch

            var gridModel = new TranslationGridModel();
            //gridModel.AllTexts = this._textTranslationService.GetAllTexts(texttypeid).ToList();
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


            var model = new GlobalSettingIndexViewModel
            {
                GlobalSettings = this._globalSettingService.GetGlobalSettings(),
                Holidays = this._holidayService.GetHolidaysByHeaderId(holidayheaderid),
                LanguagesToTranslateInto = this._languageService.GetLanguagesForGlobalSettings(),
                Texts = this._textTranslationService.GetAllNewTexts(searchOption.TextType).ToList(),
                ListForIndex = this._textTranslationService.GetIndexListToTextTranslations(languageId),
                WatchDateCalendarValues = this._watchDateCalendarService.GetAllWatchDateCalendarValues().ToList(),
                TextType = this._textTranslationService.GetTextType(searchOption.TextType),
                HolidayHeader = this._holidayService.GetHolidayHeader(1),
                GridModel = gridModel,
                TextTypes = this._textTranslationService.GetTextTypes().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),
                HolidayHeaders = this._holidayService.GetHolidayHeaders().Select(x => new SelectListItem
                {
                    Text = x.Id == DEFAULT_HOLIDAYS_CALENDAR_ID ? string.Format("{0} ({1})", Translation.Get(x.Name), Translation.Get("standardkalender")) : Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList(),
                WatchDateCalendars = this._watchDateCalendarService.GetAllWatchDateCalendars().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            model.SearchConditions = new List<SelectListItem>();
            model.SearchConditions.Add(new SelectListItem { Text = Translation.Get("Börjar med"), Value = "1"});
            model.SearchConditions.Add(new SelectListItem { Text = Translation.Get("Innehåller"), Value = "2"});

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
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }

        private GlobalSettingHolidayViewModel SaveHolidayViewModel(HolidayHeader holidayheader, int year)
        {
            
            #region SelectListItems

            List<SelectListItem> li = new List<SelectListItem>();

            li.Add(new SelectListItem()
            {
                Text = Translation.Get("00:00", Enums.TranslationSource.TextTranslation),
                Value = "0",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("01:00", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("02:00", Enums.TranslationSource.TextTranslation),
                Value = "2",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("03:00", Enums.TranslationSource.TextTranslation),
                Value = "3",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("04:00", Enums.TranslationSource.TextTranslation),
                Value = "4",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("05:00", Enums.TranslationSource.TextTranslation),
                Value = "5",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("06:00", Enums.TranslationSource.TextTranslation),
                Value = "6",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("07:00", Enums.TranslationSource.TextTranslation),
                Value = "7",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("08:00", Enums.TranslationSource.TextTranslation),
                Value = "8",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("09:00", Enums.TranslationSource.TextTranslation),
                Value = "9",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("10:00", Enums.TranslationSource.TextTranslation),
                Value = "10",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("11:00", Enums.TranslationSource.TextTranslation),
                Value = "11",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("12:00", Enums.TranslationSource.TextTranslation),
                Value = "12",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("13:00", Enums.TranslationSource.TextTranslation),
                Value = "13",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("14:00", Enums.TranslationSource.TextTranslation),
                Value = "14",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("15:00", Enums.TranslationSource.TextTranslation),
                Value = "15",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("16:00", Enums.TranslationSource.TextTranslation),
                Value = "16",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("17:00", Enums.TranslationSource.TextTranslation),
                Value = "17",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("18:00", Enums.TranslationSource.TextTranslation),
                Value = "18",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("19:00", Enums.TranslationSource.TextTranslation),
                Value = "19",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("20:00", Enums.TranslationSource.TextTranslation),
                Value = "20",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("21:00", Enums.TranslationSource.TextTranslation),
                Value = "21",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("22:00", Enums.TranslationSource.TextTranslation),
                Value = "22",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("23:00", Enums.TranslationSource.TextTranslation),
                Value = "23",
                Selected = false
            }); li.Add(new SelectListItem()
            {
                Text = Translation.Get("24:00", Enums.TranslationSource.TextTranslation),
                Value = "24",
                Selected = false
            });
            //for (int i = 00; i < 24; i++)
            //{
            //    li.Add(new SelectListItem
            //    {
            //        Text = i.ToString(),
            //        Value = i.ToString()
            //    });
            //}
            List<SelectListItem> lis = new List<SelectListItem>();
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("00:00", Enums.TranslationSource.TextTranslation),
                Value = "0",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("01:00", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("02:00", Enums.TranslationSource.TextTranslation),
                Value = "2",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("03:00", Enums.TranslationSource.TextTranslation),
                Value = "3",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("04:00", Enums.TranslationSource.TextTranslation),
                Value = "4",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("05:00", Enums.TranslationSource.TextTranslation),
                Value = "5",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("06:00", Enums.TranslationSource.TextTranslation),
                Value = "6",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("07:00", Enums.TranslationSource.TextTranslation),
                Value = "7",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("08:00", Enums.TranslationSource.TextTranslation),
                Value = "8",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("09:00", Enums.TranslationSource.TextTranslation),
                Value = "9",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("10:00", Enums.TranslationSource.TextTranslation),
                Value = "10",
                Selected = false
            });
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("11:00", Enums.TranslationSource.TextTranslation),
                Value = "11",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("12:00", Enums.TranslationSource.TextTranslation),
                Value = "12",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("13:00", Enums.TranslationSource.TextTranslation),
                Value = "13",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("14:00", Enums.TranslationSource.TextTranslation),
                Value = "14",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("15:00", Enums.TranslationSource.TextTranslation),
                Value = "15",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("16:00", Enums.TranslationSource.TextTranslation),
                Value = "16",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("17:00", Enums.TranslationSource.TextTranslation),
                Value = "17",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("18:00", Enums.TranslationSource.TextTranslation),
                Value = "18",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("19:00", Enums.TranslationSource.TextTranslation),
                Value = "19",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("20:00", Enums.TranslationSource.TextTranslation),
                Value = "20",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("21:00", Enums.TranslationSource.TextTranslation),
                Value = "21",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("22:00", Enums.TranslationSource.TextTranslation),
                Value = "22",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("23:00", Enums.TranslationSource.TextTranslation),
                Value = "23",
                Selected = false
            }); 
            lis.Add(new SelectListItem()
            {
                Text = Translation.Get("24:00", Enums.TranslationSource.TextTranslation),
                Value = "24",
                Selected = false
            });
            //for (int i = 00; i < 24; i++)
            //{
            //    lis.Add(new SelectListItem
            //    {
            //        Text = i.ToString(),
            //        Value = i.ToString()
            //    });
            //}

            List<SelectListItem> yearlist = new List<SelectListItem>();
            for (int j = 2010; j < 2020; j++)
            {
                yearlist.Add(new SelectListItem
                {
                    Text = j.ToString(),
                    Value = j.ToString()
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
                TimeFromList = li,
                TimeTilList = lis,
                YearList = yearlist,
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
            List<SelectListItem> yearlist = new List<SelectListItem>();
            for (int j = 2010; j < 2020; j++)
            {
                yearlist.Add(new SelectListItem
                {
                    Text = j.ToString(),
                    Value = j.ToString()
                });
            }

            var model = new GlobalSettingWatchDateViewModel
            {
                WatchDateCalendarValue = null,
                WatchDateCalendarValues = this._watchDateCalendarService.GetWDCalendarValuesByWDCIdAndYear(watchdatecalendar.Id, year),
                WatchDateCalendarValuesForList = this._watchDateCalendarService.GetWDCalendarValuesByWDCIdAndYearForList(watchdatecalendar.Id, year),
                WatchDateCalendar = watchdatecalendar,
                YearList = yearlist
            };
            model.Year = year;
            return model;
        }

        private GlobalSettingWatchDateViewModel SaveWatchDateViewModel(WatchDateCalendarValue watchDateCalendarValue)
        {
            List<SelectListItem> yearlist = new List<SelectListItem>();
            for (int j = 2000; j < 2015; j++)
            {
                yearlist.Add(new SelectListItem
                {
                    Text = j.ToString(),
                    Value = j.ToString()
                });
            }

            var model = new GlobalSettingWatchDateViewModel
            {
                WatchDateCalendarValue = watchDateCalendarValue,
                YearList = yearlist,
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

        //public string ChangeHolidayList(int id)
        //{
        //    var list = this._holidayService.GetAll().Where(x => x.HolidayHeader_Id == id);
        //    var str = this.RenderRazorViewToString("_HolidayList", list.ToList());

        //    return str;
        //}

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

            return this.PartialView(view , model);                                     
        }

        [HttpGet]
        public PartialViewResult SearchText(int textTypeId, string searchValue, int searchOption)
        {
            var model = new TranslationGridModel();
            var allTexts = this._textTranslationService.GetAllTexts(textTypeId, SessionFacade.CurrentCustomer.Language_Id).ToList();

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

            var searchOpt = new SearchOption{TextType = textTypeId, TextSearch = searchValue, CompareMethod = searchOption} ;
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

            IDictionary<string,string> errors = new Dictionary<string,string>();
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
    }
}