namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseQuestionHeaderRepository : IRepository<CaseQuestionHeader>
    {
    }

    public class CaseQuestionHeaderRepository : RepositoryBase<CaseQuestionHeader>, ICaseQuestionHeaderRepository
    {
        public CaseQuestionHeaderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}