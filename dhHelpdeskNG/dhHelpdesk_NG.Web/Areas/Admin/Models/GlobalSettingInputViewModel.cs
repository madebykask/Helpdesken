using System;
using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DH.Helpdesk.BusinessData.Enums.BusinessRules;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure;

    public class GlobalSettingIndexViewModel : BaseTabInputViewModel
    {

        public IEnumerable<GlobalSetting> GlobalSettings { get; set; }
        public IEnumerable<Language> LanguagesToTranslateInto { get; set; }
        public IEnumerable<TextTranslationLanguageList> ListForIndex { get; set; }

        public IEnumerable<Holiday> Holidays { get; set; }
        
        public IList<WatchDateCalendarValue> WatchDateCalendarValues { get; set; }
        public IList<SelectListItem> Languages { get; set; }
        
        public IList<Text> Texts { get; set; }
        public Language Language { get; set; }
        public TranslationGridModel GridModel { get; set; }
        public IList<SelectListItem> TextTypes { get; set; }
        public TextType TextType { get; set; }
        public IList<SelectListItem> HolidayHeaders { get; set; }
        public IList<SelectListItem> WatchDateCalendars { get; set; }
        public HolidayHeader HolidayHeader { get; set; }

        public User User { get; set; }

        public string SearchTextTr { get; set; }
        public IList<SelectListItem> SearchConditions { get; set; }

        public bool HasDataPrivacyAccess { get; set; }
		public List<string> FileUploadWhiteList { get; internal set; }
		public bool LimitFileUploadExtensions { get; internal set; }

	}

    public class TranslationGridModel
    {
        public SearchOption SearchOption { get; set; } 
        public IList<TextList> AllTexts { get; set; }
        public IList<TextList> AllTextAndTranslations { get; set; }
    }

    public class SearchOption
    {
        public int TextType {get; set;}
        public string TextSearch {get; set;}
        public int CompareMethod {get; set;}
    }

    public class DataPrivacyModel
    {
        public DataPrivacyModel()
        {
            IsAvailable = false;
            ClosedOnly = true;
            ReplaceEmails = true;
            Fields = new List<SelectListItem>();
            CaseTypes = new List<SelectListItem>();
            GDPRType = new List<SelectListItem>();
        }

        public bool IsAvailable { get; set; }
        public int SelectedCustomerId { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public int SelectedFavoriteId { get; set; }
        public List<SelectListItem> Favorites { get; set; }
        public int? RetentionPeriod { get; set; }
        public DateTime? RegisterDateFrom { get; set; }
        public DateTime? RegisterDateTo { get; set; }

        public DateTime? FinishedDateFrom { get; set; }
        public DateTime? FinishedDateTo { get; set; }
        
        public bool ClosedOnly { get; set; }
        public bool CalculateRegistrationDate { get; set; }
        public List<SelectListItem> Fields { get; set; }        
        public List<string> FieldsNames { get; set; }
        public List<SelectListItem> CaseTypes { get; set; }
        public List<string> CaseTypeNames { get; set; }
        public bool ReplaceEmails { get; set; }
        public string ReplaceDataWith { get; set; }
        public DateTime? ReplaceDatesWith { get; set; }
        public bool RemoveCaseAttachments { get; set; }
        public bool RemoveLogAttachments { get; set; }
        public bool RemoveFileViewLogs { get; set; }

        public int SelectedGDPRType { get; set; }
        public List<SelectListItem> GDPRType { get; set; }
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
        public int Year { get; set; }
        public string ChangedHeaderName { get; set; }
        public string HolidayName { get; set; }

        public Holiday Holiday { get; set; }
        public HolidayHeader HolidayHeader { get; set; }

        public IList<SelectListItem> HolidayHeaders { get; set; }
        public IList<SelectListItem> TimeFromList { get; set; }
        public IList<SelectListItem> TimeTilList { get; set; }
        public IList<SelectListItem> YearList { get; set; }

        public IEnumerable<Holiday> Holidays { get; set; }
        public IList<Holiday> HolidaysForList { get; set; }
    }

    public class GlobalSettingWatchDateViewModel : BaseTabInputViewModel
    {
        public string ChangedWatchDateName { get; set; }
        public int Year { get; set; }
        public WatchDateCalendarValue WatchDateCalendarValue { get; set; }
        public WatchDateCalendar WatchDateCalendar { get; set; }
        public IList<SelectListItem> YearList { get; set; }
        public IList<SelectListItem> WatchDateCalendars { get; set; }
        public string WatchDateValueName { get; set; }

        public IEnumerable<WatchDateCalendarValue> WatchDateCalendarValues { get; set; }

        public IList<WatchDateCalendarValue> WatchDateCalendarValuesForList { get; set; }
    }

    public class GlobalSettingTextTranslationViewModel : BaseTabInputViewModel
    {
        public Text Text { get; set; }

        public TextTranslation TextTranslation { get; set; }

        public IEnumerable<TextTranslation> TextTranslations { get; set; }
        public IEnumerable<TextTranslationList> ListForNew { get; set; }

        public IList<TextTranslationLanguageList> ListForEdit { get; set; }

        public IList<SelectListItem> Languages { get; set; }
        public Language Language { get; set; }

        public IList<SelectListItem> TextTypes { get; set; }
        public TextType TextType { get; set; }
        public string TextTypeName { get; set; }

        public SearchOption SearchOption { get; set; }
    }
}