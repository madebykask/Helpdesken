namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;

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
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public ChangeObjectService(
            IChangeObjectRepository changeObjectRepository,
            IUnitOfWork unitOfWork)
        {
            this._changeObjectRepository = changeObjectRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> Validate(ChangeObjectEntity changeObjectToValidate)
        {
            if (changeObjectToValidate == null)
                throw new ArgumentNullException("changeobjecttovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChangeObjectEntity> GetChangeObjects(int customerId)
        {
            return this._changeObjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.ChangeObject).ToList();
        }

        public ChangeObjectEntity GetChangeObject(int id, int customerId)
        {
            return this._changeObjectRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChangeObject(ChangeObjectEntity changeObject)
        {
            this._changeObjectRepository.Delete(changeObject);
        }

        public DeleteMessage DeleteChangeObject(int id)
        {
            var changeObject = this._changeObjectRepository.GetById(id);

            if (changeObject != null)
            {
                try
                {
                    this._changeObjectRepository.Delete(changeObject);
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
            this._changeObjectRepository.Add(changeObject);
        }

        public void UpdateChangeObject(ChangeObjectEntity changeObject)
        {
            changeObject.ChangedDate = DateTime.UtcNow;
            this._changeObjectRepository.Update(changeObject);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
