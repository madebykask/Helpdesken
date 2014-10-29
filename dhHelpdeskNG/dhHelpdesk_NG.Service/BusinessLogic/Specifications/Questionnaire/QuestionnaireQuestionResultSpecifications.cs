namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain.Questionnaire;

    public static class QuestionnaireQuestionResultSpecifications
    {
        public static IQueryable<QuestionnaireQuestionResultEntity> GetCircularQuestionnaireQuestionResultEntities(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            int circularId)
        {
            query =
                query.Where(x => x.QuestionnaireResult.QuestionnaireCircularPart.QuestionnaireCircular_Id == circularId);

            return query;
        }

        public static IQueryable<QuestionnaireQuestionResultEntity> GetCircularsQuestionnaireQuestionResultEntities(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            List<int> circularIds)
        {
            query =
                query.Where(
                            x =>
                            circularIds.Contains(
                                x.QuestionnaireResult.QuestionnaireCircularPart.QuestionnaireCircular_Id));

            return query;
        }
    }
}
