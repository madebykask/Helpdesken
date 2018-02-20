using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region CASESOLUTION

    public interface ICaseSolutionRepository : IRepository<CaseSolution>
    {
        CaseSolutionInfo GetGetSolutionInfo(int id, int customerId);
        IList<CaseSolutionOverview> GetCustomerCaseSolutionsOverview(int customerId, bool? includeConditions = false);
        IList<CaseSolutionOverview> GetCustomerCaseSolutionsOverview(int customerId, Expression<Func<CaseSolution, bool>> filter, bool? includeConditions = false);
    }

    public class CaseSolutionRepository : RepositoryBase<CaseSolution>, ICaseSolutionRepository
    {

        public CaseSolutionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public CaseSolutionInfo GetGetSolutionInfo(int id, int customerId)
        {
            var caseSolutionInfo = 
                Table.Where(c => c.Customer_Id == customerId && c.Id == id)
                .Select(cs => new CaseSolutionInfo
                {
                    CaseSolutionId = cs.Id,
                    StateSecondaryId = cs.StateSecondary_Id ?? 0,
                    Name = cs.Name
                })
                .FirstOrDefault();

            return caseSolutionInfo;
        }

        public IList<CaseSolutionOverview> GetCustomerCaseSolutionsOverview(int customerId, bool? includeConditions = false)
        {
            return GetCustomerCaseSolutionsOverview(customerId, null, includeConditions);
        }

        public IList<CaseSolutionOverview> GetCustomerCaseSolutionsOverview(int customerId, Expression<Func<CaseSolution, bool>> filter, bool? includeConditions = false)
        {
            IList<CaseSolutionOverview> res = null;

            var query =
                from cs in DataContext.CaseSolutions
                where cs.Customer_Id == customerId
                select cs;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // these are performance optimised queries. please do not use mappers!
            if (includeConditions.HasValue && includeConditions.Value)
            {
                res =
                    (from cs in query
                        from csc in cs.Conditions
                        let ss = cs.StateSecondary
                        select new CaseSolutionOverview
                        {
                            CaseSolutionId = cs.Id,
                            Name = cs.Name,
                            StateSecondaryId = cs.StateSecondary_Id,
                            NextStepState = cs.NextStepState,
                            Status = cs.Status,
                            SortOrder = cs.SortOrder,
                            ConnectedButton = cs.ConnectedButton,
                            StateSecondary = ss != null
                                ? new StateSecondaryOverview
                                {
                                    Id = ss.Id,
                                    Name = ss.Name,
                                    StateSecondaryId = ss.StateSecondaryId,
                                }
                                : null,
                            Conditions = cs.Conditions.Select(x => new CaseSolutionConditionOverview()
                                {
                                    Id = x.Id,
                                    Property = x.Property_Name,
                                    Values = x.Values
                                })
                                .ToList()
                        })
                    .OrderBy(x => x.SortOrder)
                    .AsNoTracking()
                    .ToList();
            }
            else
            {
                res =
                    (from cs in query
                        let ss = cs.StateSecondary
                        orderby cs.SortOrder
                        select new CaseSolutionOverview
                        {
                            CaseSolutionId = cs.Id,
                            Name = cs.Name,
                            StateSecondaryId = cs.StateSecondary_Id,
                            NextStepState = cs.NextStepState,
                            Status = cs.Status,
                            SortOrder = cs.SortOrder,
                            ConnectedButton = cs.ConnectedButton,
                            StateSecondary = ss != null
                                ? new StateSecondaryOverview
                                {
                                    Id = ss.Id,
                                    Name = ss.Name,
                                    StateSecondaryId = ss.StateSecondaryId,
                                }
                                : null
                        })
                    .AsNoTracking()
                    .ToList();
            }

            return res;
        }
    }

    #endregion

    #region CASESOLUTIONCATEGORY

    public interface ICaseSolutionCategoryRepository : IRepository<CaseSolutionCategory>
    {
        void ResetDefault(int exclude);
    }

    public class CaseSolutionCategoryRepository : RepositoryBase<CaseSolutionCategory>, ICaseSolutionCategoryRepository
    {
        public CaseSolutionCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude)
        {
            foreach (CaseSolutionCategory obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }
    }
       
    #endregion

    #region CASESOLUTIONSCHEDULE

    public interface ICaseSolutionScheduleRepository : IRepository<CaseSolutionSchedule>
    {
    }

    public class CaseSolutionScheduleRepository : RepositoryBase<CaseSolutionSchedule>, ICaseSolutionScheduleRepository
    {
        public CaseSolutionScheduleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
