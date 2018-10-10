using System.Data.Entity;
using System.Linq.Expressions;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.Contract;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #region CONTRACT

    public interface IContractRepository : IRepository<Contract>
    {
        IQueryable<Contract> GetContracts(int customerId);
        IQueryable<Contract> GetContracts(Expression<Func<Contract, bool>> filter, bool loadNavigationProps = false);

        Contract GetContract(int contractId);
        int SaveContract(ContractInputModel contractModel);
        void DeleteContract(Contract contract);
    }

    public class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IQueryable<Contract> GetContracts(int customerId)
        {
            var query = this.Table.Where(c => c.ContractCategory.Customer_Id == customerId);
            return query;
        }

        public IQueryable<Contract> GetContracts(Expression<Func<Contract, bool>> filter, bool loadNavigationProps = false)
        {
            var query = this.Table.Where(filter);

            if (loadNavigationProps)
            {
                query.Include(x => x.ContractCategory)
                     .Include(x => x.ContractLogs)
                     .Include(x => x.ContractLogs.Select(l => l.Case));
            }

            return query.AsNoTracking();
        }

        public Contract GetContract(int contractId)
        {
            var query = this.Table.Where(c => c.Id == contractId).FirstOrDefault();
            return query;
        }

        public int SaveContract(ContractInputModel contractModel)
        {
            Contract contractEntity = null;

            if (contractModel.Id == 0)
            {
                contractEntity = new Contract
                {
                    CreatedDate = DateTime.UtcNow,
                    ContractGUID = Guid.NewGuid()
                };
                this.Add(contractEntity);
                this.InitializeAfterCommit(contractModel, contractEntity);
            }
            else
            {
                contractEntity = this.Table.Find(contractModel.Id);
            }
            
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
            contractEntity.ChangedDate = DateTime.UtcNow;
            contractEntity.ChangedByUser_Id = contractModel.ChangedByUser_Id;

            Commit();

            return contractEntity.Id;
        }

        public void DeleteContract(Contract contract)
        {
            this.Table.Remove(contract);
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

    public interface IContractHistoryRepository : IRepository<ContractHistory>
    {
        IList<ContractHistoryFull> GetContractractHistoryList(int contractId);
    }

    public class ContractHistoryRepository : RepositoryBase<ContractHistory>, IContractHistoryRepository
    {
        public ContractHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<ContractHistoryFull> GetContractractHistoryList(int contractId)
        {
            var query = from ch in Table
                where ch.Contract_Id == contractId
                select new ContractHistoryFull
                {
                    ContractId = ch.Contract_Id,
                    ContractNumber = ch.ContractNumber,
                    Finished = ch.Finished,
                    FollowUpInterval = ch.FollowUpInterval,
                    Running = ch.Running,
                    Info = ch.Info,
                    Files = ch.Files,
                    StartDate = ch.ContractStartDate,
                    EndDate = ch.ContractEndDate,
                    NoticeDate = ch.NoticeDate,
                    NoticeTime = ch.NoticeTime,
                    CreatedAt = ch.CreatedDate,

                    CreatedByUser = new UserBasicOvierview
                    {
                        Id = ch.CreatedByUser_Id,
                        UserID = ch.CreatedByUser.UserID,
                        FirstName = ch.CreatedByUser.FirstName,
                        SurName = ch.CreatedByUser.SurName
                    },

                    Department = ch.Department_Id != null ? new EntityOverview
                    {
                        Id = ch.Department_Id.Value,
                        Name = ch.Department.DepartmentName
                    } : null,

                    ContractCategory = new EntityOverview
                    {
                        Id = ch.ContractCategory_Id,
                        Name = ch.ContractCategory.Name
                    },

                    ResponsibleUser = ch.ResponsibleUser_Id != null ? new UserBasicOvierview
                    {
                        Id = ch.ResponsibleUser_Id.Value,
                        UserID = ch.ResponsibleUser.UserID,
                        FirstName = ch.ResponsibleUser.FirstName,
                        SurName = ch.ResponsibleUser.SurName
                    } : null,

                    FollowUpResponsibleUser = ch.FollowUpResponsibleUser_Id != null ? new UserBasicOvierview
                    {
                        Id = ch.FollowUpResponsibleUser_Id.Value,
                        UserID = ch.FollowUpResponsibleUser.UserID,
                        FirstName = ch.FollowUpResponsibleUser.FirstName,
                        SurName = ch.FollowUpResponsibleUser.SurName
                    } : null,

                    Supplier = ch.Supplier_Id != null ? new EntityOverview
                    {
                        Id = ch.Supplier_Id.Value,
                        Name = ch.Supplier.Name
                    } : null,
                };

            var list = query.ToList();
            return list.ToList();
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
