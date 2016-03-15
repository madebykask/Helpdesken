namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IOperationLogCategoryService
    {
        IDictionary<string, string> Validate(OperationLogCategory operationLogCategoryToValidate);

        IList<OperationLogCategory> GetOperationLogCategories(int customerId);

        OperationLogCategory GetOperationLogCategory(int? id, int customerId);
        DeleteMessage DeleteOperationLogCategory(int id);
        //void DeleteOperationLogCategory(OperationLogCategory operationLogCategory);
        void NewOperationLogCategory(OperationLogCategory operationLogCategory);
        void UpdateOperationLogCategory(OperationLogCategory operationLogCategory);
        void Commit();
    }

    public class OperationLogCategoryService : IOperationLogCategoryService
    {
        private readonly IOperationLogCategoryRepository _operationLogCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OperationLogCategoryService(
            IOperationLogCategoryRepository operationLogCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            this._operationLogCategoryRepository = operationLogCategoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(OperationLogCategory operationLogCategoryToValidate)
        {
            if (operationLogCategoryToValidate == null)
                throw new ArgumentNullException("operationlogcategorytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<OperationLogCategory> GetOperationLogCategories(int customerId)
        {
            return this._operationLogCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.OLCName).ToList();
        }

        public OperationLogCategory GetOperationLogCategory(int? id, int customerId)
        {
            return this._operationLogCategoryRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        //public void DeleteOperationLogCategory(OperationLogCategory operationLogCategory)
        //{
        //    _operationLogCategoryRepository.Delete(operationLogCategory);
        //}

        public DeleteMessage DeleteOperationLogCategory(int id)
        {
            var operationLogCategory = this._operationLogCategoryRepository.GetById(id);

            if (operationLogCategory != null)
            {
                try
                {
                    this._operationLogCategoryRepository.Delete(operationLogCategory);
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

        public void NewOperationLogCategory(OperationLogCategory operationLogCategory)
        {
            operationLogCategory.ChangedDate = DateTime.UtcNow;
            this._operationLogCategoryRepository.Add(operationLogCategory);
        }

        public void UpdateOperationLogCategory(OperationLogCategory operationLogCategory)
        {
            operationLogCategory.ChangedDate = DateTime.UtcNow;
            this._operationLogCategoryRepository.Update(operationLogCategory);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

    }
}
