using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IChangeGroupService
    {
        IDictionary<string, string> Validate(ChangeGroup changeGroupToValidate);

        IList<ChangeGroup> GetChangeGroups(int customerId);

        ChangeGroup GetChangeGroup(int id, int customerId);

        DeleteMessage DeleteChangeGroup(int id);

        void DeleteChangeGroup(ChangeGroup changeGroup);
        void NewChangeGroup(ChangeGroup changeGroup);
        void UpdateChangeGroup(ChangeGroup changeGroup);
        void Commit();
    }

    public class ChangeGroupService : IChangeGroupService
    {
        private readonly IChangeGroupRepository _changeGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeGroupService(
            IChangeGroupRepository changeGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _changeGroupRepository = changeGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(ChangeGroup changeGroupToValidate)
        {
            if (changeGroupToValidate == null)
                throw new ArgumentNullException("changegrouptovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeGroup> GetChangeGroups(int customerId)
        {
            return _changeGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChangeGroup GetChangeGroup(int id, int customerId)
        {
            return _changeGroupRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeGroup(ChangeGroup changeGroup)
        {
            _changeGroupRepository.Delete(changeGroup);
        }

        public DeleteMessage DeleteChangeGroup(int id)
        {
            var changeGroup = _changeGroupRepository.GetById(id);

            if (changeGroup != null)
            {
                try
                {
                    _changeGroupRepository.Delete(changeGroup);
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

        public void NewChangeGroup(ChangeGroup changeGroup)
        {
            changeGroup.ChangedDate = DateTime.UtcNow;
            _changeGroupRepository.Add(changeGroup);
        }

        public void UpdateChangeGroup(ChangeGroup changeGroup)
        {
            changeGroup.ChangedDate = DateTime.UtcNow;
            _changeGroupRepository.Update(changeGroup);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
