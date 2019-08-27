using System;
using System.Globalization;
using DH.Helpdesk.Common.Extensions.DateTime;

namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class DateToDate
    {
        #region ctor()

        public DateToDate()
        {
            FromDate = null;
            ToDate = null;
        }

        public DateToDate(DateTime? fromDate, DateTime? toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        #endregion

        #region Public Properties

        public DateTime? FromDate { get; private set; }

        public DateTime? ToDate { get; private set; }

        #endregion

        #region Methods

        public string GetDateString(string sep = ",", string format = "yyyy-MM-dd")
        {
            return $"{FormatDate(FromDate, format)}{sep}{FormatDate(ToDate, format)}";
        }

        private static string FormatDate(DateTime? date, string format)
        {
            return date?.ToString(format, DateTimeFormatInfo.InvariantInfo);
        }

        public bool HasValues
        {
            get { return this.FromDate.HasValue || this.ToDate.HasValue; }
        }

        #endregion
    }
}