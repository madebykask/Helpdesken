using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    using dhHelpdesk_NG.Data.Repositories.Changes;

    public interface IChangeImplementationStatusService
    {
        IDictionary<string, string> Validate(ChangeImplementationStatus changeImplementationStatusToValidate);

        IList<ChangeImplementationStatus> GetChangeImplementationStatuses(int customerId);

        ChangeImplementationStatus GetChangeImplementationStatus(int id, int customerId);
        DeleteMessage DeleteChangeImplementationStatus(int id);

        void DeleteChangeImplementationStatus(ChangeImplementationStatus changeImplementationStatus);
        void NewChangeImplementationStatus(ChangeImplementationStatus changeImplementationStatus);
        void UpdateChangeImplementationStatus(ChangeImplementationStatus changeImplementationStatus);
        void Commit();
    }

    public class ChangeImplementationStatusService : IChangeImplementationStatusService
    {
        private readonly IChangeImplementationStatusRepository _changeImplementationStatusRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeImplementationStatusService(
            IChangeImplementationStatusRepository changeImplementationStatusRepository,
            IUnitOfWork unitOfWork)
        {
            _changeImplementationStatusRepository = changeImplementationStatusRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(ChangeImplementationStatus changeImplementationStatusToValidate)
        {
            if (changeImplementationStatusToValidate == null)
                throw new ArgumentNullException("changeimplementationstatustovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeImplementationStatus> GetChangeImplementationStatuses(int customerId)
        {
            return _changeImplementationStatusRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChangeImplementationStatus GetChangeImplementationStatus(int id, int customerId)
        {
            return _changeImplementationStatusRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeImplementationStatus(ChangeImplementationStatus changeImplementationStatus)
        {
            _changeImplementationStatusRepository.Delete(changeImplementationStatus);
        }

        public DeleteMessage DeleteChangeImplementationStatus(int id)
        {
            var changeImplementationStatus = _changeImplementationStatusRepository.GetById(id);

            if (changeImplementationStatus != null)
            {
                try
                {
                    _changeImplementationStatusRepository.Delete(changeImplementationStatus);
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

        public void NewChangeImplementationStatus(ChangeImplementationStatus changeImplementationStatus)
        {
            changeImplementationStatus.ChangedDate = DateTime.UtcNow;
            _changeImplementationStatusRepository.Add(changeImplementationStatus);
        }

        public void UpdateChangeImplementationStatus(ChangeImplementationStatus changeImplementationStatus)
        {
            changeImplementationStatus.ChangedDate = DateTime.UtcNow;
            _changeImplementationStatusRepository.Update(changeImplementationStatus);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
