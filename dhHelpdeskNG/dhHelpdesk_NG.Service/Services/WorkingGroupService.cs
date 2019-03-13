using System.Data.Entity;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using Ninject.Infrastructure.Language;

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

        IList<int> ListWorkingGroupsForUser(int userId);

        IList<WorkingGroupInfo> GetWorkingGroupsAdmin(int customerId, int userId, bool isTakeOnlyActive = true, bool caseOverviewFilter = false);

        IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId, bool isTakeOnlyActive = true);

        Task<List<WorkingGroupEntity>>
            GetAllWorkingGroupsForCustomerAsync(int customerId, bool isTakeOnlyActive = true);

        IList<WorkingGroupEntity> GetWorkingGroupsForIndexPage(int customerId);

        int? GetDefaultId(int customerId, int userId);

        WorkingGroupEntity GetDefaultCreateCaseWorkingGroup(int customerId);

        List<GroupWithEmails> GetWorkingGroupsWithActiveEmails(int customerId, bool includeAdmins = true);

        IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId);

        WorkingGroupEntity GetWorkingGroup(int id);

        Task<WorkingGroupEntity> GetWorkingGroupAsync(int id);

        DeleteMessage DeleteWorkingGroup(int id);

        void SaveWorkingGroup(WorkingGroupEntity workingGroup, out IDictionary<string, string> errors);

        void Commit();

        IEnumerable<ItemOverview> GetOverviews(int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> workingGroupsIds);

        bool CaseHasWorkingGroup(int customerId, int workingGroupId);

    }

    public partial class WorkingGroupService : IWorkingGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserWorkingGroupRepository _userWorkingGroupRepository;
        private readonly ISettingService _settingService;

        public WorkingGroupService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IWorkingGroupRepository workingGroupRepository,
            ISettingService settingService)
        {
            this._unitOfWork = unitOfWork;
            this._workingGroupRepository = workingGroupRepository;
            this._userRepository = userRepository;
            this._userWorkingGroupRepository = userWorkingGroupRepository;
            this._settingService = settingService;
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroups()
        {
            return this._workingGroupRepository.GetAll().OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId)
        {
            return this._workingGroupRepository.ListUserForWorkingGroup(workingGroupId);
        }

        public IList<int> ListWorkingGroupsForUser(int userId)
        {
            return this._workingGroupRepository.ListWorkingGroupsForUser(userId);
        }

        public IList<WorkingGroupEntity> GetAllWorkingGroupsForCustomer(int customerId, bool isTakeOnlyActive = true)
        {
            return GetAllWorkingGroupsForCustomerQuery(customerId, isTakeOnlyActive)
                .OrderBy(x => x.WorkingGroupName)
                .ToList();
        }

        public IList<WorkingGroupEntity> GetWorkingGroups(int customerId, bool isTakeOnlyActive = true)
        {
            return GetAllWorkingGroupsForCustomer(customerId, isTakeOnlyActive)
                .OrderBy(x => x.WorkingGroupName)
                .ToList();
        }

        public IList<WorkingGroupEntity> GetWorkingGroups(int customerId, int userId, bool isTakeOnlyActive = true, bool caseOverviewFilter = false)
        {
            IEnumerable<int> userWorkingGroups;
            if (caseOverviewFilter)
            {
                userWorkingGroups =
                    _userWorkingGroupRepository.GetMany(uw => uw.User_Id == userId && uw.UserRole != 0)
                        .AsQueryable()
                        .Select(uw => uw.WorkingGroup_Id);
            }
            else
            {
                userWorkingGroups = 
                    _userWorkingGroupRepository.GetMany(uw => uw.User_Id == userId && uw.UserRole != 0)
                        .AsQueryable()
                        .Select(uw => uw.WorkingGroup_Id);
            }
            return GetAllWorkingGroupsForCustomer(customerId, isTakeOnlyActive)
                .Where(x => userWorkingGroups.Contains(x.Id))
                .OrderBy(x => x.WorkingGroupName)
                .ToList();
        }

        public IList<WorkingGroupInfo> GetWorkingGroupsAdmin(int customerId, int userId, bool isTakeOnlyActive = true, bool caseOverviewFilter = false)
        {
            var userWorkingGroupsIds =
                _userWorkingGroupRepository.GetMany(uw => uw.User_Id == userId && uw.UserRole == WorkingGroupUserPermission.ADMINSTRATOR)
                    .AsQueryable()
                    .Select(uw => uw.WorkingGroup_Id);

            return GetAllWorkingGroupsForCustomer(customerId, isTakeOnlyActive)
                    .Where(x => userWorkingGroupsIds.Contains(x.Id))
                    .Select(x => new WorkingGroupInfo
                    {
                        Id = x.Id,
                        WorkingGroupId = x.WorkingGroupId,
                        WorkingGroupGuid = x.WorkingGroupGUID
                    }).ToList();
        }

        public IList<WorkingGroupForSMS> GetWorkingGroupsForSMS(int customerId, bool isTakeOnlyActive = true)
        {
            var cs = _settingService.GetCustomerSetting(customerId);

            var ret = new List<WorkingGroupForSMS>();
            
            //todo: check why condition is missing?
            var userWorkingGroups = 
                _userWorkingGroupRepository.GetMany(x => true).AsQueryable()
                .Select(uw => uw.WorkingGroup_Id);

            var workingGroups =  
                GetAllWorkingGroupsForCustomer(customerId, isTakeOnlyActive)
                    .Where(x => userWorkingGroups.Contains(x.Id))
                    .ToList();

            var selectedWorkingGroupId = workingGroups.Select(w => w.Id).ToList();
            var userWorkingGroup =
                _userWorkingGroupRepository.GetMany(uw => selectedWorkingGroupId.Contains(uw.WorkingGroup_Id)).ToList();

            
            foreach(var wg in workingGroups)
            {
                var phones = 
                    userWorkingGroup.Where(uw => uw.WorkingGroup_Id == wg.Id && 
                                                 !string.IsNullOrEmpty(uw.User.CellPhone) && 
                                                 uw.User.IsActive == 1)
                    .Select(uw => new
                    {
                        phone = $"{uw.User.CellPhone.Replace(" ", "")}@{cs.SMSEMailDomain}"
                    }).ToArray();

                var phone = string.Join(",", phones.Select(p => p.phone).ToArray());

                ret.Add(new WorkingGroupForSMS(wg.Id, wg.WorkingGroupName, phone));
            }

            return ret.OrderBy(w => w.WorkingGroupName).ToList();
        }

        public IList<WorkingGroupEntity> GetWorkingGroupsForIndexPage(int customerId)
        {
            return this._workingGroupRepository
                .GetMany(x => x.Customer_Id == customerId)
                .OrderBy(x => x.WorkingGroupName).ToList();
        }

        public int? GetDefaultId(int customerId, int userId)
        {
            return this._workingGroupRepository.GetDefaultWorkingGroupId(customerId, userId);  
        }

        public WorkingGroupEntity GetDefaultCreateCaseWorkingGroup(int customerId)
        {
            return _workingGroupRepository.GetDefaultCreateCaseWorkingGroup(customerId);
        }

        public List<GroupWithEmails> GetWorkingGroupsWithActiveEmails(int customerId, bool includeAdmins = true)
        {
            var workingGroups = GetAllWorkingGroupsForCustomer(customerId, true)
                .OrderBy(w => w.WorkingGroupName)
                .ToList();
            var workingGroupIds = workingGroups.Select(g => g.Id).ToList();

            var workingGroupsUserIds = this._userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds, includeAdmins, true, true);
            
            var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).Distinct().ToList();

            var userIdsWithEmails = this._userRepository.FindUsersEmails(userIds, true);

            var workingGroupsWithEmails = new List<GroupWithEmails>(workingGroups.Count);

            foreach (var workingGroupOverview in workingGroups)
            {
                var groupEmails = new List<string>();

                if (!string.IsNullOrWhiteSpace(workingGroupOverview.EMail))
                {
                    groupEmails.Add(workingGroupOverview.EMail);
                }
                else
                {
                    var groupUserIdsWithEmails =
                        workingGroupsUserIds.FirstOrDefault(g => g.WorkingGroupId == workingGroupOverview.Id);

                    if (groupUserIdsWithEmails == null) continue;

                    groupEmails =
                        userIdsWithEmails.Where(e => groupUserIdsWithEmails.UserIds.Contains(e.ItemId))
                            .Select(e => e.Email)
                            .ToList();
               }

                var groupWithEmails = new GroupWithEmails(
                    workingGroupOverview.Id,
                    workingGroupOverview.WorkingGroupName,
                    groupEmails);

                workingGroupsWithEmails.Add(groupWithEmails);
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

        public bool CaseHasWorkingGroup(int customerId, int workingGroupId)
        {
            return this._workingGroupRepository.CaseHasWorkingGroup(customerId, workingGroupId);
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
                this._workingGroupRepository.Add(workingGroup);
            }
            else
            {
                var entityToUpdate = this._workingGroupRepository.GetById(workingGroup.Id);
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
                this._workingGroupRepository.Update(entityToUpdate);
            }

            if (workingGroup.IsDefault == 1)
            {
                this._workingGroupRepository.ResetDefault(workingGroup.Id, workingGroup.Customer_Id);
            }

            if (workingGroup.IsDefaultBulletinBoard == 1)
            {
                this._workingGroupRepository.ResetBulletinBoardDefault(workingGroup.Id, workingGroup.Customer_Id);
            }

            if (workingGroup.IsDefaultCalendar == 1)
            {
                this._workingGroupRepository.ResetCalendarDefault(workingGroup.Id, workingGroup.Customer_Id);
            }

            if (workingGroup.IsDefaultOperationLog == 1)
            {
                this._workingGroupRepository.ResetOperationLogDefault(workingGroup.Id, workingGroup.Customer_Id);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId)
        {
            return this._workingGroupRepository.GetOverviews(customerId);
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> workingGroupsIds)
        {
            return this._workingGroupRepository.GetOverviews(customerId, workingGroupsIds);
        }

        private IQueryable<WorkingGroupEntity> GetAllWorkingGroupsForCustomerQuery(int customerId, bool isTakeOnlyActive)
        {
            return _workingGroupRepository
                .GetMany(x =>
                    x.Customer_Id == customerId && ((isTakeOnlyActive && x.IsActive == 1) || !isTakeOnlyActive))
                .AsQueryable();
        }
    }
}
