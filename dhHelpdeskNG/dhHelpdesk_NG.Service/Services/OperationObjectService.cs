namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IOperationObjectService
    {
        IList<OperationObject> GetOperationObjects(int customerId);

        IList<OperationObject> GetActiveOperationObjects(int customerId);

        OperationObject GetOperationObject(int id);

        DeleteMessage DeleteOperationObject(int id);

        void SaveOperationObject(OperationObject operationObject, out IDictionary<string, string> errors);
        void Commit();
    }

    public class OperationObjectService : IOperationObjectService
    {
        private readonly IOperationObjectRepository _operationObjectRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public OperationObjectService(
            IOperationObjectRepository operationObjectRepository,
            IUnitOfWork unitOfWork)
        {
            this._operationObjectRepository = operationObjectRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<OperationObject> GetOperationObjects(int customerId)
        {
            return this._operationObjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<OperationObject> GetActiveOperationObjects(int customerId)
        {
            
            return this._operationObjectRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
           
        }

        public OperationObject GetOperationObject(int id)
        {
            return this._operationObjectRepository.GetById(id);
        }

        public DeleteMessage DeleteOperationObject(int id)
        {
            var operationObject = this._operationObjectRepository.GetById(id);

            if (operationObject != null)
            {
                try
                {
                    this._operationObjectRepository.Delete(operationObject);
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
                this._operationObjectRepository.Add(operationObject);
            else
                this._operationObjectRepository.Update(operationObject);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
