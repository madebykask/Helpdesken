using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories
{
    #region CASESOLUTION

    public interface ICaseSolutionRepository : IRepository<CaseSolution>
    {
        CaseSolutionInfo GetGetSolutionInfo(int id, int customerId);
        IQueryable<CaseSolution> GetCustomerCaseSolutions(int customerId);
        IList<CaseSolutionOverview> GetCaseSolutionsWithConditions(IList<int> Ids);
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

        public IQueryable<CaseSolution> GetCustomerCaseSolutions(int customerId)
        {
            var query =
                from cs in DataContext.CaseSolutions
                where cs.Customer_Id == customerId && cs.Status != 0
                orderby cs.SortOrder
                select cs;
            
            return query;
        }

        public IList<CaseSolutionOverview> GetCaseSolutionsWithConditions(IList<int> Ids)
        {
            var conditions =
                (from cs in Table
                 from csc in cs.Conditions
                 where Ids.Contains(cs.Id)
                 orderby cs.SortOrder
                 select new 
                 {
                     CaseSolutionId = cs.Id,
                     Name = cs.Name,
                     NextStepState = cs.NextStepState,
                     StateSecondaryId = cs.StateSecondary != null ? cs.StateSecondary.StateSecondaryId : 0,

                     csc.Id,
                     csc.Property_Name,
                     csc.Values
                 }).AsNoTracking().ToList();

            var res =
                conditions.GroupBy(x => new
                {
                    x.CaseSolutionId,
                    x.Name,
                    x.NextStepState,
                    x.StateSecondaryId
                }, x => new CaseSolutionConditionOverview()
                        {
                            Id = x.Id,
                            Property = x.Property_Name,
                            Values = x.Values
                        })
                    .Select(x => new CaseSolutionOverview
                    {
                        CaseSolutionId = x.Key.CaseSolutionId,
                        Name = x.Key.Name,
                        StateSecondaryId = x.Key.StateSecondaryId,
                        NextStepState = x.Key.NextStepState,
                        Conditions = x.ToList()
                    }).ToList();

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
