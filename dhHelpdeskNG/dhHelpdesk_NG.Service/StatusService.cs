using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IStatusService
    {
        IList<Status> GetStatuses(int customerId);
        int? GetDefaultId(int customerId); 
        Status GetStatus(int id);
        DeleteMessage DeleteStatus(int id);

        void SaveStatus(Status status, out IDictionary<string, string> errors);
        void Commit();
    }

    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StatusService(
            IStatusRepository statusRepository,
            IUnitOfWork unitOfWork)
        {
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Status> GetStatuses(int customerId)
        {
            return _statusRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public int? GetDefaultId(int customerId)
        {
            var r = _statusRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }
        
        public Status GetStatus(int id)
        {
            return _statusRepository.GetById(id);
        }

        public DeleteMessage DeleteStatus(int id)
        {
            var status = _statusRepository.GetById(id);

            if (status != null)
            {
                try
                {
                    _statusRepository.Delete(status);
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

        public void SaveStatus(Status status, out IDictionary<string, string> errors)
        {
            if (status == null)
                throw new ArgumentNullException("status");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(status.Name))
                errors.Add("Status.Name", "Du måste ange en status");

            if (status.Id == 0)
                _statusRepository.Add(status);
            else
                _statusRepository.Update(status);

            if (status.IsDefault == 1)
                _statusRepository.ResetDefault(status.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
