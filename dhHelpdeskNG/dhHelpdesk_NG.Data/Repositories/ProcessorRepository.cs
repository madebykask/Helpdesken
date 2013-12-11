using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface IProcessorRepository : IRepository<Processor>
    {
    }

    public class ProcessorRepository : RepositoryBase<Processor>, IProcessorRepository
    {
        public ProcessorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
