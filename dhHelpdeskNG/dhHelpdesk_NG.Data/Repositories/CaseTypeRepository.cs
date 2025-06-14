﻿using System.Data.Entity;
using DH.Helpdesk.Dal.DbQueryExecutor;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
	using System.Linq.Expressions;
	using System;

	public interface ICaseTypeRepository : IRepository<CaseType>
    {
        void ResetDefault(int exclude, int customerId);

        void ResetEmailDefault(int exclude, int customerId);

        CaseType GetCaseTypeFull(int caseTypeId);
        IQueryable<CaseType> GetCaseType(int caseTypeId);

        IList<ItemOverview> GetOverviews(int customerId);

        IList<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds);

        IList<int> GetChildren(int caseTypeId);

        IList<CaseTypeOverview> GetCaseTypeOverviews(int customerId);

		IQueryable<CaseType> GetManyWithSubCaseTypes(Expression<Func<CaseType, bool>> where);

        IList<CaseType> GetWithParents(int[] childIds);
    }

    public class CaseTypeRepository : RepositoryBase<CaseType>, ICaseTypeRepository
    {
        private readonly IDbQueryExecutorFactory _queryExecutorFactory;
        
        #region ctor()

        public CaseTypeRepository(IDatabaseFactory databaseFactory, IDbQueryExecutorFactory queryExecutorFactory)
            : base(databaseFactory)
        {
            _queryExecutorFactory = queryExecutorFactory;
        }

        #endregion

        public CaseType GetCaseTypeFull(int caseTypeId)
        {
            return Table.Where(x => x.Id == caseTypeId)
                .Include(x => x.Administrator)
                .Include(x => x.WorkingGroup)
                .FirstOrDefault();
        }

        public void ResetDefault(int exclude, int customerId)
        {
            foreach (CaseType obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

		public IQueryable<CaseType> GetManyWithSubCaseTypes(Expression<Func<CaseType, bool>> where)
		{
			return this.DataContext.CaseTypes.Include(x => x.SubCaseTypes).Where(where);
		}

        public IQueryable<CaseType> GetCaseType(int caseTypeId)
        {
            return DataContext.CaseTypes.Where(c => c.Id == caseTypeId);
        }

		public void ResetEmailDefault(int exclude, int customerId)
        {
            foreach (CaseType obj in this.GetMany(s => s.IsEMailDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsEMailDefault = 0;
                this.Update(obj);
            }
        }
        
        public IList<ItemOverview> GetOverviews(int customerId)
        {
            var entities = GetCustomerCaseTypes(customerId, true)
                    .Select(g => new { Value = g.Id, g.Name })
                    .OrderBy(g => g.Name)
                    .ToList();

            return entities.Select(g => new ItemOverview(g.Name, g.Value.ToString(CultureInfo.InvariantCulture))).ToList();            
        }

        public IList<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds)
        {
            var all = caseTypesIds == null || !caseTypesIds.Any();

            var entities = 
                GetCustomerCaseTypes(customerId, true)
                    .Where(g => all || caseTypesIds.Contains(g.Id))
                    .Select(g => new { Value = g.Id, g.Name })
                    .OrderBy(g => g.Name)
                    .ToList();

            return entities.Select(g => new ItemOverview(g.Name, g.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public IList<int> GetChildren(int caseTypeId)
        {
            var children = new List<int>();
            this.GetChildrenProcess(caseTypeId, children);
            return children.ToList();
        }

        public IList<CaseTypeOverview> GetCaseTypeOverviews(int customerId)
        {
            //var entities =
            //    this.DataContext.CaseTypes.Where(c => c.Customer_Id == customerId && c.IsActive != 0)
            //        .Select(c => new { c.Id, ParentId = c.Parent_CaseType_Id, c.Name, c.ShowOnExternalPage })
            //        .OrderBy(c => c.Name)
            //        .ToList();
            //For next release #57742

            var mappingExpr = MapToCaseTypeOverview();
            var entities = GetCustomerCaseTypes(customerId, true).Select(mappingExpr).OrderBy(c => c.Name).ToList();
            return entities;
        }

        public IList<CaseType> GetWithParents(int[] childIds)
        {
            if (childIds == null || !childIds.Any()) return new List<CaseType>();

            var ids = string.Join(",", childIds);
            var sql = $@"  with parents as (
                           select [Id], [Parent_CaseType_Id], [CaseType]
                           from [dbo].[tblCaseType]
                           where [Id] in ({ids})
                           union all
                           select c.[Id], c.[Parent_CaseType_Id], c.[CaseType]
                           from [dbo].[tblCaseType] c
                             join parents p on p.[Parent_CaseType_Id] = c.id 
                        ) 
                        select distinct [Id], [Parent_CaseType_Id], [CaseType] as Name
                        from parents";
            var queryExecutor = _queryExecutorFactory.Create();
            var caseTypes = queryExecutor.QueryList<CaseType>(sql);

            return caseTypes;
        }

        private IQueryable<CaseType> GetCustomerCaseTypes(int customerId, bool activeOnly)
        {
            return Table.Where(g => g.Customer_Id == customerId && (!activeOnly || g.IsActive == 1));
        }

        private void GetChildrenProcess(int caseTypeId, ICollection<int> children)
        {
            var caseType = Table.Include(ct => ct.SubCaseTypes).SingleOrDefault(ct => ct.Id == caseTypeId);
            if (caseType != null && caseType.SubCaseTypes != null)
            {
                foreach (var child in caseType.SubCaseTypes)
                {
                    children.Add(child.Id);
                    this.GetChildrenProcess(child.Id, children);
                }
            }            
        }

        private Expression<Func<CaseType, CaseTypeOverview>> MapToCaseTypeOverview()
        {
            Expression<Func<CaseType, CaseTypeOverview>> exp = (c) => new CaseTypeOverview()
            {
                Id = c.Id,
                ParentId = c.Parent_CaseType_Id,
                Name = c.Name,
                ShowOnExternalPage = c.ShowOnExternalPage,
                ShowOnExtPageCases = c.ShowOnExtPageCases,
                IsActive = c.IsActive,
                WorkingGroupId = c.WorkingGroup_Id
            };
            return exp;
        }

    }
}
