using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IOperationLogCategoryService
    {
        IDictionary<string, string> Validate(OperationLogCategory operationLogCategoryToValidate);

        IList<OperationLogCategory> GetOperationLogCategories(int customerId);

        OperationLogCategory GetOperationLogCategory(int id, int customerId);
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
            _operationLogCategoryRepository = operationLogCategoryRepository;
            _unitOfWork = unitOfWork;
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
            return _operationLogCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.OLCName).ToList();
        }

        public OperationLogCategory GetOperationLogCategory(int id, int customerId)
        {
            return _operationLogCategoryRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        //public void DeleteOperationLogCategory(OperationLogCategory operationLogCategory)
        //{
        //    _operationLogCategoryRepository.Delete(operationLogCategory);
        //}

        public DeleteMessage DeleteOperationLogCategory(int id)
        {
            var operationLogCategory = _operationLogCategoryRepository.GetById(id);

            if (operationLogCategory != null)
            {
                try
                {
                    _operationLogCategoryRepository.Delete(operationLogCategory);
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
            _operationLogCategoryRepository.Add(operationLogCategory);
        }

        public void UpdateOperationLogCategory(OperationLogCategory operationLogCategory)
        {
            operationLogCategory.ChangedDate = DateTime.UtcNow;
            _operationLogCategoryRepository.Update(operationLogCategory);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

    }
}
