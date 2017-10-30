using DH.Helpdesk.BusinessData.Models.Questionnaire;
using DH.Helpdesk.Dal.NewInfrastructure;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire;

    public static class CircularSpecifications
    {
        public static List<CircularOverview> MapToOverviews(this IQueryable<QuestionnaireCircularEntity> query)
        {
            var anonymus =
                query.Select(
                    c =>
                    new
                        {
                            c.Id,
                            circularName = c.CircularName,
                            date = c.ChangedDate,
                            state = c.Status,
                            total = c.QuestionnaireCircularPartEntities.Count,
                            sent = c.QuestionnaireCircularPartEntities.Count(x => x.SendDate != null),
                            answered = (int?)c.QuestionnaireCircularPartEntities.Sum(x => x.QuestionnaireResultEntities.Count)
                        }).ToList();

            List<CircularOverview> overviews =
                anonymus.Select(
                    q => new CircularOverview(q.Id, q.circularName, q.date, (CircularStates)q.state, q.total, q.sent, q.answered ?? 0))
                    .ToList();
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
            var anonymus =
                query.GetById(id)
                    .SingleOrDefault();

            if (anonymus != null)
            {
				return MapToCircularForEdit(anonymus);
			}

            return null;
        }

		public static CircularForEdit MapToEditModelByQuestionnaireId(this IQueryable<QuestionnaireCircularEntity> query, int id)
		{
			var anonymus =
				query.SingleOrDefault(x => x.Questionnaire_Id == id);

			if (anonymus != null)
			{
				return MapToCircularForEdit(anonymus);
			}

			return null;
		}

	    private static CircularForEdit MapToCircularForEdit( QuestionnaireCircularEntity anonymus)
	    {
		    var businessModel = new CircularForEdit(
			    anonymus.Id,
			    anonymus.CircularName,
			    anonymus.Questionnaire_Id,
			    (CircularStates) anonymus.Status,
			    anonymus.CreatedDate,
			    anonymus.ChangedDate,
			    new CircularCaseFilter
			    {
				    IsUniqueEmail = anonymus.IsUniqueEmail,
				    FinishingDateFrom = anonymus.FinishingDateFrom,
				    FinishingDateTo = anonymus.FinishingDateTo,
				    SelectedProcent = anonymus.SelectedProcent,
				    SelectedDepartments = anonymus.QuestionnaireCircularDepartmentEntities.Select(x => x.DepartmentId).ToList(),
				    SelectedCaseTypes = anonymus.QuestionnaireCircularCaseTypeEntities.Select(x => x.CaseTypeId).ToList(),
				    SelectedProductAreas = anonymus.QuestionnaireCircularProductAreaEntities.Select(x => x.ProductAreaId).ToList(),
				    SelectedWorkingGroups = anonymus.QuestionnaireCircularWorkingGroupEntities.Select(x => x.WorkingGroupId).ToList()
			    });
	        businessModel.MailTemplateId = anonymus.MailTemplate_Id;
		    return businessModel;
	    }
    }
}
