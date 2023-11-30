using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Common.Extensions.String;

namespace DH.Helpdesk.Dal.Repositories
{
    public class CaseSearchQueryBuilder
    {
        private bool _useFts = false;
        private readonly Regex _fullTextExpression;
		private IFileIndexingRepository _fileIndexingRepository;

		public CaseSearchQueryBuilder(IFileIndexingRepository fileIndexingRepository)
        {
			_fileIndexingRepository = fileIndexingRepository;
			_fullTextExpression = new Regex("(?:\"([^\"]*)\")|([^\\s]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        #region Tables Fields Constants

        protected static class Tables
        {
            public static class Case
            {
                public const string ReportedBy = "ReportedBy";
                public const string Persons_Name = "Persons_Name";
                public const string RegUserName = "RegUserName";
                public const string Persons_EMail = "Persons_EMail";
                public const string Persons_Phone = "Persons_Phone";
                public const string Persons_CellPhone = "Persons_CellPhone";
                public const string Place = "Place";
                public const string Caption = "Caption";
                public const string Description = "Description";
                public const string Miscellaneous = "Miscellaneous";
                public const string ReferenceNumber = "ReferenceNumber";
                public const string InvoiceNumber = "InvoiceNumber";
                public const string InventoryNumber = "InventoryNumber";
                public const string InventoryType = "InventoryType";
                public const string InventoryLocation = "InventoryLocation";
                public const string VerifiedDescription = "VerifiedDescription";
                public const string UserCode = "UserCode";
                public const string CostCentre = "CostCentre";
                public const string Available = "Available";
            }

            public static class CaseIsAbout
            {
                public const string ReportedBy = "ReportedBy";
                public const string Person_Name = "Person_Name";
                public const string UserCode = "UserCode";
                public const string Person_Email = "Person_Email";
                public const string Place = "Place";
                public const string Person_CellPhone = "Person_CellPhone";
                public const string Person_Phone = "Person_Phone";
                public const string CostCentre = "CostCentre";

            }

            public static class Log
            {
                public const string Text_Internal = "Text_Internal";
                public const string Text_External = "Text_External";
            }

            public static class Region
            {
                public const string RegionName = "Region";
            }

            public static class Department
            {
                public const string DepartmentId = "DepartmentId";
                public const string DepartmentName = "Department";
            }

            public static class FormFieldValue
            {
                public const string FormFieldValueField = "FormFieldValue";
            }
        }

        #endregion

        #region FreeText Search Condition Fields

        private readonly string[] _initiatorCaseConditionFields = new string[]
        {
            Tables.Case.ReportedBy,
            Tables.Case.Persons_Name,
            Tables.Case.UserCode,
            Tables.Case.Persons_EMail,
            Tables.Case.Place,
            Tables.Case.Persons_CellPhone,
            Tables.Case.Persons_Phone
        };

        private readonly string[] _freeTextCaseConditionFields = new string[]
        {
            Tables.Case.ReportedBy,
            Tables.Case.Persons_Name,
            Tables.Case.RegUserName,
            Tables.Case.Persons_EMail,
            Tables.Case.Persons_Phone,
            Tables.Case.Persons_CellPhone,
            Tables.Case.Place,
            Tables.Case.Caption,
            Tables.Case.Description,
            Tables.Case.Miscellaneous,
            Tables.Case.ReferenceNumber,
            Tables.Case.InvoiceNumber,
            Tables.Case.InventoryNumber,
            Tables.Case.InventoryType,
            Tables.Case.InventoryLocation,
            Tables.Case.VerifiedDescription,
            Tables.Case.CostCentre,
            Tables.Case.Available

        };

        private readonly string[] _freeTextCaseIsAboutConditionFields = new string[]
        {
            Tables.CaseIsAbout.ReportedBy,
            Tables.CaseIsAbout.Person_Name,
            Tables.CaseIsAbout.UserCode,
            Tables.CaseIsAbout.Person_Email,
            Tables.CaseIsAbout.Place,
            Tables.CaseIsAbout.Person_CellPhone,
            Tables.CaseIsAbout.Person_Phone,
            Tables.CaseIsAbout.CostCentre,

        };

        private readonly string[] _freeTextLogConditionFields = new string[]
        {
            Tables.Log.Text_Internal,
            Tables.Log.Text_External
        };

        private readonly string[] _freeTextRegionConditionFields = new string[]
        {
            Tables.Region.RegionName
        };

        private readonly string[] _freeTextDepartmentConditionFields = new string[]
        {
            Tables.Department.DepartmentId,
            Tables.Department.DepartmentName
        };


		#endregion

		public string BuildCaseSearchSql(SearchQueryBuildContext ctx, bool countOnly = false)
        {
            _useFts = ctx.UseFullTextSearch;

            var search = ctx.Criterias.Search;
            var searchFilter = ctx.Criterias.SearchFilter;

            var sql = new List<string>();

            var freeTextSearchCte = BuildFreeTextSearchCTEQuery(ctx);

            
            var orderBy = BuildOrderBy(search);

            //TODO: remove top 500 limit when true sql side paging is implemented
            //vid avslutade ärenden visas bara första 500
            var sqlTop500 = (searchFilter.CaseProgress == CaseProgressFilter.ClosedCases || searchFilter.CaseProgress == CaseProgressFilter.None) ? "top 500" : "";
            const string subQueryName = "RowConstrainedResult";
            var outerFields = ctx.Criterias.FetchInfoAboutParentChild ? $",{GetParentChildInfo(subQueryName)}" : "";

            if (countOnly)
            {
                var searchQuery = BuildSearchQueryInner(ctx, true);
                sql.Add(searchQuery);
            }
            else
            {
                var searchQuery = BuildSearchQueryInner(ctx, false);
                sql.Add($@"SELECT * {outerFields} 
                           FROM ( SELECT {sqlTop500} *, ROW_NUMBER() OVER ( ORDER BY {orderBy} ) AS RowNum 
                                  FROM ( {searchQuery} ) as tbl
                           ) as {subQueryName} ");

                //if (f.PageInfo != null)
                //{
                //	sql.Add(string.Format(" WHERE RowNum > {0} AND RowNum <= {1}", f.PageInfo.PageNumber * f.PageInfo.PageSize, f.PageInfo.PageNumber * f.PageInfo.PageSize + f.PageInfo.PageSize));
                //}

                sql.Add(" ORDER BY RowNum");
            }

            var output = string.Join(" ", sql);

            //add free text search cte query at the top
            if (!string.IsNullOrEmpty(freeTextSearchCte))
            {
                output = $"{freeTextSearchCte}{Environment.NewLine}{output}";
            }

            return output;
        }

        #region ReturnCaseSearchSqlCount

        //todo: use once proper sql side paging is implemented

        private string BuildCaseSearchSqlCount(SearchQueryBuildContext ctx, out Exception ex)
        {
            var criterias = ctx.Criterias;

			ex = null;

            var userId = criterias.UserId;
            var searchFilter = criterias.SearchFilter;
            var customerSetting = criterias.CustomerSetting;

            var sql = new List<string>();
            sql.Add("select Count(*) as Total");

            // tables and joins
            sql.Add(GetTablesAndJoins(ctx));

            // WHERE ..
            var applicationType = criterias.ApplicationType;
            if (applicationType.ToLower() == ApplicationTypes.LineManager || applicationType.ToLower() == ApplicationTypes.SelfService)
            {
                var caseSearchWhere = this.ReturnCustomCaseSearchWhere(searchFilter, criterias.UserUniqueId, userId, criterias.GlobalSetting);
                sql.Add(caseSearchWhere);
            }
            else
            {
                var whereSql = this.ReturnCaseSearchWhere(ctx);
                sql.Add(whereSql);
            }

            return string.Join(" ", sql);
        }

        #endregion

        private string BuildOrderBy(ISearch search)
        {
            // ORDER BY ...
            var orderBy = new List<string>();
            var sort = search != null && !string.IsNullOrEmpty(search.SortBy)
                ? search.SortBy.Replace("_temporary_", string.Empty)
                : string.Empty;

            if (string.IsNullOrEmpty(sort))
            {
                orderBy.Add(" CaseNumber desc");
            }
            else
            {
                if (string.Compare(sort, "Priority_Id", false, CultureInfo.InvariantCulture) == 0)
                {
                    sort = "[Priority]";
                    orderBy.Add(sort);
                    if (search != null && !search.Ascending)
                    {
                        orderBy.Add("desc");
                    }

                    sort = ", OrderNum";
                    orderBy.Add(sort);
                    if (search != null && !search.Ascending)
                    {
                        orderBy.Add("desc");
                    }
                }
                else
                {
                    orderBy.Add(sort);
                    if (search != null && !search.Ascending)
                    {
                        orderBy.Add("desc");
                    }
                }

                if (sort.ToLower() != "casenumber")
                    orderBy.Add(", CaseNumber desc");
            }

            var s = string.Join(" ", orderBy);
            return s;
        }

        private string BuildSearchQueryInner(SearchQueryBuildContext ctx, bool countQuery = false)
        {
            var criterias = ctx.Criterias;
            var searchFilter = criterias.SearchFilter;
            var customerSettings = criterias.CustomerSetting;
            var searchQueryBld = new StringBuilder();

            var columns = new List<string>();

            // select 
            if (countQuery)
            {
                searchQueryBld.AppendFormat("SELECT COUNT(DISTINCT tblCase.Id)");
            }
            else
            {
                #region adding columns in SUB-SELECT

                columns.Add("tblCase.Id");
                columns.Add("tblCase.CaseNumber");
                columns.Add("tblCase.Place");
                columns.Add("tblCustomer.Name as Customer_Id");
                columns.Add("tblCase.Customer_Id as CaseCustomerId");
                columns.Add("tblCase.WorkingGroup_Id as CaseWorkingGroupId");
                columns.Add("tblRegion.Region as Region_Id");
                columns.Add("tblOU.OU as OU_Id");
                columns.Add("tblCase.UserCode");
                columns.Add("tblCase.Department_Id");
                columns.Add("tblCase.Persons_Name");
                columns.Add("tblCase.Persons_EMail");
                columns.Add("tblCase.Persons_Phone");
                columns.Add("tblCase.Persons_CellPhone");
                columns.Add("tblCase.FinishingDate");
                columns.Add("tblCase.FinishingDescription");
                columns.Add("tblCase.Caption");

                columns.Add("tblCase.Performer_User_Id as CasePerformerUserId");
                columns.Add("tblCase.CaseResponsibleUser_Id as CaseResponsibleUserId");
                columns.Add("tblCase.User_Id as CaseUserId");
                if (searchFilter.MaxTextCharacters > 0)
                    columns.Add(string.Format("Cast(tblCase.[Description] as Nvarchar({0})) as [Description] ", searchFilter.MaxTextCharacters));
                else
                    columns.Add("Cast(tblCase.[Description] as Nvarchar(4000)) as [Description] ");

                columns.Add("tblCase.Miscellaneous");
                columns.Add("tblCase.[Status] ");
                columns.Add("tblCase.ExternalTime");
                columns.Add("tblCase.RegTime");
                columns.Add("tblCase.ReportedBy");
                columns.Add("tblCase.ProductArea_Id");
                columns.Add("tblCase.InvoiceNumber");
                columns.Add("tblCustomer.Name");
                columns.Add("tblDepartment.Department as DepertmentName");
                columns.Add("tblDepartment.DepartmentId");
                columns.Add("tblDepartment.SearchKey");
                columns.Add("tblCase.ReferenceNumber");
                columns.Add("tblRegistrationSourceCustomer.SourceName as RegistrationSourceCustomer");

                columns.Add("tblCaseIsAbout.ReportedBy as IsAbout_ReportedBy");
                columns.Add("tblCaseIsAbout.Person_Name as IsAbout_Persons_Name");
                //New columns added for IsAbout 5.5.0
                columns.Add("tblCaseIsAbout.Person_CellPhone as IsAbout_Persons_CellPhone");
                columns.Add("tblCaseIsAbout.CostCentre as IsAbout_CostCentre");
                columns.Add("tblCaseIsAbout.UserCode as IsAbout_UserCode");
                columns.Add("tblCaseIsAbout.Person_Email as IsAbout_Persons_Email");
                columns.Add("tblCaseIsAbout.Place as IsAbout_Place");
                columns.Add("tblCaseIsAbout.Person_Phone as IsAbout_Persons_Phone");
                columns.Add("tblCase.AgreedDate");

                if (customerSettings != null)
                {
                    if (customerSettings.IsUserFirstLastNameRepresentation == 1)
                    {
                        columns.Add("coalesce(tblUsers.FirstName, '') + ' ' + coalesce(tblUsers.SurName, '') as Performer_User_Id");
                        columns.Add("CASE WHEN tblCase.User_Id IS NULL THEN IsNull(tblCase.RegUserName, coalesce(tblCase.RegUserId, '')) ELSE coalesce(tblUsers2.FirstName, '') + ' ' + coalesce(tblUsers2.SurName, '') END as User_Id");
                        columns.Add("coalesce(tblUsers3.FirstName, '') + ' ' + coalesce(tblUsers3.SurName, '') as CaseResponsibleUser_Id");
                    }
                    else
                    {
                        columns.Add("coalesce(tblUsers.SurName, '') + ' ' + coalesce(tblUsers.FirstName, '') as Performer_User_Id");
                        columns.Add("CASE WHEN tblCase.User_Id IS NULL THEN IsNull(tblCase.RegUserName, coalesce(tblCase.RegUserId, '')) ELSE coalesce(tblUsers2.SurName, '') + ' ' + coalesce(tblUsers2.FirstName, '') END as User_Id");
                        columns.Add("coalesce(tblUsers3.SurName, '') + ' ' + coalesce(tblUsers3.FirstName, '') as CaseResponsibleUser_Id");
                    }

                    columns.Add("coalesce(tblUsers4.SurName, '') + ' ' + coalesce(tblUsers4.FirstName, '') as tblProblem_ResponsibleUser_Id");
                }

                columns.Add("tblProblem.ProblemName as Problem");
                columns.Add("tblStatus.StatusName as Status_Id");
                columns.Add("tblSupplier.Supplier as Supplier_Id");

                string appId = string.Empty;
                try
                {
                    appId = ConfigurationManager.AppSettings["InitFromSelfService"];
                }
                catch (Exception)
                {
                }

                if (appId == "true")
                {
                    columns.Add("tblStateSecondary.AlternativeStateSecondaryName as StateSecondary_Id");
                }
                else
                {
                    columns.Add("tblStateSecondary.StateSecondary as StateSecondary_Id");
                }

                columns.Add("tblCase.Priority_Id");
                columns.Add("tblPriority.PriorityName");
                columns.Add("tblPriority.Priority");
                columns.Add("tblPriority.OrderNum");
                columns.Add("coalesce(tblPriority.SolutionTime, 0) as SolutionTime");
                columns.Add("tblCase.WatchDate");
                columns.Add("tblCaseType.RequireApproving");
                columns.Add("tblCase.ApprovedDate");
                columns.Add("tblCase.ContactBeforeAction");
                columns.Add("tblCase.SMS");
                columns.Add("tblCase.Available");
                columns.Add("tblCase.Cost");
                columns.Add("tblCase.CostCentre");
                columns.Add("tblCase.PlanDate");
                columns.Add("tblProject.Name as Project");

                if (customerSettings != null)
                {
                    columns.Add("tblWorkingGroup.WorkingGroup as WorkingGroup_Id");
                }

                columns.Add("tblCase.ChangeTime");
                columns.Add("tblCaseType.Id as CaseType_Id");
                columns.Add("tblCase.RegistrationSource");
                columns.Add("tblCase.InventoryNumber");
                columns.Add("tblCase.InventoryType as ComputerType_Id");
                columns.Add("tblCase.InventoryLocation");
                columns.Add("tblCategory.Category as Category_Id");
                columns.Add("tblCase.SolutionRate");
                columns.Add("tblSystem.SystemName as System_Id");
                columns.Add("tblUrgency.Urgency as Urgency_Id");
                columns.Add("tblImpact.Impact as Impact_Id");
                columns.Add("tblCase.Verified");
                columns.Add("tblCase.VerifiedDescription");
                columns.Add("tblCase.LeadTime");
                columns.Add("tblCase.Status_Id as aggregate_Status");
                columns.Add("tblCase.StateSecondary_Id as aggregate_SubStatus");
                //columns.Add("ClosingReasonTable.ClosingReason as ClosingReason");

                columns.Add(string.Format("'0' as [{0}]", CaseSearchConstants.TimeLeftColumn.SafeForSqlInject()));
                columns.Add("tblStateSecondary.IncludeInCaseStatistics");

                if (criterias.CaseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.CausingPart.ToString()))
                {
                    columns.Add("tblCausingPart.Name as CausingPart");
                }

                #endregion

                var columnsFormatted = string.Join(",", columns);
                searchQueryBld.AppendFormat("SELECT DISTINCT {0}", columnsFormatted);
                searchQueryBld.AppendLine();
            }
            
            // tables and joins
            var tablesJoins = GetTablesAndJoins(ctx);
            searchQueryBld.AppendLine(tablesJoins);

            // WHERE
            var whereStatement = IsHelpdeskApplication(ctx.Criterias) 
                ? ReturnCaseSearchWhere(ctx) 
                : ReturnCustomCaseSearchWhere(searchFilter, criterias.UserUniqueId, criterias.UserId, criterias.GlobalSetting);

            searchQueryBld.Append(whereStatement);

            var output = searchQueryBld.ToString();
            return output;
        }
            
        private string BuildFreeTextSearchCTEQuery(SearchQueryBuildContext ctx)
        {
            var strBld = new StringBuilder();
            
            if (IsFreeTextSearch(ctx.Criterias.SearchFilter))
            {
                var freeText = ctx.Criterias.SearchFilter.FreeTextSearch;
                
                //todo: refactor to have separate query builder classes for different application types
                if (IsHelpdeskApplication(ctx.Criterias))
                {
                    strBld.AppendLine("WITH SearchFreeTextFilter (CaseId) as ( ");

                    var items = new List<string>
                    {
                        BuildCaseFreeTextSearchQueryCte(freeText, ctx),
                        //BuildRegionFreeTextSearchQueryCte(freeText, ctx),
                        BuildDepartmentFreeTextSearchQueryCte(freeText, ctx),
                        BuildCaseIsAboutFreeTextSearchQueryCte(freeText, ctx),
                        BuildLogFreeTextSearchQueryCte(freeText, ctx),
                        BuilFormFieldValueFreeTextSearchQueryCte(freeText, ctx)
                    };

                    strBld.AppendLine(string.Join($"{Environment.NewLine} UNION ALL {Environment.NewLine} ", items));
                    strBld.AppendLine(") ");

                    ctx.UseFreeTextCaseSearchCTE = true;
                }
                else
                {
                    //todo: implement full text search conditions for other application types?
                }
            }

            return strBld.ToString();
        }

        #region FreeText Search Queries

        private string BuildCaseFreeTextSearchQueryCte(string text, SearchQueryBuildContext ctx)
        {
            var filter = ctx.Criterias.SearchFilter;
            var customerId = filter.CustomerId;
            var strBld = new StringBuilder();
            strBld.AppendLine(@"SELECT _case.Id FROM tblCase _case  WITH (NOLOCK) ");
            strBld.AppendLine(@"  INNER JOIN tblCustomer ON tblCustomer.Id = _case.Customer_Id ");
            if (!ctx.Criterias.CustomerSetting.CustomerInExtendedSearch.ToBool())
                strBld.AppendLine(@"  INNER JOIN tblCustomerUser ON tblCustomerUser.Customer_Id = tblCustomer.Id");
            strBld.AppendFormat(" WHERE _case.Customer_Id = {0} ", customerId).AppendLine();
            strBld.AppendLine(" AND _case.Deleted = 0");
            if (!ctx.Criterias.CustomerSetting.CustomerInExtendedSearch.ToBool())
                strBld.AppendFormat(" AND tblCustomerUser.User_Id = {0}", filter.UserId).AppendLine();

            var finishingDateCondition = BuildFinishingDateCondition(filter, "_case");
            if (!string.IsNullOrEmpty(finishingDateCondition))
                strBld.AppendLine(finishingDateCondition);

            strBld.AppendLine(" AND (");

            var items = BuildFreeTextConditionsFor(text, _freeTextCaseConditionFields, "_case");
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);

            strBld.AppendLine(" )");
            strBld.AppendLine(@"GROUP BY _case.Id");
            return strBld.ToString();
        }

        private string BuildCaseIsAboutFreeTextSearchQueryCte(string freeText, SearchQueryBuildContext ctx)
        {
            var filter = ctx.Criterias.SearchFilter;
            var customerId = filter.CustomerId;
            var strBld = new StringBuilder();

            strBld.AppendLine(@"SELECT Case_Id FROM tblCaseIsAbout caseIsAbout ");
            strBld.AppendLine(@"  INNER JOIN tblCase WITH (nolock) ON tblCase.Id = caseIsAbout.Case_Id ");
            strBld.AppendFormat(" WHERE tblCase.Customer_Id = {0} ", customerId).AppendLine();
            strBld.AppendLine(" AND (");

            var items = BuildFreeTextConditionsFor(freeText, _freeTextCaseIsAboutConditionFields, "caseIsAbout");
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);

            strBld.AppendLine(" )");
            strBld.AppendLine(@"GROUP BY Case_Id");
            return strBld.ToString();
        }

        private string BuildLogFreeTextSearchQueryCte(string freeText, SearchQueryBuildContext ctx)
        {
            var filter = ctx.Criterias.SearchFilter;
            var customerId = filter.CustomerId;
            var strBld = new StringBuilder();

            strBld.AppendLine(@"SELECT Case_Id FROM tblLog WITH (NOLOCK)");
            strBld.AppendLine(@"  INNER JOIN tblCase WITH (nolock) ON tblLog.Case_Id = tblCase.Id ");
            strBld.AppendLine(@"  INNER JOIN tblCustomer ON tblCustomer.Id = tblCase.Customer_Id ");
            if (!ctx.Criterias.CustomerSetting.CustomerInExtendedSearch.ToBool())
                strBld.AppendLine(@"  INNER JOIN tblCustomerUser ON tblCustomerUser.Customer_Id = tblCustomer.Id");
            strBld.AppendLine(" WHERE ");
            strBld.AppendFormat("tblCase.Customer_Id = {0} ", customerId).AppendLine();
            strBld.AppendLine(" AND tblCase.Deleted = 0");
            if (!ctx.Criterias.CustomerSetting.CustomerInExtendedSearch.ToBool())
                strBld.AppendFormat(" AND tblCustomerUser.User_Id = {0}", filter.UserId).AppendLine();

            var finishingDateCondition = BuildFinishingDateCondition(filter);
            if (!string.IsNullOrEmpty(finishingDateCondition))
                strBld.AppendLine(finishingDateCondition);
            
            strBld.AppendLine(" AND (");

            var items = BuildFreeTextConditionsFor(freeText, _freeTextLogConditionFields);
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);

            strBld.AppendLine(" )");
            strBld.AppendLine(@"GROUP BY tblLog.Case_Id");
            return strBld.ToString();
        }

        private string BuildRegionFreeTextSearchQueryCte(string freeText, CaseSearchFilter filter)
        {
            var customerId = filter.CustomerId;
            var strBld = new StringBuilder();

            strBld.AppendLine(@"SELECT caseReg.Id FROM tblRegion reg ");
            strBld.AppendLine("  INNER JOIN tblCase caseReg WITH (nolock) ON reg.Id = caseReg.Region_Id ");
            strBld.AppendFormat(" WHERE caseReg.Customer_Id = {0} ", customerId).AppendLine();
            strBld.AppendLine(" AND (");
            var items = BuildFreeTextConditionsFor(freeText, _freeTextRegionConditionFields);
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);

            strBld.AppendLine(" )");
            strBld.AppendLine(@"GROUP BY caseReg.Id");
            return strBld.ToString();
        }

        private string BuildDepartmentFreeTextSearchQueryCte(string freeText, SearchQueryBuildContext ctx)
        {
            var filter = ctx.Criterias.SearchFilter;
            var customerId = filter.CustomerId;
            var strBld = new StringBuilder();

            strBld.AppendLine(@"SELECT caseDep.Id FROM tblDepartment dep ");
            strBld.AppendLine("  INNER JOIN tblCase caseDep WITH (nolock) ON dep.Id = caseDep.Department_Id ");
            strBld.AppendFormat(" WHERE caseDep.Customer_Id = {0} ", customerId).AppendLine();
            strBld.AppendLine(" AND (");
            var items = BuildFreeTextConditionsFor(freeText, _freeTextDepartmentConditionFields);
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);

            strBld.AppendLine(" )");
            strBld.AppendLine(@"GROUP BY caseDep.Id");
            return strBld.ToString();
        }

        private string BuilFormFieldValueFreeTextSearchQueryCte(string freeText, SearchQueryBuildContext ctx)
        {
            var filter = ctx.Criterias.SearchFilter;
            var customerId = filter.CustomerId;
            var strBld = new StringBuilder();

            strBld.AppendLine(@"select tblCase.Id from tblCase WITH (NOLOCK) ");
            strBld.AppendLine(@"  INNER JOIN tblFormFieldValue WITH (NOLOCK) ON tblCase.Id = tblFormFieldValue.Case_Id ");
            strBld.AppendLine(@"  INNER JOIN tblCustomer ON tblCustomer.Id = tblCase.Customer_Id ");
            if (!ctx.Criterias.CustomerSetting.CustomerInExtendedSearch.ToBool())
                strBld.AppendLine(@"  INNER JOIN tblCustomerUser ON tblCustomerUser.Customer_Id = tblCustomer.Id");
            strBld.AppendLine(" WHERE ");
            strBld.AppendFormat("tblCase.Customer_Id = {0} ", customerId).AppendLine();
            strBld.AppendLine(" AND tblCase.Deleted = 0");
            if (!ctx.Criterias.CustomerSetting.CustomerInExtendedSearch.ToBool())
                strBld.AppendFormat(" AND tblCustomerUser.User_Id = {0}", filter.UserId).AppendLine();

            var finishingDateCondition = BuildFinishingDateCondition(filter);
            if (!string.IsNullOrEmpty(finishingDateCondition))
                strBld.AppendLine(finishingDateCondition);

            strBld.AppendLine(" AND (");

            strBld.AppendLine(BuildContainsExpession(Tables.FormFieldValue.FormFieldValueField, freeText));

            strBld.AppendLine(" )");
            strBld.AppendLine(@"GROUP BY tblCase.Id");
            return strBld.ToString();
        }

        #endregion

        private string GetParentChildInfo(string nestedTableAlias)
        {
            var columns = new List<string>();

            columns.Add($"(select count(Ancestor_Id)  from tblParentChildCaseRelations where Ancestor_Id = {nestedTableAlias}.Id) as IsParent");
            columns.Add($"(select Top 1 (Ancestor_Id)  from tblParentChildCaseRelations where Descendant_Id = {nestedTableAlias}.Id) as ParentCaseId");
            columns.Add($"(select count(MergedParent_Id)  from tblMergedCases where MergedParent_Id = {nestedTableAlias}.Id) as IsMergeParent");
            columns.Add($"(select count(MergedChild_Id)  from tblMergedCases where MergedChild_Id = {nestedTableAlias}.Id) as IsMergeChild");
            return string.Join(",", columns);
        }

        private string GetTablesAndJoins(SearchQueryBuildContext ctx)
        {
            var customerSetting = ctx.Criterias.CustomerSetting;
            var caseSettings = ctx.Criterias.CaseSettings;
            var userId = ctx.Criterias.UserId;

            var tables = new List<string>();

            #region adding tables into FROM section

            tables.Add("from tblCase WITH (NOLOCK) ");
            tables.Add("inner join tblCustomer on tblCase.Customer_Id = tblCustomer.Id ");
            
            if (ctx.Criterias.SearchFilter.IsExtendedSearch == false)
                tables.Add("inner join tblCustomerUser on tblCase.Customer_Id = tblCustomerUser.Customer_Id ");

            if (ctx.UseFreeTextCaseSearchCTE)
            {
                tables.Add("LEFT JOIN (SELECT DISTINCT sfr.CaseId " +
                           "           FROM SearchFreeTextFilter sfr) freeTextSearchResults ON tblCase.Id = freeTextSearchResults.CaseId");
            }
            if(ctx.Criterias.ApplicationType.ToLower() == "selfservice")
            {
                tables.Add("left outer join (select Distinct cef.Case_Id " +
                           "           FROM tblCaseExtrafollowers cef) follower ON tblCase.Id = follower.Case_Id");
            }
            

            tables.Add("left outer join tblDepartment on tblDepartment.Id = tblCase.Department_Id ");
            tables.Add("left outer join tblRegion on tblCase.Region_Id = tblRegion.Id ");
            tables.Add("left outer join tblOU on tblCase.OU_Id=tblOU.Id ");
            tables.Add("left outer join tblSupplier on tblCase.Supplier_Id=tblSupplier.Id ");
            tables.Add("left outer join tblSystem on tblCase.System_Id = tblSystem.Id ");
            tables.Add("left outer join tblUrgency on tblCase.Urgency_Id = tblUrgency.Id ");
            tables.Add("left outer join tblImpact on tblCase.Impact_Id = tblImpact.Id ");
            tables.Add("left outer join tblRegistrationSourceCustomer on tblCase.RegistrationSourceCustomer_Id = tblRegistrationSourceCustomer.Id ");
            tables.Add("left outer join tblUsers on tblUsers.Id = tblCase.Performer_User_Id ");
            tables.Add("left outer join tblCaseIsAbout on tblCaseIsAbout.Case_Id = tblCase.Id ");
            tables.Add("left outer join tblCaseFollowUps on tblCaseFollowUps.Case_Id = tblCase.Id ");
            tables.Add("left outer join tblProject on tblProject.Id = tblCase.Project_Id ");

            if (customerSetting != null)
            {
                const int SHOW_IF_DEFAULT_USER_GROUP = 0;
                if (customerSetting.CaseWorkingGroupSource == SHOW_IF_DEFAULT_USER_GROUP)
                {
                    tables.Add(string.Format("left join tblUserWorkingGroup as defGroup on defGroup.User_Id = {0} and defGroup.IsDefault = 1 and defGroup.WorkingGroup_Id = tblCase.WorkingGroup_Id", userId));
                    tables.Add("left join tblWorkingGroup on tblWorkingGroup.id = defgroup.WorkingGroup_Id");
                }
                else
                {
                    tables.Add("left outer join tblWorkingGroup on tblCase.WorkingGroup_Id = tblWorkingGroup.Id ");
                }
            }
            tables.Add("left outer join tblStatus on tblCase.Status_Id = tblStatus.Id ");
            tables.Add("left outer join tblCategory on tblCase.category_Id = tblCategory.Id ");
            tables.Add("left outer join tblStateSecondary on tblCase.StateSecondary_Id = tblStateSecondary.Id ");
            tables.Add("left outer join tblPriority on tblCase.Priority_Id = tblPriority.Id ");
            tables.Add("left outer join tblCaseType on tblCase.CaseType_Id = tblCaseType.Id ");
            tables.Add("left outer join tblUsers as tblUsers2 on tblCase.[User_Id] = tblUsers2.Id ");
            tables.Add("left outer join tblUsers as tblUsers3 on tblCase.CaseResponsibleUser_Id = tblUsers3.Id ");
            tables.Add("left outer join tblProblem on tblCase.Problem_Id = tblProblem.Id ");
            tables.Add("left outer join tblUsers as tblUsers4 on tblProblem.ResponsibleUser_Id = tblUsers4.Id ");
            //tables.Add("LEFT OUTER JOIN ( " +
            //     "    SELECT tblCaseHistory.Case_Id, " +
            //     "           tblCaseHistory.ClosingReason, " +
            //     "           ROW_NUMBER() OVER (PARTITION BY tblCaseHistory.Case_Id ORDER BY tblCaseHistory.CreatedDate DESC) AS RowNum " +
            //     "    FROM tblCaseHistory " +
            //     ") AS ClosingReasonTable ON tblCase.Id = ClosingReasonTable.Case_Id AND ClosingReasonTable.RowNum = 1 ");

            if (caseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.CausingPart.ToString()))
            {
                tables.Add("left outer join tblCausingPart on (tblCase.CausingPartId = tblCausingPart.Id)");
            }

            #endregion

            return string.Join(" ", tables);
        }

        //Used for LineManager & SelfService
        private string ReturnCustomCaseSearchWhere(CaseSearchFilter f, string userUserId, int userId, GlobalSetting globalSetting)
        {
            if (f == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // kund             
            sb.AppendLine($" where (tblCase.Customer_Id = {f.CustomerId}) ");
            sb.AppendLine(" and (tblCase.Deleted = 0)");

            //CaseType
            //sb.Append(" and (tblCaseType.ShowOnExternalPage <> 0)");
            //Hide this for next release #57742
            sb.AppendLine(" and (tblCaseType.ShowOnExtPageCases <> 0)");

            //ProductArea
            //sb.Append(" and (tblCase.ProductArea_Id Is Null or tblCase.ProductArea_Id in (select id from tblProductArea where ShowOnExternalPage <> 0))");
            //Hide this for next release #57742
            sb.AppendLine(" and (tblCase.ProductArea_Id Is Null or tblCase.ProductArea_Id in (select id from tblProductArea where ShowOnExtPageCases <> 0))");

            
            var criteria = f.CaseOverviewCriteria;
            var criteriaCondition = string.Empty;

            if (criteria.MyCasesRegistrator && !string.IsNullOrEmpty(criteria.UserId))
            {
                var con = $"tblCase.[RegUserId] = '{criteria.UserId.SafeForSqlInject()}'";
                criteriaCondition = criteriaCondition.AddWithSeparator($"({con})", false, " or ");
            }
            if ((criteria.MyCasesFollower && !string.IsNullOrEmpty(criteria.UserId)) || (criteria.MyCasesFollower && !string.IsNullOrEmpty(criteria.PersonEmail)))
            {
                //Skit
                var con = $"(exists (select 1 Follower from tblCaseExtraFollowers where (tblCaseExtraFollowers.Follower = '{criteria.UserId.SafeForSqlInject()}' or  tblcaseextrafollowers.Follower = '{criteria.PersonEmail.SafeForSqlInject()}') and tblCaseExtraFollowers.Case_Id = tblCase.Id))";
                criteriaCondition = criteriaCondition.AddWithSeparator($"({con})", false, " or ");
            }

            if (criteria.MyCasesInitiator && (!string.IsNullOrEmpty(criteria.UserId) || !string.IsNullOrEmpty(criteria.UserEmployeeNumber)))
            {
                var con = string.Empty;

                if (!string.IsNullOrEmpty(criteria.UserId))        
                    con = $"tblCase.[ReportedBy] = '{criteria.UserId.SafeForSqlInject()}'";

                if (!string.IsNullOrEmpty(criteria.PersonEmail))
                    con = con.AddWithSeparator($"tblCase.[Persons_EMail] = '{criteria.PersonEmail.SafeForSqlInject()}'", false, " or ");

                if (!string.IsNullOrEmpty(criteria.UserEmployeeNumber))
                    con = con.AddWithSeparator($"tblCase.[ReportedBy] = '{criteria.UserEmployeeNumber.SafeForSqlInject()}'", false, " or ");

                criteriaCondition = criteriaCondition.AddWithSeparator($"({con})", false, " or ");
            }

            if (criteria.MyCasesUserGroup)
            {
                var con = string.Empty;
                if (criteria.GroupMember != null && criteria.GroupMember.Any())
                {
                    var memberStream = string.Empty;
                    memberStream = memberStream.AddWithSeparator(criteria.GroupMember, true);
                    con = $"((tblCase.[ReportedBy] is null or tblCase.[ReportedBy] = '') And tblCase.[RegUserId] = '{criteria.UserId.SafeForSqlInject()}')" +
                              $" Or tblCase.[ReportedBy] in ({memberStream.SafeForSqlInjectForInOperator()})";                    
                }
                else
                {
                    con = $"tblCase.[RegUserId] = '{ criteria.UserId.SafeForSqlInject()}' And (tblCase.[ReportedBy] is null or tblCase.[ReportedBy] = '')";
                }                    

                criteriaCondition = criteriaCondition.AddWithSeparator($"({con})", false, " or ");
            }

            if (criteria.MyCasesInitiatorDepartmentId.HasValue)
            {
                var con = string.Empty;
                con = $"tblCase.[Department_Id] = {criteria.MyCasesInitiatorDepartmentId.Value}";
                criteriaCondition = criteriaCondition.AddWithSeparator($"({con})", false, " or ");
            }
            
            if (!string.IsNullOrEmpty(criteriaCondition))
                sb.Append($" AND ({criteriaCondition})");

            /*If there is no selected option, no one show*/
            if (!criteria.MyCasesRegistrator && 
                !criteria.MyCasesInitiator && 
                !criteria.MyCasesUserGroup && 
                !criteria.MyCasesInitiatorDepartmentId.HasValue &&
                !criteria.MyCasesFollower)
                sb.Append(" AND ( 1=2 )");

            // arende progress - iShow i gammal helpdesk
            var caseProgressConditions = BuildCaseProgressConditions(f, userId);
            if (!string.IsNullOrEmpty(caseProgressConditions))
                sb.AppendLine(caseProgressConditions);
            
            if (!string.IsNullOrWhiteSpace(f.FreeTextSearch))
            {
                var text = f.FreeTextSearch.SafeForSqlInject();
                sb.Append(" AND (");
                sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[ReportedBy]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Persons_Name]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Persons_EMail]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Persons_Phone]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Persons_CellPhone]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Place]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[InventoryLocation]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Caption]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Description]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblCase].[Miscellaneous]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblDepartment].[Department]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlContains("[tblDepartment].[DepartmentId]", text));
                sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblLog] WHERE CONTAINS([tblLog].[Text_Internal], N'\"*{0}*\"') OR CONTAINS([tblLog].[Text_External], N'\"*{0}*\"')))", text);
                //sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblLog] WHERE [tblLog].[Text_Internal] LIKE '%{0}%' OR [tblLog].[Text_External] LIKE '%{0}%'))", text);
                sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblFormFieldValue] WHERE {0}))", this.GetSqlContains("FormFieldValue", text));
                sb.Append(") ");
            }

            var s = sb.ToString();
            return s;
        }

        private string BuildCaseProgressConditions(CaseSearchFilter searchFilter, int userId)
        {
            var sb = new StringBuilder();

            var finishingDateCondition = BuildFinishingDateCondition(searchFilter);
            if (!string.IsNullOrEmpty(finishingDateCondition))
                sb.AppendLine(finishingDateCondition);
            
            //note finishingDate conditions were moved to BuildFinishingDateCondition method to be shared with other methods
            switch (searchFilter.CaseProgress)
            {
                case CaseProgressFilter.CasesInRest:
                    sb.AppendLine(" and (tblCase.StateSecondary_Id is not null and tblStateSecondary.IncludeInCaseStatistics = 0)");
                    break;
                case CaseProgressFilter.UnreadCases:
                    sb.AppendLine(" and (tblCase.Status = 1)");
                    break;
                case CaseProgressFilter.FinishedNotApproved:
                    sb.AppendLine(" and (tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case CaseProgressFilter.InProgressStatusGreater1:
                    sb.AppendLine(" and (tblCase.Status > 1)");
                    break;
                case CaseProgressFilter.CasesWithWatchDate:
                    sb.AppendLine(" and (tblCase.WatchDate is not null)");
                    break;
                case CaseProgressFilter.FollowUp:
                    //sb.Append(" and (tblCase.FollowUpdate is not null)");
                    sb.AppendLine(" and (tblCaseFollowUps.User_Id = " + userId + " and tblCaseFollowUps.IsActive = 1)");
                    break;
            }

            return sb.ToString();
        }

        private string BuildFinishingDateCondition(CaseSearchFilter f,  string caseTableAlias = null)
        {
            var condition = string.Empty;
            var caseTableNameOrAlias = string.IsNullOrEmpty(caseTableAlias) ? "tblCase" : caseTableAlias;

            switch (f.CaseProgress)
            {
                case CaseProgressFilter.None:
                case CaseProgressFilter.FollowUp:
                    break;

                case CaseProgressFilter.ClosedCases:
                case CaseProgressFilter.FinishedNotApproved:
                    condition = $" and ({caseTableNameOrAlias}.FinishingDate is not null)";
                    break;

                case CaseProgressFilter.FeedBack:
                    condition = $" and ({caseTableNameOrAlias}.FinishingDate is null or {caseTableNameOrAlias}.FinishingDate is not null)";
                    break;

                default:
                    condition = $" and ({caseTableNameOrAlias}.FinishingDate is null)";
                    break;
            }

            return condition;
        }

        
        private string ReturnCaseSearchWhere(SearchQueryBuildContext ctx)
        {
            var searchCriteria = ctx.Criterias;
            var searchFilter = searchCriteria.SearchFilter;
            var customerSetting = searchCriteria.CustomerSetting;

            #region Ignore 

            if (searchFilter == null || customerSetting == null || searchCriteria.GlobalSetting == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // kund 
            sb.Append(" where (tblCase.Customer_Id = " + searchFilter.CustomerId + ")");
            sb.Append(" and (tblCase.Deleted = 0)");


            if (searchFilter.IsConnectToParent && searchFilter.ToBeMerged == false)
            {
                sb.AppendFormat(" AND tblCase.Id NOT IN (select Descendant_Id From tblParentChildCaseRelations) ");
                if (searchFilter.CurrentCaseId.HasValue)
                    sb.AppendFormat(" AND tblCase.Id != {0} ", searchFilter.CurrentCaseId);
            }

            if (searchCriteria.CaseIds != null && searchCriteria.CaseIds.Any())
            {
                sb.AppendFormat(" AND ([tblCase].[Id] IN ({0})) ", string.Join(",", searchCriteria.CaseIds));
                //return sb.ToString();
            }

            // Related cases list http://redmine.fastdev.se/issues/11257
            if (searchCriteria.RelatedCasesCaseId.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[Id] != {0}) AND (LOWER(LTRIM(RTRIM([tblCase].[ReportedBy]))) = LOWER(LTRIM(RTRIM('{1}')))) ", searchCriteria.RelatedCasesCaseId.Value, searchCriteria.RelatedCasesUserId.SafeForSqlInject());

                // http://redmine.fastdev.se/issues/11257
                /*if (restrictedCasePermission == 1)
                {
                    if (userGroupId == (int)UserGroup.Administrator)
                    {
                        sb.AppendFormat(" AND ([tblCase].[Performer_User_Id] = {0} OR [tblCase].[CaseResponsibleUser_Id] = {0}) ", userId);
                    }
                    else if (userGroupId == (int)UserGroup.User)
                    {
                        sb.AppendFormat(" AND (LOWER(LTRIM(RTRIM([tblCase].[ReportedBy]))) = LOWER(LTRIM(RTRIM('{0}')))) ", userUserId);
                    }
                }*/

                return sb.ToString();
            }

            if (!searchFilter.IsExtendedSearch)
                sb.Append(" and (tblCustomerUser.[User_Id] = " + searchFilter.UserId + ")");

            ////////////////////////////////////////////////////////////////////////////////////
            // anvandaren far bara se avdelningar som den har behorighet till
            if (searchCriteria.UserDepartments.Any())
            {
                sb.Append(" and EXISTS(select 1 from tblDepartmentUser _depUser WHERE _depUser.Department_Id = tblCase.Department_Id AND _depUser.[User_Id] = tblCustomerUser.User_Id) ");
            }

            // finns kryssruta pa anvandaren att den bara far se sina egna arenden
            //Note, this is also checked in CasesController.cs for ExtendedSearch, so if you change logic here, change in controller
            var restrictedCasePermission = searchCriteria.CustomerUserSettings.RestrictedCasePermission;
            if (restrictedCasePermission && !searchFilter.IsExtendedSearch)
            {
                if (searchCriteria.UserGroupId == UserGroups.Administrator)
                    sb.Append(" and (tblCase.Performer_User_Id = " + searchCriteria.UserId + " or tblcase.CaseResponsibleUser_Id = " + searchCriteria.UserId + ")");
                else if (searchCriteria.UserGroupId == UserGroups.User)
                    sb.Append(" and (lower(tblCase.reportedBy) = lower('" + searchCriteria.UserUniqueId.SafeForSqlInject() + "') or tblcase.User_Id = " + searchCriteria.UserId + ")");
            }

            // arende progress - iShow i gammal helpdesk
            var caseProgressConditions = BuildCaseProgressConditions(searchFilter, searchCriteria.UserId);
            if (!string.IsNullOrEmpty(caseProgressConditions))
                sb.AppendLine(caseProgressConditions);

            // working group 
            if (!string.IsNullOrWhiteSpace(searchFilter.WorkingGroup))
            {
                var wgDict = searchFilter.WorkingGroup.Split(',').ToDictionary(it => it, it => true);
                var searchingUnassigned = wgDict.ContainsKey(int.MinValue.ToString());
                if (searchingUnassigned)
                {
                    wgDict.Remove(int.MinValue.ToString());
                }

                sb.Append(" AND (");

                if (searchingUnassigned)
                    sb.Append("tblCase.WorkingGroup_Id is NULL");

                if (wgDict.Count > 0)
                {
                    if (searchingUnassigned)
                        sb.Append(" OR");
                    if (searchCriteria.CustomerSetting.CaseWorkingGroupSource == 0)
                        sb.Append(" tblWorkingGroup.Id in (" + string.Join(",", wgDict.Keys).SafeForSqlInject() + ")");
                    else
                        sb.Append(" coalesce(tblCase.WorkingGroup_Id, 0) in (" + string.Join(",", wgDict.Keys).SafeForSqlInject() + ")");
                }

                sb.Append(") ");
            }

            if (searchFilter.SearchInMyCasesOnly)
            {
                var preparedUserIds = searchCriteria.UserId.ToString(CultureInfo.InvariantCulture).SafeForSqlInject();
                sb.AppendFormat(" AND ([tblCase].[Performer_User_Id] IN ({0}) ", preparedUserIds);

                if (searchCriteria.IsFieldResponsibleVisible)
                {
                    sb.AppendFormat(@"OR [tblCase].[CaseResponsibleUser_Id] IN ({0})", preparedUserIds);
                }

                sb.Append(") ");
            }

            // performer/utforare
            if (!string.IsNullOrWhiteSpace(searchFilter.UserPerformer))
            {
                var performersDict = searchFilter.UserPerformer.Split(',').ToDictionary(it => it, it => true);
                var searchingUnassigned = restrictedCasePermission == false && performersDict.ContainsKey(int.MinValue.ToString());
                if (searchingUnassigned)
                {
                    performersDict.Remove(int.MinValue.ToString());
                }

                if (performersDict.Count > 0 || searchingUnassigned)
                {
                    sb.Append(" AND (");
                }

                if (performersDict.Count > 0)
                {
                    sb.AppendFormat("tblCase.Performer_User_Id in ({0}) ", string.Join(",", performersDict.Keys).SafeForSqlInject());
                }

                if (searchingUnassigned || searchCriteria.CustomerUserSettings.User.ShowNotAssignedCases == 1)
                {
                    if (performersDict.Count > 0)
                    {
                        sb.AppendFormat(" OR ");
                    }

                    sb.Append("tblCase.Performer_User_Id is NULL");
                }

                if (performersDict.Count > 0 || searchingUnassigned)
                {
                    sb.Append(") ");
                }
            }

            // ansvarig
            if (!string.IsNullOrWhiteSpace(searchFilter.UserResponsible))
                sb.Append(" and (tblCase.CaseResponsibleUser_Id in (" + searchFilter.UserResponsible.SafeForSqlInject() + "))");

            // case type
            var caseTypes = ctx.Criterias.CaseTypes;
            if (caseTypes != null && caseTypes.Any())
            {
                sb.Append(" and (tblcase.CaseType_Id IN (" + string.Join(",", caseTypes) + "))");
            }

            // Product area 
            if (!string.IsNullOrWhiteSpace(searchFilter.ProductArea))
                if (string.Compare(searchFilter.ProductArea, "0", true, CultureInfo.InvariantCulture) != 0)
                    sb.Append(" and (tblcase.ProductArea_Id in (" + searchFilter.ProductArea.SafeForSqlInject() + "))");

            // department / avdelning
            if (!string.IsNullOrWhiteSpace(searchFilter.Department))
            {
				var departmentIds = searchFilter.Department.Split(',').Select(o => int.Parse(o)).ToDictionary(it => it, it => it);
				var searchUnassigned = departmentIds.ContainsKey(int.MinValue);
				if (searchUnassigned)
				{
					departmentIds.Remove(int.MinValue);
				}

				var departments = departmentIds.Any() ? departmentIds.Select(o => o.Key.ToString()).Aggregate((o, p) => $"{o}, {p}") : "";
				// organizationUnit
				if (!string.IsNullOrWhiteSpace(departments))
				{
					if (!string.IsNullOrWhiteSpace(searchFilter.OrganizationUnit))
					{
						switch (searchFilter.InitiatorSearchScope)
						{
							default:
							case CaseInitiatorSearchScope.UserAndIsAbout:
								sb.Append(" and ((tblCase.Department_Id in (" + departments.SafeForSqlInject() + ") or " +
										  "tblCaseIsAbout.Department_Id in (" + departments.SafeForSqlInject() + ") or " +
										  "tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + ") OR " +
										  "tblCaseIsAbout.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
								sb.Append(searchUnassigned ? " OR (tblCase.Department_Id IS NULL AND tblCaseIsAbout.Department_Id IS NULL))" : ")");
								break;
							case CaseInitiatorSearchScope.User:
								sb.Append(" and ((tblCase.Department_Id in (" + departments.SafeForSqlInject() + ") or " +
										  "tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
								sb.Append(searchUnassigned ? " OR (tblCase.Department_Id IS NULL))" : ")");
								break;
							case CaseInitiatorSearchScope.IsAbout:
								sb.Append(" and ((tblCaseIsAbout.Department_Id in (" + departments.SafeForSqlInject() + ") or " +
										  "tblCaseIsAbout.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
								sb.Append(searchUnassigned ? " OR (tblCaseIsAbout.Department_Id IS NULL))" : ")");
								break;
						}
					}
					else
					{
						switch (searchFilter.InitiatorSearchScope)
						{
							default:
							case CaseInitiatorSearchScope.UserAndIsAbout:
								sb.Append(" and ((tblCase.Department_Id in (" + departments.SafeForSqlInject() + ")" +
										  " or tblCaseIsAbout.Department_Id in (" + departments.SafeForSqlInject() + "))");
								sb.Append(searchUnassigned ? " OR (tblCase.Department_Id IS NULL AND tblCaseIsAbout.Department_Id IS NULL))" : ")");
								break;
							case CaseInitiatorSearchScope.User:
								sb.Append(" and ((tblCase.Department_Id in (" + departments.SafeForSqlInject() + "))");
								sb.Append(searchUnassigned ? " OR (tblCase.Department_Id IS NULL))" : ")");
								break;
							case CaseInitiatorSearchScope.IsAbout:
								sb.Append(" and ((tblCaseIsAbout.Department_Id in (" + departments.SafeForSqlInject() + "))");
								sb.Append(searchUnassigned ? " OR (tblCaseIsAbout.Department_Id IS NULL))" : ")");
								break;
						}
					}
				}
				else if (searchUnassigned)
				{

					var useOUFilter = !string.IsNullOrWhiteSpace(searchFilter.OrganizationUnit);

					switch (searchFilter.InitiatorSearchScope)
					{
						case CaseInitiatorSearchScope.UserAndIsAbout:
						default:
							sb.Append(" AND ((tblCase.Department_Id IS NULL AND tblCaseIsAbout.Department_Id IS NULL)");
							sb.Append(useOUFilter ? " OR (tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + ") OR tblCaseIsAbout.OU_Id IN (" +
								searchFilter.OrganizationUnit.SafeForSqlInject() + ")))" : ")");
							break;
						case CaseInitiatorSearchScope.User:
							sb.Append(" AND (tblCase.Department_Id IS NULL");
							sb.Append(useOUFilter ? " OR tblCase.OU_Id IN (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))" : ")");
							break;
						case CaseInitiatorSearchScope.IsAbout:
							sb.Append(" AND (tblCaseIsAbout.Department_Id IS NULL");
							sb.Append(useOUFilter ? " OR tblCaseIsAbout.OU_Id IN (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))" : ")");
							break;
					}
				}
				else
				{
					throw new ArgumentException("Invalid search filter for department");
				}
			}
            else
            {
				// organizationUnit
				if (!string.IsNullOrWhiteSpace(searchFilter.OrganizationUnit))
				{
					switch (searchFilter.InitiatorSearchScope)
					{
						case CaseInitiatorSearchScope.UserAndIsAbout:
						default:
							sb.Append("AND (tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + ") OR tblCaseIsAbout.OU_Id IN (" +
								searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
							break;
						case CaseInitiatorSearchScope.User:
							sb.Append("AND tblCase.OU_Id IN (" + searchFilter.OrganizationUnit.SafeForSqlInject() + ")");
							break;
						case CaseInitiatorSearchScope.IsAbout:
							sb.Append(" AND tblCaseIsAbout.OU_Id IN (" + searchFilter.OrganizationUnit.SafeForSqlInject() + ")");
							break;

					}
				}
            }

            // anvandare / user            
            if (!string.IsNullOrWhiteSpace(searchFilter.User))
            {
                sb.Append(" and (tblCase.User_Id in (" + searchFilter.User.SafeForSqlInject() + "))");
            }

            // region
            if (!string.IsNullOrWhiteSpace(searchFilter.Region))
            {
                var regionCondition = BuildRegionSearchCondition(searchFilter);
                sb.Append(regionCondition);
            }

			// prio
			if (!string.IsNullOrWhiteSpace(searchFilter.Priority))
			{
				var priorityIds = searchFilter.Priority.Split(",").Select(o => int.Parse(o.Trim()))
					.ToList();

				var searchNull = priorityIds.Any(o => o == int.MinValue);
				priorityIds.Remove(int.MinValue);

				if (priorityIds.Any() && searchNull)
				{
					sb.Append(" and (tblcase.Priority_Id in (" + priorityIds.Select(o => o.ToString()).Aggregate((o,p) => o + ", " + p) + ") OR tblcase.Priority_Id IS NULL)");
				}
				else if (priorityIds.Any())
				{
					sb.Append(" and (tblcase.Priority_Id in (" + priorityIds.Select(o => o.ToString()).Aggregate((o, p) => o + ", " + p) + "))");
				}
				else if (searchNull)
				{
					sb.Append(" and (tblcase.Priority_Id IS NULL)");
				}
			}
            // katagori / category
            if (!string.IsNullOrWhiteSpace(searchFilter.Category))
                sb.Append(" and (tblcase.Category_Id in (" + searchFilter.Category.SafeForSqlInject() + "))");
			// status
			if (!string.IsNullOrWhiteSpace(searchFilter.Status))
			{
				var statusIds = searchFilter.Status.Split(",").Select(o => int.Parse(o.Trim())).ToList();

				var searchNull = statusIds.Any(o => o == int.MinValue);
				statusIds.Remove(int.MinValue);

				if (statusIds.Any() && searchNull)
				{
					sb.Append(" and (tblcase.Status_Id in (" + statusIds.Select(o => o.ToString()).Aggregate((o, p) => o + ", " + p) + ") OR tblcase.Status_Id IS NULL)");
				}
				else if (statusIds.Any())
				{
					sb.Append(" and (tblcase.Status_Id in (" + statusIds.Select(o => o.ToString()).Aggregate((o, p) => o + ", " + p) + "))");
				}
				else if (searchNull)
				{
					sb.Append(" and (tblcase.Status_Id IS NULL)");
				}
			}
			// state secondery
			if (!string.IsNullOrWhiteSpace(searchFilter.StateSecondary))
			{

				var stateSecondaryIds = searchFilter.StateSecondary.Split(",").Select(o => int.Parse(o.Trim())).ToList();

				var searchNull = stateSecondaryIds.Any(o => o == int.MinValue);
				stateSecondaryIds.Remove(int.MinValue);

				if (stateSecondaryIds.Any() && searchNull)
				{
					sb.Append(" and (tblcase.StateSecondary_Id in (" + stateSecondaryIds.Select(o => o.ToString()).Aggregate((o, p) => o + ", " + p) + ") OR tblcase.StateSecondary_Id IS NULL)");
				}
				else if (stateSecondaryIds.Any())
				{
					sb.Append(" and (tblcase.StateSecondary_Id in (" + stateSecondaryIds.Select(o => o.ToString()).Aggregate((o, p) => o + ", " + p) + "))");
				}
				else if (searchNull)
				{
					sb.Append(" and (tblcase.StateSecondary_Id IS NULL)");
				}
			}

            if (searchFilter.CaseRegistrationDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[RegTime] >= '{0}')", searchFilter.CaseRegistrationDateStartFilter.ToSqlFormattedDate());
            }

            if (searchFilter.CaseRegistrationDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[RegTime] <= '{0}')", searchFilter.CaseRegistrationDateEndFilter.ToSqlFormattedDate());
            }

            if (searchFilter.CaseWatchDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[WatchDate] >= '{0}')", searchFilter.CaseWatchDateStartFilter.ToSqlFormattedDate());
            }

            if (searchFilter.CaseWatchDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[WatchDate] <= '{0}')", searchFilter.CaseWatchDateEndFilter.ToSqlFormattedDate());
            }

            if (searchFilter.CaseClosingDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[FinishingDate] >= '{0}')", searchFilter.CaseClosingDateStartFilter.ToSqlFormattedDate());
            }

            if (searchFilter.CaseClosingDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[FinishingDate] <= '{0}')", searchFilter.CaseClosingDateEndFilter.ToSqlFormattedDate());
            }

            #endregion

            if (!string.IsNullOrWhiteSpace(searchFilter.FreeTextSearch))
            {
                if (searchFilter.FreeTextSearch[0] == CaseSearchConstants.CaseNumberSearchPrefix)
                {
                    var text = searchFilter.FreeTextSearch.Substring(1, searchFilter.FreeTextSearch.Length - 1);
                    var texts = text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    if (texts.Length > 1)
                    {
                        sb.AppendFormat(" AND [tblCase].[CaseNumber] In ({0}) ", text.SafeForSqlInject());
                    }
                    else
                    {
                        int res = 0;
                        if (int.TryParse(text, out res))
                        {
                            sb.Append(" AND (");
                            sb.Append("[tblCase].[CaseNumber] = " + text.SafeForSqlInject());
                            sb.Append(") ");
                        }
                        else
                        {
                            sb.Append(" AND (");
                            sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text.SafeForSqlInject()));
                            sb.Append(") ");
                        }
                    }
                }
                else
                {
                    var text = searchFilter.FreeTextSearch.SafeForSqlInject();
                    sb.Append(" AND (");

                    sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text));

                    if (ctx.UseFreeTextCaseSearchCTE)
                    {
                        sb.AppendLine("OR freeTextSearchResults.CaseId IS NOT NULL");
                    }

                    // Get CaseNumbers/Log Ids from Indexing Service
                    if (searchFilter.SearchThruFiles)
                    {
                        if (!string.IsNullOrEmpty(searchCriteria.CustomerSetting.FileIndexingServerName) && !string.IsNullOrEmpty(searchCriteria.CustomerSetting.FileIndexingCatalogName))
                        {
							string[] excludePathPatterns = searchCriteria.HasAccessToInternalLogNotes ?
								new string[] { } :
								new string[] { @"\\" + ModuleName.LogInternal + "[0-9]{1,}$" }; // Exclude internal log note paths, ex c:\DH\LL1234

							var caseNumber_caseLogId = GetCasesContainsText(searchCriteria.CustomerSetting.FileIndexingServerName, searchCriteria.CustomerSetting.FileIndexingCatalogName, text, excludePathPatterns);
                            if (!string.IsNullOrEmpty(caseNumber_caseLogId.Item1))
                                sb.AppendFormat(" OR [tblCase].[CaseNumber] In ({0}) ", caseNumber_caseLogId.Item1.SafeForSqlInject());

                            if (!string.IsNullOrEmpty(caseNumber_caseLogId.Item2))
                                sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblLog] WHERE [tblLog].[Id] In ({0}))) ", caseNumber_caseLogId.Item2.SafeForSqlInject());
                        }
                    }

                    sb.Append(") ");
                }
            }

            // "Caption" Search
            if (!string.IsNullOrEmpty(searchFilter.CaptionSearch))
            {
                var text = searchFilter.CaptionSearch.SafeForSqlInject();
                var condition = BuildContainsExpession(Tables.Case.Caption, text, "tblCase");
                sb.AppendFormat(" AND ( {0} )", condition);
            }

            // "Initiator" search field
            if (!string.IsNullOrEmpty(searchFilter.Initiator))
            {
                sb.AppendLine(" AND (");

                var includeCases = searchFilter.InitiatorSearchScope == CaseInitiatorSearchScope.User ||
                                   searchFilter.InitiatorSearchScope == CaseInitiatorSearchScope.UserAndIsAbout;

                var includeCaseIsAbout = searchFilter.InitiatorSearchScope == CaseInitiatorSearchScope.IsAbout ||
                                         searchFilter.InitiatorSearchScope == CaseInitiatorSearchScope.UserAndIsAbout;

                var intiatorItems = BuildInitiatorFreeTextSearchConditions(searchFilter.Initiator, includeCases, includeCaseIsAbout);

                var initatorConditionsString = ConcatConditionsToString(intiatorItems);
                sb.AppendLine();
                sb.AppendFormat("( {0} )", initatorConditionsString);
                sb.AppendLine();

                //if (searchFilter.InitiatorSearchScope == CaseInitiatorSearchScope.IsAbout || searchFilter.InitiatorSearchScope == CaseInitiatorSearchScope.UserAndIsAbout)
                //{
                //if (!string.IsNullOrWhiteSpace(f.Region))
                //	sb.Append(" AND ([tblCaseIsAbout].[Region_Id] IN (" + f.Region.SafeForSqlInject() + "))");
                //if (!string.IsNullOrWhiteSpace(f.Department))
                //{
                //	// organizationUnit
                //	if (!string.IsNullOrWhiteSpace(f.OrganizationUnit))
                //		sb.Append(" AND ([tblCaseIsAbout].[Department_Id] IN (" + f.Department.SafeForSqlInject() + ") IN " +
                //						"[tblCaseIsAbout].[OU_Id] IN (" + f.OrganizationUnit.SafeForSqlInject() + "))");
                //	else
                //		sb.Append(" AND ([tblCaseIsAbout].[Department_Id] IN (" + f.Department.SafeForSqlInject() + "))");
                //}
                //else
                //{
                //	// organizationUnit
                //	if (!string.IsNullOrWhiteSpace(f.OrganizationUnit))
                //		sb.Append(" AND ([tblCaseIsAbout].[OU_Id] IN (" + f.OrganizationUnit.SafeForSqlInject() + "))");
                //}
                //}

                sb.AppendLine(" ) ");
            }

            //LockCaseToWorkingGroup
            if (!searchFilter.IsExtendedSearch && searchCriteria.UserGroupId < 3 && searchCriteria.GlobalSetting.LockCaseToWorkingGroup == 1)
            {
                sb.Append(" and (tblCase.WorkingGroup_Id in ");
                sb.Append(" (");
                sb.Append("select WorkingGroup_Id from tblUserWorkingGroup where [User_Id] = " + searchCriteria.UserId + " ");
                sb.Append("and WorkingGroup_Id in (select Id from tblWorkingGroup where Customer_Id = " + searchFilter.CustomerId + ") ");
                sb.Append(" )");

                if (searchCriteria.ShowNotAssignedWorkingGroups == 1)
                    sb.Append(" or tblCase.WorkingGroup_Id is null ");

                sb.Append(" or not exists (select id from tblWorkingGroup where Customer_Id = " + searchFilter.CustomerId + ")");
                sb.Append(") ");
            }

            return sb.ToString();
        }

        private string BuildRegionSearchCondition(CaseSearchFilter searchFilter)
        {
			var conditions = new List<string>();
			var returnStatement = string.Empty;

			var regionIds = searchFilter.Region.Split(",").Select(o => int.Parse(o.Trim())).ToList();

			if (regionIds.Count > 0)
			{

				var searchNull = regionIds.Any(o => o == int.MinValue);
				var condition = string.Empty;
				var nullCondition = string.Empty;
				var searchScope = searchFilter.InitiatorSearchScope;

				regionIds.Remove(int.MinValue);
				
				// add case search condition
				if (regionIds.Any())
				{
					var regions = regionIds.Select(o => o.ToString()).Aggregate((o, p) => o + ", " + p);

					if (searchScope == CaseInitiatorSearchScope.User || searchScope == CaseInitiatorSearchScope.UserAndIsAbout)
					{
						conditions.Add($" tblCase.Region_Id in ({regions})");
						conditions.Add($" tblDepartment.Region_Id in ({regions})");
					}

					//add isAbout search condition
					if (searchScope == CaseInitiatorSearchScope.IsAbout || searchScope == CaseInitiatorSearchScope.UserAndIsAbout)
					{
						conditions.Add($"tblCaseIsAbout.Region_Id in ({regions})");
					}

					condition = ConcatConditionsToString(conditions, CaseSearchConstants.Combinator_OR);
				}
				var nullConditions = new List<string>();
				if (searchNull)
				{
					//if (searchScope == CaseInitiatorSearchScope.User || searchScope == CaseInitiatorSearchScope.UserAndIsAbout)
					//{
					//	nullConditions.Add($" tblCase.Region_Id IS NULL");
					//	nullConditions.Add($" tblDepartment.Region_Id IS NULL");
					//}

					////add isAbout search condition
					//if (searchScope == CaseInitiatorSearchScope.IsAbout || searchScope == CaseInitiatorSearchScope.UserAndIsAbout)
					//{
					//	nullConditions.Add($"tblCaseIsAbout.Region_Id IS NULL");
					//}

					nullCondition = " tblCase.Region_Id IS NULL";// ConcatConditionsToString(nullConditions, CaseSearchConstants.Combinator_AND);
				}


				if (condition != string.Empty && nullCondition != string.Empty)
				{	
					returnStatement = $" AND ( ({condition}) OR ({nullCondition}) )";
				}
				else if (condition != string.Empty)
				{
					returnStatement = $" AND ( {condition} )";
				}
				else if (nullCondition != string.Empty)
				{
					returnStatement = $" AND ( {nullCondition} )";
				}
			}
			return returnStatement;
        }

        private string BuildCaseFreeTextSearchConditions(string text)
        {
            var items = BuildFreeTextConditionsFor(text, _freeTextCaseConditionFields, "tblCase");
            return ConcatConditionsToString(items);
        }

        private IList<string> BuildInitiatorFreeTextSearchConditions(string text, bool includeCase, bool includeCaseIsAbout)
        {
            var items = new List<string>();
            if (includeCase)
            {
                var casesConditions = BuildFreeTextConditionsFor(text, _initiatorCaseConditionFields, "tblCase");
                items.AddRange(casesConditions);
            }

            if (includeCaseIsAbout)
            {
                var caseIsAboutConditions = BuildFreeTextConditionsFor(text, _freeTextCaseIsAboutConditionFields, "tblCaseIsAbout");
                items.AddRange(caseIsAboutConditions);
            }
            return items;
        }



        #region Helper Methods
        
        private bool IsFreeTextSearch(CaseSearchFilter searchFilter)
        {
            return !string.IsNullOrWhiteSpace(searchFilter.FreeTextSearch) &&
                   searchFilter.FreeTextSearch[0] != CaseSearchConstants.CaseNumberSearchPrefix;
        }

        private bool IsHelpdeskApplication(ICaseSearchCriterias searchCriterias)
        {
            var appType = searchCriterias.ApplicationType;

            return !appType.Equals(ApplicationTypes.LineManager, StringComparison.OrdinalIgnoreCase) &&
                   !appType.Equals(ApplicationTypes.SelfService, StringComparison.OrdinalIgnoreCase);
        }

        private Tuple<string, string> GetCasesContainsText(string indexingServerName, string catalogName, string searchText, string[] excludePathPatterns)
        {
            var caseNumbers = string.Empty;
            var logIds = string.Empty;

            if (string.IsNullOrEmpty(indexingServerName) || string.IsNullOrEmpty(catalogName) || string.IsNullOrEmpty(searchText))
                return new Tuple<string, string>(caseNumbers, logIds);
            else
            {
                var caseNumeralInfo = _fileIndexingRepository.GetCaseNumeralInfoBy(indexingServerName, catalogName, searchText, excludePathPatterns);

                if (caseNumeralInfo.Item1.Any())
                    caseNumbers = string.Join(",", caseNumeralInfo.Item1.ToArray());

                if (caseNumeralInfo.Item2.Any())
                    logIds = string.Join(",", caseNumeralInfo.Item2.ToArray());
            }

            return new Tuple<string, string>(caseNumbers, logIds);
        }

        private string GetSqlLike(string field, string text, string combinator = CaseSearchConstants.Combinator_OR, bool userLower = false)
        {
            
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(text))
            {
                sb.Append(" (");
                var words = GetSearchWords(text);

                for (var i = 0; i < words.Length; i++)
                {
                    var formattedField = userLower ? $"LOWER({field})" : field;
                    sb.AppendFormat("({0} LIKE N'%{1}%')", formattedField, words[i].Trim());
                    if (words.Length > 1 && i < words.Length - 1)
                    {
                        sb.AppendFormat(" {0} ", combinator);
                    }
                }

                sb.Append(") ");
            }

            return sb.ToString();
        }
        private string GetSqlContains(string field, string text, string combinator = CaseSearchConstants.Combinator_OR, bool userLower = false)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(text))
            {
                sb.Append(" (");
                var words = GetSearchWords(text);

                for (var i = 0; i < words.Length; i++)
                {
                    var formattedField = userLower ? $"LOWER({field})" : field;
                    sb.AppendFormat("(CONTAINS({0}, N'\"*{1}*\"'))", formattedField, words[i].Trim());
                    if (words.Length > 1 && i < words.Length - 1)
                    {
                        sb.AppendFormat(" {0} ", combinator);
                    }
                }

                sb.Append(") ");
            }

            return sb.ToString();
        }

        private IList<string> BuildFreeTextConditionsFor(string text, string[] fields, string alias = null)
        {
            var items = new List<string>();

            foreach (var field in fields)
            {
                var exp = BuildContainsExpession(field, text, alias);
                items.Add(exp);
            }

            return items;
        }

        #region BuildContainsExpession

        private string BuildContainsExpession(string field, string text, string tableAlias = "", bool useWildcard = true)
        {
            return (_useFts)
                ? BuildFTSContainsExpession(field, text, tableAlias, useWildcard)
                : BuildLikeContainsExpession(field, text, tableAlias);
        }

        private string BuildFTSContainsExpession(string field, string text, string tableAlias = "", bool useWildcard = true)
        {
            var safeText = text.SafeForSqlInject();

            var fieldFormatted = FormatFieldWithAlias(field, tableAlias);
            var searchCriteria = BuildFtsContainsConditionCriteria(safeText, useWildcard);
            var expression = $"CONTAINS ({fieldFormatted}, '{searchCriteria}')";
            return expression;
        }

        private string BuildLikeContainsExpession(string field, string text, string tableAlias = "")
        {
            var fieldFormatted = FormatFieldWithAlias(field, tableAlias);
            var expression = GetSqlLike(fieldFormatted, text);
            return expression;
        }

        private string FormatFieldWithAlias(string field, string tableAlias = "")
        {
            var fieldFormatted = string.IsNullOrEmpty(tableAlias) ? field : $"{tableAlias}.{field}";
            return fieldFormatted;
        }

        private string BuildFtsContainsConditionCriteria(string text, bool useWildCard = true)
        {
            var words = GetSearchWords(text);

            var searchCriteriaText = string.Join(" OR ", 
                words
                .Where(w => !string.IsNullOrEmpty(w))
                .Select(w =>
                {
                    var isExactPhrase = w.Trim().Split(' ').Length > 1;
                    return FormatFtsContainsConditionValue(w, isExactPhrase, useWildCard );
                }));
            return searchCriteriaText;
        }

        private string[] GetSearchWords(string text)
        {
            return _fullTextExpression.Matches(text.FreeTextSafeForSqlInject())
                .Cast<Match>()
                .Where(v => !string.IsNullOrWhiteSpace(v.Groups[1].Value) || !string.IsNullOrWhiteSpace(v.Groups[2].Value))
                .Select(v => string.IsNullOrWhiteSpace(v.Groups[2].Value) ? v.Groups[1].Value : v.Groups[2].Value)
                .Distinct()
                .ToArray();
        }

        private string FormatFtsContainsConditionValue(string text, bool isExactPhrase, bool useWildCard = true)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            if (isExactPhrase) return $"\"{text}\"";
            return useWildCard ? $"\"{text}*\"" : $"{text}";
        }

        #endregion

        private string FormatWithOperand(string condition, bool useAnd = true)
        {
            return useAnd
                ? $"AND {condition}"
                : $"OR {condition}";
        }

        private string InsensitiveSearch(string fieldName)
        {
            var ret = fieldName;
            var dsn = ConfigurationManager.ConnectionStrings["HelpdeskOleDbContext"].ConnectionString;

            if (!dsn.Contains("SQLOLEDB"))
                ret = "lower(" + fieldName.SafeForSqlInject() + ")";

            return ret;
        }

        private static string ConcatConditionsToString(IList<string> items, string logicalOperator = null)
        {
            var separator = logicalOperator ?? CaseSearchConstants.Combinator_OR;
            var conditions = string.Join($"{Environment.NewLine} {separator} ", items);
            return conditions;
        }

        #endregion
    }
}