namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Domain;


    #region QUESTIONNAIREQUESTION

    public interface IQuestionnaireQuestionRepository : IRepository<QuestionnaireQuestionEntity>
    {
    }

    public class QuestionnaireQuestionRepository : RepositoryBase<QuestionnaireQuestionEntity>, IQuestionnaireQuestionRepository
    {
        public QuestionnaireQuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESLANG

    public interface IQuestionnaireQuesLangRepository : IRepository<QuestionnaireQuesLangEntity>
    {
    }

    public class QuestionnaireQuesLangRepository : RepositoryBase<QuestionnaireQuesLangEntity>, IQuestionnaireQuesLangRepository
    {
        public QuestionnaireQuesLangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESOPLANG

    public interface IQuestionnaireQuesOpLangRepository : IRepository<QuestionnaireQuesOpLangEntity>
    {
    }

    public class QuestionnaireQuesOpLangRepository : RepositoryBase<QuestionnaireQuesOpLangEntity>, IQuestionnaireQuesOpLangRepository
    {
        public QuestionnaireQuesOpLangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESTIONOPTION

    public interface IQuestionnaireQuestionOptionRepository : IRepository<QuestionnaireQuestionOptionEntity>
    {
    }

    public class QuestionnaireQuestionOptionRepository : RepositoryBase<QuestionnaireQuestionOptionEntity>, IQuestionnaireQuestionOptionRepository
    {
        public QuestionnaireQuestionOptionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region QUESTIONNAIREQUESTIONRESULT

    public interface IQuestionnaireQuestionResultRepository : IRepository<QuestionnaireQuestionResultEntity>
    {
    }

    public class QuestionnaireQuestionResultRepository : RepositoryBase<QuestionnaireQuestionResultEntity>, IQuestionnaireQuestionResultRepository
    {
        public QuestionnaireQuestionResultRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
