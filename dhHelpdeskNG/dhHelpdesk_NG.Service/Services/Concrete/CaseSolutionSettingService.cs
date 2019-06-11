namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums.Settings;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Services.Requests.Cases;

    /*TODO: this service should be marged with CaseSolutionService*/

    public class CaseSolutionSettingService : ICaseSolutionSettingService
    {
        private readonly ICaseSolutionSettingRepository _caseSolutionSettingRepository;

        public CaseSolutionSettingService(ICaseSolutionSettingRepository caseSolutionSettingRepository)
        {
            this._caseSolutionSettingRepository = caseSolutionSettingRepository;
        }

        public ReadOnlyCollection<CaseSolutionSettingOverview> GetCaseSolutionSettingOverviews(int caseSolutionId)
        {
            var businessModels = this._caseSolutionSettingRepository.Find(caseSolutionId);
            return businessModels;
        }

        public void AddCaseSolutionSettings(CaseSettingsSolutionAggregate model)
        {
            var restoredBusinessModels =
                this.CreateRestoredCaseSolutionSettings(model.BusinessModels).ToList();
            restoredBusinessModels.AddRange(model.BusinessModels);

            var businessModelsForInsert =
                restoredBusinessModels.Select(
                    x =>
                    new CaseSolutionSettingForInsert(
                        model.CaseSolutionId,
                        x.CaseSolutionField,
                        x.CaseSolutionMode,
                        model.Context.DateAndTime)).ToList();

            this._caseSolutionSettingRepository.Add(businessModelsForInsert);
            this._caseSolutionSettingRepository.Commit();
        }

        public void UpdateCaseSolutionSettings(CaseSettingsSolutionAggregate model)
        {
            var businessModelsForInsert =
                model.BusinessModels.Where(x => x.Id == 0)
                    .Select(
                        x =>
                        new CaseSolutionSettingForInsert(
                            model.CaseSolutionId,
                            x.CaseSolutionField,
                            x.CaseSolutionMode,
                            model.Context.DateAndTime)).ToList();

            this._caseSolutionSettingRepository.Add(businessModelsForInsert);
            this._caseSolutionSettingRepository.Commit();

            var businessModelsForUpdate =
                model.BusinessModels.Where(x => x.Id != 0)
                    .Select(
                        x =>
                        new CaseSolutionSettingForUpdate(
                            x.Id,
                            x.CaseSolutionField,
                            x.CaseSolutionMode,
                            model.Context.DateAndTime)).ToList();

            this._caseSolutionSettingRepository.Update(businessModelsForUpdate);
            this._caseSolutionSettingRepository.Commit();
        }

        private IEnumerable<CaseSolutionSettingForWrite> CreateRestoredCaseSolutionSettings(
            IList<CaseSolutionSettingForWrite> businessModels)
        {
            var allFields = (CaseSolutionFields[])Enum.GetValues(
                typeof(CaseSolutionFields));

            return from field in allFields
                   where businessModels.Count(x => x.CaseSolutionField == field) == 0
                   select this.CreateCaseSolutionSettingForWrite(field);
        }

        private CaseSolutionSettingForWrite CreateCaseSolutionSettingForWrite(CaseSolutionFields field)
        {
            return new CaseSolutionSettingForWrite(field, CaseSolutionModes.DisplayField);
        }
    }
}