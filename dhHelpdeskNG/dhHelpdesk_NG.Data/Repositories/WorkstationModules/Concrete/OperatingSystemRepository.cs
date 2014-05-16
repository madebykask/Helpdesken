namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.WorkstationModules;

    public class OperatingSystemRepository : Repository, IOperatingSystemRepository
    {
        public OperatingSystemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(NewItem businessModel)
        {
            var entity = new OperatingSystem
                             {
                                 Id = businessModel.Id,
                                 Name = businessModel.Name,
                                 CreatedDate = businessModel.CreatedDate,
                                 ChangedDate = businessModel.CreatedDate, // todo
                             };
            this.DbContext.OperatingSystems.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Delete(int id)
        {
            var entity = this.DbContext.OperatingSystems.Find(id);
            this.DbContext.OperatingSystems.Remove(entity);
        }

        public void Update(UpdatedItem businessModel)
        {
            var entity = this.DbContext.OperatingSystems.Find(businessModel.Id);
            entity.Name = businessModel.Name;
            entity.ChangedDate = businessModel.ChangedDate;
        }

        public List<ItemOverview> FindOverviews()
        {
            var anonymus =
                this.DbContext.OperatingSystems
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }
    }
}
