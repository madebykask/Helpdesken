namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class CaseSolutionSettingRepository : Repository<Domain.Cases.CaseSolutionSetting>,
                                                 ICaseSolutionSettingRepository
    {
        public CaseSolutionSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(CaseSolutionSettingForInsert businessModel)
        {
            var entity = new Domain.Cases.CaseSolutionSetting();
            this.Map(entity, businessModel);

            entity.CaseSolution_Id = businessModel.CaseSolutionId;
            entity.ChangedDate = DateTime.UtcNow;
            entity.CreatedDate = businessModel.CreatedDate;

            this.DbSet.Add(entity);
        }

        public void Add(IEnumerable<CaseSolutionSettingForInsert> businessModels)
        {
            foreach (CaseSolutionSettingForInsert businessModel in businessModels)
            {
                this.Add(businessModel);
            }
        }

        public void Update(CaseSolutionSettingForUpdate businessModel)
        {
            Domain.Cases.CaseSolutionSetting entity = this.DbSet.Find(businessModel.Id);
            this.Map(entity, businessModel);

            entity.ChangedDate = businessModel.ChangedDate;
        }

        public void Update(IEnumerable<CaseSolutionSettingForUpdate> businessModels)
        {
            foreach (CaseSolutionSettingForUpdate businessModel in businessModels)
            {
                this.Update(businessModel);
            }
        }

        public ReadOnlyCollection<CaseSolutionSettingOverview> Find(int caseSolutionId)
        {
            var anonymus =
                this.DbSet.Where(x => x.CaseSolution_Id == caseSolutionId)
                    .Select(x => new { x.Id, x.CaseSolutionField, x.CaseSolutionMode })
                    .ToList();

            List<CaseSolutionSettingOverview> businessModels =
                anonymus.Select(
                    x =>
                    new CaseSolutionSettingOverview(x.Id, x.CaseSolutionField, x.CaseSolutionMode))
                    .ToList();

            return new ReadOnlyCollection<CaseSolutionSettingOverview>(businessModels);
        }

        public void DeleteByCaseSolutionId(int id)
        {
            List<Domain.Cases.CaseSolutionSetting> entities = this.DbSet.Where(x => x.CaseSolution_Id == id).ToList();
            entities.ForEach(x => this.DbSet.Remove(x));
        }

        private void Map(Domain.Cases.CaseSolutionSetting entity, CaseSolutionSetting businessModel)
        {
            entity.CaseSolutionField = businessModel.CaseSolutionField;
            entity.CaseSolutionMode = businessModel.CaseSolutionMode;
        }
    }
}