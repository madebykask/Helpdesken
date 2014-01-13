using System;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service.Utils;
using dhHelpdesk_NG.DTO.Utils; 

namespace dhHelpdesk_NG.Web.Infrastructure.Extensions
{
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

        public static string SetCaseSortIcon(this string value)
        {
            var ret = string.Empty; 

            if (SessionFacade.CurrentCaseSearch != null)  
                if (SessionFacade.CurrentCaseSearch.Search != null)
                    if (SessionFacade.CurrentCaseSearch.Search.SortBy == value)
                        ret = SessionFacade.CurrentCaseSearch.Search.Ascending ? "icon-chevron-down" : "icon-chevron-up";

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


    }
}