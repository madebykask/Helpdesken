namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    using ComputerLog = DH.Helpdesk.BusinessData.Models.Inventory.Input.ComputerLog;

    public class ComputerLogRepository : Repository<Domain.Computers.ComputerLog>, IComputerLogRepository
    {
        public ComputerLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerLog businessModel)
        {
            var entity = new Domain.Computers.ComputerLog
                             {
                                 Computer_Id = businessModel.ComputerId,
                                 CreatedByUser_Id = businessModel.CreatedByUserId,
                                 ComputerLogCategory = businessModel.ComputerLogCategory,
                                 ComputerLogText = businessModel.ComputerLogText,
                                 CreatedDate = businessModel.CreatedDate
                             };

            this.DbContext.ComputerLogs.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Delete(int id)
        {
            var entity = this.DbContext.ComputerLogs.Find(id);
            this.DbContext.ComputerLogs.Remove(entity);
        }

        public void DeleteByComputerId(int computerId)
        {
            var entities = this.DbContext.ComputerLogs.Where(x => x.Computer_Id == computerId).ToList();
            entities.ForEach(x => this.DbContext.ComputerLogs.Remove(x));
        }

        public List<ComputerLogOverview> Find(int computerId)
        {
            var anonymus =
                this.DbContext.ComputerLogs.Where(x => x.Computer_Id == computerId)
                    .Select(
                        c =>
                        new
                            {
                                c.Id,
                                c.Computer_Id,
                                UserFirstName = c.CreatedByUser.FirstName,
                                UserSurName = c.CreatedByUser.SurName,
                                c.ComputerLogText,
                                c.CreatedDate
                            })
                    .ToList();

            var overviews =
                anonymus.Select(
                    c =>
                    new ComputerLogOverview(
                        c.Id,
                        c.Computer_Id,
                        new UserName(c.UserFirstName, c.UserSurName),
                        c.ComputerLogText,
                        c.CreatedDate)).ToList();

            return overviews;
        }
    }
}