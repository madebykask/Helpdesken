namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire
{
    using System.Linq;

    using DH.Helpdesk.Domain.Questionnaire;

    public static class CircularPartSpecifications
    {
        public static IQueryable<QuestionnaireCircularPartEntity> GetByQuestionnaireId(
            this IQueryable<QuestionnaireCircularPartEntity> query,
            int questionnaireId)
        {
            IQueryable<QuestionnaireCircularPartEntity> entities =
                query.Where(x => x.QuestionnaireCircular.Questionnaire_Id == questionnaireId);

            return entities;
        }

        public static IQueryable<QuestionnaireCircularPartEntity> GetCircularCases(
            this IQueryable<QuestionnaireCircularPartEntity> query,
            int circuarId)
        {
            IQueryable<QuestionnaireCircularPartEntity> entities =
                query.Where(x => x.QuestionnaireCircular_Id == circuarId);

            return entities;
        }
    }
}