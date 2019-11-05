using DH.Helpdesk.BusinessData.OldComponents;
using System;
using System.Threading;
using System.Web.Mvc;

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums.Cases;
using Field = DH.Helpdesk.Domain.Field;
using System.Collections.Generic;
using System.Web;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Web.Infrastructure.CaseOverview
{
    public class OutputFormatter
    {
        /* fields showing in DateTime format */        
        private static readonly IList<string> DateTimeFields = new List<string> { "ChangeTime", "RegTime" }.AsReadOnly();
        public TimeZoneInfo CurrentTimeZone { get; private set; }

        public OutputFormatter(bool isLastFirstName, TimeZoneInfo timeZone)
        {
            IsLastFirstName = isLastFirstName;
            CurrentTimeZone = timeZone;
        }

        public bool IsLastFirstName { get; set; }

        public string FormatField(Field field)
        {
            var isDateTime = false;
            switch (field.FieldType)
            {
                case FieldTypes.Date:
                    if (field.DateTimeValue.HasValue)
                    {
                        isDateTime = DateTimeFields.Contains(field.Key);                
                        return FormatDate(field.DateTimeValue.Value, isDateTime);
                    }
                    break;
                case FieldTypes.Time:
                    isDateTime = DateTimeFields.Contains(field.Key);                
                    return FormatNullableDate(field.DateTimeValue, isDateTime);

                case FieldTypes.NullableHours:
                    return string.IsNullOrEmpty(field.StringValue) ? " - " : $"{field.StringValue} h";
                default:
                    var content = FormatText(field);
                    return content;
            }

            return string.Empty;
        }

        public string FormatDate(DateTime input, bool isDateTime = false)
        {
            if (isDateTime)
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(input, this.CurrentTimeZone);
                return localTime.ToString(DateFormats.DateTime, Thread.CurrentThread.CurrentUICulture);
            }
            else
            {
                return  input.ToLocalTime().ToString(DateFormats.Date, Thread.CurrentThread.CurrentUICulture);                                
            }
        }

        public string FormatNullableDate(DateTime? input, bool isDateTime = false)
        {
            if (input.HasValue)
            {
                return FormatDate(input.Value, isDateTime);
            }

            return string.Empty;
        }

        public string FormatUserName(string firstName, string lastName)
        {
            if (IsLastFirstName)
            {
                return $"{firstName} {lastName}";
            }

            return $"{lastName} {firstName}";
        }
        
        internal string FormatUserName(UserNamesStruct userNamesStruct)
        {
            return userNamesStruct == null
                       ? string.Empty
                       : FormatUserName(userNamesStruct.FirstName, userNamesStruct.LastName);
        }

        public string FormatText(Field field)
        {
            var content = field.StringValue;
            if (string.IsNullOrEmpty(content))
                return content;

            if (field.TranslateThis)
            {
                if (field.Key.Equals(GlobalEnums.TranslationCaseFields.Status_Id.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    field.Key.Equals(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    content = Translation.GetMasterDataTranslation(content);
                }
                else
                {
                    content = Translation.GetCoreTextTranslation(content);
                }
            }
            
            if (!string.IsNullOrEmpty(content))
            {
                content = HttpUtility.HtmlEncode(content).Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>");
            }
            
            return content;
        }
    }

    public static class HtmlHelperExtension
    {
        public static MvcHtmlString FormaDate(this HtmlHelper h, DateTime? value, OutputFormatter formatter)
        {
            return new MvcHtmlString(formatter.FormatNullableDate(value));
        }

        public static MvcHtmlString FormaDate(this HtmlHelper h, DateTime value, OutputFormatter formatter)
        {
            return new MvcHtmlString(formatter.FormatDate(value));
        }
        
        public static MvcHtmlString FormatUserName(this HtmlHelper h, UserNamesStruct value, OutputFormatter formatter)
        {
            return new MvcHtmlString(formatter.FormatUserName(value));
        }
    }
}

