namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region QUESTIONNAIREQUESTION

    public interface IQuestionnaireQuestionRepository : IRepository<QuestionnaireQuestion>
    {
    }

    public class QuestionnaireQuestionRepository : RepositoryBase<QuestionnaireQuestion>, IQuestionnaireQuestionRepository
    {
        public QuestionnaireQuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESLANG

    public interface IQuestionnaireQuesLangRepository : IRepository<QuestionnaireQuesLang>
    {
    }

    public class QuestionnaireQuesLangRepository : RepositoryBase<QuestionnaireQuesLang>, IQuestionnaireQuesLangRepository
    {
        public QuestionnaireQuesLangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESOPLANG

    public interface IQuestionnaireQuesOpLangRepository : IRepository<QuestionnaireQuesOpLang>
    {
    }

    public class QuestionnaireQuesOpLangRepository : RepositoryBase<QuestionnaireQuesOpLang>, IQuestionnaireQuesOpLangRepository
    {
        public QuestionnaireQuesOpLangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESTIONOPTION

    public interface IQuestionnaireQuestionOptionRepository : IRepository<QuestionnaireQuestionOption>
    {
    }

    public class QuestionnaireQuestionOptionRepository : RepositoryBase<QuestionnaireQuestionOption>, IQuestionnaireQuestionOptionRepository
    {
        public QuestionnaireQuestionOptionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESTIONRESULT

    public interface IQuestionnaireQuestionResultRepository : IRepository<QuestionnaireQuestionResult>
    {
    }

    public class QuestionnaireQuestionResultRepository : RepositoryBase<QuestionnaireQuestionResult>, IQuestionnaireQuestionResultRepository
    {
        public QuestionnaireQuestionResultRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
