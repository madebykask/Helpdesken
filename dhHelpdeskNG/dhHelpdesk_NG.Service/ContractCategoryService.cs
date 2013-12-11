using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IContractCategoryService
    {
        IList<ContractCategory> GetContractCategories(int customerId);

        ContractCategory GetContractCategory(int id);

        DeleteMessage DeleteContractCategory(int id);

        void SaveContractCategory(ContractCategory contractCategory, out IDictionary<string, string> errors);
        void Commit();
    }

    public class ContractCategoryService : IContractCategoryService
    {
        private readonly IContractCategoryRepository _contractCategoryRepository;
        private readonly IUnitOfWork _unitOfwork;

        public ContractCategoryService(
            IContractCategoryRepository contractCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _contractCategoryRepository = contractCategoryRepository;
            _unitOfwork = unitOfWork;
        }

        public IList<ContractCategory> GetContractCategories(int customerId)
        {
            return _contractCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ContractCategory GetContractCategory(int id)
        {
            return _contractCategoryRepository.GetById(id);
        }

        public DeleteMessage DeleteContractCategory(int id)
        {
            var contractCategory = _contractCategoryRepository.GetById(id);

            if (contractCategory != null)
            {
                try
                {
                    _contractCategoryRepository.Delete(contractCategory);
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

        public void SaveContractCategory(ContractCategory contractCategory, out IDictionary<string, string> errors)
        {
            if (contractCategory == null)
                throw new ArgumentNullException("contractcategory");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(contractCategory.Name))
                errors.Add("ContractCategory.Name", "Du måste ange en avtalskategori");

            if (contractCategory.Id == 0)
                _contractCategoryRepository.Add(contractCategory);
            else
                _contractCategoryRepository.Update(contractCategory);

            if (errors.Count == 0)

                this.Commit();
        }

        public void Commit()
        {
            _unitOfwork.Commit();
        }
    }
}
