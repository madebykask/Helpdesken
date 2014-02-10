namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Questionnaire;

    #region QUSTIONNAIRE

    public interface IQuestionnaireRepository : IRepository<QuestionnaireEntity>
    {
    }

    public class QuestionnaireRepository : RepositoryBase<QuestionnaireEntity>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRECIRCULARPART

    public interface IQuestionnaireCircularPartRepository : IRepository<QuestionnaireCircularPartEntity>
    {
    }

    public class QuestionnaireCircularPartRepository : RepositoryBase<QuestionnaireCircularPartEntity>, IQuestionnaireCircularPartRepository
    {
        public QuestionnaireCircularPartRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRECIRCULAR

    public interface IQuestionnaireCircularRepository : IRepository<QuestionnaireCircularEntity>
    {
    }

    public class QuestionnaireCircularRepository : RepositoryBase<QuestionnaireCircularEntity>, IQuestionnaireCircularRepository
    {
        public QuestionnaireCircularRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRELANGUAGE

    public interface IQuestionnaireLanguageRepository : IRepository<QuestionnaireLanguageEntity>
    {
    }

    public class QuestionnaireLanguageRepository : RepositoryBase<QuestionnaireLanguageEntity>, IQuestionnaireLanguageRepository
    {
        public QuestionnaireLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIRERESULT

    public interface IQuestionnaireResultRepository : IRepository<QuestionnaireResultEntity>
    {
    }

    public class QuestionnaireResultRepository : RepositoryBase<QuestionnaireResultEntity>, IQuestionnaireResultRepository
    {
        public QuestionnaireResultRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
