namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.Response.Questionnaire;

    public static class QuestionnaireQuestionResultMappers
    {
        public static List<OptionResult> MapToOptionResults(this IQueryable<QuestionnaireQuestionResultEntity> query)
        {
            List<OptionResult> overviews =
                query.GroupBy(x => x.QuestionnaireQuestionOption_Id)
                    .Select(x => new { OptionId = x.Key, Count = x.Count() })
                    .ToList()
                    .Select(x => new OptionResult(x.OptionId, x.Count))
                    .ToList();

            return overviews;
        }
    }
}