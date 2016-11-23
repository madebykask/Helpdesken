namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Contract;
    using DH.Helpdesk.Dal.DbContext;

    public interface IContractService
    {
        List<Contract> GetContracts(int customerId);
        Contract GetContract(int contractId);
        List<ContractsSettingRowModel> GetContractsSettingRows(int customerId);
        void SaveContractSettings(List<ContractsSettingRowModel> ContractSettings);
        int SaveContract(ContractInputModel contract);
        void SaveContractHistory(ContractInputModel contract);
        void DeleteContract(Contract contract);


        //ContractCategory GetContractCategory(int id);

        //DeleteMessage DeleteContractCategory(int id);

        //void SaveContractCategory(ContractCategory contractCategory, out IDictionary<string, string> errors);
        //void Commit();
    }

    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IContractHistoryRepository _contractHistoryRepository;
        private readonly IContractFieldSettingsRepository _contractFieldSettingsRepository;
        private readonly IUnitOfWork _unitOfwork;

        public ContractService(
            IContractRepository contractRepository,
            IContractHistoryRepository contractHistoryRepository,
            IContractFieldSettingsRepository contractFieldSettingsRepository,
            IUnitOfWork unitOfWork)
        {
            this._contractRepository = contractRepository;
            this._contractHistoryRepository = contractHistoryRepository;
            this._contractFieldSettingsRepository = contractFieldSettingsRepository;
            this._unitOfwork = unitOfWork;
        }

        public List<Contract> GetContracts(int customerId)
        {
            return this._contractRepository.GetContracts(customerId).OrderBy(c => c.ContractNumber).ToList();
        }

        public Contract GetContract(int contractId)
        {
            return this._contractRepository.GetContract(contractId);
        }

        public List<ContractsSettingRowModel> GetContractsSettingRows(int customerId)
        {
            return this._contractFieldSettingsRepository.GetContractsSettingRows(customerId).ToList();
        }

        public void Commit()        
        {
            this._unitOfwork.Commit();            
        }

        public void SaveContractSettings(List<ContractsSettingRowModel> contractsettings)
        {

            if (contractsettings == null)
                throw new ArgumentNullException("ContractSetting");

            foreach (var contractsetting in contractsettings)
            {
                var ContractFieldsSettingEntity = new ContractFieldSettings()
                {
                    Customer_Id = contractsetting.CustomerId,
                    Id = contractsetting.Id,
                    ContractField = contractsetting.ContractField,
                    Label = contractsetting.ContractFieldLable,
                    Label_ENG = contractsetting.ContractFieldLable_Eng,
                    Show = contractsetting.show ? 1 : 0,
                    FieldHelp = contractsetting.FieldHelp,
                    ShowInList = contractsetting.showInList ? 1 : 0,
                    ShowExternal = contractsetting.ShowExternal ? 1 : 0,
                    Required = contractsetting.reguired ? 1 : 0,
                    CreatedDate = contractsetting.CreatedDate,
                    ChangedDate = contractsetting.ChangedDate
                };

                if (contractsetting.Id == 0)
                    this._contractFieldSettingsRepository.Add(ContractFieldsSettingEntity);
                else
                    this._contractFieldSettingsRepository.Update(ContractFieldsSettingEntity);
            }

            this.Commit();
        }

        public int SaveContract(ContractInputModel contract)
        {

            if (contract == null)
                throw new ArgumentNullException("Contract");

            if (contract.ContractNumber == null)
                contract.ContractNumber = string.Empty;

            if (contract.SupplierId == 0)
                contract.SupplierId = null;

            if (contract.DepartmentId == 0)
                contract.DepartmentId = null;

            if (contract.ResponsibleUserId == 0)
                contract.ResponsibleUserId = null;

            if (contract.FollowUpResponsibleUserId == 0)
                contract.FollowUpResponsibleUserId = null;

            if (contract.Id == 0)            
                contract.ContractGUID = Guid.NewGuid();

            this._contractRepository.SaveContract(contract);
            this._contractRepository.Commit();           

            return contract.Id;
        }

        public void SaveContractHistory(ContractInputModel contract)
        {
            if (contract == null)
                throw new ArgumentNullException("Contract");

            if (contract.ContractNumber == null)
                contract.ContractNumber = string.Empty;

            if (contract.SupplierId == 0)
                contract.SupplierId = null;

            if (contract.DepartmentId == 0)
                contract.DepartmentId = null;

            if (contract.ResponsibleUserId == 0)
                contract.ResponsibleUserId = null;

            if (contract.FollowUpResponsibleUserId == 0)
                contract.FollowUpResponsibleUserId = null;

            this._contractHistoryRepository.SaveContractHistory(contract);
            this._contractHistoryRepository.Commit();
        }


        public void DeleteContract(Contract contract)
        {
            this._contractHistoryRepository.DeleteContractHistory(contract);
            this._contractHistoryRepository.Commit();

            this._contractRepository.DeleteContract(contract);
            this._contractRepository.Commit();
        }
       
    }
}
