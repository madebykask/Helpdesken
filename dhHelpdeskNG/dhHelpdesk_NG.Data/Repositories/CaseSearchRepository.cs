using DH.Helpdesk.Dal.DbQueryExecutor;

namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSearch;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The CaseSearchRepository interface.
    /// </summary>
    public interface ICaseSearchRepository
    {
        DataTable Search(CaseSearchContext context);
        int SearchCount(CaseSearchContext context);
    }

    public class CaseSearchRepository : ICaseSearchRepository
    {
        private readonly ICustomerUserRepository _customerUserRepository;
        private readonly ICaseTypeRepository _caseTypeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDbQueryExecutorFactory _queryExecutorFactory;
        private readonly IFeatureToggleRepository _featureToggleRepository;
        private readonly IFileIndexingRepository _fileIndexingRepository;

        public CaseSearchRepository(
                ICustomerUserRepository customerUserRepository,
                ICaseTypeRepository caseTypeRepository,
                IDepartmentRepository departmentRepository,
                IDbQueryExecutorFactory queryExecutorFactory,
                IFeatureToggleRepository featureToggleRepository,
                IFileIndexingRepository fileIndexingRepository)
        {
            _customerUserRepository = customerUserRepository;
            _caseTypeRepository = caseTypeRepository;
            _departmentRepository = departmentRepository;
            _queryExecutorFactory = queryExecutorFactory;
            _featureToggleRepository = featureToggleRepository;
            _fileIndexingRepository = fileIndexingRepository;

        }

        public DataTable Search(CaseSearchContext context)
        { 
            //check if freetext is searchable
            var freetext = context.f.FreeTextSearch;
            if (freetext != null)
            {
                if (freetext.Contains("#") && freetext.Length == 1)
                {
                    context.f.FreeTextSearch = string.Empty;
                }
            }

            var sql = BuildSeachSql(context);

            if (string.IsNullOrEmpty(sql))
                return null;
            
            var queryExecutor = _queryExecutorFactory.Create();
            var dataTable = queryExecutor.ExecuteTable(sql, commandType: CommandType.Text, timeout: 300);

            return dataTable;
        }

        public int SearchCount(CaseSearchContext context)
        {
            var sql = BuildSeachSql(context, true);

            if (string.IsNullOrEmpty(sql))
                return 0;

            var queryExecutor = _queryExecutorFactory.Create();
            var count = queryExecutor.ExecuteScalar<int>(sql, commandType: CommandType.Text, timeout: 300);
            return count;
        }

        private string BuildSeachSql(CaseSearchContext context, bool countOnly = false)
        {
            var queryBuilderContext = BuildSearchQueryContext(context);

            //build search query
            var searchQueryBuilder = new CaseSearchQueryBuilder(_fileIndexingRepository);
            var sql = searchQueryBuilder.BuildCaseSearchSql(queryBuilderContext, countOnly);
            return sql;
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