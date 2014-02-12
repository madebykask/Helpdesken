namespace DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Questionnaire;

    public sealed class QuestionnaireRepository : RepositoryBase<QuestionnaireEntity>, IQuestionnaireRepository
    {
        #region Constructors and Destructors

        public QuestionnaireRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void DeleteById(int questionnaireId)
        {
            throw new System.NotImplementedException();
        }

        public List<QuestionnaireOverview> FindOverviews(int customerId)
        {
            var questionnaires =
                this.DataContext.Questionnaires.Where(q => q.Customer_Id == customerId)
                    .Select(
                        q => new { Id = q.Id, Name = q.QuestionnaireName, Description = q.QuestionnaireDescription })
                    .ToList();

            return
                questionnaires.Select(
                    q => new QuestionnaireOverview(q.Id, q.Name, q.Description))
                    .ToList();
        }

        #endregion

        #region Methods
    
        #endregion
    }
}