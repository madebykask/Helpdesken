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

    public interface IChangeStatusService
    {
        IDictionary<string, string> Validate(ChangeStatusEntity changeStatusToValidate);

        IList<ChangeStatusEntity> GetChangeStatuses(int customerId);

        ChangeStatusEntity GetChangeStatus(int id, int customerId);
        DeleteMessage DeleteChangeStatus(int id);

        void DeleteChangeStatus(ChangeStatusEntity changeStatus);
        void NewChangeStatus(ChangeStatusEntity changeStatus);
        void UpdateChangeStatus(ChangeStatusEntity changeStatus);
        void Commit();
    }

    public class ChangeStatusService : IChangeStatusService
    {
        private readonly IChangeStatusRepository _changeStatusRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeStatusService(
            IChangeStatusRepository changeStatusRepository,
            IUnitOfWork unitOfWork)
        {
            _changeStatusRepository = changeStatusRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(ChangeStatusEntity changeStatusToValidate)
        {
            if (changeStatusToValidate == null)
                throw new ArgumentNullException("changestatustovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeStatusEntity> GetChangeStatuses(int customerId)
        {
            return _changeStatusRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChangeStatusEntity GetChangeStatus(int id, int customerId)
        {
            return _changeStatusRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeStatus(ChangeStatusEntity changeStatus)
        {
            _changeStatusRepository.Delete(changeStatus);
        }

        public DeleteMessage DeleteChangeStatus(int id)
        {
            var changeStatus = _changeStatusRepository.GetById(id);

            if (changeStatus != null)
            {
                try
                {
                    _changeStatusRepository.Delete(changeStatus);
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

        public void NewChangeStatus(ChangeStatusEntity changeStatus)
        {
            changeStatus.ChangedDate = DateTime.UtcNow;
            _changeStatusRepository.Add(changeStatus);
        }

        public void UpdateChangeStatus(ChangeStatusEntity changeStatus)
        {
            changeStatus.ChangedDate = DateTime.UtcNow;
            _changeStatusRepository.Update(changeStatus);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
