using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IChangeStatusService
    {
        IDictionary<string, string> Validate(ChangeStatus changeStatusToValidate);

        IList<ChangeStatus> GetChangeStatuses(int customerId);

        ChangeStatus GetChangeStatus(int id, int customerId);
        DeleteMessage DeleteChangeStatus(int id);

        void DeleteChangeStatus(ChangeStatus changeStatus);
        void NewChangeStatus(ChangeStatus changeStatus);
        void UpdateChangeStatus(ChangeStatus changeStatus);
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

        public IDictionary<string, string> Validate(ChangeStatus changeStatusToValidate)
        {
            if (changeStatusToValidate == null)
                throw new ArgumentNullException("changestatustovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeStatus> GetChangeStatuses(int customerId)
        {
            return _changeStatusRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChangeStatus GetChangeStatus(int id, int customerId)
        {
            return _changeStatusRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeStatus(ChangeStatus changeStatus)
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

        public void NewChangeStatus(ChangeStatus changeStatus)
        {
            changeStatus.ChangedDate = DateTime.UtcNow;
            _changeStatusRepository.Add(changeStatus);
        }

        public void UpdateChangeStatus(ChangeStatus changeStatus)
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
