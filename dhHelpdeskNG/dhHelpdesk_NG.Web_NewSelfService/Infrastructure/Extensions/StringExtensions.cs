namespace DH.Helpdesk.NewSelfService.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.utils;

    public static class StringExtensions
    {
        public static string ToEllipsisString(this string s, int cutOffLength)
        {
            return s.Length > cutOffLength ? s.Substring(0, Math.Min(s.Length, cutOffLength)) + "..." : s;
        }

        public static int? ToNullableInt32(this string s)
        {
            int i;
            if (Int32.TryParse(s, out i))
            {
                if (i == 0)
                    return null;
                return i;
            }

            return null;
        }

        public static string DepartmentDescription(this Department d, int departmentFilterFormat)
        {
            return d.departmentDescription(departmentFilterFormat);  
        }

        public static string ConcatStrings(this string orginal, string valueToAdd, string sep)
        {
            string ret = string.Empty;

            if (!string.IsNullOrWhiteSpace(orginal))
                ret = orginal;

            if (!string.IsNullOrWhiteSpace(valueToAdd))
            {
                ret += sep + valueToAdd;
            }

            return ret;
        }

        public static string ReturnFormValue(this FormCollection collection, string valueToReturn)
        {
            var value = valueToReturn.ToLower(); 
            var ret = string.Empty;

            for (var i = 0; i <= collection.Keys.Count - 1; i++)
            {
                if (value == collection.Keys[i].ToLower())
                {
                    ret = collection[valueToReturn];
                    break;
                }

            }
            return ret;
        }

        public static string ReturnCustomerUserValue(this string valueToReturn)
        {
            var ret = string.Empty;

            if (!string.IsNullOrWhiteSpace(valueToReturn))  
                if (valueToReturn != "0")   
                    ret = valueToReturn; 

            return ret;
        }

        public static string IdIsSelected(this int id, string selectedValues)
        {
            var ret = string.Empty; 
            if (!string.IsNullOrWhiteSpace(selectedValues))
                if (selectedValues.Split(',').Contains(id.ToString()))  
                    ret = "selected";

            return ret;
        }

        public static string ValueIsSelected(this string value, string selectedValues)
        {
            string ret = string.Empty;
            if (!string.IsNullOrWhiteSpace(selectedValues))
                if (selectedValues.Split(',').Contains(value))
                    ret = "selected";

            return ret;
        }

        public static string displayHtml(this int value)
        {
            if (value == 0)
                return "display:none";
            return string.Empty;
        }

        public static string CaseIconTitle(this GlobalEnums.CaseIcon value )
        {
            string ret; 

            if (value == GlobalEnums.CaseIcon.Finished)
                ret = Translation.Get("Avslutat ärende", Enums.TranslationSource.TextTranslation);
            else if (value == GlobalEnums.CaseIcon.FinishedNotApproved)
                ret = Translation.Get("Åtgärdat ärende, ej godkänt", Enums.TranslationSource.TextTranslation);
            else if (value == GlobalEnums.CaseIcon.Urgent)
                ret = Translation.Get("Ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("akut", Enums.TranslationSource.TextTranslation) + ")";
            else
                ret = Translation.Get("Ärende", Enums.TranslationSource.TextTranslation);

            return ret;
        }

        public static string CaseIconSrc(this GlobalEnums.CaseIcon value)
        {
            var ret = "case.png";

            if (value == GlobalEnums.CaseIcon.Finished)
                ret = "case_close.png";
            else if (value == GlobalEnums.CaseIcon.FinishedNotApproved)
                ret = "case_close_notapproved.png";
            else if (value == GlobalEnums.CaseIcon.Urgent)
                ret = "case_Log_urgent.png";

            return ret;
        }

        

        public static string SetCalendarSortIcon(this string value)
        {
            var ret = string.Empty;

            if (SessionFacade.CurrentCalenderSearch != null)                
                    if (SessionFacade.CurrentCalenderSearch.SortBy == value)
                        ret = SessionFacade.CurrentCalenderSearch.Ascending ? "icon-chevron-down" : "icon-chevron-up";

            return ret;
        }

        public static string SetDocumentSortIcon(this string value)
        {
            var ret = string.Empty;

            if (SessionFacade.CurrentDocumentSearch != null)
                if (SessionFacade.CurrentDocumentSearch.SortBy == value)
                    ret = SessionFacade.CurrentDocumentSearch.Ascending ? "icon-chevron-down" : "icon-chevron-up";

            return ret;
        }

        public static string SetOpertionLogSortIcon(this string value)
        {
            var ret = string.Empty;

            if (SessionFacade.CurrentOperationLogSearch != null)
                if (SessionFacade.CurrentOperationLogSearch.SortBy == value)
                    ret = SessionFacade.CurrentOperationLogSearch.Ascending ? "icon-chevron-down" : "icon-chevron-up";

            return ret;
        }

        public static string SetBulletinBoardSortIcon(this string value)
        {
            var ret = string.Empty;

            if (SessionFacade.CurrentBulletinBoardSearch != null)
                if (SessionFacade.CurrentBulletinBoardSearch.SortBy == value)
                    ret = SessionFacade.CurrentBulletinBoardSearch.Ascending ? "icon-chevron-down" : "icon-chevron-up";

            return ret;
        }

        public static string SetCaseSolutionSortIcon(this string value)
        {
            var ret = string.Empty;

            if (SessionFacade.CurrentCaseSolutionSearch != null)
                if (SessionFacade.CurrentCaseSolutionSearch.SortBy == value)
                    ret = SessionFacade.CurrentCaseSolutionSearch.Ascending ? "icon-chevron-down" : "icon-chevron-up";

            return ret;
        }

        public static string GetClassForCaseRowTr(this bool unread, bool urgent )
        {
            var ret = unread == true ? "textbold" : string.Empty;
            return urgent == true ? ret + " textred" : ret;
        }


        public static string SetCaseSortIcon(this string value)
        {
            var ret = string.Empty;

            if (SessionFacade.CurrentCaseSearch != null)
                if (SessionFacade.CurrentCaseSearch.Search != null)
                    if (SessionFacade.CurrentCaseSearch.Search.SortBy == value)
                        ret = SessionFacade.CurrentCaseSearch.Search.Ascending ? "icon-chevron-down" : "icon-chevron-up";

            return ret;
        }

        public static string SetUrlParameters(this string value, int id = 0)
        {
            string strUrl = value.ToLower();

            if (strUrl.IndexOf("&clearsession=1") > 0)
                strUrl = strUrl.Replace("&clearsession=1", string.Empty);

            if (strUrl.IndexOf("[caseid]") > 0)
                if (id > 0) strUrl = strUrl.Replace("[caseid]", id.ToString());            

            if (strUrl.IndexOf("[userid]") > 0)            
                if (SessionFacade.CurrentSystemUser != null) strUrl = strUrl.Replace("[userid]", SessionFacade.CurrentSystemUser);

            if (strUrl.IndexOf("[language]") > 0)
                if (SessionFacade.CurrentLanguageId > 0)
                {                    
                    var langId = SessionFacade.AllLanguages.Where(a => a.Id == SessionFacade.CurrentLanguageId).Select(a => a.LanguageId).SingleOrDefault();                                        
                    strUrl = strUrl.Replace("[language]", langId);
                }

            if (strUrl.IndexOf("[customerid]") > 0)
                if (SessionFacade.CurrentCustomer != null) strUrl = strUrl.Replace("[customerid]", SessionFacade.CurrentCustomer.Id.ToString());                                    

            return strUrl;
        }

        public static string GetMailTemplateName(this int value)
        {
            var ret = string.Empty; 
            switch (value)
            {
                case 1:
                    ret = Translation.Get("Nytt ärende", Enums.TranslationSource.TextTranslation); 
                    break;
                case 2:
                    ret = Translation.Get("Tilldelat ärende", Enums.TranslationSource.TextTranslation) + " " + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation);     
                    break;
                case 3: case 14:
                    ret = Translation.Get("Ärendet avslutat", Enums.TranslationSource.TextTranslation);
                    break;
                case 4:
                    ret = Translation.Get("Informera anmälaren om åtgärden", Enums.TranslationSource.TextTranslation);
                    break;
                case 5:
                    ret = Translation.Get("Skicka intern loggpost till", Enums.TranslationSource.TextTranslation);
                    break;
                case 6:
                    ret = Translation.Get("Enkät", Enums.TranslationSource.TextTranslation);
                    break;
                case 7:
                    ret = Translation.Get("Tilldelat ärende", Enums.TranslationSource.TextTranslation) + " " + Translation.Get("Driftgrupp", Enums.TranslationSource.TextTranslation);     
                    break;
                case 8:
                    ret = Translation.Get("Beställning", Enums.TranslationSource.TextTranslation);
                    break;
                case 9:
                    ret = Translation.Get("Bevakningsdatum inträffar", Enums.TranslationSource.TextTranslation);
                    break;
                case 10:
                    ret = Translation.Get("Anmälaren uppdaterat ärende", Enums.TranslationSource.TextTranslation);
                    break;
                case 11:
                    ret = Translation.Get("SMS", Enums.TranslationSource.TextTranslation) + ": " + Translation.Get("Tilldelat ärende", Enums.TranslationSource.TextTranslation) + " " + Translation.Get("Handläggare", Enums.TranslationSource.TextTranslation);     
                    break;
                case 12:
                    ret = Translation.Get("Skicka mail när planerat åtgärdsdatum inträffar", Enums.TranslationSource.TextTranslation);
                    break;
                default:
                    ret = string.Empty; 
                    break;
            }
            return ret;

        }

    }
}