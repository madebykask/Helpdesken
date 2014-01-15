namespace dhHelpdesk_NG.Service.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Common.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeService : IChangeService
    {
        private readonly IChangeRepository _changeRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILanguageRepository languageRepository;

        private readonly IChangeFieldSettingRepository changeFieldSettingRepository;

        public ChangeService(
            IChangeRepository changeRepository,
            IUnitOfWork unitOfWork, IChangeFieldSettingRepository changeFieldSettingRepository, 
            ILanguageRepository languageRepository)
        {
            this._changeRepository = changeRepository;
            this._unitOfWork = unitOfWork;
            this.changeFieldSettingRepository = changeFieldSettingRepository;
            this.languageRepository = languageRepository;
        }

        public SearchResultDto SearchDetailedChangeOverviews(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> processAffectedIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            Data.Enums.Changes.ChangeStatus status,
            int selectCount)
        {
            return this._changeRepository.SearchOverviews(
                customerId,
                statusIds,
                objectIds,
                ownerIds,
                processAffectedIds,
                workingGroupIds,
                administratorIds,
                pharse,
                status,
                selectCount);
        }

        public FieldOverviewSettingsDto FindFieldOverviewSettings(int customerId, int languageId)
        {
            var languageTextId = this.languageRepository.FindLanguageIdById(languageId);
            
            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.FindSwedishByCustomerId(customerId);
                case LanguageTextId.English:
                    return this.changeFieldSettingRepository.FindEnglishByCustomerId(customerId);
                default:
                    throw new ArgumentOutOfRangeException("languageTextId", languageTextId);
            }
        }

        public void UpdateSettings(UpdatedFieldSettingsDto updatedSettings)
        {
            this.changeFieldSettingRepository.UpdateSettings(updatedSettings);
            this.changeFieldSettingRepository.Commit();
        }

        public FieldSettingsDto FindSettings(int customerId, int languageId)
        {
            return this.changeFieldSettingRepository.FindByCustomerIdAndLanguageId(customerId, languageId);
        }

        public IDictionary<string, string> Validate(Change changeToValidate)
        {
            if (changeToValidate == null)
                throw new ArgumentNullException("changetovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<Change> GetChanges(int customerId)
        {
            return this._changeRepository.GetChanges(customerId).OrderBy(x => x.OrdererName).ToList();
        }

        public IList<Change> GetChange(int customerId)
        {
            return this._changeRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.OrdererName).ToList();
        }

        public Change GetChange(int id, int customerId)
        {
            return this._changeRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChange(Change change)
        {
            this._changeRepository.Delete(change);
        }

        public void NewChange(Change change)
        {
            this._changeRepository.Add(change);
        }

        public void UpdateChange(Change change)
        {
            this._changeRepository.Update(change);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
