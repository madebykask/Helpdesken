using System;
using System.Collections.Generic;
using System.Threading;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Services.BusinessLogic.Contracts;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Components.Contracts
{
    public class ContractHistoryFormatter : IContractHistoryFormatter
    {
        private readonly bool _isUserFirstLastNameRepresentation;
        private readonly IDictionary<int, string> _followUpIntervals;
        private TimeZoneInfo CurrentTimeZone { get; set; }

        #region ctor()

        public ContractHistoryFormatter(TimeZoneInfo currentTimeZoneInfo,
            IDictionary<int, string> followUpIntervals,
            bool isUserFirstLastNameRepresentation)
        {
            CurrentTimeZone = currentTimeZoneInfo;
            _followUpIntervals = followUpIntervals;
            _isUserFirstLastNameRepresentation = isUserFirstLastNameRepresentation;
        }

        #endregion

        public string FormatToBoolean(int? val)
        {
            if (!val.HasValue)
                return string.Empty;
            return val.Value.ToBool().ToString();
        }

        public string FormatNullableDate(DateTime? input, bool isDateTime = false)
        {
            if (input.HasValue)
            {
                return this.FormatDate(input.Value, isDateTime);
            }

            return string.Empty;
        }

        public string FormatNoticeTime(int val)
        {
            return string.Format("{0} {1}", val, Translation.GetCoreTextTranslation("months"));
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
                return input.ToLocalTime().ToString("yyyy-MM-dd", Thread.CurrentThread.CurrentUICulture);
            }
        }

        public string FormatUserName(UserBasicOvierview user)
        {
            if (user == null)
                return null;

            var userNameFormatted = _isUserFirstLastNameRepresentation
                ? $"{user.FirstName} {user.SurName}"
                : $"{user.SurName} {user.FirstName}";

            return userNameFormatted;
        }

        public string FormartFollowUpInterval(int? val)
        {
            if (val.HasValue)
            {
                return _followUpIntervals.ContainsKey(val.Value) ? _followUpIntervals[val.Value] : val.ToString();
            }
            return string.Empty;
        }
    }
}