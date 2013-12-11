using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IOperationObjectService
    {
        IList<OperationObject> GetOperationObjects(int customerId);

        OperationObject GetOperationObject(int id);

        DeleteMessage DeleteOperationObject(int id);

        void SaveOperationObject(OperationObject operationObject, out IDictionary<string, string> errors);
        void Commit();
    }

    public class OperationObjectService : IOperationObjectService
    {
        private readonly IOperationObjectRepository _operationObjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OperationObjectService(
            IOperationObjectRepository operationObjectRepository,
            IUnitOfWork unitOfWork)
        {
            _operationObjectRepository = operationObjectRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<OperationObject> GetOperationObjects(int customerId)
        {
            return _operationObjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public OperationObject GetOperationObject(int id)
        {
            return _operationObjectRepository.GetById(id);
        }

        public DeleteMessage DeleteOperationObject(int id)
        {
            var operationObject = _operationObjectRepository.GetById(id);

            if (operationObject != null)
            {
                try
                {
                    _operationObjectRepository.Delete(operationObject);
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

        public void SaveOperationObject(OperationObject operationObject, out IDictionary<string, string> errors)
        {
            if (operationObject == null)
                throw new ArgumentNullException("operationobject");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(operationObject.Name))
                errors.Add("OperationObject.Name", "Du måste ange ett driftobjekt");

            if (string.IsNullOrEmpty(operationObject.Description))
                errors.Add("OperationObject.Description", "Du måste ange en beskrivning");

            operationObject.ChangedDate = DateTime.UtcNow;

            if (operationObject.Id == 0)
                _operationObjectRepository.Add(operationObject);
            else
                _operationObjectRepository.Update(operationObject);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
