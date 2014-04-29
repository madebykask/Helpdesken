namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IWorkingGroupRepository : IRepository<WorkingGroupEntity>
    {
        List<ItemOverview> FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
            int customerId, int specifiedWorkingGroupId);

        List<ItemOverview> FindActiveOverviews(int customerId);

        List<IdAndNameOverview> FindActiveIdAndNameOverviews(int customerId); 
            
        IList<UserWorkingGroup> ListUserForWorkingGroup(int workingGroupId);

        int? GetDefaultWorkingGroupId(int customerId, int userId);

        string GetWorkingGroupName(int workingGroupId);

        void ResetDefault(int exclude);
    }

    public sealed class WorkingGroupRepository : RepositoryBase<WorkingGroupEntity>, IWorkingGroupRepository
    {
        public WorkingGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
            int customerId, int specifiedWorkingGroupId)
        {
            var workingGroups =
                this.FindByCustomerIdCore(customerId).Where(g => (g.IsActive != 0 || g.Id == specifiedWorkingGroupId));

            var overviews = workingGroups.Select(g => new { Name = g.WorkingGroupName, Value = g.Id }).ToList();

            return
                overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        private IQueryable<WorkingGroupEntity> FindByCustomerIdCore(int customerId)
        {
            return this.DataContext.WorkingGroups.Where(g => g.Customer_Id == customerId);
        } 

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            var workingGroups = this.FindByCustomerIdCore(customerId).Where(g => g.IsActive != 0);
            var overviews = workingGroups.Select(g => new { Name = g.WorkingGroupName, Value = g.Id }).ToList();

            return
                overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public List<IdAndNameOverview> FindActiveIdAndNameOverviews(int customerId)
        {
            var workingGroups = this.FindByCustomerIdCore(customerId).Where(g => g.IsActive != 0);
            var overviews = workingGroups.Select(g => new { g.WorkingGroupName, g.Id }).ToList();

            return
                overviews.Select(o => new IdAndNameOverview(o.Id, o.WorkingGroupName)).ToList();
        }

        public int? GetDefaultWorkingGroupId(int customerId, int userId)
        {
            // get setting from user
            int? idFromUserSetting =
                this.DataContext.Users.Where(u => u.Id == userId)
                    .Select(u => u.Default_WorkingGroup_Id)
                    .FirstOrDefault();
            // get setting from working group
            int? idFromWorkingGroup =
                this.DataContext.WorkingGroups.Where(g => g.Customer_Id == customerId && g.IsDefault == 1)
                    .Select(g => g.Id)
                    .FirstOrDefault();
            return idFromUserSetting.HasValue ? idFromUserSetting : idFromWorkingGroup;  
        }

        public IList<UserWorkingGroup> ListUserForWorkingGroup(int workingGroupId)
        {
            var query = from uw in this.DataContext.UserWorkingGroups
                        where uw.WorkingGroup_Id == workingGroupId
                        select uw;
            return query.ToList();
        }

        public string GetWorkingGroupName(int workingGroupId)
        {
            return
                this.DataContext.WorkingGroups.Where(g => g.Id == workingGroupId)
                    .Select(g => g.WorkingGroupName)
                    .Single();
        }

        public void ResetDefault(int exclude)
        {
            foreach (WorkingGroupEntity obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
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
