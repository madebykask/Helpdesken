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

    public class CaseSolutionSettingService : ICaseSolutionSettingService
    {
        private readonly CaseSolutionSettingRepository caseSolutionSettingRepository;

        public CaseSolutionSettingService(CaseSolutionSettingRepository caseSolutionSettingRepository)
        {
            this.caseSolutionSettingRepository = caseSolutionSettingRepository;
        }

        public ReadOnlyCollection<CaseSolutionSettingOverview> GetCaseSolutionSettingOverviews(int caseSolutionId)
        {
            ReadOnlyCollection<CaseSolutionSettingOverview> businessModels = this.caseSolutionSettingRepository.Find(caseSolutionId);
            return businessModels;
        }

        public void AddCaseSolutionSettings(CaseSettingsSolutionAggregate model)
        {
            List<CaseSolutionSettingForWrite> restoredBusinessModels =
                this.CreateRestoredCaseSolutionSettings(model.BusinessModels).ToList();
            restoredBusinessModels.AddRange(model.BusinessModels);

            IEnumerable<CaseSolutionSettingForInsert> businessModelsForInsert =
                restoredBusinessModels.Select(
                    x =>
                    new CaseSolutionSettingForInsert(
                        model.CaseSolutionId,
                        x.CaseSolutionField,
                        x.CaseSolutionMode,
                        model.Context.DateAndTime));

            this.caseSolutionSettingRepository.Add(businessModelsForInsert);
            this.caseSolutionSettingRepository.Commit();
        }

        public void UpdateCaseSolutionSettings(CaseSettingsSolutionAggregate model)
        {
            IEnumerable<CaseSolutionSettingForInsert> businessModelsForInsert =
                model.BusinessModels.Where(x => x.Id == 0)
                    .Select(
                        x =>
                        new CaseSolutionSettingForInsert(
                            model.CaseSolutionId,
                            x.CaseSolutionField,
                            x.CaseSolutionMode,
                            model.Context.DateAndTime));

            this.caseSolutionSettingRepository.Add(businessModelsForInsert);
            this.caseSolutionSettingRepository.Commit();

            IEnumerable<CaseSolutionSettingForUpdate> businessModelsForUpdate =
                model.BusinessModels.Where(x => x.Id != 0)
                    .Select(
                        x =>
                        new CaseSolutionSettingForUpdate(
                            x.Id,
                            x.CaseSolutionField,
                            x.CaseSolutionMode,
                            model.Context.DateAndTime));

            this.caseSolutionSettingRepository.Update(businessModelsForUpdate);
            this.caseSolutionSettingRepository.Commit();
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