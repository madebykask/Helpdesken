using System.Web;
using DH.Helpdesk.BusinessData.Models.User.Interfaces;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.FileViewLog;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.utils;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using System.Collections.Generic;
    using System.Data;

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

        public static string DepartmentDescription(this IDepartmentInfo d, int departmentFilterFormat)
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

        public static DateTime? GetDate(this FormCollection form, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            DateTime date;
            if (!DateTime.TryParse(form[key], out date))
            {
                return null;
            }

            return date;
        }

        public static bool IsFormValueTrue(this FormCollection form, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var value = form[key];

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return value.ToLower().Contains("true");
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

        public static string IdIsSelected(this int id, string selectedValues)
        {
            var ret = string.Empty; 
            if (!string.IsNullOrWhiteSpace(selectedValues))
                if (selectedValues.Split(',').Contains(id.ToString()))  
                    ret = "selected";

            return ret;
        }

        public static string IdIsSelected(this int id, List<int> selectedIds)
        {
            var ret = string.Empty;
            if (selectedIds != null && selectedIds.Any())
                if (selectedIds.Contains(id))
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

        public static string CaseIconTitle(this GlobalEnums.CaseIcon value)
        {
            string ret;

            if (value == GlobalEnums.CaseIcon.Finished)
                ret = Translation.GetCoreTextTranslation("Avslutat ärende");
            else if (value == GlobalEnums.CaseIcon.FinishedNotApproved)
                ret = Translation.GetCoreTextTranslation("Åtgärdat ärende, ej godkänt");
            else if (value == GlobalEnums.CaseIcon.Urgent)
                ret = Translation.GetCoreTextTranslation("Ärende") + " (" +Translation.GetCoreTextTranslation("akut") + ")";
            else if (value == GlobalEnums.CaseIcon.Locked)
                ret = Translation.GetCoreTextTranslation("Låst");
            else
                ret = Translation.GetCoreTextTranslation("Ärende");

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
            else if (value == GlobalEnums.CaseIcon.Locked)
                ret = "case_locked.png";

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
                    ret = SessionFacade.CurrentBulletinBoardSearch.Ascending ? "icon-chevron-up" : "icon-chevron-down";

            return ret;
        }

        public static string SetCaseSolutionSortIcon(this string value)
        {
            var ret = string.Empty;

            if (SessionFacade.CurrentCaseSolutionSearch != null)
                if (SessionFacade.CurrentCaseSolutionSearch.SortBy == value)
                    ret = SessionFacade.CurrentCaseSolutionSearch.Ascending ? "icon-chevron-up" : "icon-chevron-down";

            return ret;
        }

        public static string GetClassForCaseRowTr(this bool unread, bool urgent )
        {
            var ret = unread == true ? "textbold" : string.Empty;
            return urgent == true ? ret + " textred" : ret;
        }

        public static string GetUrlForNavigationBetweenCases(this int id, bool forward = false)
        {
            if (SessionFacade.CurrentCaseSearch == null)
                return "/cases/edit/" + id.ToString();
            if (SessionFacade.CurrentCaseSearch == null)
                return "/cases/edit/" + id.ToString();
            if (SessionFacade.CurrentCaseSearch.Search == null)
                return "/cases/edit/" + id.ToString();
            if (string.IsNullOrWhiteSpace(SessionFacade.CurrentCaseSearch.Search.IdsForLastSearch))
                return "/cases/edit/" + id.ToString();

            string[] ids = SessionFacade.CurrentCaseSearch.Search.IdsForLastSearch.Split(',') ;

            int pos = Array.IndexOf(ids, id.ToString());
            if (pos >- 1)
            {
                if (forward)
                {
                    if (ids.Length > (pos + 1))
                        return "/cases/edit/" + ids[pos + 1];
                }
                else
                {
                    if (pos > 0)
                        return "/cases/edit/" + ids[pos - 1];
                }
                return "/cases/edit/" + (id).ToString();            
            }
            else
                return "/cases/edit/" + id.ToString();

        }

        public static string GetMailTemplateName(this int value, List<CustomMailTemplate> customMailTemplates)
        {
            var ret = string.Empty; 
            switch (value)
            {
                case 1:
                    ret = Translation.GetCoreTextTranslation("Nytt ärende"); 
                    break;
                case 2:
                    ret = Translation.GetCoreTextTranslation("Tilldelat ärende") + " " + Translation.GetCoreTextTranslation("Handläggare");     
                    break;
                case 3: case 14:
                    ret = Translation.GetCoreTextTranslation("Ärendet avslutat");
                    break;
                case 4:
                    ret = Translation.GetCoreTextTranslation("Informera anmälaren om åtgärden");
                    break;
                case 5:
                    ret = Translation.GetCoreTextTranslation("Skicka intern loggpost till");
                    break;
                case 6:
                    ret = Translation.GetCoreTextTranslation("Enkät");
                    break;
                case 7:
                    ret = Translation.GetCoreTextTranslation("Tilldelat ärende") + " " + Translation.GetCoreTextTranslation("Driftgrupp");     
                    break;
                case 8:
                    ret = Translation.GetCoreTextTranslation("Beställning");
                    break;
                case 9:
                    ret = Translation.GetCoreTextTranslation("Bevakningsdatum inträffar");
                    break;
                case 10:
                    ret = Translation.GetCoreTextTranslation("Anmälaren uppdaterat ärende");
                    break;
                case 11:
                    ret = Translation.GetCoreTextTranslation("SMS") + ": "
                                                                    + Translation.GetCoreTextTranslation(
                                                                        "Tilldelat ärende") + " "
                                                                    + Translation.GetCoreTextTranslation("Handläggare");
                    break;
                case 12:
                    ret = Translation.GetCoreTextTranslation("Skicka mail när planerat åtgärdsdatum inträffar");
                    break;
                case 13:
                    ret = Translation.GetCoreTextTranslation("Prioritet");
                    break;
                case 17:
                    ret = Translation.GetCoreTextTranslation("Påminnelse");
                    break;
                case 18:
                    ret = Translation.GetCoreTextTranslation("Sammanfogat ärende");
                    break;
                default:
                    if (value > 99 && customMailTemplates.Any())
                    {
                        var mailTemplate = customMailTemplates.Where(m=> m.MailId == value).FirstOrDefault();
                        if (mailTemplate != null && mailTemplate.TemplateLanguages.Any())
                            ret = mailTemplate.TemplateLanguages.FirstOrDefault().TemplateName;
                        else
                            ret = string.Empty;
                        
                    }
                    else
                    {
                        ret = string.Empty; 
                    }
                    
                    break;
            }

            return ret;
        }

        public static string HtmlReadOnlyFlag(this int caseId, int permission)
        {
            return caseId < 0 ? string.Empty : permission != 1 ? "disabled=disabled" : string.Empty;
        }

        /// <summary>
        /// The for html view.
        /// </summary>
        /// <param name="str">
        /// The string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static MvcHtmlString ForHtmlView(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Create(str);
            }
            return MvcHtmlString.Create(str.Replace(Environment.NewLine, "<br />").Replace("\r\n","<br />"));
        }

        public static int[] GetIntValues(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new int[0];   
            }

            return str.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        public static string GetYesNoText(this bool val)
        {
            return val ? Translation.GetCoreTextTranslation("Ja") : Translation.GetCoreTextTranslation("Nej");
        }

        public static string ToHtmlString(this String s, bool replaceLineBreaks = true)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            var encodedString = HttpUtility.HtmlEncode(s);
            return encodedString.Replace(Environment.NewLine, "<br />").Replace("\n", "<br />");
        }

        public static string Translate(this FileViewLogOperation operation)
        {
            switch (operation)
            {
                case FileViewLogOperation.Legacy:
                    return "";
                case FileViewLogOperation.View:
                    return Translation.GetCoreTextTranslation("Öppna");
                case FileViewLogOperation.Delete:
                    return Translation.GetCoreTextTranslation("Ta bort");
                case FileViewLogOperation.Add:
                    return Translation.GetCoreTextTranslation("Lägg till");
                case FileViewLogOperation.AddTemporary:
                    return Translation.GetCoreTextTranslation("Lägg till");
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

        public static string Translate(this FileViewLogFileSource source)
        {
            switch (source)
            {
                case FileViewLogFileSource.Helpdesk:
                    return "Helpdesk";
                case FileViewLogFileSource.Selfservice:
                    return "SelfService";
                case FileViewLogFileSource.WebApi:
                    return "Mobile";
                default:
                    return "";
            }
        }

        public static string Translate(this EmailSendMethod method)
        {
            switch (method)
            {
                case EmailSendMethod.SeparateEmails:
                    return Translation.GetCoreTextTranslation("Separata e-postmeddelanden till alla");
                case EmailSendMethod.OneEmail:
                    return Translation.GetCoreTextTranslation("Alla mottagare i en e-post");
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }
    }
}