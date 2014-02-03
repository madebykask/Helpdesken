using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Collections.Generic;
using System.Linq;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;

    public interface IWorkingGroupRepository : IRepository<WorkingGroupEntity>
    {
        List<ItemOverviewDto> FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
            int customerId, int specifiedWorkingGroupId);

        List<ItemOverviewDto> FindActiveOverviews(int customerId);

        IList<UserWorkingGroup> ListUserForWorkingGroup(int workingGroupId);
        //IList<WorkingGroup> GetCaseWorkingGroups(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid);
        //IList<WorkingGroup> GetCaseWorkingGroupsAvailable(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid, string[] reg);
        //IList<WorkingGroup> GetCaseWorkingGroupsSelected(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid, string[] reg);
    }

    public sealed class WorkingGroupRepository : RepositoryBase<WorkingGroupEntity>, IWorkingGroupRepository
    {
        public WorkingGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
            int customerId, int specifiedWorkingGroupId)
        {
            var workingGroups =
                this.FindByCustomerIdCore(customerId).Where(g => (g.IsActive != 0 || g.Id == specifiedWorkingGroupId));

            var overviews = workingGroups.Select(g => new { Name = g.WorkingGroupName, Value = g.Id }).ToList();

            return
                overviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Value.ToString(CultureInfo.InvariantCulture) })
                         .ToList();
        }

        private IQueryable<WorkingGroupEntity> FindByCustomerIdCore(int customerId)
        {
            return this.DataContext.WorkingGroups.Where(g => g.Customer_Id == customerId);
        } 

        public List<ItemOverviewDto> FindActiveOverviews(int customerId)
        {
            var workingGroups = this.FindByCustomerIdCore(customerId).Where(g => g.IsActive != 0);
            var overviews = workingGroups.Select(g => new { Name = g.WorkingGroupName, Value = g.Id }).ToList();

            return
                overviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Value.ToString(CultureInfo.InvariantCulture) })
                         .ToList();
        }

        public IList<UserWorkingGroup> ListUserForWorkingGroup(int workingGroupId)
        {

            var query = from uw in this.DataContext.UserWorkingGroups
                        where uw.WorkingGroup_Id == workingGroupId
                        select uw;

            return query.ToList();

    
        }
        //public IList<WorkingGroup> GetCaseWorkingGroupsAvailable(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid, string[] reg)
        //{
        //    List<WorkingGroup> rlist = new List<WorkingGroup>();

        //    if (globalLockCaseToWorkingGroup == 1 && usergroup < 3)
        //    {
        //        var query = (from w in this.DataContext.Set<WorkingGroup>().Where(d => d.Customer_Id == customer && d.IsActive == 1)
        //                     join uw in this.DataContext.Set<UserWorkingGroup>().Where(d => d.User_Id == userid) on w.Id equals uw.WorkingGroup_Id
        //                     orderby w.WorkingGroupName
        //                     select w);

        //        bool add = false;
        //        if (reg != null)
        //        {
        //            foreach (WorkingGroup fg in query)// query)
        //            {
        //                add = true;
        //                foreach (string i in reg)
        //                {
        //                    if (i == fg.Id.ToString())
        //                    {
        //                        add = false;
        //                    }
        //                }

        //                if (add == true)
        //                {
        //                    rlist.Add(fg);
        //                }
        //            }
        //        }
        //        if (rlist.Count == 0)
        //        {
        //            return query.ToList();
        //        }
        //        else
        //        {
        //            return rlist.ToList();
        //        }
        //    }
        //    else
        //    {
        //        var query = (from w in this.DataContext.Set<WorkingGroup>().Where(d => d.Customer_Id == customer && d.IsActive == 1)
        //                     orderby w.WorkingGroupName
        //                     select w);

        //        bool add = false;
        //        if (reg != null)
        //        {
        //            foreach (WorkingGroup fg in query)// query)
        //            {
        //                add = true;
        //                foreach (string i in reg)
        //                {
        //                    if (i == fg.Id.ToString())
        //                    {
        //                        add = false;
        //                    }
        //                }

        //                if (add == true)
        //                {
        //                    rlist.Add(fg);
        //                }
        //            }
        //        }
        //        if (rlist.Count == 0)
        //        {
        //            return query.ToList();
        //        }
        //        else
        //        {
        //            return rlist.ToList();
        //        }
        //    }
        //}

        //public IList<WorkingGroup> GetCaseWorkingGroupsSelected(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid, string[] reg)
        //{
        //    List<WorkingGroup> rlist = new List<WorkingGroup>();
        //    if (globalLockCaseToWorkingGroup == 1 && usergroup < 3)
        //    {
        //        var query = (from w in this.DataContext.Set<WorkingGroup>().Where(d => d.Customer_Id == customer && d.IsActive == 1)
        //                     join uw in this.DataContext.Set<UserWorkingGroup>().Where(d => d.User_Id == userid) on w.Id equals uw.WorkingGroup_Id
        //                     orderby w.WorkingGroupName
        //                     select w);

        //        bool add = false;
        //        if (reg != null)
        //        {
        //            foreach (WorkingGroup fg in query) // rr)
        //            {
        //                add = false;
        //                foreach (string i in reg)
        //                {
        //                    if (i == fg.Id.ToString())
        //                    {
        //                        add = true;
        //                    }
        //                }

        //                if (add == true)
        //                {
        //                    rlist.Add(fg);
        //                }
        //            }
        //        }
        //        return rlist;
        //    }
        //    else
        //    {
        //        var query = (from w in this.DataContext.Set<WorkingGroup>().Where(d => d.Customer_Id == customer && d.IsActive == 1)
        //                     orderby w.WorkingGroupName
        //                     select w);

        //        bool add = false;
        //        if (reg != null)
        //        {
        //            foreach (WorkingGroup fg in query) // rr)
        //            {
        //                add = false;
        //                foreach (string i in reg)
        //                {
        //                    if (i == fg.Id.ToString())
        //                    {
        //                        add = true;
        //                    }
        //                }

        //                if (add == true)
        //                {
        //                    rlist.Add(fg);
        //                }
        //            }
        //        }
        //        return rlist;
        //    }
        //}

        //public IList<WorkingGroup> GetCaseWorkingGroups(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid)
        //{
        //    if (globalLockCaseToWorkingGroup == 1 && usergroup < 3)
        //    {
        //        var query = (from w in this.DataContext.Set<WorkingGroup>().Where(d => d.Customer_Id == customer && d.IsActive == 1)
        //                     join uw in this.DataContext.Set<UserWorkingGroup>().Where(d => d.User_Id == userid) on w.Id equals uw.WorkingGroup_Id
        //                     orderby w.WorkingGroupName
        //                     select w);

        //        return query.ToList();
        //    }
        //    else
        //    {
        //        var query = (from w in this.DataContext.Set<WorkingGroup>().Where(d => d.Customer_Id == customer && d.IsActive == 1)
        //                     orderby w.WorkingGroupName
        //                     select w);

        //        return query.ToList();
        //    }
        //}
    }
}
