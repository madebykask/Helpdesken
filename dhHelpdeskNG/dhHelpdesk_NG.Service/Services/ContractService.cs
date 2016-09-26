namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IContractService
    {
        IList<Contract> GetContractsWithCategories(int customerId);

        //ContractCategory GetContractCategory(int id);

        //DeleteMessage DeleteContractCategory(int id);

        //void SaveContractCategory(ContractCategory contractCategory, out IDictionary<string, string> errors);
        //void Commit();
    }

    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfwork;

        public ContractService(
            IContractRepository contractRepository,
            IUnitOfWork unitOfWork)
        {
            this._contractRepository = contractRepository;
            this._unitOfwork = unitOfWork;
        }

        public IList<Contract> GetContractsWithCategories(int customerId)
        {
            return this._contractRepository.GetContractsWithCategories(customerId).OrderBy(c => c.ContractNumber).ToList();
        }

        

        public void Commit()
        {
            this._unitOfwork.Commit();
        }
    }
}
