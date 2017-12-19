using DH.Helpdesk.BusinessData.Enums.Users;

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
    using DH.Helpdesk.BusinessData.Models.WorkingGroup;

    public interface IWorkingGroupService
    {
        IList<WorkingGroupEntity> GetAllWorkingGroups();

        IList<WorkingGroupEntity> GetWorkingGroups(int customerId, bool isTakeOnlyActive = true);

        IList<WorkingGroupForSMS> GetWorkingGroupsForSMS(int customerId, bool isTakeOnlyActive = true);

        IList<WorkingGroupEntity> GetWorkingGroups(int customerId, int userId, bool isTakeOnlyActive = true, bool caseOverviewFilter = false);

        IList<WorkingGroupEntity> GetWorkingGroupsAdmin(int customerId, int userId, bool isTakeOnlyActive = true, bool caseOverviewFilter = false);

        IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId, bool isTakeOnlyActive = true);

        IList<WorkingGroupEntity> GetWorkingGroupsForIndexPage(int customerId);

        int? GetDefaultId(int customerId, int userId);

        List<GroupWithEmails> GetWorkingGroupsWithActiveEmails(int customerId, bool includeAdmins = true);

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

        private readonly ISettingService settingService;

        public WorkingGroupService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IWorkingGroupRepository workingGroupRepository,
            ISettingService settingService,
            IWorkContext workContext)
        {
            this.unitOfWork = unitOfWork;
            this.workingGroupRepository = workingGroupRepository;
            this.userRepository = userRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.workContext = workContext;
            this.settingService = settingService;
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroups()
        {
            return this.workingGroupRepository.GetAll().OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId)
        {
            return this.workingGroupRepository.ListUserForWorkingGroup(workingGroupId);
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId, bool isTakeOnlyActive = true)
        {
            return this.workingGroupRepository
                .GetMany(x => x.Customer_Id == customerId && ((isTakeOnlyActive && x.IsActive == 1) || !isTakeOnlyActive))
                .OrderBy(x => x.WorkingGroupName).ToList();
        }

        /// <summary>
        /// The get working groups.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="isTakeOnlyActive"></param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<WorkingGroupEntity> GetWorkingGroups(int customerId, bool isTakeOnlyActive = true)
        {
            return this.workingGroupRepository
                    .GetMany(x => x.Customer_Id == customerId && (!isTakeOnlyActive || (isTakeOnlyActive && x.IsActive == 1)))
                    .OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<WorkingGroupEntity> GetWorkingGroups(int customerId, int userId, bool isTakeOnlyActive = true, bool caseOverviewFilter = false)
        {
            IEnumerable<int> userWorkingGroups;
            if (caseOverviewFilter)
            {
                userWorkingGroups = userWorkingGroupRepository.GetAll()
                .Where(uw => uw.User_Id == userId && uw.UserRole != 0).Select(uw => uw.WorkingGroup_Id);
            }
            else
            {
                userWorkingGroups = userWorkingGroupRepository.GetAll()
                .Where(uw => uw.User_Id == userId && uw.UserRole != 0).Select(uw => uw.WorkingGroup_Id);
            }
            return  this.workingGroupRepository
                    .GetMany(x => x.Customer_Id == customerId && (!isTakeOnlyActive || (isTakeOnlyActive && x.IsActive == 1)))
                    .Where(w=> userWorkingGroups.Contains(w.Id))
                    .OrderBy(x => x.WorkingGroupName).ToList();

        }

        public IList<WorkingGroupEntity> GetWorkingGroupsAdmin(int customerId, int userId, bool isTakeOnlyActive = true, bool caseOverviewFilter = false)
        {
            IEnumerable<int> userWorkingGroups;
                userWorkingGroups = userWorkingGroupRepository.GetAll()
                .Where(uw => uw.User_Id == userId && uw.UserRole == WorkingGroupUserPermission.ADMINSTRATOR).Select(uw => uw.WorkingGroup_Id);
         
            return this.workingGroupRepository
                    .GetMany(x => x.Customer_Id == customerId && (!isTakeOnlyActive || (isTakeOnlyActive && x.IsActive == 1)))
                    .Where(w => userWorkingGroups.Contains(w.Id))
                    .OrderBy(x => x.WorkingGroupName).ToList();

        }

        public IList<WorkingGroupForSMS> GetWorkingGroupsForSMS(int customerId, bool isTakeOnlyActive = true)
        {
            var cs = this.settingService.GetCustomerSetting(customerId);

            var ret = new List<WorkingGroupForSMS>();
            var userWorkingGroups = this.userWorkingGroupRepository.GetAll()
                                                                   .Select(uw => uw.WorkingGroup_Id);
            var workingGroups =  this.workingGroupRepository
                    .GetMany(x => x.Customer_Id == customerId && (!isTakeOnlyActive || (isTakeOnlyActive && x.IsActive == 1)))
                    .Where(w => userWorkingGroups.Contains(w.Id)).ToList();
                    

            var selectedWGId = workingGroups.Select(w=> w.Id).ToList();
            var userWorkingGroup = this.userWorkingGroupRepository.GetAll().Where(uw => selectedWGId.Contains(uw.WorkingGroup_Id))
                                                                 .ToList();
            
            foreach(var wg in workingGroups)
            {
                var phones = userWorkingGroup.Where(uw=> uw.WorkingGroup_Id == wg.Id &&  !string.IsNullOrEmpty(uw.User.CellPhone))
                                .Select(uw=> new {phone = string.Format("{0}@{1}",uw.User.CellPhone.Replace(" ",""),cs.SMSEMailDomain)})                                
                                .ToArray();

                var phone = string.Join(",", phones.Select(p=> p.phone).ToArray());

                ret.Add(new WorkingGroupForSMS(wg.Id, wg.WorkingGroupName, phone));
            }

            return ret.OrderBy(w => w.WorkingGroupName).ToList();
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

        public List<GroupWithEmails> GetWorkingGroupsWithActiveEmails(int customerId, bool includeAdmins = true)
        {
            var workingGroupOverviews = this.workingGroupRepository.FindActiveIdAndNameOverviews(customerId);
            var workingGroupIds = workingGroupOverviews.Select(g => g.Id).ToList();

            var workingGroupsUserIds = this.userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds, includeAdmins, true);
            
            var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).Distinct().ToList();

            var userIdsWithEmails = this.userRepository.FindUsersEmails(userIds, true);

            var workingGroupsWithEmails = new List<GroupWithEmails>(workingGroupOverviews.Count);

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
            workingGroup.ChangedDate = DateTime.UtcNow;
            

            if (string.IsNullOrEmpty(workingGroup.WorkingGroupName))
            {
                errors.Add("WorkingGroup.Name", "Du måste ange en driftgrupp");
            }

            if (workingGroup.Id == 0)
            {
                workingGroup.WorkingGroupGUID = Guid.NewGuid();
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
                entityToUpdate.ChangedDate = DateTime.UtcNow;
                this.workingGroupRepository.Update(entityToUpdate);
            }

            if (workingGroup.IsDefault == 1)
            {
                this.workingGroupRepository.ResetDefault(workingGroup.Id, workingGroup.Customer_Id);
            }

            if (workingGroup.IsDefaultBulletinBoard == 1)
            {
                this.workingGroupRepository.ResetBulletinBoardDefault(workingGroup.Id, workingGroup.Customer_Id);
            }

            if (workingGroup.IsDefaultCalendar == 1)
            {
                this.workingGroupRepository.ResetCalendarDefault(workingGroup.Id, workingGroup.Customer_Id);
            }

            if (workingGroup.IsDefaultOperationLog == 1)
            {
                this.workingGroupRepository.ResetOperationLogDefault(workingGroup.Id, workingGroup.Customer_Id);
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
