namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;

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
            this._changeStatusRepository = changeStatusRepository;
            this._unitOfWork = unitOfWork;
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
            return this._changeStatusRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChangeStatusEntity GetChangeStatus(int id, int customerId)
        {
            return this._changeStatusRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeStatus(ChangeStatusEntity changeStatus)
        {
            this._changeStatusRepository.Delete(changeStatus);
        }

        public DeleteMessage DeleteChangeStatus(int id)
        {
            var changeStatus = this._changeStatusRepository.GetById(id);

            if (changeStatus != null)
            {
                try
                {
                    this._changeStatusRepository.Delete(changeStatus);
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
            this._changeStatusRepository.Add(changeStatus);
        }

        public void UpdateChangeStatus(ChangeStatusEntity changeStatus)
        {
            changeStatus.ChangedDate = DateTime.UtcNow;
            this._changeStatusRepository.Update(changeStatus);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
