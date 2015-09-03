namespace DH.Helpdesk.Web.Infrastructure.CaseOverview
{
    using System;
    using System.Threading;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Enums.Cases;
    using DH.Helpdesk.Domain;

    public class OutputFormatter
    {
        public OutputFormatter(bool isLastFirstName)
        {
            this.IsLastFirstName = isLastFirstName;
        }

        public bool IsLastFirstName { get; set; }

        public string FormatField(Field field)
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
                    return this.FormatNullableDate(field.DateTimeValue);
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

        public string FormatDate(DateTime input)
        {
            return input.ToLocalTime().ToString("g", Thread.CurrentThread.CurrentUICulture);
        }

        public string FormatNullableDate(DateTime? input)
        {
            if (input.HasValue)
            {
                return this.FormatDate(input.Value);
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
}