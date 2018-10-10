using System.Linq.Expressions;
using DH.Helpdesk.BusinessData.Models.Inventory;
using DH.Helpdesk.Common.Linq;
using DH.Helpdesk.Services.Enums;
using ContractStatuses = DH.Helpdesk.Services.Enums.ContractStatuses;

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
        IList<Contract> GetContracts(int customerId);
        IList<ContractSearchItemData> SearchContracts(ContractsSearchFilter filter);

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

        public IList<Contract> GetContracts(int customerId)
        {
            return _contractRepository.GetContracts(customerId).ToList();
        }

        public IList<ContractSearchItemData> SearchContracts(ContractsSearchFilter filter)
        {
            var selectedStatus = filter.State;
            var customerId = filter.CustomerId;

            Expression<Func<Contract, bool>> exp = c => c.ContractCategory.Customer_Id == customerId;

            if (filter.SelectedContractCategories.Any())
                exp = PredicateBuilder<Contract>.AndAlso(exp, c => filter.SelectedContractCategories.Contains(c.ContractCategory_Id));

            if (filter.SelectedDepartments.Any())
                exp = PredicateBuilder<Contract>.AndAlso(exp, c => filter.SelectedDepartments.Contains(c.Department_Id ?? 0));
            

            if (filter.SelectedResponsibles.Any())
                exp = PredicateBuilder<Contract>.AndAlso(exp, c => filter.SelectedResponsibles.Contains(c.ResponsibleUser_Id ?? 0));

            if (filter.SelectedSuppliers.Any())
                exp = PredicateBuilder<Contract>.AndAlso(exp, c => filter.SelectedSuppliers.Contains(c.Supplier_Id ?? 0));

            //filter by state 
            if (selectedStatus == ContractStatuses.Closed)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, c => c.Finished == 1);
            }
            else if (selectedStatus == ContractStatuses.Running)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, c => c.Running == 1);
            }
            else if (selectedStatus != ContractStatuses.All)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, c => c.Finished == 0);
            }

            if (filter.StartDateFrom.HasValue)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, t => t.ContractStartDate >= filter.StartDateFrom.Value);
            }

            if (filter.StartDateTo.HasValue)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, t => t.ContractStartDate <= filter.StartDateTo.Value);
            }

            if (filter.EndDateFrom.HasValue)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, t => t.ContractEndDate >= filter.EndDateFrom.Value);
            }

            if (filter.EndDateTo.HasValue)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, t => t.ContractEndDate <= filter.EndDateTo.Value);
            }

            if (filter.NoticeDateFrom.HasValue)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, t => t.NoticeDate >= filter.NoticeDateFrom.Value);
            }

            if (filter.NoticeDateTo.HasValue)
            {
                exp = PredicateBuilder<Contract>.AndAlso(exp, t => t.NoticeDate <= filter.NoticeDateTo.Value);
            }

            //search text
            //todo: sql injection
            if (!string.IsNullOrEmpty(filter.SearchText)) 
                exp = PredicateBuilder<Contract>.AndAlso(exp, t => t.ContractNumber.Contains(filter.SearchText) || t.Info.Contains(filter.SearchText));

            var queryable = _contractRepository.GetContracts(exp).OrderBy(c => c.ContractNumber).AsQueryable();

            var contracts = 
                queryable.Select(contract => new ContractSearchItemData()
                {
                    Id = contract.Id,
                    ContractGUID = contract.ContractGUID,
                    ContractNumber = contract.ContractNumber,
                    Finished = contract.Finished,
                    NoticeTime = contract.NoticeTime,
                    NoticeDate = contract.NoticeDate,
                    ContractStartDate = contract.ContractStartDate,
                    ContractEndDate = contract.ContractEndDate,
                    Running = contract.Running,
                    FollowUpInterval = contract.FollowUpInterval,
                    Info = contract.Info,
                    
                    Supplier = contract.Supplier_Id != null ? new SupplierOverview()
                    {
                        Id = contract.Supplier_Id.Value,
                        Name = contract.Supplier.Name,
                    } : null,

                    ContractCategory = new ContractCategoryOverview()
                    {
                        Id = contract.ContractCategory_Id,
                        Name = contract.ContractCategory.Name
                    },

                    Department = contract.Department_Id != null ? new DepartmentOverview()
                    {
                        Id = contract.Department_Id.Value,
                        Name = contract.Department.DepartmentName
                    }: null,

                    ResponsibleUser = contract.ResponsibleUser_Id != null ? new UserOverview()
                    {
                        Id = contract.ResponsibleUser_Id.Value,
                        FirstName =  contract.ResponsibleUser.FirstName,
                        SurName = contract.ResponsibleUser.SurName
                    } : null,

                    FollowUpResponsibleUser = contract.FollowUpResponsibleUser_Id != null ? new UserOverview()
                    {
                        Id = contract.FollowUpResponsibleUser_Id.Value,
                        FirstName = contract.FollowUpResponsibleUser.FirstName,
                        SurName = contract.FollowUpResponsibleUser.SurName
                    } : null,

                    Cases = contract.ContractLogs.Where(l => l.Case_Id.HasValue).Select(l => new  ContractCaseData()
                    {
                        LogId =  l.Id,
                        CaseId = l.Case_Id,
                        CaseNumber = l.Case.CaseNumber,
                        CaseFinishingDate = l.Case.FinishingDate,
                        CaseApprovedDate = l.Case.ApprovedDate,
                        CaseTypeRequireApproving = l.Case.CaseType.RequireApproving,

                    }).ToList()
                }).ToList();

            return contracts;
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
