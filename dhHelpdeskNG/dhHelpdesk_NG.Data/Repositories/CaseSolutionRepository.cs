using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        IList<CaseSolutionOverview> GetCustomerCaseSolutions(int customerId);
        IList<CaseSolutionOverview> GetCaseSolutionsConditions(IList<int> Ids);
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

        public IList<CaseSolutionOverview> GetCustomerCaseSolutions(int customerId)
        {
            var query =
                from cs in DataContext.CaseSolutions
                let ss = cs.StateSecondary
                where cs.Customer_Id == customerId &&
                      cs.Status != 0
                select new CaseSolutionOverview
                {
                    CaseSolutionId = cs.Id,
                    Name = cs.Name,
                    CategoryId = cs.CaseSolutionCategory_Id,
                    StateSecondaryId = cs.StateSecondary_Id,
                    NextStepState = cs.NextStepState,
                    Status = cs.Status,
                    WorkingGroupId = cs.WorkingGroup_Id,
                    WorkingGroupName = cs.WorkingGroup.WorkingGroupName,
                    SortOrder = cs.SortOrder,
                    ConnectedButton = cs.ConnectedButton,
                    ShowInsideCase = cs.ShowInsideCase,
                    ShowOnCaseOverview = cs.ShowOnCaseOverview,
                    StateSecondary = cs.StateSecondary != null ? new StateSecondaryOverview
                    {
                        Id = ss.Id,
                        Name = ss.Name,
                        StateSecondaryId = ss.StateSecondaryId,
                    } : null
                };

            var res = query.OrderBy(x => x.SortOrder).AsNoTracking().ToList();
            return res;
        }

        public IList<CaseSolutionOverview> GetCaseSolutionsConditions(IList<int> Ids)
        {
            var conditions =
                (from cs in DataContext.CaseSolutions
                 from csc in cs.Conditions
                 where Ids.Contains(cs.Id)
                 select new 
                 {
                     CaseSolutionId = cs.Id,
                     csc.Id,
                     csc.Property_Name,
                     csc.Values
                 }).AsNoTracking().ToList();

            var res =
                conditions.GroupBy(x => x.CaseSolutionId, x => new CaseSolutionConditionOverview()
                    {
                        Id = x.Id,
                        Property = x.Property_Name,
                        Values = x.Values
                    })
                    .Select(x => new CaseSolutionOverview
                    {
                        CaseSolutionId = x.Key,
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
