namespace dhHelpdesk_NG.Service.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Domain;

    public class ChangeService : IChangeService
    {
        private readonly IChangeRepository _changeRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IChangeFieldSettingRepository changeFieldSettingRepository;

        public ChangeService(
            IChangeRepository changeRepository,
            IUnitOfWork unitOfWork, IChangeFieldSettingRepository changeFieldSettingRepository)
        {
            this._changeRepository = changeRepository;
            this._unitOfWork = unitOfWork;
            this.changeFieldSettingRepository = changeFieldSettingRepository;
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
