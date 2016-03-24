namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
            this._contractCategoryRepository = contractCategoryRepository;
            this._unitOfwork = unitOfWork;
        }

        public IList<ContractCategory> GetContractCategories(int customerId)
        {
            return this._contractCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ContractCategory GetContractCategory(int id)
        {
            return this._contractCategoryRepository.GetById(id);
        }

        public DeleteMessage DeleteContractCategory(int id)
        {
            var contractCategory = this._contractCategoryRepository.GetById(id);

            if (contractCategory != null)
            {
                try
                {
                    this._contractCategoryRepository.Delete(contractCategory);
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
            contractCategory.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(contractCategory.Name))
                errors.Add("ContractCategory.Name", "Du måste ange en avtalskategori");

            if (contractCategory.Id == 0)
                this._contractCategoryRepository.Add(contractCategory);
            else
                this._contractCategoryRepository.Update(contractCategory);

            if (errors.Count == 0)

                this.Commit();
        }

        public void Commit()
        {
            this._unitOfwork.Commit();
        }
    }
}
