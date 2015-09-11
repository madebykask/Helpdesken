namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseQuestionRepository : IRepository<CaseQuestion>
    {
    }

    public class CaseQuestionRepository : RepositoryBase<CaseQuestion>, ICaseQuestionRepository
    {
        public CaseQuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
