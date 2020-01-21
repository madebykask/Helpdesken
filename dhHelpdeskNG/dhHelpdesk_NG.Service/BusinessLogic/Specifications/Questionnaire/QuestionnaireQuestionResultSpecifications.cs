using DH.Helpdesk.Common.Tools;

namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire
{
    using System;
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

        public static IQueryable<QuestionnaireQuestionResultEntity> GetCircularDateFromQuestionnaireQuestionResultEntities(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return query;

            dateTime = dateTime.GetStartOfDay();
            query = query.Where(x => x.QuestionnaireResult.QuestionnaireCircularPart.QuestionnaireCircular.CreatedDate >= dateTime);

            return query;
        }

        public static IQueryable<QuestionnaireQuestionResultEntity> GetCircularDateToQuestionnaireQuestionResultEntities(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return query;

            dateTime = dateTime.GetEndOfDay();
            query = query.Where(x => x.QuestionnaireResult.QuestionnaireCircularPart.QuestionnaireCircular.CreatedDate <= dateTime);

            return query;
        }

        public static IQueryable<QuestionnaireQuestionResultEntity> GetCircularsQuestionnaireQuestionResultEntities(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            List<int> circularIds)
        {
            if (circularIds == null || circularIds.Count == 0)
            {
                return query;
            }

            query =
                query.Where(
                            x =>
                            circularIds.Contains(
                                x.QuestionnaireResult.QuestionnaireCircularPart.QuestionnaireCircular_Id));

            return query;
        }

        public static IQueryable<QuestionnaireQuestionResultEntity> GetQuestionResultDateFrom(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return query;

            dateTime = dateTime.GetStartOfDay();
            query = query.Where(x => x.QuestionnaireResult.CreatedDate >= dateTime);

            return query;
        }

        public static IQueryable<QuestionnaireQuestionResultEntity> GetQuestionResultDateTo(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return query;

            dateTime = dateTime.GetEndOfDay();
            query = query.Where(x => x.QuestionnaireResult.CreatedDate <= dateTime);

            return query;
        }
        public static IQueryable<QuestionnaireQuestionResultEntity> GetQuestionResultDepartment(
            this IQueryable<QuestionnaireQuestionResultEntity> query,
            List<int> departments)
        {
            if (!departments.Any())
            {
                return query;
            }

            query = query.Where(x => x.QuestionnaireResult.QuestionnaireCircularPart.Case.Department_Id.HasValue 
            && departments.Contains(x.QuestionnaireResult.QuestionnaireCircularPart.Case.Department_Id.Value));

            return query;
        }
    }
}
