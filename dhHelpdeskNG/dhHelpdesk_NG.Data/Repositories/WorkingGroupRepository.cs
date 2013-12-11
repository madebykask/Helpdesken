using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Collections.Generic;
using System.Linq;

namespace dhHelpdesk_NG.Data.Repositories
{
    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;

    public interface IWorkingGroupRepository : IRepository<WorkingGroup>
    {
        List<WorkingGroupOverview> FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
            int customerId, int specifiedWorkingGroupId);

        List<WorkingGroupOverview> FindActiveByCustomerId(int customerId);

        //IList<WorkingGroup> GetCaseWorkingGroups(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid);
        //IList<WorkingGroup> GetCaseWorkingGroupsAvailable(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid, string[] reg);
        //IList<WorkingGroup> GetCaseWorkingGroupsSelected(int globalLockCaseToWorkingGroup, int usergroup, int customer, int userid, string[] reg);
    }

    public sealed class WorkingGroupRepository : RepositoryBase<WorkingGroup>, IWorkingGroupRepository
    {
        public WorkingGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<WorkingGroupOverview> FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(int customerId, int specifiedWorkingGroupId)
        {
            var workingGroupEntities =
                this.DataContext.WorkingGroups.Where(
                    g => g.Customer_Id == customerId && (g.IsActive != 0 || g.Id == specifiedWorkingGroupId));

            return
                workingGroupEntities.Select(
                    g => new DTO.DTOs.Faq.Output.WorkingGroupOverview { Id = g.Id, Name = g.WorkingGroupName }).ToList();
        }

        public List<WorkingGroupOverview> FindActiveByCustomerId(int customerId)
        {
            var workingGroupEntities =
                this.DataContext.WorkingGroups.Where(g => g.Customer_Id == customerId && g.IsActive != 0);

            return
                workingGroupEntities.Select(
                    g => new DTO.DTOs.Faq.Output.WorkingGroupOverview { Id = g.Id, Name = g.WorkingGroupName }).ToList();
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
