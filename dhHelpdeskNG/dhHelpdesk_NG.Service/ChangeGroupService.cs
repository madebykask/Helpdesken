using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Domain.Changes;

    public interface IChangeGroupService
    {
        IDictionary<string, string> Validate(ChangeGroupEntity changeGroupToValidate);

        IList<ChangeGroupEntity> GetChangeGroups(int customerId);

        ChangeGroupEntity GetChangeGroup(int id, int customerId);

        DeleteMessage DeleteChangeGroup(int id);

        void DeleteChangeGroup(ChangeGroupEntity changeGroup);
        void NewChangeGroup(ChangeGroupEntity changeGroup);
        void UpdateChangeGroup(ChangeGroupEntity changeGroup);
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

        public IDictionary<string, string> Validate(ChangeGroupEntity changeGroupToValidate)
        {
            if (changeGroupToValidate == null)
                throw new ArgumentNullException("changegrouptovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeGroupEntity> GetChangeGroups(int customerId)
        {
            return _changeGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.ChangeGroup).ToList();
        }

        public ChangeGroupEntity GetChangeGroup(int id, int customerId)
        {
            return _changeGroupRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeGroup(ChangeGroupEntity changeGroup)
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

        public void NewChangeGroup(ChangeGroupEntity changeGroup)
        {
            changeGroup.ChangedDate = DateTime.UtcNow;
            _changeGroupRepository.Add(changeGroup);
        }

        public void UpdateChangeGroup(ChangeGroupEntity changeGroup)
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
