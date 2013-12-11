using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class GlobalSettingIndexViewModel : BaseTabInputViewModel
    {
        public IEnumerable<GlobalSetting> GlobalSettings { get; set; }
        public IEnumerable<Language> LanguagesToTranslateInto { get; set; }
        public IEnumerable<TextTranslationLanguageList> ListForIndex { get; set; }

        public IList<Holiday> Holidays { get; set; }
        public IList<Text> Texts { get; set; }
        public IList<WatchDateCalendarValue> WatchDateCalendarValues { get; set; }
    }

    public class GlobalSettingInputViewModel : BaseTabInputViewModel
    {
        public GlobalSetting GlobalSetting { get; set; }

        public IList<SelectListItem> Languages { get; set; }
    }

    public class GlobalSettingHolidayViewModel : BaseTabInputViewModel
    {
        public bool HalfDay { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTil { get; set; }
        public string ChangedHeaderName { get; set; }

        public Holiday Holiday { get; set; }
        public HolidayHeader HolidayHeader { get; set; }

        public IList<SelectListItem> HolidayHeaders { get; set; }
        public IList<SelectListItem> TimeFromList { get; set; }
        public IList<SelectListItem> TimeTilList { get; set; }
    }

    public class GlobalSettingWatchDateViewModel : BaseTabInputViewModel
    {
        public string ChangedWatchDateName { get; set; }

        public WatchDateCalendarValue WatchDateCalendarValue { get; set; }
        public WatchDateCalendar WatchDateCalendar { get; set; }

        public IList<SelectListItem> WatchDateCalendars { get; set; }
    }

    public class GlobalSettingTextTranslationViewModel : BaseTabInputViewModel
    {
        public Text Text { get; set; }

        public IEnumerable<TextTranslation> TextTranslations { get; set; }
        public IEnumerable<TextTranslationList> ListForNew { get; set; }

        public IList<TextTranslationLanguageList> ListForEdit { get; set; }
    }
}