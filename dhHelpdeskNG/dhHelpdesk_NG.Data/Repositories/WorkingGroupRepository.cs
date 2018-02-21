namespace DH.Helpdesk.Dal.Repositories
{
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IWorkingGroupRepository : IRepository<WorkingGroupEntity>
    {
        List<ItemOverview> FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
            int customerId, int specifiedWorkingGroupId);

        List<ItemOverview> FindActiveOverviews(int customerId);

        List<IdAndNameOverview> FindActiveIdAndNameOverviews(int customerId); 
            
        IList<UserWorkingGroup> ListUserForWorkingGroup(int workingGroupId);
        IList<int> ListWorkingGroupsForUser(int userId);

        int? GetDefaultWorkingGroupId(int customerId, int userId);

        string GetWorkingGroupName(int workingGroupId);

        void ResetDefault(int exclude, int customerId);
        void ResetBulletinBoardDefault(int exclude, int customerId);
        void ResetCalendarDefault(int exclude, int customerId);
        void ResetOperationLogDefault(int exclude, int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> workingGroupsIds);

        bool CaseHasWorkingGroup(int customerId, int workingGroupId);
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

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            var workingGroups = this.FindByCustomerIdCore(customerId).Where(g => g.IsActive != 0);
            var overviews = workingGroups.Select(g => new { Name = g.WorkingGroupName, Value = g.Id }).ToList();

            return
                overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public List<IdAndNameOverview> FindActiveIdAndNameOverviews(int customerId)
        {
            var workingGroups = this.FindByCustomerIdCore(customerId).Where(g => g.IsActive != 0).OrderBy(g => g.WorkingGroupName);
            var overviews = workingGroups.Select(g => new { g.WorkingGroupName, g.Id }).ToList();

            return
                overviews.Select(o => new IdAndNameOverview(o.Id, o.WorkingGroupName)).ToList();
        }

        public int? GetDefaultWorkingGroupId(int customerId, int userId)
        {
            // http://redmine.fastdev.se/issues/10997
            /*
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
             */

            var entities = (from cu in this.DataContext.CustomerUsers.Where(x => x.User_Id == userId)
                            join c in this.DataContext.Customers.Where(c => c.Id == customerId) on cu.Customer_Id equals c.Id
                            join wg in this.DataContext.WorkingGroups on c.Id equals wg.Customer_Id
                            join u in this.DataContext.Users on userId equals u.Id
                            from uwg in this.DataContext.UserWorkingGroups.Where(x => x.WorkingGroup_Id == wg.Id && x.User_Id == userId).DefaultIfEmpty()
                            where uwg.IsDefault == 1
                            select wg.Id).ToList();

            if (entities.Any())
            {
                return entities.First();
            }

            return null;
        }

        public IList<int> ListWorkingGroupsForUser(int userId)
        {
            var userWorkingGroups =
                this.DataContext.UserWorkingGroups.Where(uw => uw.User_Id == userId)
                    .Select(uw => uw.WorkingGroup_Id)
                    .ToList();

            return userWorkingGroups;
        }

        public IList<UserWorkingGroup> ListUserForWorkingGroup(int workingGroupId)
        {
            var query = from uw in this.DataContext.UserWorkingGroups.Include(x => x.User)
                        where uw.WorkingGroup_Id == workingGroupId
                        select uw;
            return query.ToList();
        }

        public string GetWorkingGroupName(int workingGroupId)
        {
            return
                this.DataContext.WorkingGroups.Where(g => g.Id == workingGroupId)
                    .Select(g => g.WorkingGroupName)
                    .FirstOrDefault();
        }

        public void ResetDefault(int exclude, int customerId)
        {
            foreach (WorkingGroupEntity obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

        public void ResetBulletinBoardDefault(int exclude, int customerId)
        {
            foreach (WorkingGroupEntity obj in this.GetMany(s => s.IsDefaultBulletinBoard == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefaultBulletinBoard = 0;
                this.Update(obj);
            }
        }

        public void ResetCalendarDefault(int exclude, int customerId)
        {
            foreach (WorkingGroupEntity obj in this.GetMany(s => s.IsDefaultCalendar == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefaultCalendar = 0;
                this.Update(obj);
            }
        }

        public void ResetOperationLogDefault(int exclude, int customerId)
        {
            foreach (WorkingGroupEntity obj in this.GetMany(s => s.IsDefaultOperationLog == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefaultOperationLog = 0;
                this.Update(obj);
            }
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId)
        {
            var entities = this.Table
                    .Where(g => g.Customer_Id == customerId && g.IsActive == 1)
                    .Select(g => new { Value = g.Id, Name = g.WorkingGroupName })
                    .OrderBy(g => g.Name)
                    .ToList();

            return entities.Select(g => new ItemOverview(g.Name, g.Value.ToString(CultureInfo.InvariantCulture)));
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> workingGroupsIds)
        {
            var all = workingGroupsIds == null || !workingGroupsIds.Any();
            var entities = this.Table
                    .Where(g => g.Customer_Id == customerId && 
                                g.IsActive == 1 && 
                                (all || workingGroupsIds.Contains(g.Id)))
                    .Select(g => new { Value = g.Id, Name = g.WorkingGroupName })
                    .OrderBy(g => g.Name)
                    .ToList();

            return entities.Select(g => new ItemOverview(g.Name, g.Value.ToString(CultureInfo.InvariantCulture)));            
        }

        public bool CaseHasWorkingGroup(int customerId, int workingGroupId)
        {
            var caseCount =
                this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.WorkingGroup_Id == workingGroupId && c.FinishingDate == null).Count();

            if (caseCount > 0)
                return true;
            else
                return false;
        }

        private IQueryable<WorkingGroupEntity> FindByCustomerIdCore(int customerId)
        {
            return this.DataContext.WorkingGroups.Where(g => g.Customer_Id == customerId);
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
