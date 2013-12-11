using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IChangeService
    {
        IDictionary<string, string> Validate(Change changeToValidate);

        IList<Change> GetChange(int customerId);
        IList<Change> GetChanges(int customerId);
        Change GetChange(int id, int customerId);

        void DeleteChange(Change change);
        void NewChange(Change change);
        void UpdateChange(Change change);
        void Commit();
    }

    public class ChangeService : IChangeService
    {
        private readonly IChangeRepository _changeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeService(
            IChangeRepository changeRepository,
            IUnitOfWork unitOfWork)
        {
            _changeRepository = changeRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(Change changeToValidate)
        {
            if (changeToValidate == null)
                throw new ArgumentNullException("changetovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<Change> GetChanges(int customerId)
        {
            return _changeRepository.GetChanges(customerId).OrderBy(x => x.OrdererName).ToList();
        }

        public IList<Change> GetChange(int customerId)
        {
            return _changeRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.OrdererName).ToList();
        }

        public Change GetChange(int id, int customerId)
        {
            return _changeRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChange(Change change)
        {
            _changeRepository.Delete(change);
        }

        public void NewChange(Change change)
        {
            _changeRepository.Add(change);
        }

        public void UpdateChange(Change change)
        {
            _changeRepository.Update(change);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
