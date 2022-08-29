namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;

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
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public ChangeGroupService(
            IChangeGroupRepository changeGroupRepository,
            IUnitOfWork unitOfWork)
        {
            this._changeGroupRepository = changeGroupRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> Validate(ChangeGroupEntity changeGroupToValidate)
        {
            if (changeGroupToValidate == null)
                throw new ArgumentNullException("changegrouptovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeGroupEntity> GetChangeGroups(int customerId)
        {
            return this._changeGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.ChangeGroup).ToList();
        }

        public ChangeGroupEntity GetChangeGroup(int id, int customerId)
        {
            return this._changeGroupRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeGroup(ChangeGroupEntity changeGroup)
        {
            this._changeGroupRepository.Delete(changeGroup);
        }

        public DeleteMessage DeleteChangeGroup(int id)
        {
            var changeGroup = this._changeGroupRepository.GetById(id);

            if (changeGroup != null)
            {
                try
                {
                    this._changeGroupRepository.Delete(changeGroup);
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
            this._changeGroupRepository.Add(changeGroup);
        }

        public void UpdateChangeGroup(ChangeGroupEntity changeGroup)
        {
            changeGroup.ChangedDate = DateTime.UtcNow;
            this._changeGroupRepository.Update(changeGroup);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
