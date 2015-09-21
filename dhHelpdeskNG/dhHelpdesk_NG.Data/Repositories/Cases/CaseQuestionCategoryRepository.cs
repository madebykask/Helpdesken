namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseQuestionCategoryRepository : IRepository<CaseQuestionCategory>
    {
    }

    public class CaseQuestionCategoryRepository : RepositoryBase<CaseQuestionCategory>, ICaseQuestionCategoryRepository
    {
        public CaseQuestionCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}