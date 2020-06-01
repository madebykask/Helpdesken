using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Enums.Reports;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services.CaseStatistic;
using DH.Helpdesk.Web.Areas.Reports.Models.ReportService;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using WebGrease.Css.Extensions;

namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using Services.Services.Reports;
    using BusinessData.Models.ReportService;
    using System;
    using DH.Helpdesk.BusinessData.OldComponents;

    public class ServicesController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;
        private readonly IReportServiceService _reportServiceService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseStatisticService _caseStatisticService;

        public ServicesController(
            IMasterDataService masterDataService,
            IUserService userService,
            IWorkContext workContext,
            IWorkingGroupService workingGroupService,
            ICaseTypeService caseTypeService,
            IReportServiceService reportServiceService,
            IProductAreaService productAreaService,
            ICaseSearchService caseSearchService,
            ICaseSettingsService caseSettingService,
            ICaseFieldSettingService caseFieldSettingService,
            ICustomerUserService customerUserService,
            ICaseStatisticService caseStatisticService)
            : base(masterDataService)
        {
            _userService = userService;
            _workContext = workContext;
            _workingGroupService = workingGroupService;
            _caseTypeService = caseTypeService;
            _reportServiceService = reportServiceService;
            _productAreaService = productAreaService;
            _caseSearchService = caseSearchService;
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
            _customerUserService = customerUserService;
            _caseStatisticService = caseStatisticService;
        }

        [HttpGet]
        public JsonResult GetWorkingGroupUsers(int? workingGroupId)
        {
            var users = _userService.GetWorkingGroupUsers(_workContext.Customer.CustomerId, workingGroupId);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHistoricalData(HistoricalReportFilterModel filter)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);

            //*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */
            var workingGroups = user.UserGroup_Id > UserGroups.Administrator ?
                _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).ToList() :
                _workingGroupService.GetWorkingGroups(customerId, user.Id, false, true).ToList();

            if (filter.HistoricalWorkingGroups != null && filter.HistoricalWorkingGroups.Any())
                workingGroups = workingGroups.Where(o => filter.HistoricalWorkingGroups.Contains(o.Id)).ToList();

            var dataFilter = GetCommonDataFilter<HistoricalDataFilter>(filter);
            dataFilter.CaseStatus = filter.CaseStatus == 2 ? 1 : filter.CaseStatus == 1 ? 0 : (int?)null; // 1 active, 0 closed else null
            dataFilter.ChangeFrom = filter.HistoricalChangeDateFrom ?? DateTime.Now.AddYears(-100);
            dataFilter.ChangeTo = filter.HistoricalChangeDateTo.HasValue
                ? filter.HistoricalChangeDateTo.GetEndOfDay().Value
                : DateTime.Now.AddYears(20);
            dataFilter.ChangeWorkingGroups = workingGroups.Select(o => o.Id).ToList();
            dataFilter.IncludeHistoricalCasesWithNoWorkingGroup = filter.HistoricalWorkingGroups == null ||
                                                        !filter.HistoricalWorkingGroups.Any();

            var result = _reportServiceService.GetHistoricalData(dataFilter, SessionFacade.CurrentUser.Id);

            var wgs = result.Select(o => new { o.WorkingGroup, o.WorkingGroupID }).Distinct().OrderBy(o => o.WorkingGroup).ToArray();
            var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true);
            TranslateCaseTypes(caseTypes, 0);
            var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList()).ToList();
            var total = result.Count();
            var responce = new
            {
                totalLabel = string.Format("{0}: {1}", Translation.GetCoreTextTranslation("Summa"), total),
                data = new
                {
                    labels = wgs.Select(o => o.WorkingGroup ?? "").ToArray(),
                    datasets = result.GroupBy(o => new { o.CaseTypeID, o.CaseType }).Select(ct => new
                    {
                        label = caseTypesFullNames.Single(ctf => ctf.Id == ct.Key.CaseTypeID).Name,
                        data = wgs.Select(wg => ct.Count(o => o.WorkingGroupID == wg.WorkingGroupID)).ToArray()
                    }).ToArray()
                }
            };

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportedTimeData(ReportedTimeReportFilterModel filter)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            var dataFilter = GetCommonDataFilter<ReportedTimeDataFilter>(filter);
            dataFilter.GroupBy = (ReportedTimeGroup)filter.GroupBy;
            dataFilter.LogNoteFrom = filter.LogNoteFrom;
            dataFilter.LogNoteTo = filter.LogNoteTo.HasValue ? filter.LogNoteTo.GetEndOfDay() : new DateTime?();

            var result = _reportServiceService.GetReportedTimeData(dataFilter, SessionFacade.CurrentUser.Id);
            var minutesInHour = 60.0;
            var totalHours = result.Sum(o => o.TotalTime) / minutesInHour;
            switch (dataFilter.GroupBy)
            {
                case ReportedTimeGroup.CaseType_Id:
                    {
                        var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true);
                        TranslateCaseTypes(caseTypes, 0);
                        var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList()).ToList();
                        result.ForEach(p => p.Label = caseTypesFullNames
                                                          .SingleOrDefault(c => c.Id == p.Id)
                                                          ?.Name ?? "");
                        result = result.OrderBy(d => d.Label).ToList();
                        break;
                    }
                case ReportedTimeGroup.ProductArea_Id:
                    {
                        var productAreas = _productAreaService.GetTopProductAreasWithChilds(customerId, false);
                        TranslateProductAreas(productAreas, 0);
                        var productAreasFullNames = _productAreaService.GetChildrenInRow(productAreas.OrderBy(p => p.Name).ToList()).ToList();
                        result.ForEach(p => p.Label = productAreasFullNames
                           .SingleOrDefault(c => c.Id == p.Id)
                           ?.Name ?? "");
                        result = result.OrderBy(d => d.Label).ToList();
                        break;
                    }
                case ReportedTimeGroup.LogNoteDate:
                    {
                        var formatter = new OutputFormatter(true, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                        result.ForEach(d => d.Label = formatter.FormatNullableDate(d.DateTime));
                        result = result.OrderBy(d => d.DateTime).ToList();
                        break;
                    }
                default:
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
            }
            var responce = new
            {
                totalLabel = string.Format("{0}: {1}", Translation.GetCoreTextTranslation("Summa"), totalHours),
                data = new
                {
                    labels = result.Select(o => o.Label ?? "").ToList(),
                    datasets = new[]
                    {
                        new
                        {
                            label = Translation.GetCoreTextTranslation("Antal"),
                            data = result.Select(o => o.TotalTime / minutesInHour)
                        }
                    }
                }
            };

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNumberOfCasesData(NumberOfCasesReportFilterModel filter)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            var dataFilter = GetCommonDataFilter<NumberOfCasesDataFilter>(filter);
            dataFilter.GroupBy = (NumberOfCasesGroup)filter.GroupBy;

            var result = _reportServiceService.GetNumberOfCasesData(dataFilter, SessionFacade.CurrentUser.Id);
            var totalCases = result.Sum(c => c.CasesAmount);
            switch (dataFilter.GroupBy)
            {
                case NumberOfCasesGroup.CaseType_Id:
                    var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true);
                    TranslateCaseTypes(caseTypes, 0);
                    var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList()).ToList();
                    result.ForEach(p => p.Label = caseTypesFullNames
                                                      .SingleOrDefault(c => c.Id == p.Id)
                                                      ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationWeekday:
                    result.ForEach(p => { p.Label = GetWeekDayName(p.Id); });
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationMonth:
                    result.ForEach(p => { p.Label = GetMonthName(p.Id); });
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationYear:
                case NumberOfCasesGroup.RegistrationHour:
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationDate:
                case NumberOfCasesGroup.FinishingDate:
                    var formatter = new OutputFormatter(true, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    result.ForEach(d => d.Label = formatter.FormatNullableDate(d.DateTime));
                    result = result.OrderBy(d => d.DateTime).ToList();
                    break;
                case NumberOfCasesGroup.ProductArea_Id:
                    var productAreas = _productAreaService.GetTopProductAreasWithChilds(customerId, false);
                    TranslateProductAreas(productAreas, 0);
                    var productAreasFullNames = _productAreaService.GetChildrenInRow(productAreas.OrderBy(p => p.Name).ToList()).ToList();
                    result.ForEach(p => p.Label = productAreasFullNames
                                                      .SingleOrDefault(c => c.Id == p.Id)
                                                      ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                default:
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
            }

            var responce = new
            {
                totalLabel = string.Format("{0}: {1}", Translation.GetCoreTextTranslation("Summa"), totalCases),
                data = new
                {
                    labels = result.Select(o => o.Label ?? "").ToList(),
                    datasets = new[]
                    {
                        new
                        {
                            label = Translation.GetCoreTextTranslation("Antal"),
                            data = result.Select(c => c.CasesAmount)
                        }
                    }
                }
            };
            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSolvedInTimeData(SolvedInTimeReportFilterModel filter)
        {

            var dataFilter = GetCommonDataFilter<SolvedInTimeDataFilter>(filter);
            dataFilter.GroupBy = (SolvedInTimeGroup)filter.GroupBy;

            var result = _reportServiceService.GetSolvedInTimeData(dataFilter, SessionFacade.CurrentUser.Id);
            switch (dataFilter.GroupBy)
            {
                case SolvedInTimeGroup.CaseType_Id:
                    var caseTypes = _caseTypeService.GetAllCaseTypes(SessionFacade.CurrentCustomer.Id, false, true);
                    TranslateCaseTypes(caseTypes, 0);
                    var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList())
                        .ToList();
                    result.ForEach(p => p.Label = caseTypesFullNames
                                                      .SingleOrDefault(c => c.Id == p.Id)
                                                      ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                case SolvedInTimeGroup.ProductArea_Id:
                    var productAreas = _productAreaService.GetTopProductAreasWithChilds(SessionFacade.CurrentCustomer.Id, false);
                    TranslateProductAreas(productAreas, 0);
                    var productAreasFullNames = _productAreaService.GetChildrenInRow(productAreas.OrderBy(p => p.Name).ToList()).ToList();
                    result.ForEach(p => p.Label = productAreasFullNames
                                                      .SingleOrDefault(c => c.Id == p.Id)
                                                      ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                case SolvedInTimeGroup.FinishingMonth:
                    result.ForEach(p => { p.Label = GetMonthName(p.Id); });
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                case SolvedInTimeGroup.FinishingYear:
                    result.ForEach(p => { p.Label = p.Id.ToString(); });
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                default:
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
            }

            //var searchFilter = new CaseSearchFilter();
            //searchFilter.CustomerId = SessionFacade.CurrentCustomer.Id;
            //searchFilter.UserId = SessionFacade.CurrentUser.Id;
            //searchFilter.CaseProgress = CaseProgressFilter.ClosedCases;
            //searchFilter.CaseClosingDateStartFilter = filter.CloseFrom.GetStartOfDay();
            //searchFilter.CaseClosingDateEndFilter = filter.CloseTo.GetEndOfDay();
            //searchFilter.UserPerformer = filter.Administrators?.JoinToString() ?? string.Empty;
            //searchFilter.CaseListType = filter.CaseTypes?.JoinToString() ?? string.Empty;
            //searchFilter.Department = filter.Departments?.JoinToString() ?? string.Empty;
            //searchFilter.WorkingGroup = filter.WorkingGroups?.JoinToString() ?? string.Empty;
            //searchFilter.ProductArea = filter.ProductAreas?.JoinToString() ?? string.Empty;
            //var statistics = CountSolvedInTime(searchFilter, (NumberOfCasesGroup) filter.GroupBy);
            //var allStatisticsCount = statistics.Key;
            //var inTimeStatisticsCount = statistics.Value;

            //allStatisticsCount == 0 ? 0 : ((inTimeStatisticsCount * 100) / allStatisticsCount);
            var total = result.Sum(r => r.Total);
            var totalSolvedInTime = result.Sum(r => r.SolvedInTimeTotal);
            var totalPercent = total == 0 ? 0 : ((totalSolvedInTime * 100) / total);
            var responce =  new
                {
                    totalLabel = string.Format("{0}: {1}% ({2}/{3})", Translation.GetCoreTextTranslation("Lösta i tid"), totalPercent, totalSolvedInTime, total),
                    data = new {
                        labels = result.Select(o => o.Label ?? "").ToList(),
                        datasets = new[]
                        {
                            new
                            {
                                label = "%",
                                data = result.Select(r => r.Total == 0 ? 0 : ((r.SolvedInTimeTotal * 100) / r.Total)),
                                rawData = result.Select(r => new { label = r.Label, solvedInTimeTotal = r.SolvedInTimeTotal, total = r.Total })
                            }
                        }
                    }
                };

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        //private KeyValuePair<int, int> CountSolvedInTime(CaseSearchFilter filter, NumberOfCasesGroup groupBy)
        //{
        //    var customerId = SessionFacade.CurrentCustomer.Id;
        //    var user = SessionFacade.CurrentUser;
        //    var userId = user.Id;
        //    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

        //    var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId).ToArray();
        //    var customerUserSettings = _customerUserService.GetCustomerUserSettings(customerId, userId);
        //    var caseUserSettings = _caseSettingService.GetCaseSettingsWithUser(customerId, userId, SessionFacade.CurrentUser.UserGroupId);

        //    CaseRemainingTimeData remainingTime;
        //    CaseAggregateData aggregateData;
        //    var search = new Search();
        //    search.SortBy = "CaseNumber";
        //    search.Ascending = false;

        //    var searchResult = _caseSearchService
        //                            .Search(
        //                            filter,
        //                            caseUserSettings,
        //                            caseFieldSettings,
        //                            user.Id,
        //                            user.UserId,
        //                            user.ShowNotAssignedWorkingGroups,
        //                            user.UserGroupId,
        //                            customerUserSettings.RestrictedCasePermission,
        //                            search,
        //                            0,
        //                            0,
        //                            userTimeZone,
        //                            ApplicationTypes.Helpdesk,
        //                            false,
        //                            out remainingTime,
        //                            out aggregateData,
        //                            null,
        //                            null,
        //                            null,
        //                            false);
        //    switch (groupBy)
        //    {
        //        case NumberOfCasesGroup.CaseType_Id:
        //            var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true);
        //            TranslateCaseTypes(caseTypes, 0);
        //            var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList()).ToList();
        //            searchResult.Items.ForEach( p => p.Columns.Select(f => f.) = caseTypesFullNames
        //                                                            .SingleOrDefault(c => c.Id == p.Id)
        //                                                            ?.Name ?? "");
        //            result = result.OrderBy(d => d.Label).ToList();


        //            break;
        //        case NumberOfCasesGroup.WorkingGroup_Id:
        //            break;
        //        case NumberOfCasesGroup.StateSecondary_Id:
        //            break;
        //        case NumberOfCasesGroup.Department_Id:
        //            break;
        //        case NumberOfCasesGroup.Priority_Id:
        //            break;
        //        case NumberOfCasesGroup.FinishingDate:
        //            break;
        //        case NumberOfCasesGroup.ProductArea_Id:
        //            break;
        //        case NumberOfCasesGroup.FinishingMonth:
        //            break;
        //        case NumberOfCasesGroup.FinishingYear:
        //        default:
        //            break;
        //    }
        //    var casesIds = searchResult.Items.Select(c => c.Id).ToList();
        //    var statistics = _caseStatisticService.GetForCases(casesIds);
        //    var inTime = statistics.Count(s => !s.WasSolvedInTime.HasValue || s.WasSolvedInTime.Value.ToBool());

        //    return new KeyValuePair<int, int>(statistics.Count, inTime);
        //}

        private string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return Translation.GetCoreTextTranslation("Januari");
                case 2:
                    return Translation.GetCoreTextTranslation("Februari");
                case 3:
                    return Translation.GetCoreTextTranslation("Mars");
                case 4:
                    return Translation.GetCoreTextTranslation("April");
                case 5:
                    return Translation.GetCoreTextTranslation("Maj");
                case 6:
                    return Translation.GetCoreTextTranslation("Juni");
                case 7:
                    return Translation.GetCoreTextTranslation("Juli");
                case 8:
                    return Translation.GetCoreTextTranslation("Augusti");
                case 9:
                    return Translation.GetCoreTextTranslation("September");
                case 10:
                    return Translation.GetCoreTextTranslation("Oktober");
                case 11:
                    return Translation.GetCoreTextTranslation("November");
                case 12:
                    return Translation.GetCoreTextTranslation("December");
            }

            return "";
        }

        private string GetWeekDayName(int weekDay)
        {
            switch (weekDay)
            {
                case 1:
                    return Translation.GetCoreTextTranslation("Söndag");
                case 2:
                    return Translation.GetCoreTextTranslation("Måndag");
                case 3:
                    return Translation.GetCoreTextTranslation("Tisdag");
                case 4:
                    return Translation.GetCoreTextTranslation("Onsdag");
                case 5:
                    return Translation.GetCoreTextTranslation("Torsdag");
                case 6:
                    return Translation.GetCoreTextTranslation("Fredag");
                case 7:
                    return Translation.GetCoreTextTranslation("Lördag");
            }

            return Translation.GetCoreTextTranslation("Saknas");
        }

        private T GetCommonDataFilter<T>(CommonReportFilterModel filter) where T : CommonReportDataFilter, new()
        {
            return new T
            {
                CustomerID = SessionFacade.CurrentCustomer.Id,
                Administrators = filter.Administrators,
                CaseStatus = filter.CaseStatus,
                CaseTypes = filter.CaseTypes,
                RegisterFrom = filter.RegisterFrom,
                RegisterTo = filter.RegisterTo.HasValue ? filter.RegisterTo.GetEndOfDay() : new DateTime?(),
                CloseFrom = filter.CloseFrom,
                CloseTo = filter.CloseTo.HasValue ? filter.CloseTo.GetEndOfDay() : new DateTime?(),
                Departments = filter.Departments,
                ProductAreas = filter.ProductAreas,
                WorkingGroups = filter.WorkingGroups,
            };
        }

        private void TranslateCaseTypes(IEnumerable<CaseType> caseTypes, int depth)
        {
            if (depth >= 20)
                throw new Exception("Iteration depth exceeded. Suspicion of infinte loop.");

            depth++;

            caseTypes.ForEach(p =>
            {
                p.Name = Translation.GetCoreTextTranslation(p.Name);

                if (p.SubCaseTypes != null && p.SubCaseTypes.Any())
                {
                    TranslateCaseTypes(p.SubCaseTypes, depth);
                }
            });
        }

        private void TranslateProductAreas(IEnumerable<ProductArea> products, int depth)
        {
            if (depth >= 20)
                throw new Exception("Iteration depth exceeded. Suspicion of infinte loop.");

            depth++;

            products.ForEach(p =>
            {
                p.Name = Translation.GetCoreTextTranslation(p.Name);
                if (p.SubProductAreas != null && p.SubProductAreas.Any())
                {
                    TranslateProductAreas(p.SubProductAreas, depth);
                }
            });
        }
    }
}
