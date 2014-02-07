namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region QUSTIONNAIRE

    public interface IQuestionnaireRepository : IRepository<Questionnaire>
    {
    }

    public class QuestionnaireRepository : RepositoryBase<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRECIRCULARPART

    public interface IQuestionnaireCircularPartRepository : IRepository<QuestionnaireCircularPart>
    {
    }

    public class QuestionnaireCircularPartRepository : RepositoryBase<QuestionnaireCircularPart>, IQuestionnaireCircularPartRepository
    {
        public QuestionnaireCircularPartRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRECIRCULAR

    public interface IQuestionnaireCircularRepository : IRepository<QuestionnaireCircular>
    {
    }

    public class QuestionnaireCircularRepository : RepositoryBase<QuestionnaireCircular>, IQuestionnaireCircularRepository
    {
        public QuestionnaireCircularRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRELANGUAGE

    public interface IQuestionnaireLanguageRepository : IRepository<QuestionnaireLanguage>
    {
    }

    public class QuestionnaireLanguageRepository : RepositoryBase<QuestionnaireLanguage>, IQuestionnaireLanguageRepository
    {
        public QuestionnaireLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRERESULT

    public interface IQuestionnaireResultRepository : IRepository<QuestionnaireResult>
    {
    }

    public class QuestionnaireResultRepository : RepositoryBase<QuestionnaireResult>, IQuestionnaireResultRepository
    {
        public QuestionnaireResultRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
