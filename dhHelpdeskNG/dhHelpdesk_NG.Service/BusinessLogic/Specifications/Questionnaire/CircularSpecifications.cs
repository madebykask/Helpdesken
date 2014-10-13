namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire
{
    using System.Linq;

    using DH.Helpdesk.Domain.Questionnaire;

    public static class CircularSpecifications
    {
        public static IQueryable<QuestionnaireCircularEntity> GetById(
            this IQueryable<QuestionnaireCircularEntity> query,
            int id)
        {
            IQueryable<QuestionnaireCircularEntity> entities = query.Where(x => x.Id == id);

            return entities;
        }

        public static IQueryable<QuestionnaireCircularEntity> GetByQuestionnaireId(
            this IQueryable<QuestionnaireCircularEntity> query,
            int questionnaireId)
        {
            IQueryable<QuestionnaireCircularEntity> entities = query.Where(x => x.Questionnaire_Id == questionnaireId);

            return entities;
        }

        public static IQueryable<QuestionnaireCircularEntity> GetByState(
            this IQueryable<QuestionnaireCircularEntity> query,
            int state)
        {
            IQueryable<QuestionnaireCircularEntity> entities = query.Where(x => x.Status == state);

            return entities;
        }
    }
}
