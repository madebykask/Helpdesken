using System;
using DH.Helpdesk.BusinessData.Models.Case;

namespace DH.Helpdesk.Services.BusinessLogic.Contracts
{
    public interface IContractHistoryFormatter
    {
        string FormatToBoolean(int? val);
        string FormatNullableDate(DateTime? input, bool isDateTime = false);
        string FormatNoticeTime(int val);
        string FormatDate(DateTime input, bool isDateTime = false);
        string FormatUserName(UserBasicOvierview user);
        string FormartFollowUpInterval(int? val);
    }
}