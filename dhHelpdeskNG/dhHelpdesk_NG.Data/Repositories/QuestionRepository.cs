namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region QUESTION

    public interface IQuestionRepository : IRepository<Question>
    {
    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONCATEGORY

    public interface IQuestionCategoryRepository : IRepository<QuestionCategory>
    {
    }

    public class QuestionCategoryRepository : RepositoryBase<QuestionCategory>, IQuestionCategoryRepository
    {
        public QuestionCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONGROUP

    public interface IQuestionGroupRepository : IRepository<QuestionGroup>
    {
    }

    public class QuestionGroupRepository : RepositoryBase<QuestionGroup>, IQuestionGroupRepository
    {
        public QuestionGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONREGISTRATION

    public interface IQuestionRegistrationRepository : IRepository<QuestionRegistration>
    {
    }

    public class QuestionRegistrationRepository : RepositoryBase<QuestionRegistration>, IQuestionRegistrationRepository
    {
        public QuestionRegistrationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
