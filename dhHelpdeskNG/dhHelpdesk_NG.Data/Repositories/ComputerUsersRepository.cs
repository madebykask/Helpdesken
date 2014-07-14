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

            var anonymus =
                this.DbSet.Where(x => x.Customer_Id == customerId && x.UserId.ToLower().Contains(search))
                    .Select(
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
                            })
                    .ToList();

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
