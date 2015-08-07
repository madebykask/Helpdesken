namespace DH.Helpdesk.Web.Infrastructure.CaseOverview
{
    using System;
    using System.Threading;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Enums.Cases;
    using DH.Helpdesk.Domain;

    public class OutputFormatter
    {
        public string FormatField(Field field, TimeZoneInfo userTimeZone)
        {
            switch (field.FieldType)
            {
                case FieldTypes.Date:
                    if (field.DateTimeValue.HasValue)
                    {
                        return field.DateTimeValue.Value.ToShortDateString();
                    }
                    break;
                case FieldTypes.Time:
                    if (field.DateTimeValue.HasValue)
                    {
                        return TimeZoneInfo.ConvertTimeFromUtc(field.DateTimeValue.Value, userTimeZone).ToString("g", Thread.CurrentThread.CurrentUICulture);
                    }
                    break;
                case FieldTypes.NullableHours:
                    return string.IsNullOrEmpty(field.StringValue) ? " - " : string.Format("{0} h", field.StringValue);
                default:
                    if (field.TranslateThis)
                    {
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
    }
}