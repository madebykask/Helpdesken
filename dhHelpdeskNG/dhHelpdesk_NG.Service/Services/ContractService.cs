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

        List<Contract> GetContractsNotFinished(int customerId);
        
        Contract GetContract(int contractId);
        List<ContractsSettingRowModel> GetContractsSettingRows(int customerId);
        void SaveContractSettings(List<ContractsSettingRowModel> ContractSettings);
        int SaveContract(ContractInputModel contract);
        void SaveContractHistory(Contract contract, List<string> files);
        void SaveContracFile(ContractFileModel contractFile);
        List<ContractFileModel> GetContractFiles(int contractId);
        ContractFileModel GetContractFile(int fileId);

        IList<ContractHistoryFull> GetContractractHistoryList(int contractId);

        void DeleteContract(Contract contract);
        void DeleteContractFile(int fileId);


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
        private readonly IContractFileRepository _contractFileRepository;
        private readonly IUnitOfWork _unitOfwork;

        public ContractService(
            IContractRepository contractRepository,
            IContractHistoryRepository contractHistoryRepository,
            IContractFieldSettingsRepository contractFieldSettingsRepository,
            IContractFileRepository contractFileRepository,
            IUnitOfWork unitOfWork)
        {
            this._contractRepository = contractRepository;
            this._contractHistoryRepository = contractHistoryRepository;
            this._contractFieldSettingsRepository = contractFieldSettingsRepository;
            this._contractFileRepository = contractFileRepository;
            this._unitOfwork = unitOfWork;
        }

        public List<Contract> GetContracts(int customerId)
        {
            return this._contractRepository.GetContracts(customerId).OrderBy(c => c.ContractNumber).ToList();
        }

        public List<Contract> GetContractsNotFinished(int customerId)
        {
            return this._contractRepository.GetContractsNotFinished(customerId).OrderBy(c => c.ContractNumber).ToList();
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

            var contractId = this._contractRepository.SaveContract(contract);
            
            var entity = _contractRepository.GetById(contractId);

            SaveContractHistory(entity, contract.Files);
            return contract.Id;
        }

        public void SaveContractHistory(Contract contract, List<string> files)
        {
            if (contract == null)
                throw new ArgumentNullException("Contract");

            var contractHistory = new ContractHistory()
            {
                Contract_Id = contract.Id,
                ContractCategory_Id = contract.ContractCategory_Id,
                Department_Id = contract.Department_Id,
                Finished = contract.Finished,
                FollowUpInterval = contract.FollowUpInterval,
                FollowUpResponsibleUser_Id = contract.FollowUpResponsibleUser_Id,
                ResponsibleUser_Id = contract.ResponsibleUser_Id,
                Running = contract.Running,
                Supplier_Id = contract.Supplier_Id,
                ContractNumber = contract.ContractNumber,
                Info = contract.Info,
                Files = files.Any() ? string.Join(";", files) : string.Empty,
                ContractStartDate = contract.ContractStartDate,
                ContractEndDate = contract.ContractEndDate,
                NoticeDate = contract.NoticeDate,
                NoticeTime = contract.NoticeTime,
                CreatedDate = DateTime.UtcNow,
                CreatedByUser_Id = contract.ChangedByUser_Id
            };

            this._contractHistoryRepository.Add(contractHistory);
            this._contractHistoryRepository.Commit();
        }

        public void SaveContracFile(ContractFileModel contractFile)
        {
            if (contractFile == null)
                throw new ArgumentNullException("Contract");

            this._contractFileRepository.SaveContracFile(contractFile);
            this._contractFileRepository.Commit();
        }

        public List<ContractFileModel> GetContractFiles(int contractId)
        {
            var contractFileEntities = this._contractFileRepository.GetContractFiles(contractId);
     
            var contractFileModules = contractFileEntities.Select(conf => new ContractFileModel()
            {
                Id = conf.Id,
                Contract_Id = conf.Contract_Id,
                ArchivedContractFile_Id = conf.ArchivedContractFile_Id,
                FileName = conf.FileName,
                ArchivedDate = conf.ArchivedDate,
                Content = conf.File,
                ContentType = conf.ContentType,
                ContractFileGuid = conf.ContractFileGUID,
                CreatedDate = conf.CreatedDate
            }).ToList();
            return contractFileModules;
        }

        public ContractFileModel GetContractFile(int fileId)
        {
            var contractFileEntity = this._contractFileRepository.GetContractFile(fileId);

            var contractFileModule = new ContractFileModel()
            {
                Id = contractFileEntity.Id,
                Contract_Id = contractFileEntity.Contract_Id,
                ArchivedContractFile_Id = contractFileEntity.ArchivedContractFile_Id,
                FileName = contractFileEntity.FileName,
                ArchivedDate = contractFileEntity.ArchivedDate,
                Content = contractFileEntity.File,
                ContentType = contractFileEntity.ContentType,
                ContractFileGuid = contractFileEntity.ContractFileGUID,
                CreatedDate = contractFileEntity.CreatedDate
            };
            return contractFileModule;
        }

        public IList<ContractHistoryFull> GetContractractHistoryList(int contractId)
        {
            return _contractHistoryRepository.GetContractractHistoryList(contractId);
        }

        public void DeleteContract(Contract contract)
        {
            this._contractHistoryRepository.Delete(c => c.Contract_Id == contract.Id);
            this._contractHistoryRepository.Commit();

            this.DeleteAllContractFiles(contract.Id);

            this._contractRepository.DeleteContract(contract);
            this._contractRepository.Commit();
        }

        public void DeleteAllContractFiles(int contractId)
        {
            this._contractFileRepository.DeleteAllContractFiles(contractId);
            this._contractFileRepository.Commit();
        }

        public void DeleteContractFile(int fileId)
        {
            this._contractFileRepository.DeleteContractFile(fileId);
            this._contractFileRepository.Commit();
        }

    }
}
