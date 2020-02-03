using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services.CaseStatistic;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;

    public sealed class StatisticsService : IStatisticsService
    {
        private readonly IWorkContext _workContext;
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseStatisticService _caseStatisticService;

        public StatisticsService(IWorkContext workContext,
            ICaseSearchService caseSearchService,
            ICaseSettingsService caseSettingService,
            ICaseFieldSettingService caseFieldSettingService,
            ICustomerUserService customerUserService,
            ICaseStatisticService caseStatisticService)
        {
            _workContext = workContext;
            _caseSearchService = caseSearchService;
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
            _customerUserService = customerUserService;
            _caseStatisticService = caseStatisticService;
        }

        public StatisticsOverview GetStatistics(int[] customers, UserOverview user)
        {
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

            var model = new StatisticsOverview();
            foreach (var customerId in customers)
            {
                var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId).ToArray();
                var customerUserSettings = _customerUserService.GetCustomerUserSettings(customerId, user.Id);
                var caseUserSettings = _caseSettingService.GetCaseSettingsWithUser(customerId, user.Id, user.UserGroupId);
                
                var filter = new CaseSearchFilter();
                filter.CustomerId = customerId;
                filter.UserId = user.Id;
                filter.InitiatorSearchScope = CaseInitiatorSearchScope.UserAndIsAbout;
                filter.CaseProgress = CaseProgressFilter.CasesInProgress;
                model.InProgress += CountCases(user, filter, caseUserSettings, caseFieldSettings, customerUserSettings, userTimeZone);

                filter.CaseProgress = CaseProgressFilter.UnreadCases;
                model.Unopened += CountCases(user, filter, caseUserSettings, caseFieldSettings, customerUserSettings, userTimeZone);

                filter.CaseProgress = CaseProgressFilter.CasesInRest;
                model.Onhold += CountCases(user, filter, caseUserSettings, caseFieldSettings, customerUserSettings, userTimeZone);

                filter = new CaseSearchFilter();
                filter.CustomerId = customerId;
                filter.UserId = user.Id;
                filter.CaseProgress = CaseProgressFilter.None;
                filter.CaseRegistrationDateStartFilter = DateTime.Today.GetStartOfDay();
                filter.CaseRegistrationDateEndFilter = DateTime.Today.GetEndOfDay();
                model.NewToday += CountCases(user, filter, caseUserSettings, caseFieldSettings, customerUserSettings, userTimeZone);

                filter = new CaseSearchFilter();
                filter.CustomerId = customerId;
                filter.UserId = user.Id;
                filter.CaseProgress = CaseProgressFilter.ClosedCases;
                filter.CaseClosingDateStartFilter = DateTime.Today.GetStartOfDay();
                filter.CaseClosingDateEndFilter = DateTime.Today.GetEndOfDay();
                model.SolvedToday += CountCases(user, filter, caseUserSettings, caseFieldSettings, customerUserSettings, userTimeZone);

                filter = new CaseSearchFilter();
                filter.CustomerId = customerId;
                filter.UserId = user.Id;
                filter.CaseProgress = CaseProgressFilter.CasesInProgress;
                var result = CountOverdue(user, filter, caseUserSettings, caseFieldSettings,
                    customerUserSettings, userTimeZone);
                model.Overdue += result.Overdue;
                model.DueToday += result.DueToday;

                filter = new CaseSearchFilter();
                filter.CustomerId = customerId;
                filter.UserId = user.Id;
                filter.CaseProgress = CaseProgressFilter.ClosedCases;
                filter.CaseClosingDateStartFilter = DateTime.Today.GetStartOfDay();
                filter.CaseClosingDateEndFilter = DateTime.Today.GetEndOfDay();
                model.SolvedInTimeToday += CountSolvedInTime(user, filter, caseUserSettings, caseFieldSettings,
                    customerUserSettings, userTimeZone);
            }

            return model;
        }

        private int CountSolvedInTime(UserOverview user, CaseSearchFilter filter, IList<CaseSettings> caseUserSettings,
            CaseFieldSetting[] caseFieldSettings, CustomerUser customerUserSettings, TimeZoneInfo userTimeZone)
        {
            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;
            var search = new Search();
            search.SortBy = "CaseNumber";
            search.Ascending = false;

            var searchResult = _caseSearchService.Search(
                filter,
                caseUserSettings,
                caseFieldSettings,
                user.Id,
                user.UserId,
                user.ShowNotAssignedWorkingGroups,
                user.UserGroupId,
                customerUserSettings.RestrictedCasePermission,
                search,
                0,
                0,
                userTimeZone,
                ApplicationTypes.Helpdesk,
                false,
                out remainingTime,
                out aggregateData,
                null,
                null,
                null,
                false);
            var casesIds = searchResult.Items.Select(c => c.Id).ToList();
            var statistics = _caseStatisticService.GetForCases(casesIds);
            var inTime = statistics.Count(s => !s.WasSolvedInTime.HasValue || s.WasSolvedInTime.Value.ToBool());

            return !statistics.Any() ? 0 : (int)Math.Round((double)(inTime * 100) / statistics.Count);
        }

        private OverdueValues CountOverdue(UserOverview user, CaseSearchFilter filter,
            IList<CaseSettings> caseUserSettings, CaseFieldSetting[] caseFieldSettings,
            CustomerUser customerUserSettings, TimeZoneInfo userTimeZone)
        {

            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;
            var search = new Search();
            search.SortBy = "CaseNumber";
            search.Ascending = false;

            var searchResult = _caseSearchService.Search(
                filter,
                caseUserSettings,
                caseFieldSettings,
                user.Id,
                user.UserId,
                user.ShowNotAssignedWorkingGroups,
                user.UserGroupId,
                customerUserSettings.RestrictedCasePermission,
                search,
                _workContext.Customer.WorkingDayStart,
                _workContext.Customer.WorkingDayEnd,
                userTimeZone,
                ApplicationTypes.Helpdesk,
                true,
                out remainingTime,
                out aggregateData,
                null,
                null,
                null,
                false);

            //var now = TimeZoneInfo.ConvertTime(DateTime.Now, userTimeZone);
            //var endOfWorkingDay =  TimeZoneInfo.ConvertTime(new DateTime(now.Year, now.Month, now.Day, _workContext.Customer.WorkingDayEnd, 0, 0, DateTimeKind.Unspecified), userTimeZone);

            //var workingHoursLeftToday = Convert.ToInt32(Math.Round((endOfWorkingDay - now).TotalHours));
            int workingHoursCount = 24;
            if (_workContext.Customer.WorkingDayEnd > _workContext.Customer.WorkingDayStart)
            {
                workingHoursCount = _workContext.Customer.WorkingDayEnd - _workContext.Customer.WorkingDayStart;
            }
            else if (_workContext.Customer.WorkingDayEnd < _workContext.Customer.WorkingDayStart)
            {
                workingHoursCount = _workContext.Customer.WorkingDayStart - _workContext.Customer.WorkingDayEnd;
            }
           
            return new OverdueValues 
            {
                Overdue = remainingTime.CaseRemainingTimes.Count(t => t.RemainingTime < 0),
                DueToday = remainingTime.CaseRemainingTimes.Count(t => workingHoursCount >= t.RemainingTime && t.RemainingTime >= 0)
            };
        }

        private int CountCases(UserOverview user, CaseSearchFilter filter, IList<CaseSettings> caseUserSettings,
            CaseFieldSetting[] caseFieldSettings, CustomerUser customerUserSettings, TimeZoneInfo userTimeZone)
        {
            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;

            var searchResult = _caseSearchService.Search(
                filter,
                caseUserSettings,
                caseFieldSettings,
                user.Id,
                user.UserId,
                user.ShowNotAssignedWorkingGroups,
                user.UserGroupId,
                customerUserSettings.RestrictedCasePermission,
                null, //sm.Search,
                0,
                0,
                userTimeZone,
                ApplicationTypes.Helpdesk,
                false,
                out remainingTime,
                out aggregateData,
                null,
                null,
                null,
                true);
            return searchResult.Count;
        }

        private struct OverdueValues
        {
            public int Overdue { get; set; }
            public int DueToday { get; set; }
        }
    }
}