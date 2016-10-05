namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Contract;

    public interface IContractService
    {
        List<Contract> GetContracts(int customerId);
        List<ContractsSettingRowModel> GetContractsSettingRows(int customerId);
        void SaveContractSettings(List<ContractsSettingRowModel> ContractSettings);

        //ContractCategory GetContractCategory(int id);

        //DeleteMessage DeleteContractCategory(int id);

        //void SaveContractCategory(ContractCategory contractCategory, out IDictionary<string, string> errors);
        //void Commit();
    }

    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IContractFieldSettingsRepository _contractFieldSettingsRepository;
        private readonly IUnitOfWork _unitOfwork;

        public ContractService(
            IContractRepository contractRepository,
            IContractFieldSettingsRepository contractFieldSettingsRepository,
            IUnitOfWork unitOfWork)
        {
            this._contractRepository = contractRepository;
            this._contractFieldSettingsRepository = contractFieldSettingsRepository;
            this._unitOfwork = unitOfWork;
        }

        public List<Contract> GetContracts(int customerId)
        {
            return this._contractRepository.GetContracts(customerId).OrderBy(c => c.ContractNumber).ToList();
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
    }
}
