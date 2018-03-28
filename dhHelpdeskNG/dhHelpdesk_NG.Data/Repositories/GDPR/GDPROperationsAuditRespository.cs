using System.Collections.Generic;
using System.Linq;

using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPROperationsAuditRespository : IRepository<GDPROperationsAudit>
    {
        IDictionary<int, string> GetOperationsAuditCustomers();
    }

    public class GDPROperationsAuditRespository : RepositoryBase<GDPROperationsAudit>, IGDPROperationsAuditRespository
    {
        #region ctor()

        public GDPROperationsAuditRespository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        #endregion

        public IDictionary<int, string> GetOperationsAuditCustomers()
        {
            var items =
                Table.Where(x => x.Customer_Id != null && x.Customer_Id > 0)
                    .Select(x => new
                    {
                        Id = x.Customer_Id,
                        x.Customer.Name
                    })
                    .Distinct()
                    .OrderBy(x => x.Name)
                    .ToDictionary(x => x.Id ?? 0, x => x.Name);

            return items;
        }
    }
}