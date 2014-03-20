namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerRepository : Repository<Domain.Computers.Computer>, IComputerRepository
    {
        public ComputerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public Computer FindById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ComputerOverview> FindOverviews(
            int customerId,
            int? departmentId,
            int? computerTypeId,
            int? contractStatusId,
            DateTime? contractStartDateFrom,
            DateTime? contractStartDateTo,
            DateTime? contractEndDateFrom,
            DateTime? contractEndDateTo,
            DateTime? scanDateFrom,
            DateTime? scanDateTo,
            DateTime? scrapDateFrom,
            DateTime? scrapDateTo,
            string searchFor)
        {
            throw new NotImplementedException();
        }

        // todo
        public List<ComputerResults> Search(int customerId, string searchFor)
        {
            var s = searchFor.ToLower();

            var query =
                from c in this.DbContext.Computers
                join ct in this.DbContext.ComputerTypes on c.ComputerType_Id equals ct.Id into res
                from k in res.DefaultIfEmpty()
                where c.Customer_Id == customerId
                      && (
                          c.ComputerName.ToLower().Contains(s)
                          || c.Location.ToLower().Contains(s)
                          || k.ComputerTypeDescription.ToLower().Contains(s))
                select new ComputerResults
                {
                    Id = c.Id,
                    ComputerName = c.ComputerName,
                    Location = c.Location,
                    ComputerType = k.Name,
                    ComputerTypeDescription = k.ComputerTypeDescription
                };

            return query.OrderBy(x => x.ComputerName).ThenBy(x => x.Location).ToList();
        }
    }
}
