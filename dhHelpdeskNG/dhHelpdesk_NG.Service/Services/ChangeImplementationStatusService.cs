namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeImplementationStatusService
    {
        IDictionary<string, string> Validate(ChangeImplementationStatusEntity changeImplementationStatusToValidate);

        IList<ChangeImplementationStatusEntity> GetChangeImplementationStatuses(int customerId);

        ChangeImplementationStatusEntity GetChangeImplementationStatus(int id, int customerId);
        DeleteMessage DeleteChangeImplementationStatus(int id);

        void DeleteChangeImplementationStatus(ChangeImplementationStatusEntity changeImplementationStatus);
        void NewChangeImplementationStatus(ChangeImplementationStatusEntity changeImplementationStatus);
        void UpdateChangeImplementationStatus(ChangeImplementationStatusEntity changeImplementationStatus);
        void Commit();
    }

    public class ChangeImplementationStatusService : IChangeImplementationStatusService
    {
        private readonly IChangeImplementationStatusRepository _changeImplementationStatusRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public ChangeImplementationStatusService(
            IChangeImplementationStatusRepository changeImplementationStatusRepository,
            IUnitOfWork unitOfWork)
        {
            this._changeImplementationStatusRepository = changeImplementationStatusRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> Validate(ChangeImplementationStatusEntity changeImplementationStatusToValidate)
        {
            if (changeImplementationStatusToValidate == null)
                throw new ArgumentNullException("changeimplementationstatustovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeImplementationStatusEntity> GetChangeImplementationStatuses(int customerId)
        {
            return this._changeImplementationStatusRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.ImplementationStatus).ToList();
        }

        public ChangeImplementationStatusEntity GetChangeImplementationStatus(int id, int customerId)
        {
            return this._changeImplementationStatusRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeImplementationStatus(ChangeImplementationStatusEntity changeImplementationStatus)
        {
            this._changeImplementationStatusRepository.Delete(changeImplementationStatus);
        }

        public DeleteMessage DeleteChangeImplementationStatus(int id)
        {
            var changeImplementationStatus = this._changeImplementationStatusRepository.GetById(id);

            if (changeImplementationStatus != null)
            {
                try
                {
                    this._changeImplementationStatusRepository.Delete(changeImplementationStatus);
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

        public void NewChangeImplementationStatus(ChangeImplementationStatusEntity changeImplementationStatus)
        {
            changeImplementationStatus.ChangedDate = DateTime.UtcNow;
            this._changeImplementationStatusRepository.Add(changeImplementationStatus);
        }

        public void UpdateChangeImplementationStatus(ChangeImplementationStatusEntity changeImplementationStatus)
        {
            changeImplementationStatus.ChangedDate = DateTime.UtcNow;
            this._changeImplementationStatusRepository.Update(changeImplementationStatus);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
