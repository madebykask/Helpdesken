namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.Contract;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    #region CONTRACT

    public interface IContractRepository : INewRepository
    {
        IList<Contract> GetContracts(int customerId);
        Contract GetContract(int contractId);
        void SaveContract(ContractInputModel contractModel);
        void DeleteContract(Contract contract);
        IList<Contract> GetContractsNotFinished(int customerId);
    }

    public class ContractRepository : Repository, IContractRepository
    {
        public ContractRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<Contract> GetContracts(int customerId)
        {
            var query = this.DbContext.Contracts.Where(c => c.Finished == 0 && c.ContractCategory.Customer_Id == customerId);
            return query.ToList();
        }

        public IList<Contract> GetContractsNotFinished(int customerId)
        {
            var query = this.DbContext.Contracts.Where(c => c.ContractCategory.Customer_Id == customerId);
            return query.ToList();
        }

        public Contract GetContract(int contractId)
        {
            var query = this.DbContext.Contracts.Where(c => c.Id == contractId).FirstOrDefault();
            return query;
        }

        public void SaveContract(ContractInputModel contractModel)
        {

            if (contractModel.Id == 0)
            {
                var contractEntity = new Contract()
                {
                    Id = contractModel.Id,
                    ChangedByUser_Id = contractModel.ChangedByUser_Id,
                    ContractCategory_Id = contractModel.CategoryId,
                    Department_Id = contractModel.DepartmentId,
                    Finished = contractModel.Finished ? 1 : 0,
                    FollowUpInterval = contractModel.FollowUpInterval,
                    FollowUpResponsibleUser_Id = contractModel.FollowUpResponsibleUserId,
                    ResponsibleUser_Id = contractModel.ResponsibleUserId,
                    Running = contractModel.Running ? 1 : 0,
                    Supplier_Id = contractModel.SupplierId,
                    NoticeTime = contractModel.NoticeTime,
                    ContractNumber = contractModel.ContractNumber,
                    Info = contractModel.Other,
                    ContractStartDate = contractModel.ContractStartDate,
                    ContractEndDate = contractModel.ContractEndDate,
                    NoticeDate = contractModel.NoticeDate,
                    CreatedDate = contractModel.CreatedDate,
                    ChangedDate = contractModel.ChangedDate,
                    ContractGUID = contractModel.ContractGUID
                };

                this.DbContext.Contracts.Add(contractEntity);
                this.InitializeAfterCommit(contractModel, contractEntity);
            }
            else
            {
                var contractEntity = this.DbContext.Contracts.Find(contractModel.Id);

                contractEntity.ChangedByUser_Id = contractModel.ChangedByUser_Id;
                contractEntity.ContractCategory_Id = contractModel.CategoryId;
                contractEntity.Department_Id = contractModel.DepartmentId;
                contractEntity.Finished = contractModel.Finished ? 1 : 0;
                contractEntity.FollowUpInterval = contractModel.FollowUpInterval;
                contractEntity.FollowUpResponsibleUser_Id = contractModel.FollowUpResponsibleUserId;
                contractEntity.ResponsibleUser_Id = contractModel.ResponsibleUserId;
                contractEntity.Running = contractModel.Running ? 1 : 0;
                contractEntity.Supplier_Id = contractModel.SupplierId;
                contractEntity.NoticeTime = contractModel.NoticeTime;
                contractEntity.ContractNumber = contractModel.ContractNumber;
                contractEntity.Info = contractModel.Other;
                contractEntity.ContractStartDate = contractModel.ContractStartDate;
                contractEntity.ContractEndDate = contractModel.ContractEndDate;
                contractEntity.NoticeDate = contractModel.NoticeDate;
                contractEntity.ChangedDate = contractModel.ChangedDate;

            }
        }
        public void DeleteContract(Contract contract)
        {
            this.DbContext.Contracts.Remove(contract);
        }
    }

    #endregion

    #region CONTRACTCATEGORY

    public interface IContractCategoryRepository : IRepository<ContractCategory>
    {
    }

    public class ContractCategoryRepository : RepositoryBase<ContractCategory>, IContractCategoryRepository
    {
        public ContractCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CONTRACTFIELDSETTINGS

    public interface IContractFieldSettingsRepository : IRepository<ContractFieldSettings>
    {
        IList<ContractsSettingRowModel> GetContractsSettingRows(int customerId);
    }


    public class ContractFieldSettingsRepository : RepositoryBase<ContractFieldSettings>, IContractFieldSettingsRepository
    {
        public ContractFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<ContractsSettingRowModel> GetContractsSettingRows(int customerId)
        {
            var query = this.DataContext.ContractFieldSettings.Where(c => c.Customer_Id == customerId);
            var ret = query.Select(c => new ContractsSettingRowModel
            {
                Id = c.Id,
                CustomerId = c.Customer_Id,
                ContractField = c.ContractField,
                ContractFieldLable = c.Label,
                ContractFieldLable_Eng = c.Label_ENG,
                FieldHelp = c.FieldHelp,
                show = c.Show != 0,
                showInList = c.ShowInList != 0,
                ShowExternal = c.ShowExternal != 0,
                reguired = c.Required != 0,
                CreatedDate = c.CreatedDate,
                ChangedDate = c.ChangedDate
            }).ToList();
            return ret;
        }
    }

    #endregion

    #region CONTRACTFILE

    public interface IContractFileRepository : INewRepository
    {
        void SaveContracFile(ContractFileModel file);
        List<ContractFile> GetContractFiles(int contractId);
        ContractFile GetContractFile(int fileId);
        void DeleteContractFile(int fileId);
        void DeleteAllContractFiles(int contractId);
    }

    public class ContractFileRepository : Repository, IContractFileRepository
    {
        public ContractFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void SaveContracFile(ContractFileModel file)
        {
            var contractFileEntity = new ContractFile()
            {
                Id = 0,
                Contract_Id = file.Contract_Id,
                File = file.Content,
                ContentType = file.ContentType,
                ArchivedContractFile_Id = file.ArchivedContractFile_Id,
                FileName = file.FileName,
                ArchivedDate = file.ArchivedDate,
                CreatedDate = file.CreatedDate,
                ContractFileGUID = file.ContractFileGuid
            };

            this.DbContext.ContractFiles.Add(contractFileEntity);
            this.InitializeAfterCommit(file, contractFileEntity);

        }

        public List<ContractFile> GetContractFiles(int contractId)
        {
            var query = this.DbContext.ContractFiles.Where(c => c.Contract_Id == contractId);
            return query.ToList();
        }

        public ContractFile GetContractFile(int fileId)
        {
            var query = this.DbContext.ContractFiles.FirstOrDefault(c => c.Id == fileId);
            return query;
        }

        public void DeleteContractFile(int fileId)
        {
            //var contractFileEntity = new ContractFile()
            //{
            //    Id = contractFile.Id,
            //    Contract_Id = contractFile.Contract_Id,
            //    File = contractFile.Content,
            //    ContentType = contractFile.ContentType,
            //    ArchivedContractFile_Id = contractFile.ArchivedContractFile_Id,
            //    FileName = contractFile.FileName,
            //    ArchivedDate = contractFile.ArchivedDate,
            //    CreatedDate = contractFile.CreatedDate,
            //    ContractFileGUID = contractFile.ContractFileGuid
            //};

            var contractFileEntity = this.DbContext.ContractFiles.Find(fileId);
            this.DbContext.ContractFiles.Remove(contractFileEntity);
        }

        public void DeleteAllContractFiles(int contractId)
        {
            var contractFiles = this.DbContext.ContractFiles.Where(c => c.Contract_Id == contractId).ToList();
            if (contractFiles != null)
            {
                foreach (var contractFile in contractFiles)
                {
                    this.DbContext.ContractFiles.Remove(contractFile);
                }
            }

        }


    }

    #endregion

    #region CONTRACTHISTORY

    public interface IContractHistoryRepository : INewRepository
    {
        void SaveContractHistory(ContractInputModel contract);
        void DeleteContractHistory(Contract contract);
    }

    public class ContractHistoryRepository : Repository, IContractHistoryRepository
    {
        public ContractHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void SaveContractHistory(ContractInputModel contract)
        {
            var contractHistoryEntity = new ContractHistory()
            {
                Contract_Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                ContractCategory_Id = contract.CategoryId,
                ResponsibleUser_Id = contract.ResponsibleUserId,
                Supplier_Id = contract.SupplierId,
                Department_Id = contract.DepartmentId,
                ContractStartDate = contract.ContractStartDate,
                ContractEndDate = contract.ContractEndDate,
                NoticeDate = contract.NoticeDate,
                NoticeTime = contract.NoticeTime,
                Finished = contract.Finished ? 1 : 0,
                FollowUpInterval = contract.FollowUpInterval,
                FollowUpResponsibleUser_Id = contract.FollowUpResponsibleUserId,
                Running = contract.Running ? 1 : 0,
                Info = contract.Other,
                CreatedDate = DateTime.UtcNow,
                CreatedByUser_Id = contract.ChangedByUser_Id
            };

            this.DbContext.ContractHistories.Add(contractHistoryEntity);
            this.InitializeAfterCommit(contract, contractHistoryEntity);
        }

        public void DeleteContractHistory(Contract contract)
        {
            var contractHistories = this.DbContext.ContractHistories.Where(c => c.Contract_Id == contract.Id).ToList();
            foreach (var contractHistory in contractHistories)
            {
                this.DbContext.ContractHistories.Remove(contractHistory);
            }
        }
    }

    #endregion

    #region CONTRACTLOG

    public interface IContractLogRepository : IRepository<ContractLog>
    {
    }

    public class ContractLogRepository : RepositoryBase<ContractLog>, IContractLogRepository
    {
        public ContractLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
