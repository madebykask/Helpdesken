namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IWorkingGroupService
    {
        IList<WorkingGroupEntity> GetAllWorkingGroups();

        IList<WorkingGroupEntity> GetWorkingGroups(int customerId);

        IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId);

        IList<WorkingGroupEntity> GetWorkingGroupsForIndexPage(int customerId);

        int? GetDefaultId(int customerId, int userId);

        List<GroupWithEmails> GetWorkingGroupsWithActiveEmails(int customerId);

        IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId);

        WorkingGroupEntity GetWorkingGroup(int id);

        DeleteMessage DeleteWorkingGroup(int id);

        void SaveWorkingGroup(WorkingGroupEntity workingGroup, out IDictionary<string, string> errors);

        void Commit();

        IEnumerable<ItemOverview> GetOverviews(int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> workingGroupsIds);

        bool CaseHasWorkingGroup(int customerId, int workingGroupId);

    }

    public class WorkingGroupService : IWorkingGroupService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IUserRepository userRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IWorkContext workContext;


        public WorkingGroupService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IWorkingGroupRepository workingGroupRepository,
            IWorkContext workContext)
        {
            this.unitOfWork = unitOfWork;
            this.workingGroupRepository = workingGroupRepository;
            this.userRepository = userRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.workContext = workContext;
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroups()
        {
            return this.workingGroupRepository.GetAll().OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId)
        {
            return this.workingGroupRepository.ListUserForWorkingGroup(workingGroupId);
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId)
        {
            return this.workingGroupRepository
                .GetMany(x => x.Customer_Id == customerId)
                .OrderBy(x => x.WorkingGroupName).ToList();
        }

        /// <summary>
        /// The get working groups.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<WorkingGroupEntity> GetWorkingGroups(int customerId)
        {
            return this.workingGroupRepository
                    .GetMany(x => x.Customer_Id == customerId && x.IsActive == 1)
                    .OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<WorkingGroupEntity> GetWorkingGroupsForIndexPage(int customerId)
        {
            return this.workingGroupRepository
                .GetMany(x => x.Customer_Id == customerId)
                .OrderBy(x => x.WorkingGroupName).ToList();
        }

        public int? GetDefaultId(int customerId, int userId)
        {
            return this.workingGroupRepository.GetDefaultWorkingGroupId(customerId, userId);  
        }

        public List<GroupWithEmails> GetWorkingGroupsWithActiveEmails(int customerId)
        {
            List<GroupWithEmails> workingGroupsWithEmails;

            var workingGroupOverviews = this.workingGroupRepository.FindActiveIdAndNameOverviews(customerId);
            var workingGroupIds = workingGroupOverviews.Select(g => g.Id).ToList();
            var workingGroupsUserIds = this.userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
            var userIdsSet = new HashSet<int>();
            foreach (var id in workingGroupsUserIds.SelectMany(g => g.UserIds))
            {
                if (!userIdsSet.Contains(id))
                {
                    userIdsSet.Add(id);
                }
            }
           
            var userIds =
                this.userRepository.GetAll()
                    .Where(it => it.IsActive == 1 && userIdsSet.Contains(it.Id))
                    .Select(it => it.Id)
                    .ToList();
            
            var userIdsWithEmails = this.userRepository.FindUsersEmails(userIds);

            workingGroupsWithEmails = new List<GroupWithEmails>(workingGroupOverviews.Count);

            foreach (var workingGroupOverview in workingGroupOverviews)
            {
                var groupUserIdsWithEmails = workingGroupsUserIds.FirstOrDefault(g => g.WorkingGroupId == workingGroupOverview.Id);
                if (groupUserIdsWithEmails != null)
                {
                    var groupEmails =
                        userIdsWithEmails.Where(e => groupUserIdsWithEmails.UserIds.Contains(e.ItemId))
                            .Select(e => e.Email)
                            .ToList();

                    var groupWithEmails = new GroupWithEmails(
                        workingGroupOverview.Id,
                        workingGroupOverview.Name,
                        groupEmails);

                    workingGroupsWithEmails.Add(groupWithEmails);
                }
            }

            return workingGroupsWithEmails;
        }

        public WorkingGroupEntity GetWorkingGroup(int id)
        {
            return this.workingGroupRepository.GetById(id);
        }

        public DeleteMessage DeleteWorkingGroup(int id)
        {
            var workingGroup = this.workingGroupRepository.GetById(id);

            if (workingGroup != null)
            {
                try
                {
                    this.workingGroupRepository.Delete(workingGroup);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public bool CaseHasWorkingGroup(int customerId, int workingGroupId)
        {
            return this.workingGroupRepository.CaseHasWorkingGroup(customerId, workingGroupId);
        }

        public void SaveWorkingGroup(WorkingGroupEntity workingGroup, out IDictionary<string, string> errors)
        {
            if (workingGroup == null)
            {
                throw new ArgumentNullException("workingGroup");
            }

            errors = new Dictionary<string, string>();
            workingGroup.EMail = workingGroup.EMail ?? string.Empty;
            workingGroup.SendExternalEmailToWGUsers = workingGroup.SendExternalEmailToWGUsers.HasValue ? workingGroup.SendExternalEmailToWGUsers.Value : 0;

            if (string.IsNullOrEmpty(workingGroup.WorkingGroupName))
            {
                errors.Add("WorkingGroup.Name", "Du måste ange en driftgrupp");
            }

            if (workingGroup.Id == 0)
            {
                this.workingGroupRepository.Add(workingGroup);
            }
            else
            {
                var entityToUpdate = this.workingGroupRepository.GetById(workingGroup.Id);
                entityToUpdate.WorkingGroupName = workingGroup.WorkingGroupName;
                entityToUpdate.Code = workingGroup.Code;
                entityToUpdate.EMail = workingGroup.EMail;
                entityToUpdate.IsDefault = workingGroup.IsDefault;
                entityToUpdate.IsDefaultCalendar = workingGroup.IsDefaultCalendar;
                entityToUpdate.IsDefaultBulletinBoard = workingGroup.IsDefaultBulletinBoard;
                entityToUpdate.IsDefaultOperationLog = workingGroup.IsDefaultOperationLog;
                entityToUpdate.IsActive = workingGroup.IsActive;
                entityToUpdate.AllocateCaseMail = workingGroup.AllocateCaseMail;
                entityToUpdate.SendExternalEmailToWGUsers = workingGroup.SendExternalEmailToWGUsers;
                entityToUpdate.StateSecondary_Id = workingGroup.StateSecondary_Id;
                this.workingGroupRepository.Update(entityToUpdate);
            }

            if (workingGroup.IsDefault == 1)
            {
                this.workingGroupRepository.ResetDefault(workingGroup.Id);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        public void Commit()
        {
            this.unitOfWork.Commit();
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId)
        {
            return this.workingGroupRepository.GetOverviews(customerId);
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> workingGroupsIds)
        {
            return this.workingGroupRepository.GetOverviews(customerId, workingGroupsIds);
        }
    }
}
