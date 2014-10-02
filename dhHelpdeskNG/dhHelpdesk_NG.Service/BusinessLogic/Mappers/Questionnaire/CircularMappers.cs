namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire;

    public static class CircularSpecifications
    {
        public static List<CircularOverview> MapToOverviews(this IQueryable<QuestionnaireCircularEntity> query)
        {
            var anonymus =
                query.Select(c => new { c.Id, circularName = c.CircularName, date = c.ChangedDate, state = c.Status })
                    .ToList();

            List<CircularOverview> overviews =
                anonymus.Select(q => new CircularOverview(q.Id, q.circularName, q.date, q.state)).ToList();
            return overviews;
        }

        public static List<CircularOverview> MapToOverviewsByQuestionnaireId(
            this IQueryable<QuestionnaireCircularEntity> query,
            int questionnaireId)
        {
            List<CircularOverview> overviews = query.GetByQuestionnaireId(questionnaireId).MapToOverviews();
            return overviews;
        }

        public static CircularForEdit MapToEditModelById(this IQueryable<QuestionnaireCircularEntity> query, int id)
        {
            CircularForEdit businessModel = null;

            var anonymus =
                query.GetById(id)
                    .Select(
                        c => new { c.Id, circularName = c.CircularName, c.ChangedDate, c.CreatedDate, state = c.Status })
                    .SingleOrDefault();

            if (anonymus != null)
            {
                businessModel = new CircularForEdit(
                    anonymus.Id,
                    anonymus.circularName,
                    anonymus.state,
                    anonymus.CreatedDate,
                    anonymus.ChangedDate);
            }

            return businessModel;
        }
    }
}
