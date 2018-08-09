using System.Diagnostics;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.Dal.DbQueryExecutor;

namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSearch;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Enums.Cases;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Dal.Repositories;

    using ProductAreaEntity = DH.Helpdesk.Domain.ProductArea;
    using DH.Helpdesk.Dal.Enums;

    /// <summary>
    /// The CaseSearchRepository interface.
    /// </summary>
    public interface ICaseSearchRepository
    {
        DataTable Search(CaseSearchContext context);
    }

    public class CaseSearchRepository : ICaseSearchRepository
    {
        private readonly ICustomerUserRepository _customerUserRepository;
        private readonly ICaseTypeRepository _caseTypeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDbQueryExecutorFactory _queryExecutorFactory;

        public CaseSearchRepository(
                ICustomerUserRepository customerUserRepository,
                ICaseTypeRepository caseTypeRepository,
                IDepartmentRepository departmentRepository,
                IDbQueryExecutorFactory queryExecutorFactory)
        {
            _customerUserRepository = customerUserRepository;
            _caseTypeRepository = caseTypeRepository;
            _departmentRepository = departmentRepository;
            _queryExecutorFactory = queryExecutorFactory;
        }

        public DataTable Search(CaseSearchContext context)
        {

            //check if freetext is searchable
            var freetext = context.f.FreeTextSearch;
            if (freetext != null)
            {
                if (freetext.Contains("#") && freetext.Length == 1)
                {
                    context.f.FreeTextSearch = String.Empty;
                }
            }

            var queryBuilderContext = BuildSearchQueryContext(context);
                       
            //build search query
            var searchQueryBuilder = new CaseSearchQueryBuilder();
            var sql = searchQueryBuilder.BuildCaseSearchSql(queryBuilderContext);

            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            
            var queryExecutor = _queryExecutorFactory.Create();
            var dataTable = queryExecutor.ExecuteTable(sql, commandType: CommandType.Text, timeout: 300);

            return dataTable;
        }

        private SearchQueryBuildContext BuildSearchQueryContext(CaseSearchContext context)
        {
            var f = context.f;
            var userId = f.UserId;
            var userDepartments = new List<Department>();

            var customerUserSettings = _customerUserRepository.GetCustomerSettings(f.CustomerId, userId);
            if (f.IsExtendedSearch)
            {
                customerUserSettings = _customerUserRepository.GetCustomerSettingsByCustomer(f.CustomerId);
            }
            else
            {
                userDepartments = _departmentRepository.GetDepartmentsByUserPermissions(userId, f.CustomerId).ToList();
            }

            var validateUserCaseSettings = new List<CaseSettings>();
            foreach (var us in context.userCaseSettings)
            {
                if (!validateUserCaseSettings.Select(v => v.Name).Contains(us.Name))
                    validateUserCaseSettings.Add(us);
            }

            var caseSettings = validateUserCaseSettings.ToDictionary(it => it.Name, it => it);

            var caseTypes = new List<int>();
            if (context.f.CaseType != 0)
            {
                caseTypes.Add(context.f.CaseType);
                var caseTypeChildren = this._caseTypeRepository.GetChildren(context.f.CaseType);
                if (caseTypeChildren != null)
                {
                    caseTypes.AddRange(caseTypeChildren);
                }
            }

            var searchQueryBuildContext = 
                new SearchQueryBuildContext(context, customerUserSettings, caseSettings, userDepartments, caseTypes);

            return searchQueryBuildContext;
        }
    }
}