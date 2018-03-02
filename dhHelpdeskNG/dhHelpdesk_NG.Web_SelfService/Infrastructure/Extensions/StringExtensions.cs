using System.Web;

namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.utils;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

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
                        ret = SessionFacade.CurrentCaseSearch.Search.Ascending ? "fa fa-chevron-down" : "fa fa-chevron-up";

            return ret;
        }

        public static string SetUrlParameters(this string value, int id = 0)
        {
            string res = value;
            string lowerStr = res.ToLower();

            if (lowerStr.IndexOf("&clearsession=1") > 0)
            {
                res = Regex.Replace(res, @"&clearsession=1", string.Empty, RegexOptions.IgnoreCase);  
                lowerStr = res.ToLower();
            }

            if (lowerStr.IndexOf("[caseid]") > 0 && id > 0)
            {
                res = Regex.Replace(res, @"\[caseid\]", id.ToString(), RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            if (lowerStr.IndexOf("[userid]") > 0 && SessionFacade.CurrentSystemUser != null)
            {
                res = Regex.Replace(res, @"\[userid\]", SessionFacade.CurrentSystemUser, RegexOptions.IgnoreCase);
                lowerStr = HttpUtility.UrlEncode(res.ToLower()); // encode backslash in userId with domain 
            }                

            if (lowerStr.IndexOf("[username]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[username\]", SessionFacade.CurrentUserIdentity.FirstName, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            } 
               
            if (lowerStr.IndexOf("[userfamily]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[userfamily\]", SessionFacade.CurrentUserIdentity.LastName, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }                 

            if (lowerStr.IndexOf("[usersurname]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[usersurname\]", SessionFacade.CurrentUserIdentity.LastName, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }                

            if (lowerStr.IndexOf("[userlastname]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[userlastname\]", SessionFacade.CurrentUserIdentity.LastName, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }              

            if (lowerStr.IndexOf("[language]") > 0 && SessionFacade.CurrentLanguageId > 0)                
            {                    
                var langId = SessionFacade.AllLanguages.Where(a => a.Id == SessionFacade.CurrentLanguageId).Select(a => a.LanguageId).SingleOrDefault();
                res = Regex.Replace(res, @"\[language\]", langId, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();                
            }

            if (lowerStr.IndexOf("[customerid]") > 0 && SessionFacade.CurrentCustomer != null)
            {
                res = Regex.Replace(res, @"\[customerid\]", SessionFacade.CurrentCustomer.Id.ToString(), RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }                                                  

            return res;
        }

        public static string SetHtmlParameters(this string value)
        {
            string res = value;
            string lowerStr = res.ToLower();

            //Current Customer Id
            if (lowerStr.IndexOf("[#1]") > 0 && SessionFacade.CurrentCustomer != null)
            {
                res = Regex.Replace(res, @"\[#1\]", SessionFacade.CurrentCustomer.Id.ToString(), RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //Current Customer Name
            if (lowerStr.IndexOf("[#2]") > 0 && SessionFacade.CurrentCustomer != null)
            {
                res = Regex.Replace(res, @"\[#2\]", SessionFacade.CurrentCustomer.Name, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //Current LanguageId
            if (lowerStr.IndexOf("[#3]") > 0 && SessionFacade.CurrentLanguageId > 0)
            {
                var langId = SessionFacade.AllLanguages.Where(a => a.Id == SessionFacade.CurrentLanguageId).Select(a => a.LanguageId).SingleOrDefault();
                res = Regex.Replace(res, @"\[#3\]", langId, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //Current LanguageName
            if (lowerStr.IndexOf("[#4]") > 0 && SessionFacade.CurrentLanguageId > 0)
            {
                var langName = SessionFacade.AllLanguages.Where(a => a.Id == SessionFacade.CurrentLanguageId).Select(a => a.Name).SingleOrDefault();
                res = Regex.Replace(res, @"\[#4\]", Translation.Get(langName), RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //RegUser
            if (lowerStr.IndexOf("[#5]") > 0 && SessionFacade.CurrentSystemUser != null)
            {
                res = Regex.Replace(res, @"\[#5\]", SessionFacade.CurrentSystemUser, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //Principal User Id
            if (lowerStr.IndexOf("[#6]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[#6\]", 
                                    SessionFacade.CurrentUserIdentity.UserId == null ? string.Empty : SessionFacade.CurrentUserIdentity.UserId,
                                    RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }  

            //UserName
            if (lowerStr.IndexOf("[#7]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[#7\]",
                                    SessionFacade.CurrentUserIdentity.FirstName == null ? string.Empty : SessionFacade.CurrentUserIdentity.FirstName, 
                                    RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //UserLastName
            if (lowerStr.IndexOf("[#8]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[#8\]",
                                    SessionFacade.CurrentUserIdentity.LastName == null ? string.Empty : SessionFacade.CurrentUserIdentity.LastName, 
                                    RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //User email
            if (lowerStr.IndexOf("[#9]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[#9\]",
                                    SessionFacade.CurrentUserIdentity.Email == null ? string.Empty : SessionFacade.CurrentUserIdentity.Email,
                                    RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //User Domain
            if (lowerStr.IndexOf("[#10]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[#10\]", 
                                    SessionFacade.CurrentUserIdentity.Domain == null? string.Empty:SessionFacade.CurrentUserIdentity.Domain, 
                                    RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //Employee Number
            if (lowerStr.IndexOf("[#11]") > 0 && SessionFacade.CurrentUserIdentity != null)
            {
                res = Regex.Replace(res, @"\[#11\]", 
                                    SessionFacade.CurrentUserIdentity.EmployeeNumber == null? string.Empty:SessionFacade.CurrentUserIdentity.EmployeeNumber, 
                                    RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            //Version Number
            if (lowerStr.IndexOf("[#12]") > 0)
            {
                
                res = Regex.Replace(res, @"\[#12\]", ApplicationFacade.Version, RegexOptions.IgnoreCase);
                lowerStr = res.ToLower();
            }

            return res;
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

        public static MvcHtmlString ForHtmlView(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Create(str);
            }

            return MvcHtmlString.Create(str.Replace(Environment.NewLine, "<br />"));
        }

        public static string AddCharacterInParts(this string s, int partLength, string charToSearch, string replaceStr)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            if (s.Length <= partLength)
                return s;

            var newStr = "";
            var arryStr = s.Split(new string[] { replaceStr }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < arryStr.Length; i++)
            {
                if (arryStr[i].Length > partLength)
                {
                    var splitedStr = arryStr[i].SplitInParts(partLength).ToList();
                    var linesStr = "";
                    var lastCarryPart = "";
                    var extraStr = "";
                    splitedStr.Add(" ");
                    splitedStr.Add(" ");
                    foreach (var part in splitedStr)
                    {
                        var partProcess = lastCarryPart + part;
                        lastCarryPart = "";
                        if (!partProcess.Contains(charToSearch) && partProcess.Length > partLength - 1)
                        {
                            var _splitedStr = partProcess.SplitInParts(partLength);
                            var newLongStr = _splitedStr.First();

                            extraStr = "";
                            var replaceIndex = 0;
                            replaceIndex = newLongStr.LastIndexOf(" ") > replaceIndex ? newLongStr.LastIndexOf(" ") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(".") > replaceIndex ? newLongStr.LastIndexOf(".") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(",") > replaceIndex ? newLongStr.LastIndexOf(",") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(";") > replaceIndex ? newLongStr.LastIndexOf(";") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("?") > replaceIndex ? newLongStr.LastIndexOf("?") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("!") > replaceIndex ? newLongStr.LastIndexOf("!") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("(") > replaceIndex ? newLongStr.LastIndexOf("(") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(")") > replaceIndex ? newLongStr.LastIndexOf(")") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("-") > replaceIndex ? newLongStr.LastIndexOf("-") : replaceIndex;

                            var isFirst = true;
                            foreach (var _part in _splitedStr)
                                if (isFirst)
                                    isFirst = false;
                                else
                                    lastCarryPart += _part;

                            var _str = string.Empty;
                            if (replaceIndex == 0)
                            {
                                extraStr = "-";
                                _str = newLongStr + extraStr;
                                linesStr += _str + replaceStr;
                            }
                            else
                            {
                                var baseIndex = replaceIndex + 1;
                                _str = newLongStr.Substring(0, baseIndex);
                                lastCarryPart = newLongStr.Substring(baseIndex) + lastCarryPart;
                                linesStr += _str.Insert(baseIndex, replaceStr) + extraStr;
                            }
                        }
                        else
                            linesStr += partProcess;
                    }
                    newStr = newStr + linesStr + lastCarryPart + replaceStr;
                }
                else
                {
                    newStr = newStr + arryStr[i] + replaceStr;
                }
            }

            return newStr;
        }

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

    }
}