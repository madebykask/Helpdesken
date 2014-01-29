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
        private readonly IUnitOfWork _unitOfWork;

        public ChangePriorityService(
            IChangePriorityRepository changePriorityRepository,
            IUnitOfWork unitOfWork)
        {
            _changePriorityRepository = changePriorityRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(ChangePriorityEntity changePriorityToValidate)
        {
            if (changePriorityToValidate == null)
                throw new ArgumentNullException("changeprioritytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangePriorityEntity> GetChangePriorities(int customerId)
        {
            return _changePriorityRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChangePriorityEntity GetChangePriority(int id, int customerId)
        {
            return _changePriorityRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangePriority(ChangePriorityEntity changePriority)
        {
            _changePriorityRepository.Delete(changePriority);
        }

        public DeleteMessage DeleteChangePriority(int id)
        {
            var changePriority = _changePriorityRepository.GetById(id);

            if (changePriority != null)
            {
                try
                {
                    _changePriorityRepository.Delete(changePriority);
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
            _changePriorityRepository.Add(changePriority);
        }

        public void UpdateChangePriority(ChangePriorityEntity changePriority)
        {
            changePriority.ChangedDate = DateTime.UtcNow;
            _changePriorityRepository.Update(changePriority);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
