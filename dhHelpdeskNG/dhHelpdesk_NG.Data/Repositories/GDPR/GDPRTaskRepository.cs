using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPRTaskRepository : IRepository<GDPRTask>
    {
        IList<GDPRTask> GetTasks(GDPRTaskStatus? status);
    }

    public class GDPRTaskRepository : RepositoryBase<GDPRTask>, IGDPRTaskRepository
    {
        #region ctor()

        public GDPRTaskRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
        }

        #endregion

        public IList<GDPRTask> GetTasks(GDPRTaskStatus? status)
        {
            if (status.HasValue)
            {
                return Table.Where(x => x.Status == status.Value).ToList();
            }

            return Table.ToList();
        }
    }
}