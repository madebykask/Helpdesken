namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Domain;


    #region QUESTIONNAIREQUESTION
    
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
