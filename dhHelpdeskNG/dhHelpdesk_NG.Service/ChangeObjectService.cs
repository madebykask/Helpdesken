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

    public interface IChangeObjectService
    {
        IDictionary<string, string> Validate(ChangeObjectEntity changeObjectToValidate);

        IList<ChangeObjectEntity> GetChangeObjects(int customerId);

        ChangeObjectEntity GetChangeObject(int id, int customerId);
        DeleteMessage DeleteChangeObject(int id);

        void DeleteChangeObject(ChangeObjectEntity changeObject);
        void NewChangeObject(ChangeObjectEntity changeObject);
        void UpdateChangeObject(ChangeObjectEntity changeObject);
        void Commit();
    }

    public class ChangeObjectService : IChangeObjectService
    {
        private readonly IChangeObjectRepository _changeObjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeObjectService(
            IChangeObjectRepository changeObjectRepository,
            IUnitOfWork unitOfWork)
        {
            _changeObjectRepository = changeObjectRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(ChangeObjectEntity changeObjectToValidate)
        {
            if (changeObjectToValidate == null)
                throw new ArgumentNullException("changeobjecttovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeObjectEntity> GetChangeObjects(int customerId)
        {
            return _changeObjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChangeObjectEntity GetChangeObject(int id, int customerId)
        {
            return _changeObjectRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeObject(ChangeObjectEntity changeObject)
        {
            _changeObjectRepository.Delete(changeObject);
        }

        public DeleteMessage DeleteChangeObject(int id)
        {
            var changeObject = _changeObjectRepository.GetById(id);

            if (changeObject != null)
            {
                try
                {
                    _changeObjectRepository.Delete(changeObject);
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

        public void NewChangeObject(ChangeObjectEntity changeObject)
        {
            changeObject.ChangedDate = DateTime.UtcNow;
            _changeObjectRepository.Add(changeObject);
        }

        public void UpdateChangeObject(ChangeObjectEntity changeObject)
        {
            changeObject.ChangedDate = DateTime.UtcNow;
            _changeObjectRepository.Update(changeObject);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
