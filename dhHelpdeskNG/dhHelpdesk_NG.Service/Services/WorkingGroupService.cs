using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IWorkingGroupService
    {
        IList<WorkingGroupEntity> GetAllWorkingGroups();
        IList<WorkingGroupEntity> GetWorkingGroups(int customerId);
        IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId);
        IList<WorkingGroupEntity> GetWorkingGroupsForIndexPage(int customerId);
        int? GetDefaultId(int customerId, int userId);

        List<GroupWithEmails> GetWorkingGroupsWithEmails(int customerId);
        IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId);
        WorkingGroupEntity GetWorkingGroup(int id);
        DeleteMessage DeleteWorkingGroup(int id);

        void SaveWorkingGroup(WorkingGroupEntity workingGroup, out IDictionary<string, string> errors);
        void Commit();
    }

    public class WorkingGroupService : IWorkingGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserWorkingGroupRepository _userWorkingGroupRepository;
        private readonly IWorkContext _workContext;

        public WorkingGroupService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IWorkingGroupRepository workingGroupRepository,
            IWorkContext workContext)
        {
            this._unitOfWork = unitOfWork;
            this._workingGroupRepository = workingGroupRepository;
            this._userRepository = userRepository;
            this._userWorkingGroupRepository = userWorkingGroupRepository;
            _workContext = workContext;
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroups()
        {
            return this._workingGroupRepository.GetAll().OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId)
        {
            return this._workingGroupRepository.ListUserForWorkingGroup(workingGroupId);
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId)
        {
            return _workingGroupRepository
                .GetMany(x => x.Customer_Id == customerId)
                .OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<WorkingGroupEntity> GetWorkingGroups(int customerId)
        {
            var userGroups = _workContext.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id);

            return _workingGroupRepository
                .GetMany(x => x.Customer_Id == customerId && x.IsActive == 1)
                .Where(g => userGroups.Contains(g.Id))
                .OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<WorkingGroupEntity> GetWorkingGroupsForIndexPage(int customerId)
        {
            return _workingGroupRepository
                .GetMany(x => x.Customer_Id == customerId && x.IsActive == 1)
                .OrderBy(x => x.WorkingGroupName).ToList();
        }

        public int? GetDefaultId(int customerId, int userId)
        {
            return this._workingGroupRepository.GetDefaultWorkingGroupId(customerId, userId);  
        }

        public List<GroupWithEmails> GetWorkingGroupsWithEmails(int customerId)
        {
            List<GroupWithEmails> workingGroupsWithEmails = null;

            var workingGroupOverviews = this._workingGroupRepository.FindActiveIdAndNameOverviews(customerId);
            var workingGroupIds = workingGroupOverviews.Select(g => g.Id).ToList();
            var workingGroupsUserIds = this._userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
            var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
            var userIdsWithEmails = this._userRepository.FindUsersEmails(userIds);

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
            return this._workingGroupRepository.GetById(id);
        }

        public DeleteMessage DeleteWorkingGroup(int id)
        {
            var workingGroup = this._workingGroupRepository.GetById(id);

            if (workingGroup != null)
            {
                try
                {
                    this._workingGroupRepository.Delete(workingGroup);
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

        public void SaveWorkingGroup(WorkingGroupEntity workingGroup, out IDictionary<string, string> errors)
        {
            if (workingGroup == null)

                throw new ArgumentNullException("workinggroup");

            errors = new Dictionary<string, string>();
            workingGroup.EMail = workingGroup.EMail ?? string.Empty;

            if (string.IsNullOrEmpty(workingGroup.WorkingGroupName))
                errors.Add("WorkingGroup.Name", "Du måste ange en driftgrupp");

            if (workingGroup.Id == 0)
                this._workingGroupRepository.Add(workingGroup);
            else
                this._workingGroupRepository.Update(workingGroup);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
