using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPROperationsAuditRespository : IRepository<GDPROperationsAudit>
    {
    }

    public class GDPROperationsAuditRespository : RepositoryBase<GDPROperationsAudit>, IGDPROperationsAuditRespository
    {
        #region ctor()

        public GDPROperationsAuditRespository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            
        }

        #endregion
    }
}