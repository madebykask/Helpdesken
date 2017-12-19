using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.Web.Infrastructure.CaseOverview
{
    using System;
    using System.Threading;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums.Cases;
    using Field = DH.Helpdesk.Domain.Field;
    using System.Collections.Generic;    
    public class OutputFormatter
    {
        /* fields showing in DateTime format */        
        private static readonly IList<string> dateTimeFields = new List<string> { "ChangeTime", "RegTime" }.AsReadOnly();
        public TimeZoneInfo CurrentTimeZone { get; private set; }

        public OutputFormatter(bool isLastFirstName, TimeZoneInfo timeZone)
        {
            this.IsLastFirstName = isLastFirstName;
            this.CurrentTimeZone = timeZone;
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
                        isDateTime = dateTimeFields.Contains(field.Key);                
                        return this.FormatDate(field.DateTimeValue.Value, isDateTime);
                    }
                    break;
                case FieldTypes.Time:
                    isDateTime = dateTimeFields.Contains(field.Key);                
                    return this.FormatNullableDate(field.DateTimeValue, isDateTime);

                case FieldTypes.NullableHours:
                    return string.IsNullOrEmpty(field.StringValue) ? " - " : string.Format("{0} h", field.StringValue);
                default:
                    if (field.TranslateThis)
                    {
                        if (field.Key.Equals(GlobalEnums.TranslationCaseFields.Status_Id.ToString()) || field.Key.Equals(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()))
                        {
                            return Translation.GetMasterDataTranslation(field.StringValue);
                        }
                        return Translation.Get(field.StringValue);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(field.StringValue))
                        {
                            return
                                MvcHtmlString.Create(field.StringValue.Replace(Environment.NewLine, "<br />"))
                                    .ToHtmlString();
                        }
                    }

                    break;
            }

            return string.Empty;
        }

        public string FormatDate(DateTime input, bool isDateTime = false)
        {
            if (isDateTime)
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(input, this.CurrentTimeZone);
                return localTime.ToString("yyyy-MM-dd HH:mm:ss", Thread.CurrentThread.CurrentUICulture);
            }
            else
            {
                return  input.ToLocalTime().ToString("yyyy-MM-dd", Thread.CurrentThread.CurrentUICulture);                                
            }
        }

        public string FormatNullableDate(DateTime? input, bool isDateTime = false)
        {
            if (input.HasValue)
            {
                return this.FormatDate(input.Value, isDateTime);
            }

            return string.Empty;
        }

        public string FormatUserName(string firstName, string lastName)
        {
            if (this.IsLastFirstName)
            {
                return string.Format("{0} {1}", firstName, lastName);
            }

            return string.Format("{0} {1}", lastName, firstName);
        }
        
        internal string FormatUserName(BusinessData.Models.Case.UserNamesStruct userNamesStruct)
        {
            return userNamesStruct == null
                       ? string.Empty
                       : this.FormatUserName(userNamesStruct.FirstName, userNamesStruct.LastName);
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

