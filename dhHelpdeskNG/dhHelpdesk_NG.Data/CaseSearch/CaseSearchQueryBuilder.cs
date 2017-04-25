using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using DbExtensions;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseSearch;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using Log = DH.Helpdesk.BusinessData.Models.Changes.Output.Log;

namespace DH.Helpdesk.Dal.Repositories
{
    public class CaseSearchQueryBuilder
    {
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
                public const string UserCode = "UserCode";
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
            }

            public static class Log
            {
                public const string Text_Internal = "Text_Internal";
                public const string Text_External = "Text_External";
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

        private string[] _initiatorCaseConditionFields = new string[]
        {
            Tables.Case.ReportedBy,
            Tables.Case.Persons_Name,
            Tables.Case.UserCode,
            Tables.Case.Persons_EMail,
            Tables.Case.Place,
            Tables.Case.Persons_CellPhone,
            Tables.Case.Persons_Phone
        };

        private string[] _freeTextCaseConditionFields = new string[]
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
            Tables.Case.InventoryNumber
        };

        private string[] _freeTextCaseIsAboutConditionFields = new string[]
        {
            Tables.CaseIsAbout.ReportedBy,
            Tables.CaseIsAbout.Person_Name,
            Tables.CaseIsAbout.UserCode,
            Tables.CaseIsAbout.Person_Email,
            Tables.CaseIsAbout.Place,
            Tables.CaseIsAbout.Person_CellPhone,
            Tables.CaseIsAbout.Person_Phone
        };

        private string[] _freeTextLogConditionFields = new string[]
        {
            Tables.Log.Text_Internal,
            Tables.Log.Text_External
        };

        private string[] _freeTextDepartmentConditionFields = new string[]
        {
            Tables.Department.DepartmentId,
            Tables.Department.DepartmentName
        };

        #endregion

        public string BuildCaseSearchSql(SearchQueryBuildContext ctx)
        {
            var search = ctx.Criterias.Search;
            var searchFilter = ctx.Criterias.SearchFilter;

            var sql = new List<string>();

            var freeTextSearchCte = BuildFreeTextSearchCTEQuery(ctx);

            var searchQuery = BuildSearchQueryInner(ctx);
            var orderBy = BuildOrderBy(search);

            //TODO: remove top 500 limit when true sql side paging is implemented
            //vid avslutade ärenden visas bara första 500
            var sqlTop500 = (searchFilter.CaseProgress == CaseProgressFilter.ClosedCases || searchFilter.CaseProgress == CaseProgressFilter.None) ? "top 500" : "";

            sql.Add($@"SELECT * 
                       FROM ( SELECT {sqlTop500} *, ROW_NUMBER() OVER ( ORDER BY {orderBy} ) AS RowNum 
                              FROM ( {searchQuery} ) as tbl
                            ) as RowConstrainedResult");

            //if (f.PageInfo != null)
            //{
            //	sql.Add(string.Format(" WHERE RowNum > {0} AND RowNum <= {1}", f.PageInfo.PageNumber * f.PageInfo.PageSize, f.PageInfo.PageNumber * f.PageInfo.PageSize + f.PageInfo.PageSize));
            //}

            sql.Add(" ORDER BY RowNum");

            var output = String.Empty;
            output = string.Join(" ", sql);

            //add free text search cte query at the top
            if (!string.IsNullOrEmpty(freeTextSearchCte))
            {
                output = $"{freeTextSearchCte}{Environment.NewLine}{output}";
            }

            return output;
        }

        #region ReturnCaseSearchSqlCount

        //todo: use once proper sql side paging is implemented
        private string ReturnCaseSearchSqlCount(SearchQueryBuildContext ctx)
        {
            var criterias = ctx.Criterias;

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
                var caseSearchWhere = this.ReturnCustomCaseSearchWhere(searchFilter, criterias.UserUniqueId, userId);
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

        private string BuildSearchQueryInner(SearchQueryBuildContext ctx)
        {
            var criterias = ctx.Criterias;
            var searchFilter = criterias.SearchFilter;
            var customerSettings = criterias.CustomerSetting;
            var searchQueryBld = new StringBuilder();

            var columns = new List<string>();

            #region adding columns in SUB-SELECT

            columns.Add("tblCase.Id");
            columns.Add("tblCase.CaseNumber");
            columns.Add("tblCase.Place");
            columns.Add("tblCustomer.Name as Customer_Id");
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

            if (searchFilter.MaxTextCharacters > 0)
                columns.Add(string.Format("Cast(tblCase.[Description] as Nvarchar({0})) as [Description] ", searchFilter.MaxTextCharacters));
            else
                columns.Add("Cast(tblCase.[Description] as Nvarchar(Max)) as [Description] ");

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
            columns.Add("tblCase.AgreedDate");

            if (customerSettings != null)
            {
                if (customerSettings.IsUserFirstLastNameRepresentation == 1)
                {
                    columns.Add("coalesce(tblUsers.FirstName, '') + ' ' + coalesce(tblUsers.SurName, '') as Performer_User_Id");
                    columns.Add("IsNull(tblCase.RegUserName, coalesce(tblUsers2.FirstName, '') + ' ' + coalesce(tblUsers2.SurName, '')) as User_Id");
                    columns.Add("coalesce(tblUsers3.FirstName, '') + ' ' + coalesce(tblUsers3.SurName, '') as CaseResponsibleUser_Id");
                }
                else
                {
                    columns.Add("coalesce(tblUsers.SurName, '') + ' ' + coalesce(tblUsers.FirstName, '') as Performer_User_Id");
                    columns.Add("IsNull(tblCase.RegUserName, coalesce(tblUsers2.Surname, '') + ' ' + coalesce(tblUsers2.Firstname, '')) as User_Id");
                    columns.Add("coalesce(tblUsers3.Surname, '') + ' ' + coalesce(tblUsers3.Firstname, '') as CaseResponsibleUser_Id");
                }

                columns.Add("coalesce(tblUsers4.Surname, '') + ' ' + coalesce(tblUsers4.Firstname, '') as tblProblem_ResponsibleUser_Id");
            }

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
            columns.Add("tblCase.PlanDate");

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

            columns.Add(string.Format("'0' as [{0}]", CaseSearchConstants.TimeLeftColumn.SafeForSqlInject()));
            columns.Add("tblStateSecondary.IncludeInCaseStatistics");

            if (criterias.CaseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()))
            {
                columns.Add("tblCaseHistory.ClosingReason as ClosingReason");
            }

            if (criterias.CaseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.CausingPart.ToString()))
            {
                columns.Add("tblCausingPart.Name as CausingPart");
            }

            #endregion

            var columnsFormatted = string.Join(",", columns);
            searchQueryBld.AppendFormat("SELECT DISTINCT {0}", columnsFormatted);
            searchQueryBld.AppendLine();

            // tables and joins
            var tablesJoins = GetTablesAndJoins(ctx);
            searchQueryBld.AppendLine(tablesJoins);

            // WHERE
            string whereStatement;

            if (IsHelpdeskApplication(ctx.Criterias))
            {
                whereStatement = ReturnCaseSearchWhere(ctx);
            }
            else
            {
                whereStatement = ReturnCustomCaseSearchWhere(searchFilter, criterias.UserUniqueId, criterias.UserId);
            }

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
                        BuildCaseIsAboutFreeTextSearchQueryCte(freeText),
                        BuildLogFreeTextSearchQueryCte(freeText),
                        BuildDepartmentFreeTextSearchQueryCte(freeText),
                        BuilFormFieldValueFreeTextSearchQueryCte(freeText)
                    };

                    strBld.AppendLine(string.Join($"{Environment.NewLine} UNION {Environment.NewLine} ", items));
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

        private string BuildCaseIsAboutFreeTextSearchQueryCte(string freeText)
        {
            var strBld = new StringBuilder();
            strBld.AppendLine(@"SELECT Case_Id FROM tblCaseIsAbout WHERE  ");

            var items = BuildFreeTextConditionsFor(freeText, _freeTextCaseIsAboutConditionFields);
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);

            strBld.AppendLine(@"GROUP BY Case_Id");
            return strBld.ToString();
        }

        private string BuildLogFreeTextSearchQueryCte(string freeText)
        {
            var strBld = new StringBuilder();

            strBld.AppendLine(@"SELECT Case_Id FROM tblLog WHERE ");

            var items = BuildFreeTextConditionsFor(freeText, _freeTextLogConditionFields);
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);
            
            strBld.AppendLine(@"GROUP BY Case_Id");
            return strBld.ToString();
        }

        private string BuildDepartmentFreeTextSearchQueryCte(string freeText)
        {
            var strBld = new StringBuilder();
            strBld.AppendLine(@"SELECT caseDep.Id FROM tblDepartment dep JOIN tblCase caseDep ON dep.Id = caseDep.Department_Id WHERE ");

            var items = BuildFreeTextConditionsFor(freeText, _freeTextDepartmentConditionFields);
            var formattedConditions = ConcatConditionsToString(items);
            strBld.AppendLine(formattedConditions);
            
            strBld.AppendLine(@"GROUP BY caseDep.Id");
            return strBld.ToString();
        }

        private string BuilFormFieldValueFreeTextSearchQueryCte(string freeText)
        {
            var strBld = new StringBuilder();
            strBld.AppendLine(@"SELECT Case_Id FROM tblFormFieldValue WHERE ");
            strBld.AppendLine(BuildContainsExpession(Tables.FormFieldValue.FormFieldValueField, freeText));
            strBld.AppendLine(@"GROUP BY Case_Id");
            return strBld.ToString();
        }

        #endregion

        private string GetTablesAndJoins(SearchQueryBuildContext ctx)
        {
            var customerSetting = ctx.Criterias.CustomerSetting;
            var caseSettings = ctx.Criterias.CaseSettings;
            var userId = ctx.Criterias.UserId;

            var tables = new List<string>();

            #region adding tables into FROM section

            tables.Add("from tblCase WITH ( NOLOCK, INDEX(IX_tblCase_Customer_Id)) ");
            tables.Add("inner join tblCustomer on tblCase.Customer_Id = tblCustomer.Id ");
            tables.Add("inner join tblCustomerUser on tblCase.Customer_Id = tblCustomerUser.Customer_Id ");

            if (ctx.UseFreeTextCaseSearchCTE)
            {
                tables.Add("LEFT JOIN (SELECT DISTINCT TOP(2000000) sfr.CaseId " +
                           "           FROM SearchFreeTextFilter sfr ORDER BY sfr.CaseId) freeTextSearchResults ON tblCase.Id = freeTextSearchResults.CaseId");
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
            tables.Add("inner join tblCaseType on tblCase.CaseType_Id = tblCaseType.Id ");
            tables.Add("left outer join tblUsers as tblUsers2 on tblCase.[User_Id] = tblUsers2.Id ");
            tables.Add("left outer join tblUsers as tblUsers3 on tblCase.CaseResponsibleUser_Id = tblUsers3.Id ");
            tables.Add("left outer join tblProblem on tblCase.Problem_Id = tblProblem.Id ");
            tables.Add("left outer join tblUsers as tblUsers4 on tblProblem.ResponsibleUser_Id = tblUsers4.Id ");

            if (caseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()))
            {
                tables.Add("left outer join (SELECT ch.Id, ch.Case_Id, ch.ClosingReason FROM [tblCaseHistory] ch INNER JOIN (SELECT [Case_Id], MAX(Id) as Id FROM [tblCaseHistory] GROUP BY Case_Id) chi ON ch.Id = chi.Id AND ch.Id = chi.Id) as tblCaseHistory on tblCaseHistory.Case_Id = tblCase.Id ");
            }

            if (caseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.CausingPart.ToString()))
            {
                tables.Add("left outer join tblCausingPart on (tblCase.CausingPartId = tblCausingPart.Id)");
            }

            #endregion

            return string.Join(" ", tables);
        }

        //Used for LineManager & SelfService
        private string ReturnCustomCaseSearchWhere(CaseSearchFilter f, string userUserId, int userId)
        {
            if (f == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // kund 
            sb.Append(" where (tblCase.Customer_Id = " + f.CustomerId + ")");
            sb.Append(" and (tblCase.Deleted = 0)");


            //CaseType
            sb.Append(" and (tblCaseType.ShowOnExternalPage <> 0)");

            //ProductArea
            sb.Append(" and (tblCase.ProductArea_Id Is Null or tblCase.ProductArea_Id in (select id from tblProductArea where ShowOnExternalPage <> 0))");


            if (f.ReportedBy.Trim() == string.Empty)
                f.ReportedBy = "''";
            switch (f.CaseListType.ToLower())
            {

                case CaseListTypes.UserCases:
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId.SafeForSqlInject() + "')");
                    break;

                //Manager Cases Only
                case CaseListTypes.ManagerCases:
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId.SafeForSqlInject() + "' or tblCase.[ReportedBy] = " + f.ReportedBy.SafeForSqlInjectForInOperator() + ")");
                    break;

                //CoWorkers Cases Only
                case CaseListTypes.CoWorkerCases:
                    if (string.IsNullOrEmpty(f.ReportedBy.Replace(" ", "").Replace("'", "")))
                        sb.Append(" and (tblCase.[RegUserId] = '" + userUserId.SafeForSqlInject() + "')");
                    else
                        sb.Append(" and (((tblCase.[ReportedBy] is null or tblCase.[ReportedBy] = '') and tblCase.[RegUserId] = '" + userUserId.SafeForSqlInject() + "') or tblCase.[ReportedBy] in (" + f.ReportedBy.SafeForSqlInjectForInOperator() + "))");
                    break;

                //Manager & Coworkers Cases
                case CaseListTypes.ManagerCoWorkerCases:
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId.SafeForSqlInject() + "' or tblCase.[ReportedBy] in (" + f.ReportedBy.SafeForSqlInjectForInOperator() + "))");
                    break;

            }

            // ärende progress - iShow i gammal helpdesk
            switch (f.CaseProgress)
            {
                case CaseProgressFilter.None:
                    break;
                case CaseProgressFilter.ClosedCases:
                    sb.Append(" and (tblCase.FinishingDate is not null)");
                    break;
                case CaseProgressFilter.CasesInProgress:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
                case CaseProgressFilter.CasesInRest:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.StateSecondary_Id is not null)");
                    break;
                case CaseProgressFilter.UnreadCases:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status = 1)");
                    break;
                case CaseProgressFilter.FinishedNotApproved:
                    sb.Append(" and (tblCase.FinishingDate is not null and tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case CaseProgressFilter.InProgressStatusGreater1:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status > 1)");
                    break;
                case CaseProgressFilter.CasesWithWatchDate:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.WatchDate is not null)");
                    break;
                case CaseProgressFilter.FollowUp:
                    //sb.Append(" and (tblCase.FollowUpdate is not null)");
                    sb.Append(" and (tblCaseFollowUps.User_Id = " + userId + " and tblCaseFollowUps.IsActive = 1)");
                    break;
                default:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
            }

            if (!string.IsNullOrWhiteSpace(f.FreeTextSearch))
            {
                var text = f.FreeTextSearch.SafeForSqlInject();
                sb.Append(" AND (");
                sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[ReportedBy]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Name]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_EMail]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Phone]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_CellPhone]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Place]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Caption]", text));
                sb.AppendFormat(" OR [tblCase].[Description] LIKE '%{0}%'", text);
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Miscellaneous]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblDepartment].[Department]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblDepartment].[DepartmentId]", text));
                sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblLog] WHERE [tblLog].[Text_Internal] LIKE '%{0}%' OR [tblLog].[Text_External] LIKE '%{0}%'))", text);
                sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblFormFieldValue] WHERE {0}))", this.GetSqlLike("FormFieldValue", text));
                sb.Append(") ");
            }

            return sb.ToString();
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

            if (searchFilter.IsConnectToParent)
            {
                sb.AppendFormat(" AND tblCase.Id NOT IN (select Descendant_Id From tblParentChildCaseRelations) ");
                if (searchFilter.CurrentCaseId.HasValue)
                    sb.AppendFormat(" AND tblCase.Id != {0} ", searchFilter.CurrentCaseId);
            }

            if (searchCriteria.CaseIds != null && searchCriteria.CaseIds.Any())
            {
                sb.AppendFormat(" AND ([tblCase].[Id] IN ({0})) ", string.Join(",", searchCriteria.CaseIds));
                return sb.ToString();
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

            sb.Append(" and (tblCustomerUser.[User_Id] = " + searchFilter.UserId + ")");

            ////////////////////////////////////////////////////////////////////////////////////
            // användaren får bara se avdelningar som den har behörighet till
            if (searchCriteria.UserDepartments.Any())
            {
                sb.Append(" and EXISTS(select 1 from tblDepartmentUser _depUser WHERE _depUser.Department_Id = tblCase.Department_Id AND _depUser.[User_Id] = tblCustomerUser.User_Id) ");
            }

            // finns kryssruta på användaren att den bara får se sina egna ärenden
            var restrictedCasePermission = searchCriteria.CustomerUserSettings.User.RestrictedCasePermission;
            if (restrictedCasePermission == 1)
            {
                if (searchCriteria.UserGroupId == 2)
                    sb.Append(" and (tblCase.Performer_User_Id = " + searchCriteria.UserId + " or tblcase.CaseResponsibleUser_Id = " + searchCriteria.UserId + ")");
                else if (searchCriteria.UserGroupId == 1)
                    sb.Append(" and (lower(tblCase.reportedBy) = lower('" + searchCriteria.UserUniqueId.SafeForSqlInject() + "') or tblcase.User_Id = " + searchCriteria.UserId + ")");
            }

            // ärende progress - iShow i gammal helpdesk
            switch (searchFilter.CaseProgress)
            {
                case CaseProgressFilter.None:
                    break;
                case CaseProgressFilter.ClosedCases:
                    sb.Append(" and (tblCase.FinishingDate is not null)");
                    break;
                case CaseProgressFilter.CasesInProgress:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
                case CaseProgressFilter.CasesInRest:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.StateSecondary_Id is not null and tblStateSecondary.IncludeInCaseStatistics = 0)");
                    break;
                case CaseProgressFilter.UnreadCases:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status = 1)");
                    break;
                case CaseProgressFilter.FinishedNotApproved:
                    sb.Append(" and (tblCase.FinishingDate is not null and tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case CaseProgressFilter.InProgressStatusGreater1:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status > 1)");
                    break;
                case CaseProgressFilter.CasesWithWatchDate:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.WatchDate is not null)");
                    break;
                case CaseProgressFilter.FollowUp:
                    //sb.Append(" and (tblCase.FollowUpdate is not null)");
                    sb.Append(" and (tblCaseFollowUps.User_Id = " + searchCriteria.UserId + " and tblCaseFollowUps.IsActive = 1)");
                    break;
                default:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
            }

            // working group 
            if (!string.IsNullOrWhiteSpace(searchFilter.WorkingGroup))
            {
                if (searchCriteria.CustomerSetting.CaseWorkingGroupSource == 0)
                    sb.Append(" and (tblWorkingGroup.Id in (" + searchFilter.WorkingGroup.SafeForSqlInject() + ")) ");
                else
                    sb.Append(" and (coalesce(tblCase.WorkingGroup_Id, 0) in (" + searchFilter.WorkingGroup.SafeForSqlInject() + ")) ");
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

            // performer/utförare
            if (!string.IsNullOrWhiteSpace(searchFilter.UserPerformer))
            {
                var performersDict = searchFilter.UserPerformer.Split(',').ToDictionary(it => it, it => true);
                var searchingUnassigned = restrictedCasePermission != 1 && performersDict.ContainsKey(int.MinValue.ToString());
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
                // organizationUnit
                if (!string.IsNullOrWhiteSpace(searchFilter.OrganizationUnit))
                {
                    switch (searchFilter.InitiatorSearchScope)
                    {
                        case CaseInitiatorSearchScope.UserAndIsAbout:
                            sb.Append(" and (tblCase.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ") or " +
                                      "tblCaseIsAbout.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ") or " +
                                      "tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
                            break;
                        case CaseInitiatorSearchScope.User:
                            sb.Append(" and (tblCase.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ") or " +
                                      "tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
                            break;
                        case CaseInitiatorSearchScope.IsAbout:
                            sb.Append(" and (tblCaseIsAbout.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ") or " +
                                      "tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
                            break;
                        default:
                            sb.Append(" and (tblCase.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ") or " +
                                      "tblCaseIsAbout.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ") or " +
                                      "tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
                            break;
                    }
                }
                else
                {
                    switch (searchFilter.InitiatorSearchScope)
                    {
                        case CaseInitiatorSearchScope.UserAndIsAbout:
                            sb.Append(" and (tblCase.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ")" +
                                      " or tblCaseIsAbout.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + "))");
                            break;
                        case CaseInitiatorSearchScope.User:
                            sb.Append(" and (tblCase.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + "))");
                            break;
                        case CaseInitiatorSearchScope.IsAbout:
                            sb.Append(" and (tblCaseIsAbout.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + "))");
                            break;
                        default:
                            sb.Append(" and (tblCase.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + ")" +
                                      " or tblCaseIsAbout.Department_Id in (" + searchFilter.Department.SafeForSqlInject() + "))");
                            break;
                    }
                }
            }
            else
            {
                // organizationUnit
                if (!string.IsNullOrWhiteSpace(searchFilter.OrganizationUnit))
                    sb.Append(" and (tblCase.OU_Id in (" + searchFilter.OrganizationUnit.SafeForSqlInject() + "))");
            }

            // användare / user            
            if (!string.IsNullOrWhiteSpace(searchFilter.User))
            {
                sb.Append(" and (tblCase.User_Id in (" + searchFilter.User.SafeForSqlInject() + "))");
            }

            // region
            if (!string.IsNullOrWhiteSpace(searchFilter.Region))
            {
                switch (searchFilter.InitiatorSearchScope)
                {
                    case CaseInitiatorSearchScope.UserAndIsAbout:
                        sb.Append(" and (tblDepartment.Region_Id in (" + searchFilter.Region.SafeForSqlInject() + ")" + " or tblCaseIsAbout.Region_Id in (" + searchFilter.Region.SafeForSqlInject() + "))");
                        break;
                    case CaseInitiatorSearchScope.User:
                        sb.Append(" and (tblDepartment.Region_Id in (" + searchFilter.Region.SafeForSqlInject() + "))");
                        break;
                    case CaseInitiatorSearchScope.IsAbout:
                        sb.Append(" and (tblCaseIsAbout.Region_Id in (" + searchFilter.Region.SafeForSqlInject() + "))");
                        break;
                    default:
                        sb.Append(" and (tblDepartment.Region_Id in (" + searchFilter.Region.SafeForSqlInject() + ")" + " or tblCaseIsAbout.Region_Id in (" + searchFilter.Region.SafeForSqlInject() + "))");
                        break;
                }
            }
            // prio
            if (!string.IsNullOrWhiteSpace(searchFilter.Priority))
                sb.Append(" and (tblcase.Priority_Id in (" + searchFilter.Priority.SafeForSqlInject() + "))");
            // katagori / category
            if (!string.IsNullOrWhiteSpace(searchFilter.Category))
                sb.Append(" and (tblcase.Category_Id in (" + searchFilter.Category.SafeForSqlInject() + "))");
            // status
            if (!string.IsNullOrWhiteSpace(searchFilter.Status))
                sb.Append(" and (tblcase.Status_Id in (" + searchFilter.Status.SafeForSqlInject() + "))");
            // state secondery
            if (!string.IsNullOrWhiteSpace(searchFilter.StateSecondary))
                sb.Append(" and (tblcase.StateSecondary_Id in (" + searchFilter.StateSecondary.SafeForSqlInject() + "))");

            if (searchFilter.CaseRegistrationDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[RegTime] >= '{0}')", searchFilter.CaseRegistrationDateStartFilter);
            }

            if (searchFilter.CaseRegistrationDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[RegTime] <= '{0}')", searchFilter.CaseRegistrationDateEndFilter);
            }

            if (searchFilter.CaseWatchDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[WatchDate] >= '{0}')", searchFilter.CaseWatchDateStartFilter);
            }

            if (searchFilter.CaseWatchDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[WatchDate] <= '{0}')", searchFilter.CaseWatchDateEndFilter);
            }

            if (searchFilter.CaseClosingDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[FinishingDate] >= '{0}')", searchFilter.CaseClosingDateStartFilter);
            }

            if (searchFilter.CaseClosingDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[FinishingDate] <= '{0}')", searchFilter.CaseClosingDateEndFilter);
            }

            #endregion

            if (!string.IsNullOrWhiteSpace(searchFilter.FreeTextSearch))
            {
                if (searchFilter.FreeTextSearch[0] == '#')
                {
                    var text = searchFilter.FreeTextSearch.Substring(1, searchFilter.FreeTextSearch.Length - 1);
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
                else
                {
                    var text = searchFilter.FreeTextSearch.SafeForSqlInject();
                    sb.Append(" AND (");

                    sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text));
                    
                    if (ctx.UseFreeTextCaseSearchCTE)
                    {
                        sb.AppendLine("OR freeTextSearchResults.CaseId IS NOT NULL");
                    }

                    //free text search conditions for tblCase table
                    var caseSearchConditions = BuildCaseFreeTextSearchConditions(text);
                    sb.AppendFormat("OR {0}", caseSearchConditions);

                    // Get CaseNumbers/Log Ids from Indexing Service
                    if (searchFilter.SearchThruFiles)
                    {
                        if (!string.IsNullOrEmpty(searchCriteria.CustomerSetting.FileIndexingServerName) && !string.IsNullOrEmpty(searchCriteria.CustomerSetting.FileIndexingCatalogName))
                        {
                            var caseNumber_caseLogId = GetCasesContainsText(searchCriteria.CustomerSetting.FileIndexingServerName, searchCriteria.CustomerSetting.FileIndexingCatalogName, text);
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
            if (searchCriteria.UserGroupId < 3 && searchCriteria.GlobalSetting.LockCaseToWorkingGroup == 1)
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
                   searchFilter.FreeTextSearch[0] != '#';
        }

        private bool IsHelpdeskApplication(ICaseSearchCriterias searchCriterias)
        {
            var appType = searchCriterias.ApplicationType;

            return !appType.Equals(ApplicationTypes.LineManager, StringComparison.OrdinalIgnoreCase) &&
                   !appType.Equals(ApplicationTypes.SelfService, StringComparison.OrdinalIgnoreCase);
        }

        private Tuple<string, string> GetCasesContainsText(string indexingServerName, string catalogName, string searchText)
        {
            var caseNumbers = string.Empty;
            var logIds = string.Empty;

            if (string.IsNullOrEmpty(indexingServerName) || string.IsNullOrEmpty(catalogName) || string.IsNullOrEmpty(searchText))
                return new Tuple<string, string>(caseNumbers, logIds);
            else
            {
                var caseNumeralInfo = FileIndexingRepository.GetCaseNumeralInfoBy(indexingServerName, catalogName, searchText);

                if (caseNumeralInfo.Item1.Any())
                    caseNumbers = string.Join(",", caseNumeralInfo.Item1.ToArray());

                if (caseNumeralInfo.Item2.Any())
                    logIds = string.Join(",", caseNumeralInfo.Item2.ToArray());
            }

            return new Tuple<string, string>(caseNumbers, logIds);
        }

        private string GetSqlLike(string field, string text, string combinator = CaseSearchConstants.Combinator_OR)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(text))
            {
                sb.Append(" (");
                var words = text.FreeTextSafeForSqlInject().ToLower().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < words.Length; i++)
                {
                    sb.AppendFormat("(LOWER({0}) LIKE N'%{1}%')", field, words[i].Trim());
                    if (words.Length > 1 && i < words.Length - 1)
                    {
                        sb.Append(string.Format(" {0} ", combinator));
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

        private string BuildContainsExpession(string field, string text, string tableAlias = "", bool useWildcard = true)
        {
            var safeText = text.SafeForSqlInject();

            var fieldFormatted = string.IsNullOrEmpty(tableAlias) ? 
                                    field : 
                                    $"{tableAlias}.{field}";

            var searchCriteria = useWildcard ? $"'\"{safeText}*\"'" : $"{safeText}";
            var expression = $"CONTAINS ({fieldFormatted}, {searchCriteria})";
            return expression;
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