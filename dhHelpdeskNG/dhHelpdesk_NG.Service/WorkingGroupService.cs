using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IWorkingGroupService
    {
        IList<WorkingGroup> GetAllWorkingGroups();
        IList<WorkingGroup> GetWorkingGroups(int customerId);
        int? GetDefaultId(int customerId);

        IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId);
        WorkingGroup GetWorkingGroup(int id);
        DeleteMessage DeleteWorkingGroup(int id);

        void SaveWorkingGroup(WorkingGroup workingGroup, out IDictionary<string, string> errors);
        void Commit();
    }

    public class WorkingGroupService : IWorkingGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;

        public WorkingGroupService(
            IUnitOfWork unitOfWork,
            IWorkingGroupRepository workingGroupRepository)
        {
            _unitOfWork = unitOfWork;
            _workingGroupRepository = workingGroupRepository;
        }

        public IList<WorkingGroup> GetAllWorkingGroups()
        {
            return _workingGroupRepository.GetAll().OrderBy(x => x.WorkingGroupName).ToList();
        }

        public IList<UserWorkingGroup> GetUsersForWorkingGroup(int workingGroupId)
        {
            return _workingGroupRepository.ListUserForWorkingGroup(workingGroupId);
        }

        public IList<WorkingGroup> GetWorkingGroups(int customerId)
        {
            return _workingGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.WorkingGroupName).ToList();
        }

        public int? GetDefaultId(int customerId)
        {
            var r = _workingGroupRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }
        
        public WorkingGroup GetWorkingGroup(int id)
        {
            return _workingGroupRepository.GetById(id);
        }

        public DeleteMessage DeleteWorkingGroup(int id)
        {
            var workingGroup = _workingGroupRepository.GetById(id);

            if (workingGroup != null)
            {
                try
                {
                    _workingGroupRepository.Delete(workingGroup);
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

        public void SaveWorkingGroup(WorkingGroup workingGroup, out IDictionary<string, string> errors)
        {
            if (workingGroup == null)

                throw new ArgumentNullException("workinggroup");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(workingGroup.WorkingGroupName))
                errors.Add("WorkingGroup.Name", "Du måste ange en driftgrupp");
            if (string.IsNullOrEmpty(workingGroup.EMail))
                errors.Add("WorkingGroup.EMail", "Du måste ange en e-postadress");

            if (workingGroup.Id == 0)
                _workingGroupRepository.Add(workingGroup);
            else
                _workingGroupRepository.Update(workingGroup);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
