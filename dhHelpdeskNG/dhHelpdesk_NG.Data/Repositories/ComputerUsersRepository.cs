namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public class ComputerUsersRepository : Repository<ComputerUser>, IComputerUsersRepository
    {
        public ComputerUsersRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ComputerUserOverview> GetOverviews(int customerId, string userId)
        {
            var search = userId.ToLower();

            var anonymus = this.DbSet.Where(x => x.Customer_Id == customerId && x.UserId.ToLower().Contains(search));

            var models = GetConnectedToComputerOverviews(anonymus);

            return models;
        }

        public List<ComputerUserOverview> GetConnectedToComputerOverviews(int computerId)
        {
            // collation conflict between "SQL_SwedishStd_Pref_CP1_CI_AS" and "SQL_Latin1_General_CP1_CI_AS" in the equal to operation.
            // var anonymus = this.DbContext.ComputerHistories.Where(x => x.Computer_Id == computerId).Join(this.DbSet, x => x.UserId, x => x.UserId, (x, y) => y);*

            // todo need to replace with *
            var ids = this.DbContext.ComputerHistories.Where(x => x.Computer_Id == computerId).Select(x => x.UserId).ToList();
            var anonymus = this.DbSet.Where(x => ids.Contains(x.UserId));

            var models = GetConnectedToComputerOverviews(anonymus);

            return models;
        }

        public string FindUserGuidById(int id)
        {
            var guid = this.DbSet.Where(x => x.Id == id).Select(x => x.UserId).Single();
            return guid;
        }

        private static List<ComputerUserOverview> GetConnectedToComputerOverviews(IQueryable<ComputerUser> queryable)
        {
            var anonymus =
                queryable.Select(
                    x =>
                    new
                        {
                            x.Id,
                            x.UserId,
                            x.FirstName,
                            x.SurName,
                            x.Email,
                            x.Phone,
                            x.Phone2,
                            x.Department.DepartmentName,
                            UnitName = x.OU.Name
                        }).ToList();

            var models =
                anonymus.Select(
                    x =>
                    new ComputerUserOverview(
                        x.Id,
                        x.UserId,
                        x.FirstName,
                        x.SurName,
                        x.Email,
                        x.Phone,
                        x.Phone2,
                        x.DepartmentName,
                        x.UnitName)).ToList();
            return models;
        }
    }
}
