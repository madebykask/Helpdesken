namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
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
            entity.CreatedDate = businessModel.CreatedDate;
        }

        public void Add(ReadOnlyCollection<CaseSolutionSettingForInsert> businessModels)
        {
            foreach (CaseSolutionSettingForInsert businessModel in businessModels)
            {
                this.Add(businessModel);
            }
        }

        public void Update(CaseSolutionSettingForUpdate businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            this.Map(entity, businessModel);

            entity.ChangedDate = businessModel.ChangedDate;
        }

        public void Update(ReadOnlyCollection<CaseSolutionSettingForUpdate> businessModels)
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
                    .Select(x => new { x.Id, x.CaseSolutionField, x.Readonly, x.Show })
                    .ToList();

            var businessModels =
                anonymus.Select(
                    x =>
                    new CaseSolutionSettingOverview(x.Id, x.CaseSolutionField, x.Readonly.ToBool(), x.Show.ToBool()))
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
            entity.Readonly = businessModel.IsReadonly.ToInt();
            entity.Show = businessModel.IsShow.ToInt();
        }
    }
}