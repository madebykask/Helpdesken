namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.WorkstationModules;

    public class NICRepository : Repository, INICRepository
    {
        public NICRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(NewItem businessModel)
        {
            var entity = new NIC
            {
                Id = businessModel.Id,
                Name = businessModel.Name,
                CreatedDate = businessModel.CreatedDate,
                ChangedDate = businessModel.CreatedDate, // todo
            };
            this.DbContext.NICs.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Delete(int id)
        {
            var entity = this.DbContext.NICs.Find(id);
            this.DbContext.NICs.Remove(entity);
        }

        public void Update(UpdatedItem businessModel)
        {
            var entity = this.DbContext.NICs.Find(businessModel.Id);
            entity.Name = businessModel.Name;
            entity.ChangedDate = businessModel.ChangedDate;
        }

        public List<ItemOverview> FindOverviews()
        {
            var anonymus =
                this.DbContext.NICs
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }
    }
}
