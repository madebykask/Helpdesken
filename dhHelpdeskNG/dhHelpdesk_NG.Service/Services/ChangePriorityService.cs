namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangePriorityService
    {
        IDictionary<string, string> Validate(ChangePriorityEntity changePriorityToValidate);

        IList<ChangePriorityEntity> GetChangePriorities(int customerId);

        ChangePriorityEntity GetChangePriority(int id, int customerId);
        DeleteMessage DeleteChangePriority(int id);

        void DeleteChangePriority(ChangePriorityEntity changePriority);
        void NewChangePriority(ChangePriorityEntity changePriority);
        void UpdateChangePriority(ChangePriorityEntity changePriority);
        void Commit();
    }

    public class ChangePriorityService : IChangePriorityService
    {
        private readonly IChangePriorityRepository _changePriorityRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public ChangePriorityService(
            IChangePriorityRepository changePriorityRepository,
            IUnitOfWork unitOfWork)
        {
            this._changePriorityRepository = changePriorityRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> Validate(ChangePriorityEntity changePriorityToValidate)
        {
            if (changePriorityToValidate == null)
                throw new ArgumentNullException("changeprioritytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangePriorityEntity> GetChangePriorities(int customerId)
        {
            return this._changePriorityRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.ChangePriority).ToList();
        }

        public ChangePriorityEntity GetChangePriority(int id, int customerId)
        {
            return this._changePriorityRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangePriority(ChangePriorityEntity changePriority)
        {
            this._changePriorityRepository.Delete(changePriority);
        }

        public DeleteMessage DeleteChangePriority(int id)
        {
            var changePriority = this._changePriorityRepository.GetById(id);

            if (changePriority != null)
            {
                try
                {
                    this._changePriorityRepository.Delete(changePriority);
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

        public void NewChangePriority(ChangePriorityEntity changePriority)
        {
            changePriority.ChangedDate = DateTime.UtcNow;
            this._changePriorityRepository.Add(changePriority);
        }

        public void UpdateChangePriority(ChangePriorityEntity changePriority)
        {
            changePriority.ChangedDate = DateTime.UtcNow;
            this._changePriorityRepository.Update(changePriority);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
