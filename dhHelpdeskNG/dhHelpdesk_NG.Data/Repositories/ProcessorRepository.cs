namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
